using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace restlessmedia.Module.Web.Mvc
{
  [Serializable]
  public class GrowlCollection : ICollection<Growl>
  {
    public GrowlCollection(ControllerBase controller)
    {
      if (controller == null)
      {
        throw new ArgumentNullException(nameof(controller));
      }

      _store = controller.TempData[_key] as IList<Growl>;

      if (_store == null)
      {
        controller.TempData[_key] = _store = new List<Growl>(0);
      }
    }

    public void Add(Growl growl)
    {
      if (_store.Any(x => x.Message == growl.Message && x.Type == growl.Type))
      {
        return;
      }

      _store.Add(growl);
    }

    public void Clear()
    {
      _store.Clear();
    }

    public bool Contains(Growl item)
    {
      return _store.Contains(item);
    }

    public void CopyTo(Growl[] array, int arrayIndex)
    {
      _store.CopyTo(array, arrayIndex);
    }

    public bool Remove(Growl item)
    {
      return _store.Remove(item);
    }

    public IEnumerator<Growl> GetEnumerator()
    {
      return _store.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
      return GetEnumerator();
    }

    public static IEnumerable<T> Each<T>(ControllerBase controller, Func<Growl, T> fn)
    {
      return new GrowlCollection(controller).Select(x => fn(x));
    }

    /// <summary>
    /// This uses the view context to get around the dynamic type and lambda issue in views
    /// </summary>
    /// <param name="context"></param>
    /// <param name="doRender"></param>
    /// <returns></returns>
    public static MvcHtmlString RenderScripts(ControllerBase controller, Func<Growl, string> scriptFactory, bool includeScriptTags = true)
    {
      GrowlCollection collection = new GrowlCollection(controller);

      if (scriptFactory != null)
      {
        if (collection.Count > 0)
        {
          StringBuilder builder = new StringBuilder();

          if (includeScriptTags)
          {
            builder.AppendLine("<script type=\"text/javascript\">");
          }

          foreach (Growl growl in collection)
          {
            builder.AppendLine(scriptFactory(growl));
          }

          // reset
          collection.Clear();

          if (includeScriptTags)
          {
            builder.AppendLine("</script>");
          }

          return MvcHtmlString.Create(builder.ToString());
        }
      }

      return null;
    }

    public int Count
    {
      get
      {
        return _store.Count;
      }
    }

    public bool IsReadOnly
    {
      get
      {
        return false;
      }
    }

    public static string ToString(ControllerBase controller, Func<Growl, string> fn)
    {
      const string separator = ";";
      return string.Join(separator, new GrowlCollection(controller).Select(x => fn(x)));
    }

    private readonly IList<Growl> _store;

    private const string _key = "growls";
  }
}