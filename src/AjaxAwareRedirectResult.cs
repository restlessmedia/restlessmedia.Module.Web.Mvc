using System.Web.Mvc;

namespace restlessmedia.Module.Web.Mvc
{
  public class AjaxAwareRedirectResult : RedirectResult
  {
    public AjaxAwareRedirectResult(string url)
      : base(url) { }

    public override void ExecuteResult(ControllerContext context)
    {
      if (context.RequestContext.HttpContext.Request.IsAjaxRequest())
      {
        string destinationUrl = System.Web.Mvc.UrlHelper.GenerateContentUrl(Url, context.HttpContext);

        JavaScriptResult result = new JavaScriptResult
        {
          Script = $"window.location='{destinationUrl}';"
        };
        result.ExecuteResult(context);
      }
      else
      {
        base.ExecuteResult(context);
      }
    }
  }
}