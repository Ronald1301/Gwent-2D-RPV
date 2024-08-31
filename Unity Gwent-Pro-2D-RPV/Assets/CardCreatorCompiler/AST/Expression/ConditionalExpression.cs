

using System.Runtime.InteropServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Gwent
{

    public class ConditionalExpression : Expression
    {
        public readonly Expression Condition ;
        public readonly Statement TrueExpression ;
        public readonly Statement FalseExpression ;
        protected override Scope? Context { get; set; }

        public ConditionalExpression(Expression condition, Statement trueExpression, Statement falseExpression)
        {
            Condition = condition;
            TrueExpression = trueExpression;
            FalseExpression = falseExpression;
        }

        public override Scope.DataType CheckSemantic()
        {
            if (Condition.CheckSemantic() != Scope.DataType.Boolean)
            {
                Additional.errors.Add(new TypeError(ErrorCode.SemanticError, "The condition must be a boolean expression"));
            }
            var trueType = TrueExpression.CheckSemantic();
            var falseType = FalseExpression.CheckSemantic();
            if (trueType != falseType)
            {
                Additional.errors.Add(new TypeError(ErrorCode.SemanticError, "The types of the expressions must be the same"));
            }
            return Scope.DataType.Unknown;
        }

        public override object Evaluate()
        {
            if ((bool)Condition.Evaluate())
            {
                return TrueExpression.Evaluate();
            }
            else
                return FalseExpression.Evaluate();
        }

        public override void SetScope(Scope current)
        {
            Context = current;
            Scope son=new(current,new(),new());
            Condition.SetScope(son);
            TrueExpression.SetScope(son);
            FalseExpression.SetScope(son);
        }
    }
}