using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HelloData.Web.Settings
{
    public class SiteSettings : SettingBase
    {
        public static string WebName { get; set; }


        public static string TitleAttach { get; set; }

        public static string MetaKeywords { get; set; }

        public static string MetaDescription { get; set; }
    }
}
