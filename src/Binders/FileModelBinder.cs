using restlessmedia.Module.Web.Mvc.Model;
using System;
using System.Web.Mvc;

namespace restlessmedia.Module.Web.Mvc.Binders
{
  public class FileModelBinder : HttpPostedFileBaseModelBinder, IFilteredModelBinder
  {
    public new object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
    {
      // the model
      FileModel model = bindingContext.Model as FileModel ?? new FileModel();

      // set the file
      model.File = controllerContext.HttpContext.Request.Files[string.Concat(bindingContext.ModelName, ".File")];

      // src
      model.Src = controllerContext.HttpContext.Request[string.Concat(bindingContext.ModelName, ".Src")];

      // file id
      string fileIdValue = controllerContext.HttpContext.Request[string.Concat(bindingContext.ModelName, ".Id")];
      
      if (!string.IsNullOrEmpty(fileIdValue) && int.TryParse(fileIdValue, out int fileId))
      {
        model.Id = fileId;
      }

      return model;
    }

    public bool IsMatch(Type modelType)
    {
      return typeof(FileModel).IsAssignableFrom(modelType);
    }
  }
}