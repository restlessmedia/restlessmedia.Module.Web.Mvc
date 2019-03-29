using System;
using System.Web;
using System.Web.Mvc;

namespace restlessmedia.Module.Web.Mvc.Filters
{
  public class CacheFilterAttribute : ActionFilterAttribute
  {
    public CacheFilterAttribute(int duration)
    {
      Duration = duration;
    }

    public CacheFilterAttribute()
      : this(DefaultDuration) { }

    public override void OnActionExecuted(ActionExecutedContext filterContext)
    {
      if (filterContext.Exception != null || Duration <= 0)
      {
        return;
      }

      HttpCachePolicyBase cache = filterContext.HttpContext.Response.Cache;
      TimeSpan cacheDuration = TimeSpan.FromSeconds(Duration);

      cache.SetCacheability(HttpCacheability.Public);
      cache.SetExpires(DateTime.Now.Add(cacheDuration));
      cache.SetMaxAge(cacheDuration);
      cache.AppendCacheExtension("must-revalidate, proxy-revalidate");
    }

    /// <summary>
    /// Gets or sets the cache duration in seconds. The default is DefaultDuration seconds.
    /// </summary>
    /// <value>The cache duration in seconds.</value>
    public int Duration { get; set; }

    public const int DefaultDuration = 60 * 60;
  }
}