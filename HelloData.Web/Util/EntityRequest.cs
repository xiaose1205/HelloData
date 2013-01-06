using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HelloData.Web.Util
{
    public class EntityRequest<T>: where new()
    {

        public T FromRequest(System.Web.HttpRequest reqquest)
        {
            if(reqquest ==null)
                return default(T);
        }
    }
}
