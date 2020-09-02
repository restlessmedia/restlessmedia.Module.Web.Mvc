using CsvWriter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace restlessmedia.Module.Web.Mvc
{
  public class CsvResult<T> : FileResult
  {
    public CsvResult(IEnumerable<T> data, string contentType = "text/csv", string columnDelimiter = ",", string rowDelimiter = null)
        : base(contentType)
    {
      _data = data;
      _columnDelimiter = columnDelimiter;
      _rowDelimiter = rowDelimiter ?? Environment.NewLine;
    }

    protected override void WriteFile(HttpResponseBase response)
    {
      IEnumerable<PropertyInfo> publicProperties = typeof(T).GetProperties();

      using (Writer writer = new Writer(response.OutputStream, new CsvOptions(_columnDelimiter, _rowDelimiter, " ", false)))
      {
        // header row
        writer.Write(publicProperties.Select(x => x.Name));

        // data rows
        foreach (T obj in _data)
        {
          writer.WriteRow(publicProperties.Select(x => x.GetValue(obj)));
        }
      }
    }

    private readonly IEnumerable<T> _data;

    private readonly string _columnDelimiter;

    private readonly string _rowDelimiter;
  }
}