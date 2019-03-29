using restlessmedia.Module.Security;
using System;
using System.Web;
using System.Web.Mvc;

namespace restlessmedia.Module.Web.Mvc.Attributes
{
  public class ActivityAttribute : AuthorizeAttribute
  {
    public ActivityAttribute(string activity, ActivityAccess access = ActivityAccess.Basic)
    {
      if (string.IsNullOrWhiteSpace(activity))
      {
        throw new ArgumentNullException(nameof(activity));
      }

      _activity = activity;
      _access = access;
    }

    protected override bool AuthorizeCore(HttpContextBase httpContext)
    {
      return SecurityService.Authorize(httpContext, _activity, _access);
    }

    protected ISecurityService SecurityService
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

    protected T Resolve<T>()
    {
      return System.Web.Mvc.DependencyResolverExtensions.GetService<T>(System.Web.Mvc.DependencyResolver.Current);
    }

    private ISecurityService _securityService;

    private readonly string _activity;

    private readonly ActivityAccess _access;
  }
}