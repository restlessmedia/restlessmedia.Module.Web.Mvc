using System.Web.Mvc;

namespace restlessmedia.Module.Web.Mvc
{
  public class ViewDataContainer : IViewDataContainer
  {
    public ViewDataContainer(ViewDataDictionary viewData)
    {
      ViewData = viewData;
    }

    public ViewDataDictionary ViewData { get; set; }
  }
}