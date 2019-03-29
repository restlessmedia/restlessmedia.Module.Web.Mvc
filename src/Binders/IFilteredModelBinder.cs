using System;
using System.Web.Mvc;

namespace restlessmedia.Module.Web.Mvc.Binders
{
  public interface IFilteredModelBinder : IModelBinder
  {
    bool IsMatch(Type modelType);
  }
}