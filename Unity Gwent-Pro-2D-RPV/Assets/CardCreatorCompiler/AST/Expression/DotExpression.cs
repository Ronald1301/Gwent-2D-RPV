
using System.Runtime.InteropServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
namespace Gwent
{

    public class DotExpression : Expression
    {
        public DotExpression(Expression left, Token dot, Expression right)
        {
            Left = left;
            Dot = dot;
            Right = right;
        }

        protected override Scope? Context { get; set; }
        Expression Left { get; }
        Token Dot { get; }
        Expression Right { get; }

        public override Scope.DataType CheckSemantic()
        {
            throw new NotImplementedException();
        }

        public override object Evaluate()
        {
            throw new NotImplementedException();
        }

        public override void SetScope(Scope current)
        {
            throw new NotImplementedException();
        }
    }

    public class DotCall : Expression
    {
        public DotCall(Token.TokenType type, Token dot, Expression arguments)
        {
            Dot = dot;
            Arguments = arguments;
            Type = type;
        }

        protected override Scope? Context { get; set; }
        Token Dot { get; }
        Token.TokenType Type { get; }
        Expression Arguments { get; }

        public override Scope.DataType CheckSemantic()
        {
            throw new NotImplementedException();
        }

        public override object Evaluate()
        {
            throw new NotImplementedException();
        }

        public override void SetScope(Scope current)
        {
            throw new NotImplementedException();
        }
    }

}