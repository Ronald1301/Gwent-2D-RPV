using System.Runtime.InteropServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace Gwent
{
    public class Statement : Expression
    {
        public Queue<Expression> Expressions { get; set; }
        //public Queue<Expression> Children { get; set; }
#pragma warning disable CS8632 // La anotación para tipos de referencia que aceptan valores NULL solo debe usarse en el código dentro de un contexto de anotaciones "#nullable".
        protected override Scope? Context { get; set; }
#pragma warning restore CS8632 // La anotación para tipos de referencia que aceptan valores NULL solo debe usarse en el código dentro de un contexto de anotaciones "#nullable".

        public Statement(params Expression[] expressions)
        {
            //Context = scope;
            Expressions = new Queue<Expression>();
          //  Children = [];
            foreach (var expression in expressions)
            {
                Expressions.Enqueue(expression);
            }
        }
        public Statement()
        {
            Expressions = new Queue<Expression>();
        }

        public override object Evaluate()
        {
            foreach (var item in Expressions)
            {
                System.Console.WriteLine( item.Evaluate());
            }
            return 0;
        }

        public override void SetScope(Scope current)
        {
            Context = current;
            foreach (var item in Expressions)
            {
                item.SetScope(current);
            }
        }

        public override Scope.DataType CheckSemantic()
        {
            foreach (var item in Expressions)
            {
                if (item is Assignment || item is Unary || item is BinaryExpression || item is WhileExpression || item is SelectorExpression || item is ForExpression || item is DotExpression || item is ConditionalExpression)
                {
                    if (item is Unary unary)
                    {
                        if (unary.operators != Unary.Operators.SumSumLeft || unary.operators != Unary.Operators.DifDifLeft || unary.operators != Unary.Operators.SumSumRight || unary.operators != Unary.Operators.DifDifRight)
                        {
                            throw new Exception("Invalid statement");
                        }
                    }
                    item.CheckSemantic();
                }
                else
                {
                    throw new Exception("Invalid statement");
                }
            }
            /*
            foreach (var item in Children)
            {
                item.CheckSemantic();
            }
            */
            return Scope.DataType.Void;
        }
    }
}
