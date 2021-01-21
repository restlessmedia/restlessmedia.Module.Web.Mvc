using System;
using System.Diagnostics;
using System.Linq;
using System.Web.Mvc;

namespace restlessmedia.Module.Web.Mvc.Binders
{
  public class FlagEnumerationModelBinder : DefaultModelBinder, IFilteredModelBinder
  {
    public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
    {
      ValueProviderResult valueProviderResult = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);

      if (valueProviderResult != null && valueProviderResult.RawValue != null)
      {
        string enumValue = null;

        try
        {
          if (valueProviderResult.RawValue is string)
          {
            enumValue = (string)valueProviderResult.RawValue;
          }
          else if (valueProviderResult.RawValue is string[])
          {
            enumValue = GetEnumValue(bindingContext.ModelType, (string[])valueProviderResult.RawValue);
          }

          if (!string.IsNullOrEmpty(enumValue))
          {
            return Enum.Parse(bindingContext.ModelType, enumValue);
          }
        }
        catch (Exception e)
        {
          Trace.TraceWarning($"{nameof(FlagEnumerationModelBinder)} parse failed with value '{valueProviderResult.RawValue}' for model {bindingContext.ModelName}. {e}");
        }
      }

      return base.BindModel(controllerContext, bindingContext);
    }

    public bool IsMatch(Type modelType)
    {
      return modelType.IsEnum && modelType.IsDefined(typeof(FlagsAttribute), false);
    }

    private static string GetEnumValue(Type enumType, string[] rawValues)
    {
      try
      {
        return Array.ConvertAll(rawValues, int.Parse)
          .Where(value => Enum.IsDefined(enumType, value))
          .Aggregate((value, result) => result |= value).ToString();
      }
      catch
      {
        return string.Join(",", rawValues);
      }
    }
  }
}