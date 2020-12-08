using restlessmedia.Module.Category;
using restlessmedia.Module.Security;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.UI;

namespace restlessmedia.Module.Web.Mvc.Helper
{
  public class HtmlHelper<TModel> : System.Web.Mvc.HtmlHelper<TModel>
  {
    public HtmlHelper(ISecurityService securityService, IRoleService roleService, ICategoryService categoryService, ViewContext viewContext, IViewDataContainer viewDataContainer, RouteCollection routeCollection = null)
      : base(viewContext, viewDataContainer, routeCollection ?? new RouteCollection())
    {
      _securityService = securityService ?? throw new ArgumentNullException(nameof(securityService));
      _roleService = roleService ?? throw new ArgumentNullException(nameof(roleService));
      CategoryService = categoryService ?? throw new ArgumentNullException(nameof(categoryService));
    }

    /// <summary>
    /// Renders a label block
    /// </summary>
    /// <param name="htmlAttributes"></param>
    /// <returns></returns>
    public MvcTag BeginLabel(object htmlAttributes = null)
    {
      return TagHelper(HtmlTextWriterTag.Label, null, htmlAttributes: HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
    }

    /// <summary>
    /// Renders a textarea block for a model property
    /// </summary>
    /// <typeparam name="TModel"></typeparam>
    /// <typeparam name="TProperty"></typeparam>
    /// <param name="expression"></param>
    /// <param name="htmlAttributes"></param>
    /// <returns></returns>
    public MvcTag BeginTextAreaFor<TProperty>(Expression<Func<TModel, TProperty>> expression, object htmlAttributes = null)
    {
      return BeginTagFor(expression, HtmlTextWriterTag.Textarea, htmlAttributes: htmlAttributes);
    }

    /// <summary>
    /// Renders a bootstrap block for a model property
    /// </summary>
    /// <typeparam name="TModel"></typeparam>
    /// <typeparam name="TProperty"></typeparam>
    /// <param name="expression"></param>
    /// <returns></returns>
    public MvcTag BeginControlGroupFor<TProperty>(Expression<Func<TModel, TProperty>> expression)
    {
      return BeginTagFor(expression, HtmlTextWriterTag.Div, "error", new { @class = "control-group" });
    }

    /// <summary>
    /// Renders a label block for a model property
    /// </summary>
    /// <typeparam name="TModel"></typeparam>
    /// <typeparam name="TProperty"></typeparam>
    /// <param name="expression"></param>
    /// <param name="htmlAttributes"></param>
    /// <returns></returns>
    public MvcTag BeginLabelFor<TProperty>(Expression<Func<TModel, TProperty>> expression, object htmlAttributes = null)
    {
      return BeginTagFor(expression, HtmlTextWriterTag.Label, htmlAttributes: htmlAttributes);
    }

    /// <summary>
    /// Renders an html tag block
    /// </summary>
    /// <param name="htmlTag"></param>
    /// <param name="htmlAttributes"></param>
    /// <returns></returns>
    public MvcTag BeginTag(HtmlTextWriterTag htmlTag, object htmlAttributes = null)
    {
      return TagHelper(htmlTag, null, htmlAttributes: HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
    }

    public MvcTag BeginTagFor<TProperty>(Expression<Func<TModel, TProperty>> expression, HtmlTextWriterTag htmlTag, string errorClass = null, object htmlAttributes = null)
    {
      string name = ExpressionHelper.GetExpressionText(expression);
      return TagHelper(htmlTag, name, errorClass, HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
    }

    public string ValidationSummaryFor<TProperty>(Expression<Func<TModel, TProperty>> expression, string message, IDictionary<string, object> htmlAttributes)
    {
      string name = ExpressionHelper.GetExpressionText(expression);

      // Nothing to do if there aren't any errors
      if (ViewData.ModelState.IsValid)
      {
        return null;
      }

      string messageSpan;

      if (!string.IsNullOrEmpty(message))
      {
        TagBuilder spanTag = new TagBuilder("span");
        spanTag.MergeAttributes(htmlAttributes);
        spanTag.MergeAttribute("class", HtmlHelper.ValidationSummaryCssClassName);
        spanTag.SetInnerText(message);
        messageSpan = spanTag.ToString(TagRenderMode.Normal) + Environment.NewLine;
      }
      else
      {
        messageSpan = null;
      }

      StringBuilder htmlSummary = new StringBuilder();
      TagBuilder unorderedList = new TagBuilder("ul");
      unorderedList.MergeAttributes(htmlAttributes);
      unorderedList.MergeAttribute("class", HtmlHelper.ValidationSummaryCssClassName);

      foreach (ModelState modelState in ViewData.ModelState.Values)
      {
        foreach (ModelError modelError in modelState.Errors)
        {
          string errorText = GetUserErrorMessageOrDefault(ViewContext.HttpContext, modelError, null /* modelState */);
          if (!string.IsNullOrEmpty(errorText))
          {
            TagBuilder listItem = new TagBuilder("li");
            listItem.SetInnerText(errorText);
            htmlSummary.AppendLine(listItem.ToString(TagRenderMode.Normal));
          }
        }
      }

      unorderedList.InnerHtml = htmlSummary.ToString();

      return messageSpan + unorderedList.ToString(TagRenderMode.Normal);
    }

    /// <summary>
    /// Returns a string for embedding google analytics code
    /// </summary>
    /// <param name="account"></param>
    /// <param name="domain"></param>
    /// <returns></returns>
    public MvcHtmlString GoogleAnalyticsScript(string account, string domain)
    {
      // build the script
      StringBuilder script = new StringBuilder();
      script.AppendLine("var _gaq = _gaq || [];");
      script.AppendFormat("_gaq.push(['_setAccount', '{0}']);", account);
      if (!string.IsNullOrEmpty(domain))
      {
        script.AppendFormat("_gaq.push(['_setDomainName', '{0}']);", domain);
      }
      script.AppendLine("_gaq.push(['_setAllowLinker', true]);");
      script.AppendLine("_gaq.push(['_trackPageview']);");
      script.AppendLine("(function() {");
      script.AppendLine("var ga = document.createElement('script'); ga.type = 'text/javascript'; ga.async = true;");
      script.AppendLine("ga.src = ('https:' == document.location.protocol ? 'https://ssl' : 'http://www') + '.google-analytics.com/ga.js';");
      script.AppendLine("var s = document.getElementsByTagName('script')[0]; s.parentNode.insertBefore(ga, s);");
      script.AppendLine("})();");

      // build the tag
      TagBuilder scriptTag = new TagBuilder("script");
      scriptTag.Attributes["type"] = "text/javascript";
      scriptTag.InnerHtml = script.ToString();

      // return string
      return new MvcHtmlString(scriptTag.ToString());
    }

    /// <summary>
    /// Returns date select options dd/mm/yyyy
    /// </summary>
    /// <param name="format"></param>
    /// <param name="prefix"></param>
    /// <returns></returns>
    public MvcHtmlString Date(DateFormat format, string prefix = null)
    {
      DateTime now = DateTime.Now;
      StringBuilder options = new StringBuilder();
      string optionFormat = "<option value=\"{0}\" {1}>{2}</option>";
      string selected = "selected=\"selected\"";
      bool havePrefix = !string.IsNullOrEmpty(prefix);

      // container
      using (MvcTag container = new MvcTag(ViewContext, HtmlTextWriterTag.Div))
      {
        container.Builder.Attributes["class"] = "dates";

        // day
        if (format.HasFlag(DateFormat.Day))
        {
          using (MvcTag day = new MvcTag(ViewContext, HtmlTextWriterTag.Select))
          {
            day.Builder.Attributes["name"] = string.Concat(prefix, havePrefix ? "_" : string.Empty, "day");
            for (var i = 1; i < 32; i++)
            {
              options.AppendFormat(optionFormat, i, i == now.Day ? selected : string.Empty, i);
            }
            day.Builder.InnerHtml = options.ToString();
            container.AddChildTag(day);
            options.Clear();
          }
        }

        // month
        if (format.HasFlag(DateFormat.Month))
        {
          using (MvcTag month = new MvcTag(ViewContext, HtmlTextWriterTag.Select))
          {
            month.Builder.Attributes["name"] = string.Concat(prefix, havePrefix ? "_" : string.Empty, "month");
            for (var i = 1; i < 13; i++)
            {
              options.AppendFormat(optionFormat, i, i == now.Month ? selected : string.Empty, CultureInfo.CurrentCulture.DateTimeFormat.GetAbbreviatedMonthName(i));
            }
            month.Builder.InnerHtml = options.ToString();
            container.AddChildTag(month);
            options.Clear();
          }
        }

        // year
        if (format.HasFlag(DateFormat.Month))
        {
          using (MvcTag year = new MvcTag(ViewContext, HtmlTextWriterTag.Select))
          {
            year.Builder.Attributes["name"] = string.Concat(prefix, havePrefix ? "_" : string.Empty, "year");
            int yearStart = now.Year - 100;
            int yearEnd = now.Year + 10;
            for (var i = yearEnd; i > yearStart; i--)
            {
              options.AppendFormat(optionFormat, i, i == now.Year ? selected : string.Empty, i);
            }
            year.Builder.InnerHtml = options.ToString();
            container.AddChildTag(year);
          }
        }

        // return container
        return new MvcHtmlString(container.Builder.ToString());
      }
    }

    public System.Web.Mvc.UrlHelper GetUrlHelper()
    {
      return new System.Web.Mvc.UrlHelper(ViewContext.RequestContext, RouteCollection);
    }

    public bool Can(string activity, ActivityAccess access = ActivityAccess.Basic)
    {
      return _securityService.Authorize(ViewContext.HttpContext, activity, access);
    }

    public MvcHtmlString Access(string activity, Func<string> allow, ActivityAccess access = ActivityAccess.Basic, Func<string> deny = null)
    {
      return new MvcHtmlString(Can(activity, access) ? allow() : deny != null ? deny() : null);
    }

    [Flags]
    public enum DateFormat
    {
      Day = 1,
      Month = 2,
      Year = 4,
    }

    public string ResourceClassKey
    {
      get
      {
        return _resourceClassKey ?? String.Empty;
      }
      set
      {
        _resourceClassKey = value;
      }
    }

    public ICategoryService CategoryService { get; private set; }

    private MvcTag TagHelper(HtmlTextWriterTag htmlTag, string name, string errorClass = "invalid", IDictionary<string, object> htmlAttributes = null)
    {
      // tag
      MvcTag tag = new MvcTag(ViewContext, htmlTag);

      // attributes
      if (htmlAttributes != null)
      {
        tag.Builder.MergeAttributes(htmlAttributes);
      }

      // add the name if a form element
      if (new HtmlTextWriterTag[3] { HtmlTextWriterTag.Textarea, HtmlTextWriterTag.Input, HtmlTextWriterTag.Select }.Contains(htmlTag))
      {
        tag.Builder.Attributes.Add("name", name);
      }

      // model state error check
      if (!string.IsNullOrEmpty(errorClass))
      {
        ModelState state = GetState(ViewContext, name);
        if (state != null && state.Errors.Count > 0)
        {
          tag.Builder.AddCssClass("invalid");
        }
      }

      // output to writer
      ViewContext.Writer.Write(tag.Builder.ToString(TagRenderMode.StartTag));

      // return tag
      return tag;
    }

    private ModelState GetState(ViewContext context, string name)
    {
      return !string.IsNullOrEmpty(name) ? ViewContext.ViewData.ModelState[name] : null;
    }

    private string GetUserErrorMessageOrDefault(HttpContextBase httpContext, ModelError error, ModelState modelState)
    {
      if (!string.IsNullOrEmpty(error.ErrorMessage))
      {
        return error.ErrorMessage;
      }
      if (modelState == null)
      {
        return null;
      }

      string attemptedValue = (modelState.Value != null) ? modelState.Value.AttemptedValue : null;
      return string.Format(CultureInfo.CurrentCulture, GetInvalidPropertyValueResource(httpContext), attemptedValue);
    }

    private string GetInvalidPropertyValueResource(HttpContextBase httpContext)
    {
      string resourceValue = null;
      if (!string.IsNullOrEmpty(ResourceClassKey) && (httpContext != null))
      {
        // If the user specified a ResourceClassKey try to load the resource they specified.
        // If the class key is invalid, an exception will be thrown.
        // If the class key is valid but the resource is not found, it returns null, in which
        // case it will fall back to the MVC default error message.
        resourceValue = httpContext.GetGlobalResourceObject(ResourceClassKey, "InvalidPropertyValue", CultureInfo.CurrentUICulture) as string;
      }
      return resourceValue ?? "The value '{0}' is invalid.";
    }

    private string _resourceClassKey;

    private readonly IRoleService _roleService;

    private readonly ISecurityService _securityService;
  }
}