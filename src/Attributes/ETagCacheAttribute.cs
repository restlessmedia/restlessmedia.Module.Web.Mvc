using System;
using System.Web.Mvc;

namespace restlessmedia.Module.Web.Mvc
{
  public class ETagCacheAttribute : HttpCacheAttribute
  {
    public ETagCacheAttribute()
    {
      Options.IsPublic = true;
    }

    public ETagCacheAttribute(string eTag)
        : this()
    {
      ETag = eTag;
    }

    public ETagCacheAttribute(DateTime date)
        : this()
    {
      ETagDate = date;
    }

    public override void OnActionExecuting(ActionExecutingContext filterContext)
    {
      if (Options.ETagMatches(filterContext.HttpContext.Request))
      {
        filterContext.Result = NotModified();
      }
      else
      {
        base.OnActionExecuting(filterContext);
      }
    }

    protected string ETag
    {
      get
      {
        return Options.ETag;
      }
      set
      {
        Options.ETag = value;
      }
    }

    protected DateTime? ETagDate
    {
      get
      {
        return Options.LastModified;
      }
      set
      {
        ETag = value.HasValue ? value.Value.Ticks.ToString() : null;
        Options.LastModified = value;
      }
    }
  }
}