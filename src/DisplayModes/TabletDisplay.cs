namespace restlessmedia.Module.Web.Mvc.DisplayModes
{
  public class TabletDisplay : MobileDisplay
  {
    public TabletDisplay()
      : this(600, 1024) { }

    public TabletDisplay(int minWidth, int maxWidth, string suffix = "tablet")
      : base(minWidth, maxWidth, suffix) { }
  }
}