using System;
using System.Collections.Generic;
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
            object name = Name.Evaluate();
            if (!EngineCompiler.effects.ContainsKey(name.ToString()!))
            {
                EngineCompiler.error = new TypeError(ErrorCode.EvaluateError);
                throw new Exception("Effect not found");
            }
            var effect = EngineCompiler.effects[name.ToString()!];

            foreach (var item in Params)
            {
                if (item is Assignment paramThis)
                {
                    foreach (var itemDeclaration in effect.Params!)
                    {
                        if (itemDeclaration is Assignment paramsEffect)
                        {
                            if (paramThis.ID.token.Value.ToString() == paramsEffect.ID.token.Value.ToString())
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

            if (Selector is not null)
            {
                if (selectorParent is not null && IsPostAction)
                {
                    if (Selector.Source.Evaluate().ToString() == "parent")
                    {
                        Selector.Source = selectorParent.Source;
                    }
                }

                var resultSelector = Selector.Evaluate();
                if (resultSelector is Tuple<string, bool, object> tuple)
                {
                    var targets = Bridge.GetSource(Bridge.GetTriggerPlayer(), tuple.Item1);
                    if (tuple is not null)
                    {
                        if (tuple.Item3 is Predicate<(GameObject, CardData)> predicate)//object==gameobject
                        {
                            if (targets is not null)
                            {
                                targets = targets.FindAll(predicate);
                                if (tuple.Item2)
                                {
                                    List<(GameObject, CardData)> list = new();
                                    list.Add((targets[0].Item1, targets[0].Item2));
                                    targets = list;
                                }
                            }
                        }
                    }
                    foreach (var item in Context!.Items.Keys)
                    {
                        if (item.token.Value == effect.Body!.Params[0].token.Value)
                        {
                            Context.Items[item] = targets!;
                        }
                    }
                }
            }
            PostAction?.Evaluate(Selector!);
            return effect;
        }
    }
}