using restlessmedia.Module.Security;
using System;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace restlessmedia.Module.Web.Mvc.Attributes
{
  public class SecurityAccessAttribute : ActionFilterAttribute
  {
    /// <summary>
    /// Security Access check using authorisation header
    /// </summary>
    public SecurityAccessAttribute() { }

    /// <summary>
    /// Security Access check using request param value
    /// </summary>
    /// <param name="requestKey"></param>
    public SecurityAccessAttribute(string requestKey)
    {
      _requestKey = requestKey;
    }

    public override void OnActionExecuting(ActionExecutingContext filterContext)
    {
      Guid key;

      if (TryGetKey(filterContext.HttpContext, out key))
      {
        SecurityAccess access = SecurityService.CheckAccess(key);

        if (access.Status == AccessStatus.Success)
        {
          base.OnActionExecuting(filterContext);
          return;
        }
      }

      filterContext.Result = new HttpStatusCodeResult(HttpStatusCode.Unauthorized);
    }

    private ISecurityService SecurityService
    {
      get
      {
        if (_securityService == null)
        {
          _securityService = Resolve<ISecurityService>();
        }

        return _securityService;
      }
    }

    private T Resolve<T>()
    {
      return System.Web.Mvc.DependencyResolverExtensions.GetService<T>(System.Web.Mvc.DependencyResolver.Current);
    }

    private ISecurityService _securityService;

    private bool TryGetKey(HttpContextBase httpContext, out Guid key)
    {
      key = Guid.Empty;

      if (string.IsNullOrEmpty(_requestKey))
      {
        string authorisation;
        return httpContext.Request.TryGetHeader("Authorization", out authorisation) && Guid.TryParse(authorisation, out key);
      }
      else
      {
        return Guid.TryParse(httpContext.Request[_requestKey], out key);
      }
    }

    private string _requestKey;
  }
}