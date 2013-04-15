using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace HelloData.FWCommon.AOP
{
    public interface IMethodBeforeAdvice : IBeforeAdvice
    {
        void Before(MethodInfo method, object[] args, object target);
    }
}
