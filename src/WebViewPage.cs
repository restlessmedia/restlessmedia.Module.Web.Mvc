using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Text.RegularExpressions;
using restlessmedia.Module.Web.Helper;
using restlessmedia.Module.Category;
using restlessmedia.Module.Web.Asset;
using restlessmedia.Module.Security;

namespace restlessmedia.Module.Web.Mvc
{
  public abstract class WebViewPage<TModel> : System.Web.Mvc.WebViewPage<TModel>
  {
    public WebViewPage()
      : base()
    {
      ViewDataContainer = new ViewDataContainer(ViewData);
    }

    protected LocationHelper LocationHelper
    {
      get
      {
        return _locationHelper = _locationHelper ?? new LocationHelper(UIContext.FileSettings, UIContext.File);
      }
    }

    protected UrlHelper UrlHelper
    {
      get
      {
        return _urlHelper = _urlHelper ?? new UrlHelper();
      }
    }

    protected virtual Helper.HtmlHelper<TModel> HtmlHelper
    {
      get
      {

        return _html = _html ?? new Helper.HtmlHelper<TModel>(UIContext, RoleService, CategoryService, ViewContext, ViewDataContainer);
      }
    }

    protected IUIContext UIContext
    {
      get
      {
        return Resolve<IUIContext>();
      }
    }

    protected IViewDataContainer ViewDataContainer;

    protected IRoleService RoleService
    {
      get
      {
        return Resolve<IRoleService>();
      }
    }

    protected ICategoryService CategoryService
    {
      get
      {
        return Resolve<ICategoryService>();
      }
    }

    protected void IncludeCss(params string[] paths)
    {
      Include((path) => new CssAsset(UIContext.AssetSettings, path), paths);
    }

    protected void IncludeJs(params string[] paths)
    {
      IncludeJs(false, paths);
    }

    protected void IncludeJs(bool defer, params string[] paths)
    {
      Include((path) => new JsAsset(UIContext.AssetSettings, path, defer), paths);
    }

    protected HtmlString RenderCss()
    {
      return Render<CssAsset>();
    }

    protected HtmlString RenderJs()
    {
      return Render<JsAsset>();
    }

    protected HtmlString Linkify(string value)
    {
      if (string.IsNullOrWhiteSpace(value))
      {
        return null;
      }

      const string pattern = @"https?:\/\/(www\.)?[-a-zA-Z0-9@:%._\+~#=]{2,256}\.[a-z]{2,6}\b([-a-zA-Z0-9@:%_\+.~#?&//=]*)";

      return new HtmlString(Regex.Replace(value, pattern, x => $"<a href=\"{x.Value}\">x.Value</a>", RegexOptions.IgnoreCase | RegexOptions.Multiline));
    }

    private HtmlString Render<T>()
        where T : AssetBase
    {
      return Assets.Render<T>();
    }

    private void Include<T>(Func<string, T> factory, params string[] paths)
        where T : AssetBase
    {
      foreach (string path in paths.Reverse())
      {
        if (string.IsNullOrWhiteSpace(path))
        {
          continue;
        }
        Assets.Add(factory(path));
      }
    }

    protected T Resolve<T>()
    {
      return DependencyResolver.Current.GetService<T>();
    }

    private AssetCollection Assets
    {
      get
      {
        const string key = "assets";
        AssetCollection assets = Context.Items[key] as AssetCollection;

        if (assets == null)
        {
          Context.Items[key] = assets = new AssetCollection();
        }

        return assets;
      }
    }

    private Helper.HtmlHelper<TModel> _html = null;

    private LocationHelper _locationHelper;

    private UrlHelper _urlHelper;
  }
}