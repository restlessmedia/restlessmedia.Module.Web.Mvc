using System.Web.Mvc;

namespace restlessmedia.Module.Web.Mvc.Extensions
{
  public static class ViewContextExtensions
  {
    public static string CurrentAction(this ViewContext viewContext)
    {
      return viewContext.RouteData != null && viewContext.RouteData.Values.ContainsKey(_actionRouteKey) ? viewContext.RouteData.Values[_actionRouteKey].ToString() : null;
    }

    public static string CurrentController(this ViewContext viewContext)
    {
      return viewContext.RouteData != null && viewContext.RouteData.Values.ContainsKey(_controllerRouteKey) ? viewContext.RouteData.Values[_controllerRouteKey].ToString() : null;
    }

    private const string _actionRouteKey = "action";

    private const string _controllerRouteKey = "controller";
  }
}