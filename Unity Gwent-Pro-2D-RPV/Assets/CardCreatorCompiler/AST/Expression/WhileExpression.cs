//using UnityEngine;

using System;

namespace Gwent
{
    public class WhileExpression : Expression
    {
        public readonly Expression Conditional;
        public readonly Statement Body;
        protected override Scope? Context { get; set; }

        public WhileExpression(BoolExpression conditional, Statement body)
        {
            Conditional = conditional;
            Body = body;
        }
        public override void SetScope(Scope current)
        {
            Context = current;
            Scope son = new(current, new(), new());
            Conditional.SetScope(son);
            Body.SetScope(son);
        }
        public override Scope.DataType CheckSemantic()
        {
            // Verificar que la expresión condicional sea de tipo booleano
            if (Conditional.CheckSemantic() != Scope.DataType.Boolean)
            {
                EngineCompiler.error = new TypeError(ErrorCode.SemanticError);
                throw new("The condition must be a boolean expression");
            }

            // Verificar que el cuerpo de la declaración sea semánticamente correcto
            Body.CheckSemantic();

            // Retornar el tipo de dato void ya que un bucle while no retorna un valor
            return Scope.DataType.Void;
        }

        public override object Evaluate()
        {
            while (Convert.ToBoolean(Conditional.Evaluate()))
            {
                Body.Evaluate();
            }
            return 0;
        }


    }
}
