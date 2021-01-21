using FakeItEasy;
using restlessmedia.Module.Web.Mvc.Binders;
using restlessmedia.Test;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Web.Mvc;
using Xunit;

namespace restlessmedia.Module.Web.Mvc.UnitTest.Binders
{
  public class FlagEnumerationModelBinderTests
  {
    public FlagEnumerationModelBinderTests()
    {
      _flagEnumerationModelBinder = new FlagEnumerationModelBinder();
      _controllerContext = new ControllerContext();
      _modelMetadataProvider = A.Fake<ModelMetadataProvider>();
      _modelBindingContext = new ModelBindingContext
      {
        ModelMetadata = new ModelMetadata(_modelMetadataProvider, typeof(TestModel), () => null, typeof(TestEnum), nameof(TestModel.TestProp)),
      };
    }

    [Fact]
    public void matches_enumeration()
    {
      _flagEnumerationModelBinder.IsMatch(typeof(TestEnum)).MustBeTrue();
    }

    [Theory]
    [InlineData(new string[] { "1", "2" }, TestEnum.One | TestEnum.Two)]
    [InlineData(new string[] { "1", "2", "4" }, TestEnum.One | TestEnum.Two | TestEnum.Four)]
    [InlineData(new string[] { "2" }, TestEnum.Two)]
    [InlineData(new string[] { "1" }, TestEnum.One)]
    [InlineData(new string[] { "3" }, TestEnum.OneAndTwo)]
    public void binds_flags_values(string[] value, TestEnum expected)
    {
      // set-up
      const string modelName = "foo";
      _modelBindingContext.ModelName = modelName;
      _modelBindingContext.ModelMetadata = new ModelMetadata(_modelMetadataProvider, typeof(TestModel), () => null, typeof(TestEnum), nameof(TestModel.TestProp));
      _modelBindingContext.ValueProvider = CreateValueProvider(new Dictionary<string, object> { { modelName, value } });

      // call & assert
      _flagEnumerationModelBinder.BindModel(_controllerContext, _modelBindingContext)
        .MustBe(expected);
    }

    [Theory]
    [InlineData(new string[] { "does-not-exist" }, null)]
    [InlineData(new string[] { "does-not-exist", "1" }, null)] // we don't do partial parsing so even though the 1 exists as a value, it won't parse at all

    public void ignores_invalid_values(string[] value, TestEnum? expected)
    {
      // set-up
      const string modelName = "foo";
      _modelBindingContext.ModelName = modelName;
      _modelBindingContext.ModelMetadata = new ModelMetadata(_modelMetadataProvider, typeof(TestModel), () => null, typeof(TestEnum), nameof(TestModel.TestProp));
      _modelBindingContext.ValueProvider = CreateValueProvider(new Dictionary<string, object> { { modelName, value } });

      // call & assert
      _flagEnumerationModelBinder.BindModel(_controllerContext, _modelBindingContext)
        .MustBe(expected);
    }

    private DictionaryValueProvider<object> CreateValueProvider(Dictionary<string, object> values)
    {
      return new DictionaryValueProvider<object>(values, new CultureInfo("en-GB"));
    }

    private class TestModel
    {
      public TestEnum TestProp { get; set; }
    }

    [Flags]
    public enum TestEnum
    {
      One = 1,
      Two = 2,
      OneAndTwo = 3,
      Four = 4,
    }

    private readonly FlagEnumerationModelBinder _flagEnumerationModelBinder;

    private readonly ControllerContext _controllerContext;

    private readonly ModelMetadataProvider _modelMetadataProvider;

    private readonly ModelBindingContext _modelBindingContext;
  }
}
