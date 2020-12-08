using restlessmedia.Module.Web.Extensions;
using System.Web;
using System.Web.Mvc;

namespace restlessmedia.Module.Web.Mvc.Attributes
{
  /// <summary>
  /// Handles basic authentication in the header.
  /// </summary>
  public class BasicAuthenticationAttribute : AuthorizeAttribute
  {
    public BasicAuthenticationAttribute(string username, string password, bool isBase64 = true)
    {
      _username = username;
      _password = password;
      _isBase64 = isBase64;
    }

    protected override bool AuthorizeCore(HttpContextBase httpContext)
    {
      return httpContext.Request.IsAuthorized(_username, _password, _isBase64);
    }

    private string _username;

    private string _password;

    private bool _isBase64 = true;
  }
}