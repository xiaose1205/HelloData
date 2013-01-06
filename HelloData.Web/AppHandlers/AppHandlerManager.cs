

using System;
using System.Collections.Generic;
using System.Text;
using System.Web;

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
                s_Handlers.Add(handler.HandlerName, handler);
        }

        public static void ExecuteHandler(string name, HttpContext context)
        {
            IAppHandler handler;

            if (s_Handlers.TryGetValue(name, out handler))
                handler.CreateInstance().ProcessRequest(context);
        }
    }
}