using System.Runtime.InteropServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;


namespace Gwent
{
    public abstract class BinaryExpression : Expression
    {
        public readonly Expression Left ;
        public readonly Expression Right ;

        public BinaryExpression(Expression left, Expression right)
        {
            Left = left;
            Right = right;
        }

    }
}