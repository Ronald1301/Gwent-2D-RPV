using System.Collections.Generic;

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

        public override void SetScope(Scope current)
        {
            Context = current;
            Scope son = new Scope(current, new(), new());
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
        public override object Evaluate()
        {
            Queue<object> onActivation = new();
            foreach (var item in Body)
            {
                onActivation.Enqueue(item.Evaluate());
            }
            return onActivation;
            //devuelve una lista de efectos con selector y postaction
        }
    }
}