using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HelloData.FrameWork.Data.Enum
{
    /// <summary>
    /// 视图连接查询连接点
    /// </summary>
    public enum ViewJoinEnum
    {
        innerjoin = 0,
        leftjoin = 1,
        rightjoin = 2,
        fulljoin = 4,
        crossjoin = 8,
    }
}
