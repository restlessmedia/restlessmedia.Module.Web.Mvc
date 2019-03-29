using System.Linq;
using System.Text;

namespace restlessmedia.Module.Web.Navigation
{
  public abstract class Menu : NavItem
  {
    protected Menu(string title, string url, string controller, string action, params NavItem[] children)
      : base(title, url, controller, action)
    {
      Children = children;
    }

    protected Menu(string title, string url, string action, params NavItem[] children)
      : this(title, url, DefaultController, action, children) { }

    public override bool IsActive(RouteContext routeContext)
    {
      return base.IsActive(routeContext) || Children.Any(x => x.IsActive(routeContext));
    }

    public override void Render(StringBuilder builder, RouteContext routeContext)
    {
      builder.AppendFormat("<li class=\"{0}\">", GetClassName(routeContext));
      builder.Append(Title);
      builder.Append("<ul>");

      foreach (NavItem child in Children)
      {
        child.Render(builder, routeContext);
      }

      builder.Append("</ul>");
      builder.Append("</li>");
    }

    public readonly NavItem[] Children;
  }
}