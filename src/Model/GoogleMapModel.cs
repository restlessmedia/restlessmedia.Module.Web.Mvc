using restlessmedia.Module.Address;
using System.Web.UI.WebControls;

namespace restlessmedia.Module.Web.Mvc.Model
{
  public class GoogleMapModel
  {
    public GoogleMapModel()
    {
      Nearest = new NearestModel();
    }

    public GoogleMapModel(IAddress address)
    {
      Address = address;
      Nearest = new NearestModel(address);
    }

    public bool DisplayLargeLink = true;

    public Unit Height = new Unit(350);

    public Unit Width = new Unit(280);

    /// <summary>
    /// Determines whether the map is loaded immediately or a client routine invokes it
    /// </summary>
    public bool AutoLoad = true;

    public IAddress Address;

    public NearestModel Nearest;

    public string Src
    {
      get
      {
        return $"http://maps.google.co.uk/maps?q={Address.ToString(",")}&output=embed";
      }
    }
  }
}
