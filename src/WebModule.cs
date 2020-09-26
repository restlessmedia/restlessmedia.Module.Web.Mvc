using Autofac;
using Autofac.Integration.Mvc;
using restlessmedia.Module.Web.Mvc.Binders;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.WebPages;

namespace restlessmedia.Module.Web.Mvc
{
  public class WebModule : WebModuleBase
  {
    public override void OnStart(HttpConfiguration httpConfiguration, ContainerBuilder builder, IEnumerable<IWebModule> webModules)
    {
      builder.RegisterFilterProvider();

      ViewEngines.Engines.Clear();
      ViewEngines.Engines.Add(new CustomViewEngine());

      ModelBinders.Binders.DefaultBinder = new SmartModelBinder();

      DisplayModeProvider.Instance.Modes.Clear();
      DisplayModeProvider.Instance.Modes.Add(new DefaultDisplayMode());

      ModelMetadataProviders.Current = new GenericModelMetadataProvider();
      
      // register controllers from all loaded wed modules
      builder.RegisterControllers(webModules.Select(x => x.GetType().Assembly).ToArray());

      RouteTable.Routes.MapMvcAttributeRoutes();
    }

    public override void OnStart(HttpConfiguration httpConfiguration, IContainer container, IEnumerable<IWebModule> webModules)
    {
      DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
    }

    public override void OnStarted(HttpConfiguration httpConfiguration, IContainer container, IEnumerable<IWebModule> webModules)
    {
      // default route
      RouteTable.Routes.MapRoute(
          "Default",
          "{controller}/{action}/{id}",
          new { controller = "Home", action = "Index", id = UrlParameter.Optional }
      );

      // this ensures the less specific routes get registered last
      RouteTable.Routes.IgnoreRoute("robots.txt");
      RouteTable.Routes.IgnoreRoute("sitemap");
      RouteTable.Routes.IgnoreRoute("sitemap.gz");
      RouteTable.Routes.IgnoreRoute("sitemap.xml");
      RouteTable.Routes.IgnoreRoute("sitemap.xml.gz");
      RouteTable.Routes.IgnoreRoute("google_sitemap.xml");
      RouteTable.Routes.IgnoreRoute("google_sitemap.xml.gz");
      RouteTable.Routes.IgnoreRoute("favicon.ico");
      RouteTable.Routes.IgnoreRoute("{*allfiles}", new { allfiles = @".*\.(css|js|php|aspx|axd|jpg)" });
    }
  }
}