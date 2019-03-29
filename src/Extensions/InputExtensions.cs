using System.Linq.Expressions;

namespace System.Web.Mvc.Html
{
  public static class InputExtensions
  {
    /// <summary>
    /// Renders a checkbox
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="html"></param>
    /// <param name="expression"></param>
    /// <param name="addHidden">If true, adds a hidden field to fix the issue when checkbox isn't sent with the request.</param>
    /// <param name="htmlAttributes"></param>
    /// <returns></returns>
    public static MvcHtmlString CheckBoxFor<T>(this HtmlHelper<T> html, Expression<Func<T, bool>> expression, bool addHidden = true, object htmlAttributes = null)
    {
      if (addHidden)
      {
        return html.CheckBoxFor(expression, htmlAttributes);
      }

      TagBuilder tag = new TagBuilder("input");

      tag.Attributes["type"] = "checkbox";
      tag.Attributes["id"] = html.IdFor(expression).ToString();
      tag.Attributes["name"] = html.NameFor(expression).ToString();
      tag.Attributes["value"] = "true";

      // set the "checked" attribute if true
      ModelMetadata metadata = ModelMetadata.FromLambdaExpression(expression, html.ViewData);

      bool modelChecked;

      if (metadata.Model != null && bool.TryParse(metadata.Model.ToString(), out modelChecked) && modelChecked)
      {
        tag.Attributes["checked"] = "checked";
      }

      // merge custom attributes
      tag.MergeAttributes(HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));

      string tagString = tag.ToString(TagRenderMode.SelfClosing);

      return MvcHtmlString.Create(tagString);
    }
  }
}