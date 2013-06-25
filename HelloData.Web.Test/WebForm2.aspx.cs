using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HelloData.Web.Page;

namespace HelloData.Web.Test
{
    public class user
    {
        public string username { get; set; }
        public int? sex { get; set; }
        public bool? ck { get; set; }
    }

    public partial class WebForm2 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack)
            {
                user ss = new user() { sex = 222, username = "ssssss" };
                //    RequestVariable.Current.BindObjectToControl("lb_", this, ss);
             //   ss = RequestVariable.Current.BindFormToObject<user>("lb_");
            }
        }
    }
}