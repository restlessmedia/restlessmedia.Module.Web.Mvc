using restlessmedia.Module.File;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;

namespace restlessmedia.Module.Web.Mvc
{
  public class SpreadsheetResult<T> : FileResult
  {
    /// <summary>
    /// Returns an Mvc spreadsheet result of the supplied data
    /// </summary>
    /// <param name="data"></param>
    /// <param name="name">The name, without extension of the file</param>
    /// <param name="contentType"></param>
    /// <param name="extension">Extension with period of the file</param>
    public SpreadsheetResult(IEnumerable<T> data, string name, string contentType = "text/csv", string extension = ".csv")
      : base(contentType)
    {
      _data = data;
      FileDownloadName = string.Concat(name, extension);
    }

    protected override void WriteFile(HttpResponseBase response)
    {
      new FileHelper().WriteCSV(_data, response.OutputStream);
    }

    private readonly IEnumerable<T> _data;
  }
}