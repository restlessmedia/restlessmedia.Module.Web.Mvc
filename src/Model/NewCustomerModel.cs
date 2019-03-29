using restlessmedia.Module.DataAnnotations;
using System.ComponentModel.DataAnnotations;

namespace restlessmedia.Module.Web.Mvc.Model
{
  public class NewCustomerModel
  {
    [Required(ErrorMessage = "Email address is required")]
    [Email(ErrorMessage = "Invalid email address")]
    [DataType(DataType.EmailAddress)]
    [Display(Name = "Email address")]
    public string Email { get; set; }

    [Required(ErrorMessage = "Password is required")]
    [DataType(DataType.Password)]
    [Display(Name = "Password")]
    public string Password { get; set; }

    public string ReturnUrl { get; set; }

    [Required]
    [BooleanMustBeTrue(ErrorMessage = "You must agree to the terms & conditions")]
    [Display(Name = "Terms & Conditions")]
    public bool AgreeTerms { get; set; }
  }
}
