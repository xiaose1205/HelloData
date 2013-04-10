#region Version Info
/* ========================================================================
* 【本类功能概述】
* 
* 作者：王军 时间：2013/3/31 21:15:29
* 文件名：ConditionBuilder
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
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace HelloData.FrameWork.Data.Linq
{
    public class ConditionBuilder : ExpressionVisitor
    {
        private List<object> m_arguments;

        private Stack<string> m_conditionParts;
        public List<object> ConObjects { get; set; }
        public string Condition { get; private set; }

        public object[] Arguments { get; private set; }

        public void Build(Expression expression)
        {
            PartialEvaluator evaluator = new PartialEvaluator();
            Expression evaluatedExpression = evaluator.Eval(expression);

            this.m_arguments = new List<object>();
            this.m_conditionParts = new Stack<string>();
            ConObjects = new List<object>();
            this.Visit(evaluatedExpression);

            this.Arguments = this.m_arguments.ToArray();
            if (evaluatedExpression.NodeType != ExpressionType.NewArrayInit)
                this.Condition = this.m_conditionParts.Count > 0 ? this.m_conditionParts.Pop() : null;
            else
            {
             
                foreach (var mConditionPart in m_conditionParts)
                {
                    this.Condition += mConditionPart + " ,";
                }
                this.Condition = this.Condition.Trim(',');
            }

        }

        protected override Expression VisitBinary(BinaryExpression b)
        {
            if (b == null) return b;

            string opr;
            switch (b.NodeType)
            {
                case ExpressionType.Equal:
                    opr = "=";
                    break;
                case ExpressionType.NotEqual:
                    opr = "<>";
                    break;
                case ExpressionType.GreaterThan:
                    opr = ">";
                    break;
                case ExpressionType.GreaterThanOrEqual:
                    opr = ">=";
                    break;
                case ExpressionType.LessThan:
                    opr = "<";
                    break;
                case ExpressionType.LessThanOrEqual:
                    opr = "<=";
                    break;
                case ExpressionType.AndAlso:
                    opr = "AND";
                    break;
                case ExpressionType.OrElse:
                    opr = "OR";
                    break;
                case ExpressionType.Add:
                    opr = "+";
                    break;
                case ExpressionType.Subtract:
                    opr = "-";
                    break;
                case ExpressionType.Multiply:
                    opr = "*";
                    break;
                case ExpressionType.Divide:
                    opr = "/";
                    break;
                default:
                    throw new NotSupportedException(b.NodeType + "is not supported.");
            }

            this.Visit(b.Left);
            this.Visit(b.Right);

            string right = this.m_conditionParts.Pop();
            string left = this.m_conditionParts.Pop();

            string condition = String.Format("({0} {1} {2})", left, opr, right);
            this.m_conditionParts.Push(condition);

            return b;
        }

        protected override Expression VisitConstant(ConstantExpression c)
        {
            if (c == null) return c;
            if (c.Value.GetType() == typeof (BaseEntity))
            {
                this.ConObjects.Add(c.Value);
                return c;
            }
            this.m_arguments.Add(c.Value);
            this.m_conditionParts.Push(String.Format("{{{0}}}", this.m_arguments.Count - 1));

            return c;
        }

        protected override Expression VisitMemberAccess(MemberExpression m)
        {
            if (m == null) return m;

            PropertyInfo propertyInfo = m.Member as PropertyInfo;
            if (propertyInfo == null) return m;

            this.m_conditionParts.Push(String.Format("[{0}]", propertyInfo.Name));
            ConObjects.Add(propertyInfo.Name);
            return m;
        }
    }
}
