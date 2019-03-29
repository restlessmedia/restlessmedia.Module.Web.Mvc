using System;
using System.ComponentModel;
using System.Linq;
using System.Web.Mvc;

namespace restlessmedia.Module.Web.Mvc
{
  /// <summary>
  /// Provides support for interface inheritance when accessing model properties
  /// </summary>
  /// <remarks>
  /// Fixes bug where model property uses interface which inherits another interface.  The previous provider would fail when accessing the property through razor.
  /// Model
  /// {
  ///     IFoo Foo;
  /// }
  /// IFoo
  /// {
  ///     IBar Bar
  /// }
  /// i.e. TextBoxFor(m => m.Foo.Bar)
  /// </remarks>
  public class GenericModelMetadataProvider : DataAnnotationsModelMetadataProvider
  {
    public override ModelMetadata GetMetadataForProperty(Func<object> modelAccessor, Type containerType, string propertyName)
    {
      PropertyDescriptor property = GetPropertyDescriptor(containerType, propertyName);
      Type containerTypeToUse = containerType;

      if (property == null && containerType.IsInterface)
      {
        Tuple<PropertyDescriptor, Type> foundProperty = (
            from t in containerType.GetInterfaces()
            let p = GetTypeDescriptor(t).GetProperties().Find(propertyName, true)
            where p != null
            select (new Tuple<PropertyDescriptor, Type>(p, t))
            ).FirstOrDefault();

        if (foundProperty != null)
        {
          property = foundProperty.Item1;
          containerTypeToUse = foundProperty.Item2;
        }
      }

      if (property == null)
      {
        throw CreateNotFoundException(containerType, propertyName);
      }

      return GetMetadataForProperty(modelAccessor, containerTypeToUse, property);
    }

    protected PropertyDescriptor GetPropertyDescriptor(Type containerType, string propertyName)
    {
      if (containerType == null)
      {
        throw new ArgumentNullException("containerType");
      }

      if (string.IsNullOrEmpty(propertyName))
      {
        throw new ArgumentException("The property {0} cannot be null or empty", "propertyName");
      }

      return GetTypeDescriptor(containerType).GetProperties().Find(propertyName, true);
    }

    protected Exception CreateNotFoundException(Type containerType, string propertyName)
    {
      throw new ArgumentException($"The property {containerType.FullName}.{propertyName} could not be found");
    }
  }
}
