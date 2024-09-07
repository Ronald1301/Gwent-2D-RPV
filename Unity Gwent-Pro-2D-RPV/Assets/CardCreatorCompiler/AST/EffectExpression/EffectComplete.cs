using System.Collections.Generic;

namespace Gwent
{
    public class EffectComplete
    {
        public object Name { get; set; }
        public List<Expression> Params { get; set; }
        public LambdaExpression Body { get; set; }

        public Scope ContextEffect{get;set;}
        public Scope? ContextCard { get; set; }

        public EffectComplete(object name, List<Expression> Params, LambdaExpression body, Scope scope)
        {
            Name = name;
            this.Params = Params;
            Body = body;
            this.ContextEffect = scope;
        }
       
        public EffectComplete()
        {
            Name = null!;
            Params = null!;
            Body = null!;
            ContextEffect=null!;
        }

    }
}
