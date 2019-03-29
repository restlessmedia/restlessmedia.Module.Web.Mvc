using restlessmedia.Module.Address;

namespace restlessmedia.Module.Web.Mvc.Model
{
  public class NearestModel
  {
    public NearestModel(IAddress address)
    {
      Address = address;
    }

    public NearestModel() { }

    public IAddress Address;
  }
}