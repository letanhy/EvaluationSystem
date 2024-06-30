using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Reflection.Emit;
using System.Security.AccessControl;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.Optimization;
using System.Web.UI.WebControls;
using System.Xml.Serialization;

namespace EvaluationSystem.Helper
{
    public enum DropdownListStyle
    {
        [Display(Name = "")]
        DropdownListStyleDefault = 0,
        [Display(Name = "chosen-select")]
        DropdownListStyleChosen = 1,
    }
    public enum ButtonType
    {
        [Display(Name = "button")]
        Default = 0,
        [Display(Name = "submit")]
        Submit = 1,
    }
    public static class HtmlHelperExtensions
    {
        public static string GetName(this Enum type)
        {
            var enumType = type.GetType();
            var info = enumType.GetField(type.ToString());
            if (info == null)
                return string.Empty;
            var displayAttribute = info.GetCustomAttributes(false).OfType<DisplayAttribute>().FirstOrDefault();
            var value = string.Empty;
            if (displayAttribute != null)
                value = displayAttribute.GetName() ?? string.Empty;

            return value;
        }

        public static string GetEnumDescription(this Enum value)
        {
            FieldInfo fi = value.GetType().GetField(value.ToString());

            DescriptionAttribute[] attributes = fi.GetCustomAttributes(typeof(DescriptionAttribute), false) as DescriptionAttribute[];

            if (attributes != null && attributes.Any())
            {
                return attributes.First().Description;
            }

            return value.ToString();
        }
        public static MvcHtmlString ValidationErrorFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, string error)
        {
            if (HasError(htmlHelper, ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData), ExpressionHelper.GetExpressionText(expression)))
                return new MvcHtmlString(error);
            else
                return null;
        }
        private static bool HasError(this HtmlHelper htmlHelper, ModelMetadata modelMetadata, string expression)
        {
            string modelName = htmlHelper.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(expression);
            FormContext formContext = htmlHelper.ViewContext.FormContext;
            if (formContext == null)
                return false;

            if (!htmlHelper.ViewData.ModelState.ContainsKey(modelName))
                return false;

            ModelState modelState = htmlHelper.ViewData.ModelState[modelName];
            if (modelState == null)
                return false;

            ModelErrorCollection modelErrors = modelState.Errors;
            if (modelErrors == null)
                return false;

            return (modelErrors.Count > 0);
        }
        //#region CustomDropDownList
        //public static MvcHtmlString CustomDropDownListFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression,
        //    IEnumerable<SelectListItem> selectList, string emptyAlertString = null, DropdownListStyle style = DropdownListStyle.DropdownListStyleChosen,
        //    string labelClass = "col-md-2", string controlClass = "col-md-10", bool isMultiple = false)
        //{
        //    var result = htmlHelper.ViewData.Model != null ? ((LambdaExpression)expression).Compile().DynamicInvoke(htmlHelper.ViewData.Model) : null;
        //    var fullHtmlFieldName = ExpressionHelper.GetExpressionText(expression);

        //    var titleUI = htmlHelper.LabelFor(expression, new { @class = "col-form-label " + labelClass });

        //    StringBuilder inputSB = new StringBuilder();
        //    if (style == DropdownListStyle.DropdownListStyleChosen)
        //    {
        //        inputSB.Append(htmlHelper.Hidden("defaultValue" + fullHtmlFieldName, result).ToHtmlString());
        //    }
        //    MvcHtmlString inputUI;

        //    if (!isMultiple)
        //    {
        //        inputUI = htmlHelper.DropDownListFor(expression, selectList, emptyAlertString, new { @class = "form-control form-select " + EnumHelper<DropdownListStyle>.GetDisplayValue(style) });
        //    }
        //    else
        //    {
        //        inputUI = htmlHelper.ListBoxFor(expression, selectList, new { @class = "form-select " + EnumHelper<DropdownListStyle>.GetDisplayValue(style), @multiple = true });
        //    }

        //    inputSB.Append(inputUI.ToHtmlString());

        //    ViewDataDictionary viewDataDictionary = new ViewDataDictionary { { "inputUI", MvcHtmlString.Create(inputSB.ToString()) }, { "titleUI", titleUI }, { "controlClass", controlClass } };
        //    viewDataDictionary.Add("validUI", htmlHelper.ValidationMessageFor(expression, null, new { @class = "text-danger" }));
        //    return htmlHelper.Partial("_ComponentInputPartial", viewDataDictionary);
        //}
        //#endregion

        public static MvcHtmlString CreateHeaderTitle(this HtmlHelper htmlHelper, string title)
        {
            ViewDataDictionary viewDataDictionary = new ViewDataDictionary
            {
                {"title", title},
            };
            return htmlHelper.Partial("_HeaderTitlePartial", viewDataDictionary);
        }
        public static MvcHtmlString CreateButton(this HtmlHelper htmlHelper, string text, ButtonType type = ButtonType.Default, string buttonClass = "btn btn-sm btn-primary", string icon = "", object htmlAttributes = null)
        {
            string attribute = "";
            IDictionary<string, object> htmlAttributesDic = HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes);
            if (htmlAttributesDic != null)
            {
                foreach (var item in htmlAttributesDic)
                {
                    attribute += " " + item.Key + "=\"" + item.Value + "\"";
                }
            }
            ViewDataDictionary viewDataDictionary = new ViewDataDictionary
            {
                {"text", text},
                {"buttonClass", buttonClass},
                {"icon", icon},
                {"type", type.ToString()},
                {"attribute", attribute }
            };
            return htmlHelper.Partial("_ComponentButtonPartial", viewDataDictionary);
        }
        public static MvcHtmlString CustomTextboxFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, object htmlAttributes = null, string labelClass = "col-md-2", string controlClass = "col-md-10", string inputClass = "")
        {
            IDictionary<string, object> htmlAttributesDic = HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes);

            var fullHtmlFieldName = ExpressionHelper.GetExpressionText(expression);
            var titleUI = htmlHelper.LabelFor(expression, new { @class = "col-form-label " + labelClass });

            var attribute = new Dictionary<string, object>();
            attribute.Add("class", "form-control " + inputClass);
            attribute.Add("autocomplete", "off");
            if (htmlAttributesDic != null)
            {
                attribute = htmlAttributesDic.Concat(attribute.Where(x => !htmlAttributesDic.Keys.Contains(x.Key))).ToDictionary(c => c.Key, c => c.Value);
            }
            string inputUI = htmlHelper.TextBoxFor(expression, attribute).ToHtmlString();
            StringBuilder inputUISB = new StringBuilder();
            inputUISB.Append(inputUI);
            ViewDataDictionary viewDataDictionary = new ViewDataDictionary { { "inputUI", MvcHtmlString.Create(inputUISB.ToString()) }, { "titleUI", titleUI }, { "controlClass", controlClass } };
            viewDataDictionary.Add("validUI", htmlHelper.ValidationMessageFor(expression, "", new { @class = "text-danger" }));
            return htmlHelper.Partial("_ComponentInputPartial", viewDataDictionary);
        }
        public static MvcHtmlString CustomTextViewFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, string titleClass = "col-md-3", string textClass = "col-md-9", string dateFormat = "dd/MM/yyyy")
        {

            ModelMetadata metadata = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);
            var property = ExpressionHelper.GetExpressionText(expression);

            string title = metadata.DisplayName ?? metadata.PropertyName ?? property.Split('.').Last();

            var method = expression.Compile();
            var value = method(htmlHelper.ViewData.Model);


            StringBuilder item = new StringBuilder();
            item.Append("<dt class=\"" + titleClass + "\">" + title + "</dt>");
            if (value is DateTime dateTimeValue)
            {
                item.Append("<dd class=\"" + textClass + "\">" + dateTimeValue.ToString(dateFormat) + " </dd>");
            }
            else
            {
                item.Append("<dd class=\"" + textClass + "\">" + value + " </dd>");
            }

            return MvcHtmlString.Create(item.ToString());
        }
        public static MvcHtmlString CustomTextView(this HtmlHelper htmlHelper, string title, object value, string titleClass = "col-md-3", string textClass = "col-md-9")
        {
            StringBuilder item = new StringBuilder();
            item.Append("<dt class=\"" + titleClass + "\">" + title + "</dt>");
            item.Append("<dd class=\"" + textClass + "\">" + value + "</dd>");

            return MvcHtmlString.Create(item.ToString());
        }

        public static MvcHtmlString CustomSwitchesFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression)
        {
            var result = htmlHelper.ViewData.Model != null ? ((LambdaExpression)expression).Compile().DynamicInvoke(htmlHelper.ViewData.Model) : false;
            result = result != null ? result : false;
            var titleUI = htmlHelper.LabelFor(expression, new { @class = "form-check-label" });
            var fullHtmlFieldName = ExpressionHelper.GetExpressionText(expression);
            string inputUI = htmlHelper.CheckBox(fullHtmlFieldName, (bool)result, new { @class = "form-check-input" }).ToHtmlString();

            ViewDataDictionary viewDataDictionary = new ViewDataDictionary { { "inputUI", htmlHelper.Raw(inputUI) }, { "titleUI", titleUI } };


            viewDataDictionary.Add("validUI", htmlHelper.ValidationMessageFor(expression, null, new { @class = "text-danger" }));
            return htmlHelper.Partial("_ComponentSwitchPartial", viewDataDictionary);
        }
        public static MvcHtmlString CustomSwitches(this HtmlHelper htmlHelper, string name, string title, bool? value)
        {
            var result = value != null ? value : false;
            result = result != null ? result : false;
            var titleUI = htmlHelper.Label("", title, new { @class = "form-check-label" });
            var fullHtmlFieldName = ExpressionHelper.GetExpressionText(name);
            string inputUI = htmlHelper.CheckBox(fullHtmlFieldName, (bool)result, new { @class = "form-check-input" }).ToHtmlString();

            ViewDataDictionary viewDataDictionary = new ViewDataDictionary { { "inputUI", htmlHelper.Raw(inputUI) }, { "titleUI", titleUI } };
            return htmlHelper.Partial("_ComponentSwitchPartial", viewDataDictionary);
        }
        public static MvcHtmlString CustomTextboxForSelect2<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, string placeHolder, bool IsCheckValidation, string url = "", string url2 = "", string parameter = "Name", string functionSelect = "", string labelClass = "col-md-2", string controlClass = "col-md-10")
        {
            var result = htmlHelper.ViewData.Model != null ? ((LambdaExpression)expression).Compile().DynamicInvoke(htmlHelper.ViewData.Model) : string.Empty;
            result = result == null ? string.Empty : result;
            var fullHtmlFieldName = ExpressionHelper.GetExpressionText(expression);
            StringBuilder inputUISB = new StringBuilder();
            inputUISB.Append(htmlHelper.Hidden(fullHtmlFieldName, result).ToHtmlString());
            var selectListItems = new List<SelectListItem>(); ;
            var inputUI = htmlHelper.DropDownListFor(expression, selectListItems, new { @class = "form-control form-select js-data-" + fullHtmlFieldName + "-ajax" });
            inputUISB.Append(inputUI);
            var titleUI = htmlHelper.LabelFor(expression, new { @class = "col-form-label " + labelClass });
            var validErrorClass = htmlHelper.ValidationErrorFor(expression, "error");
            ViewDataDictionary viewDataDictionary = new ViewDataDictionary { { "inputUI", MvcHtmlString.Create(inputUISB.ToString()) }, { "titleUI", titleUI }, { "value", result }, { "validError", validErrorClass }, { "controlClass", controlClass }, { "url", url }, { "placeHolder", placeHolder }, { "fieldName", fullHtmlFieldName }, { "url2", url2 }, { "parameter", parameter }, { "functionSelect", functionSelect } };
            if (IsCheckValidation)
            {
                viewDataDictionary.Add("validUI", htmlHelper.ValidationMessageFor(expression, null, new { @class = "help-inline" }));
            }
            return htmlHelper.Partial("_Select2InputFieldTemplatePartial", viewDataDictionary);
        }

        public static MvcHtmlString ScriptTop_TreeStyle(this HtmlHelper htmlHelper)
        {
            string url = UrlHelper.GenerateContentUrl("~/Scripts/jstree/themes/default/style.min.css", htmlHelper.ViewContext.HttpContext);
            return MvcHtmlString.Create("<link href=\"" + url + "\" rel=\"stylesheet\" type=\"text/css\" />");
        }
        public static MvcHtmlString ScriptBottom_TreeStyle(this HtmlHelper htmlHelper)
        {
            string url = UrlHelper.GenerateContentUrl("~/Scripts/jstree/jstree.min.js", htmlHelper.ViewContext.HttpContext);
            return MvcHtmlString.Create("<script src=\"" + url + "\"/></script>");
        }
        public static MvcHtmlString ScriptTop_ChosenStyle(this HtmlHelper htmlHelper)
        {
            string url = UrlHelper.GenerateContentUrl("~/Scripts/chosen/chosen.min.css", htmlHelper.ViewContext.HttpContext);
            return MvcHtmlString.Create("<link href=\"" + url + "\" rel=\"stylesheet\" type=\"text/css\" />");
        }
        public static MvcHtmlString ScriptBottom_ChosenStyle(this HtmlHelper htmlHelper)
        {
            return htmlHelper.Partial("_ScriptBottom_ChosenStylePartial");
        }

        public static MvcHtmlString ScriptTop_Gridmvc(this HtmlHelper htmlHelper)
        {
            string url = UrlHelper.GenerateContentUrl("~/assets/Gridmvc/Gridmvc.css", htmlHelper.ViewContext.HttpContext);
            return MvcHtmlString.Create("<link href=\"" + url + "\" rel=\"stylesheet\" type=\"text/css\" />");
        }
        public static MvcHtmlString ScriptBottom_Gridmvc(this HtmlHelper htmlHelper)
        {
            string url = UrlHelper.GenerateContentUrl("~/Scripts/Gridmvc/gridmvc.min.js", htmlHelper.ViewContext.HttpContext);
            return MvcHtmlString.Create("<script src=\"" + url + "\"/></script>");
        }
        public static MvcHtmlString ScriptBottom_ValidationMvc(this HtmlHelper htmlHelper)
        {
            return htmlHelper.Partial("_ScriptBottom_ValidationMvcPartial");
        }
        public static MvcHtmlString ScriptBottom_Select2(this HtmlHelper htmlHelper)
        {
            return htmlHelper.Partial("_ScriptBottom_Select2Partial");
        }
        public static MvcHtmlString ScriptTop_Select2(this HtmlHelper htmlHelper)
        {
            string url = UrlHelper.GenerateContentUrl("~/Scripts/select2/select2.min.css", htmlHelper.ViewContext.HttpContext);
            return MvcHtmlString.Create("<link href=\"" + url + "\" rel=\"stylesheet\" type=\"text/css\" />");
        }
        public static MvcHtmlString ScriptTop_SweetAlert2(this HtmlHelper htmlHelper)
        {
            string url = UrlHelper.GenerateContentUrl("~/Scripts/SweetAlert2/sweetalert2.css", htmlHelper.ViewContext.HttpContext);
            string url2 = UrlHelper.GenerateContentUrl("~/Scripts/SweetAlert2/animate.css", htmlHelper.ViewContext.HttpContext);
            return MvcHtmlString.Create("<link href=\"" + url + "\" rel=\"stylesheet\" type=\"text/css\" />" +
                "<link href=\"" + url2 + "\" rel=\"stylesheet\" type=\"text/css\" />");
        }
        public static MvcHtmlString ScriptBottom_SweetAlert2(this HtmlHelper htmlHelper)
        {
            string url = UrlHelper.GenerateContentUrl("~/Scripts/SweetAlert2/sweetalert2.min.js", htmlHelper.ViewContext.HttpContext);
            string url2 = UrlHelper.GenerateContentUrl("~/Scripts/SweetAlert2/sweetalert2_Custom.js", htmlHelper.ViewContext.HttpContext);
            return MvcHtmlString.Create("<script src =\"" + url + "\"/></script>" +
                "<script src=\"" + url2 + "\"/></script>");
        }
    }
}