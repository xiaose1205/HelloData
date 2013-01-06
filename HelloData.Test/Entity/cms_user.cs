using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HelloData.FrameWork.Data;

namespace HelloData.Test.Entity
{
    /// <summary>
    ///	
    /// </summary>
    [Serializable]
    public class cms_user : BaseEntity
    {
        public cms_user()
        {
            base.SetIni(this, "cms_user");
        }
        /// <summary>
        ///	
        /// </summary>
        public string username { get; set; }
        /// <summary>
        ///	
        /// </summary>
        public string password { get; set; }
        /// <summary>
        /// 手机号码
        /// </summary>
        public string phone { get; set; }
        /// <summary>
        ///	
        /// </summary>
        public DateTime? logintime { get; set; }
        /// <summary>
        ///	
        /// </summary>
        public DateTime? createtime { get; set; }
        /// <summary>
        ///	
        /// </summary>
        public UInt64 isactive { get; set; }
        public int mangerid { get; set; }

        /// <summary>
        /// 是否是管理员
        /// </summary>
        public bool isadmin { get; set; }


        /// <summary>
        ///	
        /// </summary>
        [Column(NoSqlProperty = true)]
        public string logintimestr
        {
            get
            {
                if (logintime.HasValue)
                    return logintime.Value.ToString("yyyy-MM-dd HH:mm:ss");
                return "";
            }
        }
        /// <summary>
        ///	
        /// </summary>
        [Column(NoSqlProperty = true)]
        public string createtimestr
        {
            get
            {
                if (createtime.HasValue)
                    return createtime.Value.ToString("yyyy-MM-dd HH:mm:ss");
                return "";
            }
        }
        public static class Columns
        {
            public const string id = "id";
            public const string username = "username";
            public const string password = "password";
            public const string logintime = "logintime";
            public const string createtime = "createtime";
            public const string isactive = "isactive";
            public const string phone = "phone";
            public const string isadmin = "isadmin";
            public const string mangerid = "mangerid";
        }
    }
}
