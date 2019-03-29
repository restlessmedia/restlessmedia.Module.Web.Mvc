namespace restlessmedia.Module.Web.Mvc.Model
{
  public class AttributeModel
  {
    public AttributeModel(IProperty property)
    {
      Property = property;
      Show = property != null && (property.BedroomCount > 0 || property.BathroomCount > 0 || property.ReceptionCount > 0 || property.HasGarden);
    }

    /// <summary>
    /// Used to determine whether to show attributes or not. By default, this checks some values on the property to determine this.
    /// </summary>
    public bool Show = true;

    public IProperty Property;
  }
}