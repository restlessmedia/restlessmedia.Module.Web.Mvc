using restlessmedia.Module.Web.Configuration;
using System.Collections.Generic;

namespace restlessmedia.Module.Web.Mvc.Asset
{
  public class JsAsset : AssetBase
  {
    public JsAsset(IAssetSettings settings, string src, bool defer = false)
      : base(settings, _tagName, src, true)
    {
      _attributes = new Dictionary<string, string>
      {
        { "type", "text/javascript" }
      };

      if (defer)
      {
        _attributes.Add("defer", "defer");
      }
    }

    protected override string GetPathAttribute(string resolvedPath)
    {
      return string.Concat("src=\"", resolvedPath, "\"");
    }

    public override IDictionary<string, string> Attributes
    {
      get
      {
        return _attributes;
      }
    }

    private readonly IDictionary<string, string> _attributes;

    private const string _tagName = "script";
  }
}