using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;

namespace restlessmedia.Module.Web.Mvc.Asset
{
  public static class Cache
  {
    public static void Asset<T>(WebViewPage viewPage)
        where T : AssetBase
    {
      if (ContainsAsset<T>(viewPage))
      {
        return;
      }

      lock (_assetLock)
      {
        if (!ContainsAsset<T>(viewPage))
        {
          _assetStore.Add(GetKey<T>(viewPage));
        }
      }
    }

    public static HtmlString Render<T>(WebViewPage viewPage)
        where T : AssetBase
    {
      string key = GetKey<T>(viewPage);

      if (!ContainsRender<T>(viewPage))
      {
        lock (_renderLock)
        {
          if (!ContainsRender<T>(viewPage))
          {
            _renderStore.Add(key, Assets.Render<T>());
          }
        }
      }

      return _renderStore[key];
    }

    public static bool ContainsAsset<T>(WebViewPage viewPage)
       where T : AssetBase
    {
      return _assetStore.Contains(GetKey<T>(viewPage));
    }

    public static bool ContainsRender<T>(WebViewPage viewPage)
        where T : AssetBase
    {
      return _renderStore.ContainsKey(GetKey<T>(viewPage));
    }

    public static AssetCollection Assets
    {
      get
      {
        const string key = "__assets__";

        HttpContext context = HttpContext.Current;
        AssetCollection assets = context.Items[key] as AssetCollection;

        if (assets == null)
        {
          context.Items[key] = assets = new AssetCollection(0);
        }

        return assets;
      }
    }

    public static void Clear()
    {
      lock (_renderLock)
      {
        _renderStore.Clear();
      }

      lock (_assetLock)
      {
        _assetStore.Clear();
      }
    }

    private static string GetKey<T>(WebViewPage viewPage)
        where T : AssetBase
    {
      string typeName = typeof(T).Name;
      const string separator = "_";
      BuildManagerCompiledView compiledView = viewPage.ViewContext.View as BuildManagerCompiledView;
      string path = compiledView != null ? compiledView.ViewPath : viewPage.Request.Url.LocalPath;
      return string.Concat(typeName, separator, path);
    }

    private static object _renderLock = new object();

    private static object _assetLock = new object();

    private static IDictionary<string, HtmlString> _renderStore = new Dictionary<string, HtmlString>(0);

    private static IList<string> _assetStore = new List<string>();
  }
}