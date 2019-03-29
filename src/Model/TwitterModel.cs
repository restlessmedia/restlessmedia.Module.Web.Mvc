using restlessmedia.Module.Twitter;

namespace restlessmedia.Module.Web.Mvc.Model
{
  public class TwitterModel
  {
    public TwitterModel(Tweet tweet)
    {
      Status = HTMLHelper.Anchorize(tweet.Status);
      RelativeDateTime = tweet.Date.ToRelativeDate();
    }

    private TwitterModel() { }

    public string RelativeDateTime = null;

    public string Status = null;

    public static TwitterModel Error
    {
      get
      {
        return new TwitterModel()
        {
          Status = "Error loading tweets"
        };
      }
    }
  }
}