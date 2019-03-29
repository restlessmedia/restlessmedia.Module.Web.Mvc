using restlessmedia.Module.DataAnnotations;
using System.ComponentModel.DataAnnotations;

namespace restlessmedia.Module.Web.Mvc.Model
{
  public class AddAlertModel
  {
    [Email]
    [Required(ErrorMessage = "Email is required")]
    public string Email { get; set; }
  }
}