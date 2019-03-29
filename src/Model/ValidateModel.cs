using System.Collections.Generic;

namespace restlessmedia.Module.Web.Mvc.Model
{
  public class ValidateModel
  {
    public string Type { get; set; }

    public string Field { get; set; }

    KeyValuePair<string, string>[] Data { get; set; }
  }
}
