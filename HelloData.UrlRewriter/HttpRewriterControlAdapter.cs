using System.Web;
using System.Web.UI;
using System.IO;

namespace HelloData.UrlRewriter
{
    public class HttpRewriterControlAdapter :
        System.Web.UI.Adapters.ControlAdapter
    {
        protected override void Render(HtmlTextWriter writer)
        {
            base.Render(new RewriteFormHtmlTextWriter(writer));
        }
    }

    public class RewriteFormHtmlTextWriter : HtmlTextWriter
    {
        public RewriteFormHtmlTextWriter(HtmlTextWriter writer)
            : base(writer)
        {
            this.InnerWriter = writer.InnerWriter;
        }

        public RewriteFormHtmlTextWriter(TextWriter writer)
            : base(writer)
        {
            this.InnerWriter = writer;
        }

        public override void WriteAttribute(string name, string value, bool fEncode)
        {
            if (name == "action")
            {
                HttpContext context = HttpContext.Current;

                if (context.Items["ActionAlreadyWritten"] == null)
                {
                    value = context.Request.ServerVariables["HTTP_X_REWRITE_URL"]
                        ?? context.Request.RawUrl;
                    context.Items["ActionAlreadyWritten"] = true;
                }
            }

            base.WriteAttribute(name, value, fEncode);
        }
    }
}
