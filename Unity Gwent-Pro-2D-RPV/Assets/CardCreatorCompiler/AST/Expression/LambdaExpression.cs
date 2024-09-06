using System;
using System.Collections.Generic;
using UnityEngine;

namespace Gwent
{
    public class LambdaExpression : Expression
    {
        public LambdaExpression(List<IDExpression> Params, Expression body)
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
        public List<IDExpression> Params { get; }
        public DelegateType Type { get; }
        public Expression Body { get; }
        protected override Scope? Context { get; set; }

        public enum DelegateType
        {
            Predicate, Action
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
        public override Scope.DataType CheckSemantic()
        {
            foreach (IDExpression item in Params)
            {
                if (Context!.Items.ContainsKey(item))
                {
                    EngineCompiler.error =new TypeError(ErrorCode.SemanticError);
                    throw new("The parameter is already defined");
                }
                Context!.Items.Add(item, null!);
            }
            if (Type == DelegateType.Predicate)
            {
                if (Body.CheckSemantic() == Scope.DataType.Boolean)
                {
                    return Scope.DataType.Boolean;
                }
                else
                {
                    EngineCompiler.error =new TypeError(ErrorCode.SemanticError);
                    throw new("The body of the lambda is not a boolean");
                }
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
                return new Predicate<GameObject>(AuxEvaluate);
            }
            else
            {
                return new Action<object[]>(AuxEvaluate);
            }
        }

        bool AuxEvaluate(GameObject card)
        {
            var iD = Params[0] as IDExpression;
            Context.Items[iD] = card;
            return Convert.ToBoolean(Body.Evaluate());
        }
        void AuxEvaluate(object[] parameters) ///lista de cartas
        {
            for (int i = 0; i < parameters.Length; i++)
            {
                var iD = Params[i] as IDExpression;
                Context.Items[iD] = parameters[i];
            }
            Body.Evaluate();
        }
        
    }
}