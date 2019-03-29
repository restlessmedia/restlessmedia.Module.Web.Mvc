using System;
using System.Linq;
using System.Web.Mvc;

namespace restlessmedia.Module.Web.Mvc.Binders
{
  public class FlagEnumerationModelBinder : DefaultModelBinder, IFilteredModelBinder
  {
    public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
    {
      ValueProviderResult value = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);

      if (value == null)
      {
        return base.BindModel(controllerContext, bindingContext);
      }

      string[] rawValues = value.RawValue as string[];

      if (rawValues == null)
      {
        return base.BindModel(controllerContext, bindingContext);
      }

      try
      {
        return Parse(bindingContext, rawValues);
      }
      catch
      {
        return base.BindModel(controllerContext, bindingContext);
      }
    }

    public bool IsMatch(Type modelType)
    {
      return modelType.IsEnum && modelType.IsDefined(typeof(FlagsAttribute), false);
    }

    private static Enum Parse(ModelBindingContext bindingContext, string[] rawValues)
    {
      Type valueType = bindingContext.ModelType;

      try
      {
        return Parse(valueType, Array.ConvertAll(rawValues, int.Parse));
      }
      catch
      {
        return Parse(valueType, rawValues);
      }
    }

    private static Enum Parse(Type type, string[] names)
    {
      const string comma = ",";
      string value = string.Join(comma, names);
      return (Enum)Enum.Parse(type, value);
    }

    private static Enum Parse(Type type, int[] values)
    {
      int value = 0;
      foreach (int v in values.Where(v => Enum.IsDefined(type, v)))
      {
        value |= v;
      }
      return (Enum)Enum.Parse(type, value.ToString());
    }
  }
}