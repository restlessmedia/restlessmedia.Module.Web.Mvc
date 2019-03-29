using System.ComponentModel.DataAnnotations;

namespace restlessmedia.Module.Web.Mvc.Model
{
  public class LogOnModel
  {
    public LogOnModel(string returnUrl)
    {
      ReturnUrl = returnUrl;
    }

    public LogOnModel(ResetPasswordModel resetPassword)
      : this()
    {
      UserName = resetPassword.Email;
      Password = resetPassword.Password;
    }

    public LogOnModel()
      : this("/") { }

    [Required(ErrorMessage = "Username is required")]
    [Display(Name = "Username")]
    public string UserName { get; set; }

    [Required(ErrorMessage = "Password is required")]
    [DataType(DataType.Password)]
    [Display(Name = "Password")]
    public string Password { get; set; }

    [Display(Name = "Remember me?")]
    public bool RememberMe { get; set; }

    public string ReturnUrl { get; set; }

    public ForgotPasswordModel ForgotPassword = new ForgotPasswordModel();
  }
}