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