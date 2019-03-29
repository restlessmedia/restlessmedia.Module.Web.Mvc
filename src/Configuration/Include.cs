using System.Configuration;

namespace restlessmedia.Module.Web.Mvc.Configuration
{
  internal class Include : ConfigurationElement, IInclude
  {
    [ConfigurationProperty(_srcProperty, IsRequired = true, IsKey = true)]
    public string Src
    {
      get
      {
        return (string)this[_srcProperty];
      }
      set
      {
        this[_srcProperty] = value;
      }
    }

    [ConfigurationProperty(_aliasProperty, IsRequired = false)]
    public string Alias
    {
      get
      {
        return (string)this[_aliasProperty];
      }
      set
      {
        this[_aliasProperty] = value;
      }
    }

    [ConfigurationProperty(_versionProperty, IsRequired = false)]
    public string Version
    {
      get
      {
        return (string)this[_versionProperty];
      }
      set
      {
        this[_versionProperty] = value;
      }
    }

    private const string _srcProperty = "name";

    private const string _aliasProperty = "alias";

    private const string _versionProperty = "version";
  }
}