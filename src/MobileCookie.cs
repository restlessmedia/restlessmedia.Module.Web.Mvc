using restlessmedia.Module.Security;
using System;
using System.Web;

namespace restlessmedia.Module.Web.Mvc
{
  public class MobileCookie : CookieBase
  {
    public MobileCookie(HttpResponseBase response, HttpRequestBase request)
      : base(response, request, _name) { }

    public static void Expire(HttpResponseBase response, HttpRequestBase request)
    {
      new MobileCookie(response, request).Expire();
    }

    public static void Set(HttpResponseBase response, HttpRequestBase request)
    {
      new MobileCookie(response, request).CreateCookie(DateTime.Now.AddDays(1));
    }

    private const string _name = "fullsite";
  }
}