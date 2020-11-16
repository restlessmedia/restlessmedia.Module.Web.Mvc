using restlessmedia.Module.Security;
using restlessmedia.Module.Web.Mvc.Model;
using restlessmedia.Module.Web.Mvc.Model.Result;
using System;
using System.Web.Mvc;

namespace restlessmedia.Module.Web.Mvc.Controllers
{
  public class AuthenticationController : Controller
  {
    public AuthenticationController(IWebSecurityProvider authentication, ISecurityService securityService)
    {
      _authentication = authentication ?? throw new ArgumentNullException(nameof(authentication));
      _securityService = securityService ?? throw new ArgumentNullException(nameof(securityService));
    }

    [HttpGet]
    [Route("Login")]
    public virtual ActionResult Login()
    {
      return View("Login", new LogOnModel());
    }

    /// <summary>
    /// Generic ajax aware login.
    /// </summary>
    /// <param name="model"></param>
    /// <param name="returnUrl"></param>
    /// <returns></returns>
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Route("Login")]
    public virtual ActionResult Login(LogOnModel model)
    {
      bool isAjax = ControllerContext.HttpContext.Request.IsAjaxRequest();

      if (!ModelState.IsValid)
      {
        if (isAjax)
        {
          return Json(LoginJsonResultModel.Error("Invalid"));
        }

        return View("Login", model);
      }

      if (_authentication.Authorise(HttpContext, model.UserName, model.Password, true))
      {
        string redirectTo = GetLoginRedirect(model);

        if (isAjax)
        {
          return Json(LoginJsonResultModel.Success(redirectTo));
        }

        return Redirect(redirectTo);
      }

      if (isAjax)
      {
        return Json(LoginJsonResultModel.Error("Invalid username or password"));
      }

      ModelState.AddModelError(string.Empty, "Invalid username or password");

      return View("Login", model);
    }

    [Route("Logout")]
    public virtual ActionResult Logout()
    {
      _authentication.Logout(HttpContext);
      return Redirect(LogoutRedirect);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Route("ResetPassword")]
    public virtual ActionResult ResetPassword(ResetPasswordModel model)
    {
      if (!ModelState.IsValid)
      {
        return View(model);
      }

      try
      {
        if (_securityService.ResetPassword(model))
        {
          return Login(new LogOnModel(model));
        }
      }
      catch (Exception e)
      {
        ViewBag.Success = false;
        ViewBag.Message = e.Message;
      }

      return View(model);
    }

    [HttpGet]
    [Route("ResetPassword")]
    public ActionResult ResetPassword(Guid? accessKey)
    {
      if (accessKey.HasValue)
      {
        return View(new ResetPasswordModel(accessKey.Value));
      }

      return View(new ResetPasswordModel());
    }

    protected virtual string GetLoginRedirect(LogOnModel model)
    {
      // check against redirect hack prevention
      if (Url.IsLocalUrl(model.ReturnUrl))
      {
        return model.ReturnUrl;
      }

      return LogoutRedirect;
    }

    protected virtual string LogoutRedirect
    {
      get
      {
        return $"/?u={DateTime.Now.Ticks}";
      }
    }

    protected override RedirectResult Redirect(string url)
    {
      return new AjaxAwareRedirectResult(url);
    }

    private readonly IWebSecurityProvider _authentication;

    private readonly ISecurityService _securityService;
  }
}