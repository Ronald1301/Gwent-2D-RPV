
using System.Runtime.InteropServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Gwent
{
    public class LambdaExpression : Expression
    {
        public LambdaExpression(List<Expression> Params, Expression body)
        {
            this.Params = Params;
            if (Params.Count == 1)
            {
                this.Type = DelegateType.Predicate;
            }
            else
            {
                this.Type = DelegateType.Action;
            }
            this.Body = body;
        }
        public List<Expression> Params { get; }
        public DelegateType Type { get; }
        public Expression Body { get; }
        protected override Scope? Context { get; set; }

        public enum DelegateType
        {
            Predicate, Action
        }
        public override Scope.DataType CheckSemantic()
        {
            foreach (IDExpression item in Params)
            {
                if (Context!.Items.ContainsKey(item))
                {
                    Additional.errors.Add(new TypeError(ErrorCode.SemanticError, "The parameter is already defined"));
                }
                Context!.Items.Add(item, null!);
            }
            if (Type == DelegateType.Predicate)
            {
                if (Body.CheckSemantic() == Scope.DataType.Boolean)
                {
                    return Scope.DataType.Boolean;
                }
                Additional.errors.Add(new TypeError(ErrorCode.SemanticError, "The body of the lambda must be a boolean expression"));
            }
            else
            {
                Body.CheckSemantic();
            }
            // Add a return statement here
            return Scope.DataType.Void;
        }

        public override object Evaluate()
        {
            if (Type == DelegateType.Predicate)
            {
                Predicate<List<Token>> predicate = new Predicate<List<Token>>(tokens =>
                {
                    // Implement the logic for the predicate here
                    return (bool)Body.Evaluate();
                });
                return predicate;
            }
            else
            {
                new Action<object>(param => Body.Evaluate()).Invoke(Params);
            }
            return null!;
        }
        public override void SetScope(Scope current)
        {
            Context = new(current, new(), new());
            foreach (IDExpression item in Params)
            {
                Context.Items.Add(item, null!);
            }
            Body.SetScope(Context);
        }
    }
}