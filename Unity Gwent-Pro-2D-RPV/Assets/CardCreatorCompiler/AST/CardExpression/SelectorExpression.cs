using System;

namespace Gwent
{
    public class SelectorExpression : Expression
    {
        public SelectorExpression(Expression source = null!)
        {
            Source = source;
        }
        public SelectorExpression(Expression source, LambdaExpression predicate)
        {
            Source = source;
            Predicate = predicate;
        }

        public Expression Source { get; set; }
        public bool Single = false;
        public LambdaExpression? Predicate { get; set; }
        protected override Scope? Context { get; set; }

        public override object Evaluate()
        {
            (string,bool,object?) result = (Source.Evaluate()?.ToString()!,Single,Predicate?.Evaluate());
            return result;
        }

        public override void SetScope(Scope current)
        {
            Context = current;
            Scope son=new Scope(current,new(),new());
            Source.SetScope(son);
            Predicate?.SetScope(son);
        }

        public override Scope.DataType CheckSemantic()
        {
            if (Source.CheckSemantic() != Scope.DataType.String)
            {
                EngineCompiler.error = new TypeError(ErrorCode.SemanticError);
                throw new Exception("Source is not IDExpression");
            }
            if (Source.ToString() != "hand" || Source.ToString() != "deck" || Source.ToString() != "graveyard" || Source.ToString() != "parents" || Source.ToString() != "board"
            || Source.ToString() != "otherGraveyard" || Source.ToString() != "otherHand" || Source.ToString() != "otherDeck")
            {
                EngineCompiler.error = new TypeError(ErrorCode.SemanticError);
                throw new Exception("Source is not Hand, Deck, Graveyard, Banished or All");
            }
            if (Predicate is not null)
            {
                if (Predicate.CheckSemantic() != Scope.DataType.Boolean)
                {
                    EngineCompiler.error = new TypeError(ErrorCode.SemanticError);
                    throw new Exception("Predicate is not LambdaExpression");
                }
            }
            return Scope.DataType.String;
        }
    }
}