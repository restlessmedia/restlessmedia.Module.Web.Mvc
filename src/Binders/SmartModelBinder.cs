using restlessmedia.Module.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web.Mvc;

namespace restlessmedia.Module.Web.Mvc.Binders
{
  public class SmartModelBinder : DefaultModelBinder
  {
    public SmartModelBinder(params IFilteredModelBinder[] binders)
    {
      IFilteredModelBinder[] defaultBinders = new IFilteredModelBinder[2]
      {
        new FileModelBinder(),
        new FlagEnumerationModelBinder()
      };

      _modelBinders = binders == null || binders.Length == 0 ? defaultBinders : defaultBinders.Union(binders);
    }

    public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
    {
      foreach (IFilteredModelBinder binder in _modelBinders)
      {
        if (binder.IsMatch(bindingContext.ModelType))
        {
          return binder.BindModel(controllerContext, bindingContext);
        }
      }

      return base.BindModel(controllerContext, bindingContext);
    }

    protected override void BindProperty(ControllerContext controllerContext, ModelBindingContext bindingContext, PropertyDescriptor propertyDescriptor)
    {
      base.BindProperty(controllerContext, bindingContext, propertyDescriptor);
    }

    protected override object GetPropertyValue(ControllerContext controllerContext, ModelBindingContext bindingContext, PropertyDescriptor propertyDescriptor, IModelBinder propertyBinder)
    {
      BindAsAttribute bindAs = GetBindAsAttribute(controllerContext, propertyDescriptor);

      if (bindAs != null)
      {
        propertyDescriptor = BindAsProperty(bindingContext, propertyDescriptor, bindAs.ModelType);
      }

      return base.GetPropertyValue(controllerContext, bindingContext, propertyDescriptor, propertyBinder);
    }

    protected override PropertyDescriptor BindAsProperty(ModelBindingContext bindingContext, PropertyDescriptor propertyDescriptor, Type modelType)
    {
      bindingContext.ModelMetadata = ModelMetadataProviders.Current.GetMetadataForType(null, modelType);
      return GetPropertyDescriptor(propertyDescriptor, modelType);
    }

    private static PropertyDescriptor GetPropertyDescriptor(PropertyDescriptor propertyDescriptor, Type modelType)
    {
      if (!propertyDescriptor.PropertyType.IsAssignableFrom(modelType))
      {
        throw IncompatibleException(propertyDescriptor.PropertyType, modelType);
      }

      return new PropertyDescriptorWrapper(propertyDescriptor, modelType);
    }

    private static BindAsAttribute GetBindAsAttribute(ControllerContext controllerContext, PropertyDescriptor propertyDescriptor)
    {
      return propertyDescriptor.Attributes.OfType<BindAsAttribute>().FirstOrDefault();
    }

    private static Exception IncompatibleException(Type propertyType, Type modelType)
    {
      return new InvalidOperationException($"The BindAs type {modelType} is incompatible with the property type {propertyType}.");
    }

    private readonly IEnumerable<IFilteredModelBinder> _modelBinders;
  }
}