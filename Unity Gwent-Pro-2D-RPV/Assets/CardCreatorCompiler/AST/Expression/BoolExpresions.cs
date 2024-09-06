using System;

namespace Gwent
{
    public class BoolExpression : BinaryExpression
    {
        public enum OperatorsComparison
        {
            Less, More, LessOrEqual, MoreOrEqual, DoubleEqual, NoEqual,
        }
        public enum OperatorsLogic
        {
            And, Or,//, Not
            OrOr,
            AndAnd
        }
        readonly OperatorsComparison comparison;
        readonly OperatorsLogic logic;

        protected override Scope? Context { get; set; }

        public BoolExpression(Expression left, Expression right, OperatorsComparison comparison) : base(left, right)
        {
            this.comparison = comparison;
        }
        public BoolExpression(Expression left, Expression right, OperatorsLogic logic) : base(left, right)
        {
            this.logic = logic;
        }

        public override object Evaluate()
        {
            if (Convert.ToBoolean(this.comparison))
            {
                try
                {
                    double a = Convert.ToDouble(base.Left.Evaluate());
                    double b = Convert.ToDouble(base.Right.Evaluate());
                    return this.comparison switch
                    {
                        OperatorsComparison.DoubleEqual => a == b,
                        OperatorsComparison.Less => a < b,
                        OperatorsComparison.LessOrEqual => a <= b,
                        OperatorsComparison.More => a > b,
                        OperatorsComparison.MoreOrEqual => a >= b,
                        //case OperatorsComparison.NoEqual:
                        _ => (object)(a != b),
                    };
                }
                catch (System.Exception)
                {
                    string a = base.Left.Evaluate().ToString()!;
                    string b = base.Right.Evaluate().ToString()!;
                    return this.comparison switch
                    {
                        OperatorsComparison.DoubleEqual => a == b,
                        OperatorsComparison.Less => a.CompareTo(b) < 0,
                        OperatorsComparison.LessOrEqual => a.CompareTo(b) <= 0,
                        OperatorsComparison.More => a.CompareTo(b) > 0,
                        OperatorsComparison.MoreOrEqual => a.CompareTo(b) >= 0,
                        //case OperatorsComparison.NoEqual:
                        _ => (object)(a != b),
                    };
                }


            }

            try
            {
                bool x = Convert.ToBoolean(base.Left.Evaluate());
                bool y = Convert.ToBoolean(base.Right.Evaluate());

                return this.logic switch
                {
                    OperatorsLogic.And => (Convert.ToBoolean(x) == true) & (Convert.ToBoolean(y) == true),
                    OperatorsLogic.Or => (Convert.ToBoolean(x) == true) | (Convert.ToBoolean(y) == true),
                    OperatorsLogic.AndAnd => (Convert.ToBoolean(x) == true) && (Convert.ToBoolean(y) == true),
                    _ => (object)((Convert.ToBoolean(x) == true) || (Convert.ToBoolean(y) == true)),

                    /*
                     OperatorsLogic.And => (Convert.ToBoolean(a) == true) && (Convert.ToBoolean(b) == true) ? true : false,
                    OperatorsLogic.Or => (object)((Convert.ToBoolean(a) == true) || (Convert.ToBoolean(b) == true) ? true : false),
                    OperatorsLogic.Not => (object)(Convert.ToBoolean(a) == true ? false : true),
                    */
                };
            }
            catch (System.Exception)
            {
                EngineCompiler.error = new TypeError(ErrorCode.SemanticError);
                throw new("The variable is not declared");
            }

        }

        public override void SetScope(Scope current)
        {
            base.Left.SetScope(current);
            base.Right.SetScope(current);
        }

        public override Scope.DataType CheckSemantic()
        {
            if (Convert.ToBoolean(comparison))
            {
                switch (comparison)
                {
                    case OperatorsComparison.Less:
                    case OperatorsComparison.LessOrEqual:
                    case OperatorsComparison.More:
                    case OperatorsComparison.MoreOrEqual:
                        if (base.Left.CheckSemantic() == Scope.DataType.Number && base.Right.CheckSemantic() == Scope.DataType.Number)
                        {
                            return Scope.DataType.Boolean;
                        }
                        EngineCompiler.error = new TypeError(ErrorCode.SemanticError);
                        throw new("The variable is not declared");
                    case OperatorsComparison.DoubleEqual:
                    case OperatorsComparison.NoEqual:
                        if (base.Left.CheckSemantic() == base.Right.CheckSemantic())
                        {
                            return Scope.DataType.Boolean;
                        }
                        EngineCompiler.error = new TypeError(ErrorCode.SemanticError);
                        throw new("The variable is not declared");
                    default:
                        break;
                }

            }
            else
            {
                if (base.Left.CheckSemantic() == Scope.DataType.Boolean && base.Right.CheckSemantic() == Scope.DataType.Boolean)
                {
                    return Scope.DataType.Boolean;
                }
                EngineCompiler.error = new TypeError(ErrorCode.SemanticError);
                throw new("The variable is not declared");
            }

            // Add a return statement here
            EngineCompiler.error = new TypeError(ErrorCode.SemanticError);
            throw new("The variable is not declared");
        }
    }
}
