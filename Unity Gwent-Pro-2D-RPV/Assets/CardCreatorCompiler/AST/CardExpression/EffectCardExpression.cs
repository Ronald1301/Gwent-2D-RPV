using System.Runtime.InteropServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        public PostActionExpression? PostAction { get; set; } = null;
        public SelectorExpression? Selector { get; set; } = null;
        protected override Scope? Context { get; set; }

        public override Scope.DataType CheckSemantic()
        {
            if (Name is null)
            {
                throw new Exception("Name is null");
            }
            if (Name.CheckSemantic() != Scope.DataType.String)
            {
                throw new Exception("Name is not IDExpression");
            }
            foreach (var item in Params)
            {
                if (item.CheckSemantic() != Scope.DataType.String)
                {
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
            Name = (Expression)Name.Evaluate();
            for (int i = 0; i < Params.Count; i++)
            {
                Params[i] = (Expression)Params[i].Evaluate();
            }
            return null!;
        }

        public override void SetScope(Scope current)
        {
            Context = current;
            var son= new Scope(current, new(), new());
            Name.SetScope(son);
            foreach (var item in Params)
            {
                item.SetScope(son);
            }
            Selector?.SetScope(son);
            PostAction?.SetScope(son);
        }
    }
}