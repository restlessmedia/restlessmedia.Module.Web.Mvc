using System.Collections.Generic;

namespace restlessmedia.Module.Web.Mvc.Configuration
{
  public interface IAssetSettings
  {
    string Version { get; }

    IEnumerable<IInclude> Includes { get; }

    string ResolvePath(string path);
  }
}