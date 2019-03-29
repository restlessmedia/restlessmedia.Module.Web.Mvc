using System.Web.Mvc;

namespace restlessmedia.Module.Web.Mvc
{
  public class CustomViewEngine : RazorViewEngine
  {
    public CustomViewEngine()
    {
      PartialViewLocationFormats = new string[1]
      {
        "~/Views/Shared/{0}.cshtml"
      };

      ViewLocationFormats = new string[2]
      {
        "~/Views/{1}/{0}.cshtml",
        "~/Views/Shared/{0}.cshtml"
      };
    }
  }
}