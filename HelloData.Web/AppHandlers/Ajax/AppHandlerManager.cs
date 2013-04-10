

using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using HelloData.FWCommon;
using HelloData.Web.AppHandlers;

namespace HelloData.AppHandlers
{
    public class AppHandlerManager
    {
        private static Dictionary<string, IAppHandler> s_Handlers = new Dictionary<string, IAppHandler>(StringComparer.OrdinalIgnoreCase);


        public static void RegisterAppHandler(IAppHandler handler)
        {
            if (s_Handlers.ContainsKey(handler.HandlerName))
                s_Handlers[handler.HandlerName] = handler;
            else
            {
                ReflectedController reflected = new ReflectedController(handler.GetType());
                handler.ActionMethods = reflected.Parmses;
                s_Handlers.Add(handler.HandlerName, handler);
            }
        }

        public static void ExecuteHandler(string controllerName, HttpContext context, string actionName)
        {
            if (!context.Request.ContentType.StartsWith("application/json", StringComparison.OrdinalIgnoreCase))
            {
                context.Response.Write(new HandlerResponse() { Message = "只支持json传输协议", Result = 0 }.ToString());
                return;
            }
            IAppHandler handler;
          
            if (s_Handlers.TryGetValue(controllerName, out handler))
            {
                handler.HttpContext = context;
                //下一步找到Action的参数 
                bool hasAction = false;

                foreach (var action in handler.ActionMethods)
                {
                    if (action.Name.ToLower() == actionName.ToLower())
                    {// 有操作的方法action
                        hasAction = true;
                        ActionExcute excute = new ActionExcute();
                        context.Response.Write(excute.BindParamToAction(action, context, handler).ToString());
                        break;
                    }
                }
                if (!hasAction)
                {
                    context.Response.Write(new HandlerResponse() { Message = "不存在操作方法", Result = 0 }.ToString());
                    return;
                }
            }
            else
            {
                context.Response.Write(new HandlerResponse() { Message = "不存在操作对象", Result = 0 }.ToString());

            }
            //  context.Response.Write(handler.CreateInstance().ProcessRequest(context).ToString());
        }




    }
}