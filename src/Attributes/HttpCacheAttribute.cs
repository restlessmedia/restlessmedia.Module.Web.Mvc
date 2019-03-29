using System.Net;
using System.Web.Mvc;

namespace restlessmedia.Module.Web.Mvc
{
  public class HttpCacheAttribute : ActionFilterAttribute
  {
    protected HttpCacheAttribute(bool isPublic = true)
    {
      Options = new HttpCacheOptions(isPublic);
    }

    public override void OnActionExecuted(ActionExecutedContext filterContext)
    {
      base.OnActionExecuted(filterContext);
      Options.Apply(filterContext.HttpContext.Response.Cache);
    }

    protected ActionResult NotModified()
    {
      return new HttpStatusCodeResult(HttpStatusCode.NotModified);
    }

    protected T Resolve<T>()
    {
      return DependencyResolverExtensions.GetService<T>(DependencyResolver.Current);
    }

    protected virtual HttpCacheOptions Options { get; private set; }
  }
}