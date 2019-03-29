using restlessmedia.Module.Web.Mvc.Model;
using restlessmedia.Module.Web.Mvc.Model.Result;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace restlessmedia.Module.Web.Mvc.Controllers
{
  public abstract class ControllerBase : Controller, IController, System.Web.Mvc.IController
  {
    public ControllerBase(IUIContext context)
    {
      Context = context ?? throw new ArgumentNullException(nameof(context));
      LocationHelper = new LocationHelper(context.FileSettings, context.File);
      Authentication = new WebSecurity(context.Security);
    }

    [HttpGet]
    public virtual ActionResult Login()
    {
      return View<LogOnModel>("Login");
    }

    /// <summary>
    /// Generic ajax aware login.
    /// </summary>
    /// <param name="model"></param>
    /// <param name="returnUrl"></param>
    /// <returns></returns>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Login(LogOnModel model)
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

      if (Authentication.Authorise(HttpContext, model.UserName, model.Password, true))
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

    public ActionResult Logout()
    {
      Authentication.Logout(HttpContext);
      return Redirect(GetLogoutRedirect());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult ResetPassword(ResetPasswordModel model)
    {
      if (!ModelState.IsValid)
      {
        return View(model);
      }

      try
      {
        if (Context.Security.ResetPassword(model))
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

    public IUIContext Context { get; private set; }

    protected virtual string GetLoginRedirect(LogOnModel model)
    {
      // check against redirect hack prevention
      if (Url.IsLocalUrl(model.ReturnUrl))
      {
        return model.ReturnUrl;
      }

      return GetLogoutRedirect();
    }

    protected virtual string GetLogoutRedirect()
    {
      return string.Concat("/?u=", DateTime.Now.Ticks);
    }

    protected LocationHelper LocationHelper { get; private set; }

    /// <summary>
    /// Uses the internal json serializer which respects data members
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="data"></param>
    /// <returns></returns>
    protected ActionResult Json<T>(T data)
    {
      return Content(JsonHelper.Serialize(data), MimeExtensions.GetMimeType("json"));
    }

    [NonAction]
    protected ActionResult Spreadsheet<T>(IEnumerable<T> data, string name)
    {
      return new SpreadsheetResult<T>(data, name);
    }

    protected Uri PreviousPage
    {
      get
      {
        return _previousPage;
      }
    }

    protected override RedirectResult Redirect(string url)
    {
      return new AjaxAwareRedirectResult(url);
    }

    protected override void OnActionExecuting(ActionExecutingContext filterContext)
    {
      _previousPage = TempData[_previousPageKey] as Uri;
      TempData[_previousPageKey] = filterContext.RequestContext.HttpContext.Request.Url;

      base.OnActionExecuting(filterContext);
    }

    /// <summary>
    /// Creates an empty instance of TModel returning the View for viewname.
    /// </summary>
    /// <typeparam name="TModel"></typeparam>
    /// <param name="viewName"></param>
    /// <returns></returns>
    protected ViewResult View<TModel>(string viewName)
      where TModel : new()
    {
      return View(viewName, new TModel());
    }

    protected readonly WebSecurity Authentication;

    private Uri _previousPage = null;

    private const string _previousPageKey = "previousPage";
  }
}