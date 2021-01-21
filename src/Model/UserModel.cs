using restlessmedia.Module.Security;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace restlessmedia.Module.Web.Mvc.Model
{
  public class UserModel : GenericUser, IValidatableObject
  {
    public UserModel()
      : base() { }

    public UserModel(string name)
      : base(name) { }

    [DataType(DataType.Password)]
    public override string Password { get { return base.Password; } set { base.Password = value; } }

    [DataType(DataType.Password)]
    public string PasswordConfirm { get; set; }

    [Required(ErrorMessage = "Email is required")]
    public override string Email
    {
      get
      {
        return base.Email;
      }
      set
      {
        Identity = !string.IsNullOrEmpty(value) ? new FormsIdentity(value) : null;
        base.Email = base.Email = value;
      }
    }

    public new Guid? UniqueId
    {
      get
      {
        return (Guid?)(base.UniqueId ?? null);
      }
      set
      {
        base.UniqueId = value;
      }
    }

    public int[] RoleIds
    {
      get
      {
        if (_roleIds == null)
        {
          _roleIds = Roles != null ? Roles.Select(x => x.RoleId.Value).ToArray() : new int[0] { };
        }

        return _roleIds;
      }
      set
      {
        _roleIds = value;
      }
    }

    public bool IsChangingPassword
    {
      get
      {
        return !string.IsNullOrEmpty(Password) && PasswordsMatch;
      }
    }

    public bool PasswordsMatch
    {
      get
      {
        return string.Equals(Password, PasswordConfirm);
      }
    }

    public IEnumerable<System.ComponentModel.DataAnnotations.ValidationResult> Validate(ValidationContext validationContext)
    {
      if (!PasswordsMatch)
      {
        yield return new System.ComponentModel.DataAnnotations.ValidationResult("Passwords don't match", new string[2] { "Password", "PasswordConfirm" });
      }

      if (Identity == null || string.Compare(Identity.Name, "Administrator", true) == 0)
      {
        yield return new System.ComponentModel.DataAnnotations.ValidationResult("Invalid username", new string[1] { "Username" });
      }
    }

    private int[] _roleIds = null;
  }
}