using System.Web.WebPages;

namespace restlessmedia.Module.Web.Mvc.DisplayModes
{
  public class MobileDisplay : DefaultDisplayMode
  {
    public MobileDisplay()
      : this(320, 640) { }

    public MobileDisplay(int minWidth, int maxWidth, string suffix = "mobile")
      : base(suffix)
    {
      _minWidth = minWidth;
      _maxWidth = maxWidth;

      ContextCondition = (httpContext) =>
      {
        if (httpContext.Request.Browser.IsMobileDevice)
        {
          int width = httpContext.Request.Browser.ScreenPixelsWidth;
          return width >= _minWidth && width <= _maxWidth;
        }
        else
        {
          return false;
        }
      };
    }

    private int _minWidth;

    private int _maxWidth;
  }
}
