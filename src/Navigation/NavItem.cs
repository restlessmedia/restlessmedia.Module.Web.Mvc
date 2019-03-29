using System;
using System.Text;

namespace restlessmedia.Module.Web.Navigation
{
  public abstract class NavItem
  {
    protected NavItem(string title, string url, string controller, string action)
    {
      if (string.IsNullOrEmpty(title))
      {
        throw new ArgumentNullException(nameof(title));
      }

      if (string.IsNullOrEmpty(url))
      {
        throw new ArgumentNullException(nameof(url));
      }

      if (string.IsNullOrEmpty(controller))
      {
        throw new ArgumentNullException(nameof(controller));
      }

      if (string.IsNullOrEmpty(action))
      {
        throw new ArgumentNullException(nameof(action));
      }

      Title = title;
      Url = url;
      Action = action;
      Controller = controller;
    }

    protected NavItem(string title, string url, string action)
      : this(title, url, DefaultController, action) { }

    public virtual bool IsActive(RouteContext routeContext)
    {
      return string.Compare(Controller, routeContext.CurrentController, true) == 0 && string.Compare(Action, routeContext.CurrentAction, true) == 0;
    }

    public virtual void Render(StringBuilder builder, RouteContext routeContext)
    {
      if (!IsHome || (IsHome && !routeContext.IsHome))
      {
        builder.AppendFormat("<a class=\"{0}\" href=\"{1}\">{2}</a>", GetClassName(routeContext), Url, Title);
      }
    }

    public virtual string GetClassName(RouteContext routeContext)
    {
      return IsActive(routeContext) ? "navbar-item is-active" : "navbar-item";
    }

    protected bool IsHome
    {
      get
      {
        return this is Homepage;
      }
    }

    protected readonly string Title;

    protected readonly string Url;

    protected readonly string Action;

    protected readonly string Controller;

    protected const string DefaultController = "Home";

    protected const string VoidUrl = "#";
  }
}
