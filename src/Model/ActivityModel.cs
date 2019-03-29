using restlessmedia.Module.Security;
using System;
using System.ComponentModel.DataAnnotations;

namespace restlessmedia.Module.Web.Mvc.Model
{
  [Serializable]
  public class ActivityModel : Activity
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

		[Display(Name = "Access")]
		public override bool HasBasic
		{
			get
			{
        return base.HasBasic;
			}
			set
			{
        base.HasBasic = value;
			}
		}

		[Display(Name = "Create")]
		public override bool HasCreate
		{
			get
			{
        return base.HasCreate;
			}
			set
			{
        base.HasCreate = value;
			}
		}

		[Display(Name = "Read")]
		public override bool HasRead
		{
			get
			{
        return base.HasRead;
			}
			set
			{
        base.HasRead = value;
			}
		}

		[Display(Name = "Update")]
		public override bool HasUpdate
		{
			get
			{
        return base.HasUpdate;
			}
			set
			{
         base.HasUpdate = value;
			}
		}

		[Display(Name = "Delete")]
		public override bool HasDelete
		{
			get
			{
        return base.HasDelete;
			}
			set
			{
        base.HasDelete = value;
			}
		}
	}
}