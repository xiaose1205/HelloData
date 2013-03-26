#region Version Info
/* ========================================================================
* 【本类功能概述】
* 
* 作者：王军 时间：2013/3/25 20:34:43
* 文件名：ReflectedController
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
using System.Reflection;
using System.Text;

namespace HelloData.Web.AppHandlers
{
    public class ReflectedController
    {
        private static bool IsValidActionMethod(MethodInfo methodInfo)
        {
            return !(methodInfo.IsSpecialName ||
                     methodInfo.GetBaseDefinition().DeclaringType.IsAssignableFrom(typeof(BaseHandler)));
        }

        public List<MethodInfo> Parmses { get; set; }

        public ReflectedController(Type controllerType)
        {
            MethodInfo[] allMethods = controllerType.GetMethods(BindingFlags.InvokeMethod | BindingFlags.Instance | BindingFlags.Public);
            MethodInfo[] infos = Array.FindAll(allMethods, IsValidActionMethod);
            List<MethodInfo> actionParmses = new List<MethodInfo>();
            foreach (var methodInfo in infos)
            {
                actionParmses.Add(methodInfo);
            }
            if (actionParmses.Count != 0)
                Parmses = actionParmses;
            else
                Parmses = null;
        }
    }
}
