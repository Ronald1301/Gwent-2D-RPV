using System;
using System.Collections.Generic;

namespace Gwent
{
    public class PostActionExpression : Expression
    {
        public EffectCardExpression EffectPostAction { get; set; }
        protected override Scope? Context { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public PostActionExpression(Expression name, List<Expression> Params, SelectorExpression selector, PostActionExpression postAction)
            : this(new EffectCardExpression())
        {
            EffectPostAction.Name = name;
            foreach (var item in Params)
            {
                this.EffectPostAction.Params.Add(item);
            }
            EffectPostAction.Selector = selector;
            EffectPostAction.PostAction = postAction;
        }

        public PostActionExpression(EffectCardExpression effectPostAction)
        {
            EffectPostAction = effectPostAction;
        }
        public PostActionExpression()
        {
            EffectPostAction = new EffectCardExpression();
        }

        public override void SetScope(Scope current)
        {
            EffectPostAction.SetScope(current);
        }
        public override Scope.DataType CheckSemantic()
        {
            if (EffectPostAction.Name is null)
            {
                EngineCompiler.error = new TypeError(ErrorCode.SemanticError);
                throw new Exception("Name is null");
            }
            if (EffectPostAction.Name.CheckSemantic() != Scope.DataType.String)
            {
                EngineCompiler.error = new TypeError(ErrorCode.SemanticError);
                throw new Exception("Name is not IDExpression");
            }
            if (EffectPostAction.Selector is not null)
            {
                EffectPostAction.Selector.CheckSemantic();
            }
            if (EffectPostAction.PostAction is not null)
            {
                EffectPostAction.PostAction.CheckSemantic();
            }
            return Scope.DataType.Void;
        }

        public object Evaluate(SelectorExpression selector)
        {
            return EffectPostAction.Evaluate(true, selector);
        }
        public override object Evaluate()
        {
            throw new NotImplementedException();
        }
    }
}
