using System;

namespace restlessmedia.Module.Web.Mvc.Model
{
  public class ErrorModel
  {
    public string ErrorMessage { get; set; }
    public Exception TheException { get; set; }
    public string PublicMessage { get; set; }
  }
}