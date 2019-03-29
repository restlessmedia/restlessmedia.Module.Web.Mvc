using System.Web.Mvc;

namespace restlessmedia.Module.Web.Mvc.Extensions
{
  public static class UrlHelperExtensions
  {
    public static string ToFriendlyUrl(this UrlHelper urlHelper, string urlToEncode)
    {
      return Abstractions.UrlHelper.ToFriendlyUrl(urlToEncode);
    }
  }
}