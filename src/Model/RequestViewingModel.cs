using restlessmedia.Module.DataAnnotations;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace restlessmedia.Module.Web.Mvc.Model
{
  public class RequestViewingModel : RequestViewing
  {
    public RequestViewingModel() { }

    [Required(ErrorMessage = "Name")]
    public string Forename { get; set; }

    public string Surname { get; set; }

    [TelephoneNumber]
    [Remote("ValidatePhoneNumber", "Validation", ErrorMessage = "Provide a valid telephone number")]
    public override string PhoneNumber
    {
      get
      {
        return base.PhoneNumber;
      }
      set
      {
        base.PhoneNumber = value;
      }
    }

    public int? Bedrooms { get; set; }

    public decimal? MaxBudget { get; set; }

    [Email]
    [Required(ErrorMessage = "Email is required")]
    public override string Email
    {
      get
      {
        return base.Email;
      }
      set
      {
        base.Email = value;
      }
    }
  }
}