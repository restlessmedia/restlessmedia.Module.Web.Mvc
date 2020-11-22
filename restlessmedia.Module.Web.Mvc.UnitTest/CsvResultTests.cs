using FakeItEasy;
using restlessmedia.Test;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Xunit;

namespace restlessmedia.Module.Web.Mvc.UnitTest
{
  public class CsvResultTests : IDisposable
  {
    public CsvResultTests()
    {
      _responseStream = new MemoryStream();
      _httpContext = A.Fake<HttpContextBase>();
      A.CallTo(() => _httpContext.Response.OutputStream).Returns(_responseStream);
      ControllerBase controllerBase = A.Fake<ControllerBase>();
      _controllerContext = new ControllerContext(_httpContext, new RouteData(), controllerBase);
    }

    [Fact]
    public void writes_result()
    {
      // set-up
      IEnumerable<Test> data = Enumerable.Repeat(new Test
      {
        Name = "test",
        Age = 20
      }, 5);
      const string expected = "Name,Age\r\ntest,20\r\ntest,20\r\ntest,20\r\ntest,20\r\ntest,20\r\n";

      CsvResult csvResult = new CsvResult(data);

      // execute
      csvResult.ExecuteResult(_controllerContext);

      // assert
      _httpContext.Response.OutputStream.Seek(0, SeekOrigin.Begin);
      using (StreamReader streamReader = new StreamReader(_httpContext.Response.OutputStream))
      {
        string actual = streamReader.ReadToEnd();
        actual.MustBe(expected);
      }
    }

    public void Dispose()
    {
      _responseStream.Dispose();
    }

    private class Test
    {
      public string Name { get; set; }

      public int Age { get; set; }
    }

    private readonly MemoryStream _responseStream;

    private readonly ControllerContext _controllerContext;

    private readonly HttpContextBase _httpContext;
  }
}