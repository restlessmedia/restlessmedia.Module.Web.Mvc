using restlessmedia.Module.Contact;
using System.ComponentModel.DataAnnotations;

namespace restlessmedia.Module.Web.Mvc.Model
{
  public class EnquiryModel : EnquiryEntity
  {
    [Display(Name = "Name")]
    public override string Name
    {
      get
      {
        return base.Name;
      }
      set
      {
        base.Name = value;
      }
    }

    [Display(Name = "Email")]
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
