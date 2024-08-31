using System.Runtime.InteropServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Gwent
{
    public class OnActivationExpression : Expression
    {
        public OnActivationExpression(params EffectCardExpression[] effectAssignment)
        {
            foreach (var item in effectAssignment)
            {
                Body.Enqueue(item);
            }
        }
        public Queue<EffectCardExpression> Body = new();

        protected override Scope? Context { get; set; }

        public override object Evaluate()
        {
            foreach (var item in Body)
            {
                item.Evaluate();
            }
            return null!;
        }

        public override void SetScope(Scope current)
        {
            Context = current;
            Scope son = new Scope(current, new(),new());
            foreach (var item in Body)
            {
                item.SetScope(son);
            }
        }

        public override Scope.DataType CheckSemantic()
        {
            foreach (var item in Body)
            {
                item.CheckSemantic();
            }
            return Scope.DataType.Void;
        }
    }
}