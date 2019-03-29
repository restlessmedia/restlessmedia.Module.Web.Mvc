namespace restlessmedia.Module.Web.Mvc
{
  public class ValidationResult
  {
    public ValidationResult(bool isValid, object value)
    {
      IsValid = isValid;
      Value = value;
    }

    public readonly bool IsValid;

    public readonly object Value;
  }
}