using System;
using System.Collections.Generic;

namespace Gwent
{
    public class EffectDeclarationExpression : Expression
    {
        public Expression? Name { get; set; }
        public List<Expression> Params { get; set; }
        public LambdaExpression? BodyAction { get; set; }

        protected override Scope? Context { get; set; }

        public EffectDeclarationExpression(Expression name , List<Expression> Params, LambdaExpression body )
        {
            Name = name;
            this.Params = Params;
            BodyAction = body;
        }

        public EffectDeclarationExpression()
        {
            Name = null!;
            Params = new();
            BodyAction = null!;
        }

        public override void SetScope(Scope current)
        {
            Context = current;
            Name!.SetScope(current);
             if (Params is not null)
            {
                foreach (var item in Params)
                {
                    item.SetScope(current);
                }
            }
            BodyAction!.SetScope(current);
        }

        public override Scope.DataType CheckSemantic()
        {
            if (Name is null)
            {
                EngineCompiler.CreateError(ErrorCode.SemanticError, "Name is null");
            }
            if (Name.CheckSemantic() != Scope.DataType.String)
            {
                EngineCompiler.CreateError(ErrorCode.SemanticError, "Name is not string");
            }
            if (BodyAction is null)
            {
                EngineCompiler.CreateError(ErrorCode.SemanticError, "Body is null");
            }
            if (Params is not null)
            {
                foreach (var item in Params)
                {
                    if (item.CheckSemantic() != Scope.DataType.String)
                    {
                        EngineCompiler.CreateError(ErrorCode.SemanticError, "Param is not string");
                    }
                }
            }
            if (BodyAction is not null)
            {
                BodyAction.CheckSemantic();
            }
            return Scope.DataType.Void;
        }

        public override object Evaluate()
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
            effect.Body = BodyAction!;
            EngineCompiler.effectsSemi.Add(effect.Name.ToString()!, effect);
            return effect;
        }


    }
}
