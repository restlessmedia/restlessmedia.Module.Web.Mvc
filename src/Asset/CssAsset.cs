using restlessmedia.Module.Web.Configuration;
using System.Collections.Generic;

namespace restlessmedia.Module.Web.Mvc.Asset
{
  public class CssAsset : AssetBase
  {
    public CssAsset(IAssetSettings settings, string href)
      : base(settings, _tagName, href, false, new Dictionary<string, string>(_defaultAttributes)) { }

    protected override string GetPath(string resolvedPath)
    {
      return $"href=\"{resolvedPath}\"";
    }

    private static readonly IDictionary<string, string> _defaultAttributes = new Dictionary<string, string>
    {
      { "rel", "stylesheet" }
    };

    private const string _tagName = "link";
  }
}