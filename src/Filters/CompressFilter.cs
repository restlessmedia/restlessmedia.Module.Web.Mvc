using System;
using System.IO;
using System.IO.Compression;
using System.Web;
using System.Web.Mvc;

namespace restlessmedia.Module.Web.Mvc.Filters
{
  public class CompressFilter : ActionFilterAttribute
  {
    public override void OnActionExecuting(ActionExecutingContext filterContext)
    {
      HttpRequestBase request = filterContext.HttpContext.Request;
      string acceptEncoding = request.Headers["Accept-Encoding"];

      if (string.IsNullOrEmpty(acceptEncoding))
      {
        return;
      }

      HttpResponseBase response = filterContext.HttpContext.Response;

      if (response.Filter == null)
      {
        return;
      }

      if (TryApplyFilter(response, new GZipStream(response.Filter, CompressionMode.Compress), acceptEncoding, "gzip"))
      {
        return;
      }

      TryApplyFilter(response, new DeflateStream(response.Filter, CompressionMode.Compress), acceptEncoding, "deflate");
    }

    protected virtual bool TryApplyFilter(HttpResponseBase response, Stream stream, string acceptEncoding, string encoding)
    {
      if (acceptEncoding.IndexOf(encoding, StringComparison.OrdinalIgnoreCase) >= 0)
      {
        response.AppendHeader("Content-encoding", encoding);
        response.Filter = stream;
        return true;
      }

      return false;
    }
  }
}