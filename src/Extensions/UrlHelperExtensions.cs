using System;

namespace restlessmedia.Module.Web.Mvc.Extensions
{
  public static class UrlHelperExtensions
  {
    [Obsolete]
    public static string ToFriendlyUrl(this UrlHelper urlHelper, string urlToEncode)
    {
      return UrlHelper.ToFriendlyUrl(urlToEncode);
    }
  }
}