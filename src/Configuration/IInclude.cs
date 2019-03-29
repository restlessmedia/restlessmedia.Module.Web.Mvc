namespace restlessmedia.Module.Web.Mvc.Configuration
{
  public interface IInclude
  {
    string Src { get; }

    string Alias { get; }

    string Version { get; }
  }
}