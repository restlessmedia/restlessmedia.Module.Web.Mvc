﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace restlessmedia.Module.Web.Asset
{
  public class AssetCollection
  {
    public AssetCollection(int capacity)
    {
      _assets = new List<AssetBase>(capacity);
    }

    public AssetCollection()
      : this(0) { }

    public void Add<T>(params T[] assets)
        where T : AssetBase
    {
      if (assets == null)
      {
        return;
      }

      foreach (T asset in assets)
      {
        _assets.Add(asset);
      }
    }

    public HtmlString Render<T>()
        where T : AssetBase
    {
      if (_assets.Count == 0)
      {
        return null;
      }

      return new HtmlString(string.Join(Environment.NewLine, _assets.OfType<T>().Reverse()));
    }

    private readonly IList<AssetBase> _assets;
  }
}