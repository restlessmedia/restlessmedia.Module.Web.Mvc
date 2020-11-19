using restlessmedia.Test;
using System.Web.Mvc;
using Xunit;

namespace restlessmedia.Module.Web.Mvc.UnitTest
{
  public class GrowlTests
  {
    public GrowlTests()
    {
      _controller = new TestController();
    }

    [Theory]
    [InlineData("test-message", "test-type")]
    public void sets_dependencies(string message, string type)
    {
      // set-up & call
      Growl growl = new Growl(_controller, message, type);

      // assert
      growl.Message.MustBe(message);
      growl.Type.MustBe(type);
    }

    [Theory]
    [InlineData("test-message", "test-info", "test-message|test-info")]
    public void Adds_growl(string message, string type, string expected)
    {
      // call
      Growl.Add(_controller, message, type);

      // assert
      GrowlCollection.RenderScripts(_controller, growl => $"{message}|{type}", false).ToString().MustBe(expected);
    }

    private readonly ControllerBase _controller;

    /// <summary>
    /// We need this test class because of the protected ctor.
    /// </summary>
    private class TestController : ControllerBase
    {
      protected override void ExecuteCore() { }
    }
  }
}