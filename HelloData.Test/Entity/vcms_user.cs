using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HelloData.FrameWork.Data;

namespace HelloData.Test.Entity
{
    /// <summary>
    /// 视图操作
    /// </summary>
    public class vcms_user : BaseEntity
    {
        public vcms_user()
        {
            base.SetIni(this, "cms_user");
        }
    }
}
