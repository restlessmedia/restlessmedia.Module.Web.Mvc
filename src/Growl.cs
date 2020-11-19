using System;
using System.Web.Mvc;

namespace restlessmedia.Module.Web.Mvc
{
  /// <summary>
  /// Storing and annoucing growls through the viewbag
  /// </summary>
  [Serializable]
  public class Growl
  {
    public Growl(ControllerBase controller, string message, string type)
    {
      Message = message;
      Type = type;
      _collection = new GrowlCollection(controller);
    }

    public static void Add(ControllerBase controller, string message, string type = "info")
    {
      Growl growl = new Growl(controller, message, type);
      growl._collection.Add(growl);
    }

    /// <summary>
    /// Standard create growl
    /// </summary>
    /// <param name="controller"></param>
    /// <param name="obj"></param>
    public static void ForCreated(ControllerBase controller, object obj)
    {
      For(controller, obj, "created");
    }

    /// <summary>
    /// Standard update growl
    /// </summary>
    /// <param name="controller"></param>
    /// <param name="obj"></param>
    public static void ForUpdated(ControllerBase controller, object obj)
    {
      For(controller, obj, "updated");
    }

    /// <summary>
    /// Standard delete growl
    /// </summary>
    /// <param name="controller"></param>
    /// <param name="obj"></param>
    public static void ForDeleted(ControllerBase controller, object obj)
    {
      For(controller, obj, "deleted");
    }

    /// <summary>
    /// Wrapper for growling errors
    /// </summary>
    /// <param name="controller"></param>
    /// <param name="message"></param>
    public static void Error(ControllerBase controller, string message)
    {
      Add(controller, message, "error");
    }

    /// <summary>
    /// Used for standardising object crud messages
    /// </summary>
    /// <param name="controller"></param>
    /// <param name="obj"></param>
    /// <param name="action"></param>
    public static void For(ControllerBase controller, object obj, string action)
    {
      Add(controller, $"{obj} {action} successfully.");
    }

    /// <summary>
    /// Message to display - this is required.
    /// </summary>
    public string Message;

    public string Type;

    public string MessageJsEncoded
    {
      get
      {
        if (Message == null)
        {
          return string.Empty;
        }

        return Message.Replace("'", "\\'");
      }
    }

    private readonly GrowlCollection _collection;
  }
}