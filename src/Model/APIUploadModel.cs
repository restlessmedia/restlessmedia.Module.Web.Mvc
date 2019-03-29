using System.Web;
using System.Xml.Linq;

namespace restlessmedia.Module.Web.Mvc.Model
{
  public class APIUploadModel
  {
    public HttpPostedFileBase File { get; set; }

    public XDocument GetData()
    {
      return new XDocument(File.InputStream);
    }
  }
}