namespace restlessmedia.Module.Web.Mvc.Model.Result
{
  public class LoginJsonResultModel
  {
    public LoginResultState State { get; set; }

    public string Message { get; set; }

    public string Url { get; set; }

    public static LoginJsonResultModel Error(string message = null)
    {
      return new LoginJsonResultModel()
      {
        State = LoginResultState.Error,
        Message = message
      };
    }

    public static LoginJsonResultModel Success(string redirectTo = null)
    {
      return new LoginJsonResultModel()
      {
        State = LoginResultState.Success,
        Url = redirectTo
      };
    }
  }
}