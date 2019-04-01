using restlessmedia.Module.Security;

namespace restlessmedia.Module.Web.Mvc.Attributes
{
  /// <summary>
	/// There is always a client role with basic access.  This is the wrapper check for it.
	/// </summary>
	public class ClientAttribute : ActivityAttribute
	{
		public ClientAttribute()
      : base(SystemActivity.Client, ActivityAccess.Basic) { }
	}
}