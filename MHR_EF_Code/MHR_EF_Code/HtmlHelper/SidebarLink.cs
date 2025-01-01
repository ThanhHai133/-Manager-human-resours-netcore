using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace MHR_EF_Code.HtmlHelpers
{
    public static class SidebarLinkHelper
    {
        public static IHtmlContent GenerateSidebarLink(this IHtmlHelper htmlHelper, string href, string iconName, string linkText)
        {
            // Tạo thẻ <a>
            var link = new TagBuilder("a");
            link.MergeAttribute("href", href); // Sử dụng tham số href
            link.AddCssClass("sidebar-link");

            // Tạo thẻ <i> cho icon
            var icon = new TagBuilder("i");
            icon.AddCssClass("material-symbols-outlined");
            icon.InnerHtml.Append(iconName);

            // Tạo thẻ <span> cho text
            var text = new TagBuilder("span");
            text.InnerHtml.Append(linkText);

            // Kết hợp icon và text vào trong thẻ <a>
            link.InnerHtml.AppendHtml(icon);
            link.InnerHtml.AppendHtml(text);

            // Trả về thẻ <a> dưới dạng IHtmlContent
            return link;
        }
    }
}
