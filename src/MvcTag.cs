using System;
using System.IO;
using System.Web.Mvc;
using System.Web.UI;

namespace restlessmedia.Module.Web.Mvc
{
  public class MvcTag : IDisposable
  {
    #region Public Members

    public MvcTag(ViewContext viewContext, HtmlTextWriterTag tag)
    {
      _viewContext = viewContext ?? throw new ArgumentNullException(nameof(viewContext));
      _writer = viewContext.Writer;
      _tag = tag;
    }

    public TagBuilder Builder
    {
      get
      {
        return _builder = _builder ?? new TagBuilder(_tag.ToString());
      }
    }

    public void Dispose()
    {
      Dispose(true /* disposing */);
      GC.SuppressFinalize(this);
    }

    public void EndTag()
    {
      Dispose(true);
    }

    public MvcHtmlString MvcHtmlString
    {
      get
      {
        return MvcHtmlString.Create(Builder.ToString());
      }
    }

    public void AddChildTag(MvcTag tag)
    {
      Builder.InnerHtml = string.Concat(Builder.InnerHtml, tag.Builder.ToString());
    }

    #endregion

    #region Protected Members

    protected virtual void Dispose(bool disposing)
    {
      if (!_disposed)
      {
        _disposed = true;

        // TODO: work out if the tag is self closing or not from the tag name
        _writer.Write($"</{_tag}>");
      }
    }

    #endregion

    #region Private Members

    private bool _disposed;

    private readonly ViewContext _viewContext;

    private readonly TextWriter _writer;

    private readonly HtmlTextWriterTag _tag;

    private TagBuilder _builder = null;

    #endregion
  }
}