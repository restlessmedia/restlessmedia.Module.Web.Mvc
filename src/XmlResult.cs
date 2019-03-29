using System.Text;
using System.Web.Mvc;

namespace restlessmedia.Module.Web.Mvc
{
  public class XmlResult : ContentResult
  {
    public XmlResult(string content)
    {
      Content = content;
      ContentEncoding = Encoding.UTF8;
      ContentType = "application/xml;charset=UTF-8";
    }
  }
}