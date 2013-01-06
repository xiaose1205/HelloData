using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HelloData.Web.Settings
{
    /// <summary>
    /// 自定义设置项类型必须实现的接口
    /// </summary>
    public interface ISettingItem
    {
        string GetValue();
        void SetValue(string value);
    }
}
