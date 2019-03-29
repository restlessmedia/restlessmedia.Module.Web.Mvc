using System;
using System.ComponentModel;

namespace restlessmedia.Module.Web.Mvc.Binders
{
  public class PropertyDescriptorWrapper : PropertyDescriptor
  {
    public PropertyDescriptorWrapper(PropertyDescriptor descriptor, Type type)
        : base(descriptor)
    {
      _descriptor = descriptor;
      _type = type;
    }

    public override bool CanResetValue(object component)
    {
      return _descriptor.CanResetValue(component);
    }

    public override Type ComponentType
    {
      get { return _descriptor.ComponentType; }
    }

    public override object GetValue(object component)
    {
      return _descriptor.GetValue(component);
    }

    public override bool IsReadOnly
    {
      get { return _descriptor.IsReadOnly; }
    }

    public override Type PropertyType
    {
      get { return _type; }
    }

    public override void ResetValue(object component)
    {
      _descriptor.ResetValue(component);
    }

    public override void SetValue(object component, object value)
    {
      _descriptor.SetValue(component, value);
    }

    public override bool ShouldSerializeValue(object component)
    {
      return _descriptor.ShouldSerializeValue(component);
    }

    private readonly PropertyDescriptor _descriptor;

    private readonly Type _type;
  }
}
