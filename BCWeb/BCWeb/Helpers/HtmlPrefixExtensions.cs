using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace BCWeb.Helpers.Html
{
    /// <summary>
    /// based on http://blog.stevensanderson.com/2010/01/28/editing-a-variable-length-list-aspnet-mvc-2-style/
    /// used for client guardian/contact info
    /// </summary>
    public static class HtmlPrefixExtensions
    {


        public static IDisposable BeginCollectionItem(this HtmlHelper html, string collectionName)
        {
            //var idsToReuse = GetIdsToReuse(html.ViewContext.HttpContext, collectionName);
            string itemIndex = Guid.NewGuid().ToString(); //idsToReuse.Count > 0 ? idsToReuse.Dequeue() : Guid.NewGuid().ToString();

            // autocomplete="off" is needed to work around a very annoying Chrome behaviour 
            // whereby it reuses old values after the user clicks "Back", which causes the xyz.index and xyz[...] values to get out of sync.
            html.ViewContext.Writer.WriteLine(string.Format("<input type=\"hidden\" name=\"{0}.index\" autocomplete=\"off\" value=\"{1}\" />", collectionName, html.Encode(itemIndex)));

            return BeginHtmlFieldPrefixScope(html, string.Format("{0}[{1}]", collectionName, itemIndex));
        }

        public static IDisposable BeginHtmlFieldPrefixScope(this HtmlHelper html, string htmlFieldPrefix)
        {
            return new HtmlFieldPrefixScope(html.ViewData.TemplateInfo, htmlFieldPrefix);
        }

        private class HtmlFieldPrefixScope : IDisposable
        {
            private readonly TemplateInfo templateInfo;
            private readonly string previousHtmlFieldPrefix;

            public HtmlFieldPrefixScope(TemplateInfo templateInfo, string htmlFieldPrefix)
            {
                this.templateInfo = templateInfo;

                previousHtmlFieldPrefix = templateInfo.HtmlFieldPrefix;
                templateInfo.HtmlFieldPrefix = htmlFieldPrefix;
            }

            public void Dispose()
            {
                templateInfo.HtmlFieldPrefix = previousHtmlFieldPrefix;
            }
        }
    }
}