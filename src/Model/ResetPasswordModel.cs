using restlessmedia.Module.DataAnnotations;
using restlessmedia.Module.Security;
using System;
using System.ComponentModel.DataAnnotations;

namespace restlessmedia.Module.Web.Mvc.Model
{
  public class ResetPasswordModel : IResetPassword
  {
    public ResetPasswordModel() { }

    public ResetPasswordModel(Guid accessKey)
    {
      AccessKey = accessKey;
    }

    public ResetPasswordModel(SecurityAccess access)
    {
      AccessKey = access.AccessKey;
    }

    [Required(ErrorMessage = "Email address is required")]
    [Email(ErrorMessage = "Invalid email address")]
    [DataType(DataType.EmailAddress)]
    [Display(Name = "Email address")]
    public string Email { get; set; }

    [Required(ErrorMessage = "Password is required")]
    [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
    [DataType(DataType.Password)]
    [Display(Name = "Password")]
    public string Password { get; set; }

    [DataType(DataType.Password)]
    [Display(Name = "Confirm password")]
    [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
    public string ConfirmPassword { get; set; }

    [Required(ErrorMessage = "Access key is required")]
    [Display(Name = "Access key")]
    public Guid AccessKey { get; set; }
  }
}
