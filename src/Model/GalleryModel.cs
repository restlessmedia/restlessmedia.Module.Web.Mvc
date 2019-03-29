using System;
using System.Collections.Generic;

namespace restlessmedia.Module.Web.Mvc.Model
{
  public class GalleryModel
  {
    public GalleryModel(IEnumerable<string> srcCollection)
    {
      SrcCollection = srcCollection;
    }

    public string Title { get; set; }

    public Func<string, string> ThumbSrcFactory { get; set; }

    public Func<string, string> SrcFactory { get; set; }

    public IEnumerable<string> SrcCollection { get; private set; }
  }
}