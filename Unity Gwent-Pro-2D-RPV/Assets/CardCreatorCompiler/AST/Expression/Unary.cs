using System;

namespace Gwent
{
    public class Unary : Expression
    {
        public enum Operators
        {
            Log, Sen, Cos, Tan, Cot, Sqrt, Sum, Dif, SumSumLeft, DifDifLeft, SumSumRight, DifDifRight, Not
        }
        public Expression argument;
        public readonly Operators operators;
        protected override Scope? Context { get; set; }

        public Unary(Expression argument, Unary.Operators operators)
        {
            this.argument = argument;
            this.operators = operators;
        }
        public override object Evaluate()
        {
            try
            {
                double result = Convert.ToDouble(argument.Evaluate());
                switch (operators)
                {
                    case Operators.Log:
                        return Math.Log(result);
                    case Operators.Sen:
                        return Math.Sin(result);
                    case Operators.Cos:
                        return Math.Cos(result);
                    case Operators.Tan:
                        return Math.Tan(result);
                    case Operators.Cot:
                        return 1 / Math.Tan(result);
                    case Operators.Sqrt:
                        return Math.Sqrt(result);
                    case Operators.Dif:
                        return -result;
                    case Operators.SumSumLeft:
                        if (argument is IDExpression MoreId)
                        {
                            foreach (var item in MoreId.ContextPublic!.Items.Keys)
                            {
                                if (item.token.Value == MoreId.token.Value)
                                {
                                    MoreId.ContextPublic!.Items[item] = ++result;
                                    return result;
                                }
                            }
                        }
                        throw new System.Exception("Unknown operator");
                    case Operators.DifDifLeft:
                        if (argument is IDExpression LessId)
                        {
                            foreach (var item in LessId.ContextPublic!.Items.Keys)
                            {
                                if (item.token.Value == LessId.token.Value)
                                {
                                    LessId.ContextPublic!.Items[item] = --result;
                                    return result;
                                }
                            }
                        }
                        throw new System.Exception("Unknown operator");
                    case Operators.SumSumRight:
                        if (argument is IDExpression idMore)
                        {
                            foreach (var item in idMore.ContextPublic!.Items.Keys)
                            {
                                if (item.token.Value == idMore.token.Value)
                                {
                                    idMore.ContextPublic.Items[item] = ++result;
                                    return result;
                                }
                            }
                        }
                        return result;
                    case Operators.DifDifRight:
                        if (argument is IDExpression idLess)
                        {
                            foreach (var item in idLess.ContextPublic.Items.Keys)
                            {
                                if (item.token.Value == idLess.token.Value)
                                {
                                    idLess.ContextPublic.Items[item] = --result;
                                    return result;
                                }
                            }
                        }
                        throw new System.Exception("Unknown operator");
                    case Operators.Not:
                        return !Convert.ToBoolean(result);
                    default://case Operators.Sum:
                        return result;
                }
                /*
                return this.operators switch
                {
                    Operators.Log => Math.Log(result),
                    Operators.Sen => Math.Sin(result),
                    Operators.Cos => Math.Cos(result),
                    Operators.Tan => Math.Tan(result),
                    Operators.Cot => 1 / Math.Tan(result),
                    Operators.Sqrt => Math.Sqrt(result),
                    Operators.Sum => result,
                    Operators.Dif => -result,
                    Operators.SumSumLeft => ++result,
                    Operators.DifDifLeft => --result,
                    Operators.SumSumRight => result++,
                    Operators.DifDifRight => result--,
                    Operators.Not => !Convert.ToBoolean(result),
                    _ => throw new System.Exception("Unknown operator")
                };
                */
            }
            catch (System.Exception e)
            {
                EngineCompiler.CreateError(ErrorCode.EvaluateError, e.Message);
                return null;
            }
        }

        public override void SetScope(Scope current)
        {
            Context = current;
            argument.SetScope(current);
        }

        public override Scope.DataType CheckSemantic()
        {

            if (operators == Operators.Not)
            {
                if (argument.CheckSemantic() != Scope.DataType.Boolean)
                {
                    EngineCompiler.CreateError(ErrorCode.SemanticError, "The argument is not a boolean");
                }
                return Scope.DataType.Boolean;
            }
            else
            {
                if (argument.CheckSemantic() != Scope.DataType.Number)
                {
                    EngineCompiler.CreateError(ErrorCode.SemanticError, "The argument is not a number");
                }
                return Scope.DataType.Number;
            }
        }
    }
}

