using System.Configuration;

namespace restlessmedia.Module.Web.Mvc.Configuration
{
  internal class IncludeCollection : ConfigurationElementCollection
  {
    public Include this[int index]
    {
      get
      {
        return (Include)BaseGet(index);
      }
      set
      {
        if (BaseGet(index) != null)
        {
          BaseRemoveAt(index);
        }

        BaseAdd(index, value);
      }
    }

    public void Add(Include include)
    {
      BaseAdd(include);
    }

    public void Clear()
    {
      BaseClear();
    }

    protected override ConfigurationElement CreateNewElement()
    {
      return new Include();
    }

    protected override object GetElementKey(ConfigurationElement element)
    {
      return ((Include)element).Src;
    }

    public void Remove(Include include)
    {
      BaseRemove(include.Src);
    }

    public void RemoveAt(int index)
    {
      BaseRemoveAt(index);
    }

    public void Remove(string name)
    {
      BaseRemove(name);
    }
  }
}
