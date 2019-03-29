using System;

namespace restlessmedia.Module.Web.Mvc.Attributes
{
  public class LifeCacheAttribute : HttpCacheAttribute
  {
    public LifeCacheAttribute(int hours)
    {
      Options.MaxAge = TimeSpan.FromHours(hours);
    }
  }
}
