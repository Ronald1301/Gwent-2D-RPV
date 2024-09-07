using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Gwent
{
    public class EffectCardExpression : Expression
    {
        public EffectCardExpression(Expression name = null!)
        {
            Name = name;
        }
        public EffectCardExpression(Expression name, params Expression[] Params)
        {
            Name = name;
            foreach (var item in Params)
            {
                this.Params.Add(item);
            }
        }
        public EffectCardExpression(Expression name, List<Expression> Params, SelectorExpression selector)
        {
            Name = name;
            foreach (var item in Params)
            {
                this.Params.Add(item);
            }
            Selector = selector;
        }
        public EffectCardExpression(Expression name, List<Expression> Params, PostActionExpression postAction)
        {
            Name = name;
            foreach (var item in Params)
            {
                this.Params.Add(item);
            }
            PostAction = postAction;
        }
        public EffectCardExpression(Expression name, List<Expression> Params, SelectorExpression selector, PostActionExpression postAction)
        {
            Name = name;
            foreach (var item in Params)
            {
                this.Params.Add(item);
            }
            Selector = selector;
            PostAction = postAction;
        }

        public Expression Name { get; set; }
        public List<Expression> Params { get; set; } = new();
        public SelectorExpression? Selector { get; set; } = null;
        public PostActionExpression? PostAction { get; set; } = null;

        protected override Scope? Context { get; set; }

        public override void SetScope(Scope current)
        {
            Context = current;
            var son = new Scope(current, new(), new());
            Name.SetScope(son);
            foreach (var item in Params)
            {
                item.SetScope(son);
            }
            Selector?.SetScope(son);
            PostAction?.SetScope(son);
        }
        public override Scope.DataType CheckSemantic()
        {
            if (Name is null)
            {
                EngineCompiler.error = new TypeError(ErrorCode.SemanticError);
                throw new Exception("Name is null");
            }
            if (Name.CheckSemantic() != Scope.DataType.String)
            {
                EngineCompiler.error = new TypeError(ErrorCode.SemanticError);
                throw new Exception("Name is not IDExpression");
            }
            foreach (var item in Params) //verificar si coincide el tipo de dato
            {
                if (item.CheckSemantic() != Scope.DataType.String)
                {
                    EngineCompiler.error = new TypeError(ErrorCode.SemanticError);
                    throw new Exception("Param is not IDExpression");
                }
            }
            if (Selector is not null)
            {
                Selector.CheckSemantic();
            }
            if (PostAction is not null)
            {
                PostAction.CheckSemantic();
            }
            return Scope.DataType.Void;
        }

        public override object Evaluate()
        {
            return this.Evaluate(false, null!);
        }
        public object Evaluate(bool IsPostAction, SelectorExpression selectorParent)
        {
            List<string> NamesEffect = new();
            object name = Name.Evaluate();
            if (!EngineCompiler.effectsSemi.ContainsKey(name.ToString()!))
            {
                EngineCompiler.error = new TypeError(ErrorCode.EvaluateError);
                throw new Exception("Effect not found");
            }
            NamesEffect.Add(name.ToString());
            var effect = EngineCompiler.effectsSemi[name.ToString()!];
            effect.ContextCard = Context;

            if (effect.Params!.Count != Params.Count)
            {
                EngineCompiler.error = new TypeError(ErrorCode.EvaluateError);
                throw new Exception("Params not found");
            }
            foreach (var item in Params)
            {
                if (item is Assignment paramThis)
                {
                    foreach (var itemDeclaration in effect.Params!)
                    {
                        if (itemDeclaration is Assignment paramsEffect)
                        {
                            if (paramThis.ID.token.Value.ToString() == paramsEffect.ID.token.Value.ToString())//falta chekear tipo
                            {
                                paramsEffect.Argument = paramThis.Argument;
                            }
                            else
                            {
                                EngineCompiler.error = new TypeError(ErrorCode.EvaluateError);
                                throw new Exception("Param not found");
                            }
                        }
                    }
                }
            }

            if (IsPostAction)
            {
                if (Selector is not null)
                {
                    if (selectorParent is not null)
                    {
                        if (Selector.Source.Evaluate().ToString() == "parent")
                        {
                            Selector.Source = selectorParent.Source;
                        }
                    }
                }
            }
            EngineCompiler.effects.Add(name.ToString()!, (effect, Selector));
            List<string> namePos = new();
            if (PostAction is not null) namePos = (List<string>)PostAction.Evaluate(Selector!);
            return NamesEffect.Concat(namePos).ToList();
        }
    }
}