using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace restlessmedia.Module.Web.Mvc.Configuration
{
  internal class AssetSettings : ConfigurationSection, IAssetSettings
  {
    [ConfigurationProperty(_includesProperty, IsDefaultCollection = false)]
    [ConfigurationCollection(typeof(IncludeCollection), AddItemName = "add", ClearItemsName = "clear", RemoveItemName = "remove")]
    public IncludeCollection IncludesCollection
    {
      get
      {
        return (IncludeCollection)base[_includesProperty];
      }
    }

    [ConfigurationProperty(_versionProperty)]
    public string Version
    {
      get
      {
        return (string)base[_versionProperty];
      }
    }

    public IEnumerable<IInclude> Includes
    {
      get
      {
        return IncludesCollection.Cast<IInclude>();
      }
    }

    public string ResolvePath(string path)
    {
      IInclude include = FindInclude(path);

      if (include != null)
      {
        return VersionPath(include.Src, include.Version ?? Version);
      }

      return VersionPath(path, Version);
    }
    
    private IInclude FindInclude(string path)
    {
      return Includes.FirstOrDefault(x => string.Equals(x.Alias, path) || string.Equals(x.Src, path));
    }

    private static string VersionPath(string path, string version)
    {
      if (string.IsNullOrWhiteSpace(path) || string.IsNullOrWhiteSpace(version))
      {
        return path;
      }

      if (path.StartsWith("http", System.StringComparison.OrdinalIgnoreCase))
      {
        return path;
      }

      if (path.IndexOf("?") == -1)
      {
        return string.Concat(path, "?v=", version);
      }

      return string.Concat(path, "&v=", version);
    }

    private const string _includesProperty = "includes";

    private const string _versionProperty = "version";
  }
}
