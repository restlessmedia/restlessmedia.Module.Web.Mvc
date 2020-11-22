using CsvWriter;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace restlessmedia.Module.Web.Mvc
{
  public class CsvResult<T> : CsvResult
  {
    public CsvResult(IEnumerable<T> data, string contentType = "text/csv", string columnDelimiter = ",", string rowDelimiter = null)
      : base(data, contentType, columnDelimiter, rowDelimiter) { }
  }

  public class CsvResult : FileResult
  {
    public CsvResult(IEnumerable data, string contentType = "text/csv", string columnDelimiter = ",", string rowDelimiter = null)
      : base(contentType)
    {
      _data = data;
      _columnDelimiter = columnDelimiter;
      _rowDelimiter = rowDelimiter ?? Environment.NewLine;
    }

    protected override void WriteFile(HttpResponseBase response)
    {
      IEnumerable<PropertyInfo> members = null;

      // note we don't wrap in using so we don't dispose of the outputStream
      Writer writer = new Writer(response.OutputStream, new CsvOptions(_columnDelimiter, _rowDelimiter, " ", false));
      
      // data rows
      foreach (object obj in _data)
      {
        // header row
        if (members == null)
        {
          members = obj.GetType().GetProperties();
          writer.WriteRow(members.Select(x => x.Name).ToArray());
        }

        // TODO: add check if we have been passed an IEnumerable of mixed types which will fail here when the member doesn't exist
        writer.WriteRow(members.Select(x => x.GetValue(obj)).ToArray());
      }
    }

    private readonly IEnumerable _data;

    private readonly string _columnDelimiter;

    private readonly string _rowDelimiter;
  }
}