using System;
using System.Collections.Generic;

namespace Gwent
{
    public class EffectDeclarationExpression : Expression
    {
        public Expression? Name { get; set; }
        public List<Expression>? Params { get; set; }
        public LambdaExpression? Body { get; set; }

        protected override Scope? Context { get; set; }

        public EffectDeclarationExpression(Expression name = null!, List<Expression> Params = null!, LambdaExpression body = null!)
        {
            Name = name;
            this.Params = Params;
            Body = body;
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
            if (Body is null)
            {
                EngineCompiler.error = new TypeError(ErrorCode.SemanticError);
                throw new Exception("Body is null");
            }
            if (Params is not null)
            {
                foreach (var item in Params)
                {
                    if (item.CheckSemantic() != Scope.DataType.String)
                    {
                        EngineCompiler.error = new TypeError(ErrorCode.SemanticError);
                        throw new Exception("Param is not IDExpression");
                    }
                }
            }
            if (Body is not null)
            {
                Body.CheckSemantic();
            }
            return Scope.DataType.Void;
        }

        public override object Evaluate()//tengo q arreglarlo
        {
            EffectComplete effect = new();
            effect.Name = Name!.Evaluate();
            if (Params is not null)
            {
                foreach (var item in Params)
                {
                    item.Evaluate();
                }
            }
            effect.Params = Params!;
            effect.ContextEffect = Context!;
            effect.Body = Body!;
            EngineCompiler.effectsSemi.Add(effect.Name.ToString()!, effect);
            return effect;
        }


    }
}
