
using System.Runtime.InteropServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Gwent
{
    public class ArithmeticBinary : BinaryExpression
    {
        public enum Operators
        {
            add, multi, dif, div, Pow, Mod, Concat, DoubleConcat,
        }
        readonly Operators operators ;
        protected override Scope? Context { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public ArithmeticBinary(Expression left, Expression right, Operators operators) : base(left, right)
        {
            this.operators = operators;
        }
        public override object Evaluate()
        {
            try
            {
                return this.operators switch
                {
                    Operators.add =>
                       Convert.ToDouble(base.Left.Evaluate()) + Convert.ToDouble(base.Right.Evaluate()),

                    Operators.multi =>
                       Convert.ToDouble(base.Left.Evaluate()) * Convert.ToDouble(base.Right.Evaluate()),

                    Operators.dif =>
                       Convert.ToDouble(base.Left.Evaluate()) - Convert.ToDouble(base.Right.Evaluate()),

                    Operators.div =>
                        Convert.ToDouble(base.Left.Evaluate()) / Convert.ToDouble(base.Right.Evaluate()),

                    Operators.Mod =>
                        Convert.ToDouble(base.Left.Evaluate()) % Convert.ToDouble(base.Right.Evaluate()),

                    Operators.Pow =>
                        Math.Pow(Convert.ToDouble(base.Left.Evaluate()), Convert.ToDouble(base.Right.Evaluate())),

                    Operators.Concat =>
                        base.Left.Evaluate().ToString() + base.Right.Evaluate().ToString(),

                    _ =>
                        base.Left.Evaluate().ToString() + " " + base.Right.Evaluate().ToString()
                };
            }
            catch (System.Exception e)
            {
                Additional.errors.Add(new TypeError(ErrorCode.SemanticError, e.Message));
                throw new(e.Message);
            }

        }

        public override Scope.DataType CheckSemantic()
        {
            switch (this.operators)
            {
                case Operators.add:
                case Operators.multi:
                case Operators.dif:
                case Operators.div:
                case Operators.Mod:
                case Operators.Pow:
                    if (base.Left.CheckSemantic() == Scope.DataType.Number && base.Right.CheckSemantic() == Scope.DataType.Number)
                    {
                        return Scope.DataType.Number;
                    }
                    Additional.errors.Add(new TypeError(ErrorCode.SemanticError, "The expression is not of type number"));
                    throw new();
                case Operators.Concat:
                case Operators.DoubleConcat:
                    if (base.Left.CheckSemantic() == Scope.DataType.String && base.Right.CheckSemantic() == Scope.DataType.String)
                    {
                        return Scope.DataType.String;
                    }
                    Additional.errors.Add(new TypeError(ErrorCode.SemanticError, "The expression is not of type string"));
                    throw new();
            }
            Additional.errors.Add(new TypeError(ErrorCode.SemanticError, "The expression is not of type number"));
            throw new();
        }
        public override void SetScope(Scope current)
        {
            base.Left.SetScope(current);
            base.Right.SetScope(current);
        }
    }


}
