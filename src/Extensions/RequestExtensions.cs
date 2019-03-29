using System.Linq;
using System.Web.Routing;

namespace restlessmedia.Module.Web.Mvc.Extensions
{
  public static class RequestExtensions
  {
    public static bool IsCurrentRoute(this RequestContext context, string areaName, string controllerName, params string[] actionNames)
    {
      RouteData routeData = context.RouteData;
      string routeArea = routeData.DataTokens["area"] as string;
      bool current = false;

      if (((string.IsNullOrEmpty(routeArea) && string.IsNullOrEmpty(areaName)) || (routeArea == areaName)) && ((string.IsNullOrEmpty(controllerName)) || (routeData.GetRequiredString("controller") == controllerName)) && ((actionNames == null) || actionNames.Contains(routeData.GetRequiredString("action"))))
      {
        current = true;
      }

      return current;
    }
  }
}