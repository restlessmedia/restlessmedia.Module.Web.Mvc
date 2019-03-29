using restlessmedia.Module.Contact;
using restlessmedia.Module.DataAnnotations;
using System.ComponentModel.DataAnnotations;

namespace restlessmedia.Module.Web.Mvc.Model
{
  public class ContactModel : EnquiryEntity
  {
    public ContactModel() { }

    [Required(ErrorMessage = "Name is required")]
    [DataType(DataType.Text)]
    public override string Name { get { return base.Name; } set { base.Name = value; } }

    [Required(ErrorMessage = "Email is required")]
    [Email(ErrorMessage = "Invalid email")]
    [DataType(DataType.EmailAddress)]
    public override string Email { get { return base.Email; } set { base.Email = value; } }

    [TelephoneNumber(ErrorMessage = "Invalid")]
    [DataType(DataType.Text)]
    public override string ContactNumber { get { return base.ContactNumber; } set { base.ContactNumber = value; } }

    public bool DataProtection
    {
      get
      {
        return ContactFlags.HasFlag(ContactFlags.DataProtection);
      }
      set
      {
        ContactFlags.SetFlag(ContactFlags.DataProtection, value);
      }
    }

    public GoogleMapModel Map
    {
      get
      {
        return _map = _map ?? new GoogleMapModel(Address);
      }
    }

    private GoogleMapModel _map = null;
  }
}