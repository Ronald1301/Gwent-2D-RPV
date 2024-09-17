
namespace Gwent
{

    public class ConditionalExpression : Expression
    {
        public readonly Expression Condition;
        public readonly Statement TrueExpression;
        public readonly Statement FalseExpression;
        protected override Scope? Context { get; set; }

        public ConditionalExpression(Expression condition, Statement trueExpression, Statement falseExpression)
        {
            Condition = condition;
            TrueExpression = trueExpression;
            FalseExpression = falseExpression;
        }

        public override void SetScope(Scope current)
        {
            Context = current;
            Scope son = new(current, new(), new());
            Condition.SetScope(son);
            TrueExpression.SetScope(son);
            FalseExpression.SetScope(son);
        }
        public override Scope.DataType CheckSemantic()
        {
            if (Condition.CheckSemantic() != Scope.DataType.Boolean)
            {
              EngineCompiler.CreateError(ErrorCode.SemanticError, "Condition is not boolean");
            }
            var trueType = TrueExpression.CheckSemantic();
            var falseType = FalseExpression.CheckSemantic();
            if (trueType != falseType)
            {
                EngineCompiler.CreateError(ErrorCode.SemanticError, "True and False expressions must have the same type");
            }
            return Scope.DataType.Unknown;
        }

        public override object Evaluate()
        {
            if ((bool)Condition.Evaluate())
            {
                return TrueExpression.Evaluate();
            }
            else
                return FalseExpression.Evaluate();
        }

    }
}