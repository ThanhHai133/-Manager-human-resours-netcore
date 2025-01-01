using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Text;
using System.IO;

namespace MHR_EF_Code.HtmlHelpers
{
    public static class InputHelper
    {
        public static IHtmlContent InputHelp(this IHtmlHelper htmlHelper, string cssClassName, string field, string label, string href, string iconName, string linkText)
        {
            var outerDiv = new TagBuilder("div");
            outerDiv.AddCssClass(cssClassName);

            var labelTag = new TagBuilder("label");
            labelTag.MergeAttribute("for", field);
            labelTag.InnerHtml.Append(label);

            var inputTag = new TagBuilder("input");
            inputTag.AddCssClass("form-control");
            inputTag.MergeAttribute("type", "text");
            inputTag.MergeAttribute("id", field);
            inputTag.MergeAttribute("name", field);
            inputTag.MergeAttribute("placeholder", label);

            // Sử dụng StringWriter để chuyển đổi TagBuilder thành chuỗi
            var stringBuilder = new StringBuilder();
            using (var writer = new StringWriter(stringBuilder))
            {
                labelTag.WriteTo(writer, System.Text.Encodings.Web.HtmlEncoder.Default);
                inputTag.WriteTo(writer, System.Text.Encodings.Web.HtmlEncoder.Default);
            }
            outerDiv.InnerHtml.AppendHtml(stringBuilder.ToString());

            return outerDiv;
        }
    }
}
