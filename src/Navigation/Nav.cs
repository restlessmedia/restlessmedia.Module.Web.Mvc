using System;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace restlessmedia.Module.Web.Navigation
{
  public abstract class Nav
  {
    protected Nav(RouteContext routeContext, NavItem[] items)
    {
      RouteContext = routeContext ?? throw new ArgumentNullException(nameof(routeContext));
      Items = items ?? throw new ArgumentNullException(nameof(items));
    }

    protected Nav(ViewContext viewContext, NavItem[] items)
      : this(new RouteContext(viewContext), items) { }

    public virtual HtmlString Render(string className)
    {
      StringBuilder builder = new StringBuilder();

      builder.AppendFormat("<ul class=\"{0}\">", className);

      foreach (NavItem item in Items)
      {
        item.Render(builder, RouteContext);
      }

      builder.Append("</ul>");

      return new HtmlString(builder.ToString());
    }

    protected readonly RouteContext RouteContext;

    protected readonly NavItem[] Items;
  }
}