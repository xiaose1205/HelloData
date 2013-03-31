#region Version Info
/* ========================================================================
* 【本类功能概述】
* 
* 作者：王军 时间：2013/3/31 21:16:01
* 文件名：PartialEvaluator
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
using System.Text;

namespace HelloData.FrameWork.Data.Linq
{
    public class PartialEvaluator : ExpressionVisitor
    {
        private Func<Expression, bool> m_fnCanBeEvaluated;
        private HashSet<Expression> m_candidates;

        public PartialEvaluator()
            : this(CanBeEvaluatedLocally)
        { }

        public PartialEvaluator(Func<Expression, bool> fnCanBeEvaluated)
        {
            this.m_fnCanBeEvaluated = fnCanBeEvaluated;
        }

        public Expression Eval(Expression exp)
        {
            this.m_candidates = new Nominator(this.m_fnCanBeEvaluated).Nominate(exp);

            return this.Visit(exp);
        }

        protected override Expression Visit(Expression exp)
        {
            if (exp == null)
            {
                return null;
            }

            if (this.m_candidates.Contains(exp))
            {
                return this.Evaluate(exp);
            }

            return base.Visit(exp);
        }

        private Expression Evaluate(Expression e)
        {
            if (e.NodeType == ExpressionType.Constant)
            {
                return e;
            }

            LambdaExpression lambda = Expression.Lambda(e);
            Delegate fn = lambda.Compile();

            return Expression.Constant(fn.DynamicInvoke(null), e.Type);
        }

        private static bool CanBeEvaluatedLocally(Expression exp)
        {
            return exp.NodeType != ExpressionType.Parameter;
        }

        #region Nominator

        /// <summary>
        /// Performs bottom-up analysis to determine which nodes can possibly
        /// be part of an evaluated sub-tree.
        /// </summary>
        private class Nominator : ExpressionVisitor
        {
            private Func<Expression, bool> m_fnCanBeEvaluated;
            private HashSet<Expression> m_candidates;
            private bool m_cannotBeEvaluated;

            internal Nominator(Func<Expression, bool> fnCanBeEvaluated)
            {
                this.m_fnCanBeEvaluated = fnCanBeEvaluated;
            }

            internal HashSet<Expression> Nominate(Expression expression)
            {
                this.m_candidates = new HashSet<Expression>();
                this.Visit(expression);
                return this.m_candidates;
            }

            protected override Expression Visit(Expression expression)
            {
                if (expression != null)
                {
                    bool saveCannotBeEvaluated = this.m_cannotBeEvaluated;
                    this.m_cannotBeEvaluated = false;

                    base.Visit(expression);

                    if (!this.m_cannotBeEvaluated)
                    {
                        if (this.m_fnCanBeEvaluated(expression))
                        {
                            this.m_candidates.Add(expression);
                        }
                        else
                        {
                            this.m_cannotBeEvaluated = true;
                        }
                    }

                    this.m_cannotBeEvaluated |= saveCannotBeEvaluated;
                }

                return expression;
            }
        }

        #endregion
    }
}

