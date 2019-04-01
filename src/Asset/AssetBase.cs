using restlessmedia.Module.Web.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace restlessmedia.Module.Web.Mvc.Asset
{
  public abstract class AssetBase
  {
    public AssetBase(IAssetSettings configuration, string tagName, string path, bool hasClosingTag, IDictionary<string, string> attributes = null)
    {
      if (string.IsNullOrWhiteSpace(tagName))
      {
        throw new ArgumentNullException(nameof(tagName));
      }

      if (string.IsNullOrWhiteSpace(path))
      {
        throw new ArgumentNullException(nameof(path));
      }

      _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
      TagName = tagName;
      Path = path;
      HasClosingTag = hasClosingTag;
      Attributes = attributes ?? new Dictionary<string, string>(0);
    }

    public HtmlString ToHtmlString()
    {
      return new HtmlString(ToString());
    }

    public override string ToString()
    {
      string close = HasClosingTag ? $"></{TagName}>" : "/>";
      return $"<{TagName} {AttributesToString()} {GetPath(_configuration.ResolvePath(Path))} {close}";
    }

    public virtual IDictionary<string, string> Attributes { get; private set; }

    protected readonly string Path;

    protected abstract string GetPath(string resolvedPath);

    private string AttributesToString()
    {
      const string separator = " ";
      return string.Join(separator, Attributes.Select(x => $"{x.Key}=\"{x.Value}\""));
    }

    private readonly string TagName;

    private readonly bool HasClosingTag;

    private readonly IAssetSettings _configuration;
  }
}