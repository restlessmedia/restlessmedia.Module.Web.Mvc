using restlessmedia.Module.Security;
using System.Web.Mvc;

namespace restlessmedia.Module.Web.Mvc.Attributes
{
  public class ActivityFilterAttribute : ActionFilterAttribute
  {
    public ActivityFilterAttribute(string activity, string onFailRedirectTo, ActivityAccess access = ActivityAccess.Basic)
		{
      _activity = activity;
      _onFailRedirectTo = onFailRedirectTo;
      _access = access;
		}

    public override void OnActionExecuting(ActionExecutingContext filterContext)
    {
      if (Can(filterContext))
      {
        base.OnActionExecuting(filterContext);
      }
      else
      {
        Redirect(filterContext);
      }
    }

    protected bool Can(ActionExecutingContext filterContext)
    {
      return SecurityService.Authorize(filterContext.HttpContext, _activity, _access);
    }

    protected void Redirect(ActionExecutingContext filterContext)
    {
      filterContext.Result = new RedirectResult(_onFailRedirectTo);
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

    private readonly string _activity;

    private readonly ActivityAccess _access;

    private readonly string _onFailRedirectTo;

    private ISecurityService _securityService;
  }
}
