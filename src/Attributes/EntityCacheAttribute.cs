using System.Web.Mvc;
using System.Web.Routing;

namespace restlessmedia.Module.Web.Mvc.Attributes
{
  public class EntityCacheAttribute : ETagCacheAttribute
  {
    public EntityCacheAttribute()
      : base()
    { }

    public override void OnActionExecuting(ActionExecutingContext filterContext)
    {
      int key;

      if (TryGetKey(filterContext.RouteData.Values, out key))
      {
        ETagDate = Resolve<IEntityService>().UpdatedDate(Type, key);
      }

      base.OnActionExecuting(filterContext);
    }

    public EntityType Type { get; set; }

    public string KeyName { get; set; }

    private bool TryGetKey(RouteValueDictionary routeValues, out int key)
    {
      key = 0;

      if (string.IsNullOrEmpty(KeyName) || !routeValues.ContainsKey(KeyName) || routeValues[KeyName] == null)
      {
        return false;
      }

      return int.TryParse(routeValues[KeyName].ToString(), out key);
    }
  }
}