using System.Runtime.InteropServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

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

        public override Scope.DataType CheckSemantic()
        {
            if (EffectPostAction.Name is null)
            {
                throw new Exception("Name is null");
            }
            if (EffectPostAction.Name.CheckSemantic() != Scope.DataType.String)
            {
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

        public override object Evaluate()
        {
            return EffectPostAction.Evaluate();
        }

        public override void SetScope(Scope current)
        {
            EffectPostAction.SetScope(current);
        }
    }
}
