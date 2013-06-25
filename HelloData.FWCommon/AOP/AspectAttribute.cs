using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HelloData.FWCommon.AOP
{
    public abstract class BaseAttribute : Attribute
    {

    }
    public abstract class AspectAttribute : BaseAttribute
    {
        public virtual void Action(InvokeContext context) { }
    }

    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
    public abstract class PreAspectAttribute : AspectAttribute
    {
    }

    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
    public abstract class PostAspectAttribute : AspectAttribute
    {
    }
    /// <summary>
    /// 异常捕捉
    /// </summary>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
    public abstract class ExceptionAspectAttribute : AspectAttribute
    {
    }
    /// <summary>
    /// 环绕
    /// </summary>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
    public abstract class ArroundAttribute : BaseAttribute
    {
        public virtual void BeginAction(InvokeContext context) { }
        public virtual void EndAction(InvokeContext context) { }
    }
}
