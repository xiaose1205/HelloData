#region Version Info
/* ========================================================================
* 【本类功能概述】
* 
* 作者：王军 时间：2013/4/4 23:08:46
* 文件名：NullEntity
* 版本：V1.0.1
* 联系方式：511522329  
*
* 修改者： 时间： 
* 修改说明：
* ========================================================================
*/
#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HelloData.FrameWork.Data
{
    public class NullEntity : BaseEntity
    {
        public NullEntity()
        {
            TableType = Enum.TableTyleEnum.None;
            base.SetIni(this, string.Empty);
        }
    }
}
