using System.Web.Mvc;

namespace restlessmedia.Module.Web.Mvc.Filters
{
  /// <summary>
  /// Class for dealing with request restriction filtering
  /// </summary>
  public class RestrictAttribute : ActionFilterAttribute
  {
    public RestrictAttribute(bool localOnly = false)
    {
      LocalOnly = localOnly;
    }

    public bool LocalOnly = false;

    public override void OnActionExecuting(ActionExecutingContext filterContext)
    {
      if (LocalOnly && !filterContext.RequestContext.HttpContext.Request.IsLocal)
      {
        filterContext.Result = new EmptyResult();
      }

      base.OnActionExecuting(filterContext);
    }
  }
}