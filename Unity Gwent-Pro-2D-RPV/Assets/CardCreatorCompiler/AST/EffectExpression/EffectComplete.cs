using System.Collections.Generic;

namespace Gwent
{
    public class EffectComplete
    {
        public object Name { get; set; }
        public List<Expression> Params { get; set; }
        public LambdaExpression Body { get; set; }

        public Scope scope{get;set;}

        public EffectComplete(object name, List<Expression> Params, LambdaExpression body, Scope scope)
        {
            Name = name;
            this.Params = Params;
            Body = body;
            this.scope = scope;
        }
       
        public EffectComplete()
        {
            Name = null!;
            Params = null!;
            Body = null!;
            scope=null!;
        }

    }
}
