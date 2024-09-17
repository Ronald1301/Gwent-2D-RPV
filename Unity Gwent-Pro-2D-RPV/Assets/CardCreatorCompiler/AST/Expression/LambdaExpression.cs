using System;
using System.Collections.Generic;
using System.Data.Common;
using UnityEngine;
//using UnityEngine;

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
            Scope son = new(current, new(), new());
            Context = son;
            foreach (IDExpression item in Params)
            {
                son.datatype.Add(item.token, item.Type);
                son.Items.Add(item, null!);
            }
            Body.SetScope(son);

        }
        public override Scope.DataType CheckSemantic()
        {
            foreach (IDExpression item in Params)
            {
                if (Context!.Items.ContainsKey(item))
                {
                   EngineCompiler.CreateError(ErrorCode.SemanticError, "The parameter is already defined");
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
                    EngineCompiler.CreateError(ErrorCode.SemanticError, "The body of the lambda expression must return a boolean");
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
                //return new Predicate<object>(AuxEvaluate);
                Predicate<GameObject> predicate = (id) => AuxEvaluate(id, Context);
                return predicate;
            }
            else
            {
                // return new Action<object[]>(AuxEvaluate);
                Action<GameObject[]> action = (id) => AuxEvaluate(id, Context);
                return action;
            }
        }


        //bool AuxEvaluate(object card)
        bool AuxEvaluate(GameObject card, Scope current)
        {
            var iD = Params[0];// as IDExpression;
            current!.Items[iD] = card;
            return Convert.ToBoolean(Body.Evaluate());
        }
        // void AuxEvaluate(object[] parameters) 
        void AuxEvaluate(GameObject[] parameters, Scope current)
        {
            for (int i = 0; i < parameters.Length; i++)
            {
                var iD = Params[i]; //as IDExpression;
                current!.Items[iD] = parameters[i];
            }
            Body.Evaluate();
        }

    }
}