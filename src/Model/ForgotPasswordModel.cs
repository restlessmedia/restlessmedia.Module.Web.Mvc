using restlessmedia.Module.DataAnnotations;
using System.ComponentModel.DataAnnotations;

namespace restlessmedia.Module.Web.Mvc.Model
{
  public class ForgotPasswordModel
  {
    [Required(ErrorMessage = "Email address is required")]
    [Email(ErrorMessage = "Invalid email address")]
    [DataType(DataType.EmailAddress)]
    [Display(Name = "Email address")]
    public string Email { get; set; }
  }
}