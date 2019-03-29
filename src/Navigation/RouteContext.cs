using System;
using System.Web.Mvc;

namespace restlessmedia.Module.Web.Navigation
{
  public class RouteContext
  {
    public RouteContext(ViewContext viewContext)
    {
      if (viewContext == null)
      {
        throw new ArgumentNullException(nameof(viewContext));
      }

      CurrentController = viewContext.CurrentController();
      CurrentAction = viewContext.CurrentAction();
    }

    public bool IsHome
    {
      get
      {
        return CurrentController.Equals("Home") && CurrentAction.Equals("Index");
      }
    }

    public readonly string CurrentController;

    public readonly string CurrentAction;
  }
}
