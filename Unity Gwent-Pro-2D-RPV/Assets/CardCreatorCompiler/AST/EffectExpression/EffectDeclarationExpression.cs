
using System.Runtime.InteropServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Gwent
{
    public class EffectDeclarationExpression : Expression
    {
        public Expression? Name { get; set; }
        public List<Expression>? Params { get; set; }
        public Expression? Body { get; set; }

        protected override Scope? Context { get; set; }

        public EffectDeclarationExpression(Expression name = null!, List<Expression> Params = null!, Expression body = null!)
        {
            Name = name;
            this.Params = Params;
            Body = body;
        }

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
            if (Body is null)
            {
                throw new Exception("Body is null");
            }
            if (Params is not null)
            {
                foreach (var item in Params)
                {
                    if (item.CheckSemantic() != Scope.DataType.String)
                    {
                        throw new Exception("Param is not IDExpression");
                    }
                }
            }
            Body.CheckSemantic();
            return Scope.DataType.Void;
        }

        public override object Evaluate()
        {
            EffectComplete effect = new EffectComplete();
            effect.Name = Name!.Evaluate();
            List<object> param = new List<object>();
            if (Params is not null)
            {
                foreach (var item in Params)
                {
                    param.Add(item.Evaluate());
                }
            }
            effect.Params = param;
            effect.Body = Body!.Evaluate();
            return effect;
        }

        public override void SetScope(Scope current)
        {
            Name!.SetScope(current);
            Body!.SetScope(current);
            if (Params is not null)
            {
                foreach (var item in Params)
                {
                    item.SetScope(current);
                }
            }
        }
    }
}
