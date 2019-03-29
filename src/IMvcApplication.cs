using restlessmedia.Module.Web.Mvc.Binders;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reflection;
using System.Web.Mvc;
using System.Web.WebPages;

namespace restlessmedia.Module.Web.Mvc
{
  public interface IMvcApplication
  {
    void RegisterBinders(params IFilteredModelBinder[] binders);

    void RegisterViewEngines(Collection<IViewEngine> engines);

    void RegisterDisplayModes(IList<IDisplayMode> modes);

    void RegisterNamespaces(HashSet<string> namespaces);

    void RegisterAssemblies(params Assembly[] assemblies);
  }
}