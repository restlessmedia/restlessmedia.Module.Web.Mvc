using restlessmedia.Module.Category;
using restlessmedia.Module.Security;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace restlessmedia.Module.Web.Mvc.Extensions
{
  public static class SelectExtensions
  {
    public static MvcHtmlString DayList(this HtmlHelper htmlHelper, string name, object htmlAttributes = null)
    {
      return htmlHelper.DropDownList(name, DaysOfTheMonth(), htmlAttributes);
    }

    public static MvcHtmlString MonthList(this HtmlHelper htmlHelper, string name, object htmlAttributes = null)
    {
      return htmlHelper.DropDownList(name, MonthsOfTheYear(), htmlAttributes);
    }

    public static MvcHtmlString YearList(this HtmlHelper htmlHelper, string name, int? from = null, int? to = null, object htmlAttributes = null)
    {
      return htmlHelper.DropDownList(name, Years(from, to), htmlAttributes);
    }

    public static MvcHtmlString DayListFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, object htmlAttributes = null)
    {
      return htmlHelper.DropDownListFor(expression, DaysOfTheMonth(), htmlAttributes);
    }

    public static MvcHtmlString MonthListFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, object htmlAttributes = null)
    {
      return htmlHelper.DropDownListFor(expression, MonthsOfTheYear(), htmlAttributes);
    }

    public static MvcHtmlString YearListFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, int? from = null, int? to = null, object htmlAttributes = null)
    {
      return htmlHelper.DropDownListFor(expression, Years(from, to), htmlAttributes);
    }

    public static MvcHtmlString CategoryListFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, int categoryParentId, object htmlAttributes = null, string emptyOption = null, ListValueType valueType = ListValueType.Value)
    {
      List<SelectListItem> items = new List<SelectListItem>();

      if (!string.IsNullOrEmpty(emptyOption))
      {
        items.Add(new SelectListItem { Text = emptyOption, Value = string.Empty });
      }

      items.AddRange(Resolve<ICategoryService>().List(categoryParentId).Select(x => new SelectListItem
      {
        Text = x.Title,
        Value = valueType == ListValueType.Text ? x.Title : x.CategoryId.HasValue ? x.CategoryId.ToString() : string.Empty
      }));

      return htmlHelper.DropDownListFor(expression, items, htmlAttributes);
    }

    public static MvcHtmlString CurrencyListFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, IEnumerable<decimal> values, object htmlAttributes = null, string emptyOption = null)
    {
      const string format = "{0:c0}";
      return FormattedListFor(htmlHelper, expression, values, format, htmlAttributes, emptyOption);
    }

    public static MvcHtmlString RangeListFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, int from, int to, string format = "0", int step = 1, object htmlAttributes = null, string emptyOption = null)
    {
      return FormattedListFor(htmlHelper, expression, EnumerableHelper.Range(from, to, step), htmlAttributes: htmlAttributes, emptyOption: emptyOption);
    }

    public static MvcHtmlString ValueListFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, IEnumerable values, object htmlAttributes = null, string emptyOption = null)
    {
      return FormattedListFor(htmlHelper, expression, values, htmlAttributes: htmlAttributes, emptyOption: emptyOption);
    }

    public static MvcHtmlString FormattedListFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, IEnumerable values, string format = null, object htmlAttributes = null, string emptyOption = null)
    {
      IList<SelectListItem> items = new List<SelectListItem>(values.Cast<object>().Select(x => new SelectListItem
      {
        Text = string.IsNullOrEmpty(format) ? x.ToString() : string.Format(format, x),
        Value = x.ToString()
      }));

      if (!string.IsNullOrEmpty(emptyOption))
      {
        items.Insert(0, new SelectListItem { Text = emptyOption, Value = string.Empty });
      }

      return htmlHelper.DropDownListFor(expression, items, htmlAttributes);
    }

    public static MvcHtmlString RoleListFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, object htmlAttributes = null, bool multiple = false)
    {
      IEnumerable<SelectListItem> items = Resolve<IRoleService>().List().Select(x => new SelectListItem { Text = x.Name, Value = x.RoleId.ToString() });

      if (multiple)
      {
        return htmlHelper.ListBoxFor(expression, items, htmlAttributes);
      }

      return htmlHelper.DropDownListFor(expression, items, htmlAttributes);
    }

    public static MvcHtmlString UserListFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, object htmlAttributes = null, string emptyOption = null)
    {
      List<SelectListItem> items = new List<SelectListItem>();

      items.AddRange(Resolve<ISecurityService>().ListUsers().Select(x => new SelectListItem { Text = x.Email, Value = x.UniqueId.ToString() }));

      if (!string.IsNullOrEmpty(emptyOption))
      {
        items.Insert(0, new SelectListItem { Text = emptyOption, Value = string.Empty });
      }

      return htmlHelper.DropDownListFor(expression, items, htmlAttributes);
    }

    public static MvcHtmlString EnumListFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, Type enumType, object htmlAttributes = null, string emptyOption = null)
    {
      return ValueListFor(htmlHelper, expression, Enum.GetNames(enumType), htmlAttributes, emptyOption);
    }

    public static MvcHtmlString TitleListFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, object htmlAttributes = null, string emptyOption = null)
    {
      return ValueListFor(htmlHelper, expression, Titles, htmlAttributes, emptyOption);
    }

    public readonly static SelectListItem EmptyOption = new SelectListItem
    {
      Text = "-",
      Value = string.Empty
    };

    public readonly static string[] Titles = new[] { "Mr", "Mrs", "Miss", "Ms" };

    private static IEnumerable<SelectListItem> DaysOfTheMonth()
    {
      return Enumerable.Range(1, 31).Select(x => new SelectListItem { Text = x.ToString(), Value = x.ToString() });
    }

    private static IEnumerable<SelectListItem> MonthsOfTheYear()
    {
      return Enumerable.Range(1, 12).Select(x => new SelectListItem { Text = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(x), Value = x.ToString() });
    }

    private static IEnumerable<SelectListItem> Years(int? from = null, int? to = null)
    {
      int currentYear = DateTime.Now.Year;
      const int offset = 30;
      return Enumerable.Range(from.GetValueOrDefault(currentYear - offset), to.GetValueOrDefault(currentYear + offset)).Select(x => new SelectListItem { Text = x.ToString(), Value = x.ToString() });
    }

    private static T Resolve<T>()
    {
      return DependencyResolver.Current.GetService<T>();
    }
  }
}