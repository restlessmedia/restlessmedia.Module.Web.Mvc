using restlessmedia.Module.Address;

namespace restlessmedia.Module.Web.Mvc.Model
{
  public class AddressModel : AddressEntity
  {
    public AddressModel()
      : base()
    {
      CountryCode = "GBR";
    }
  }
}