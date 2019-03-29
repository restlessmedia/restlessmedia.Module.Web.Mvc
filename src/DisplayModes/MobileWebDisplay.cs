using System.Collections.Generic;
using System.Web.WebPages;

namespace restlessmedia.Module.Web.Mvc.DisplayModes
{
  public class MobileWebDisplay : List<IDisplayMode>
  {
    public MobileWebDisplay()
    {
      Register(this);
    }

    public static void Register(IList<IDisplayMode> modes)
    {
      modes.Clear();
      // these have to be in this order as the table range could overwrite the mobile one
      modes.Add(new MobileDisplay());
      modes.Add(new TabletDisplay());
      modes.Add(new DefaultDisplayMode());
    }
  }
}