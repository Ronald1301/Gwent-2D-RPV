using System;

namespace Gwent
{
    public class SelectorExpression : Expression
    {
        public SelectorExpression(Expression source = null!)
        {
            Source = source;
        }
        public SelectorExpression(Expression source,/*Expression single*/ LambdaExpression predicate)
        {
            Source = source;
            //Single=Single;
            Predicate = predicate;
        }

        public Expression Source { get; set; }
        public bool Single = false;
       // public Expression? Single { get; set; }
        public LambdaExpression? Predicate { get; set; }
        protected override Scope? Context { get; set; }

        public override void SetScope(Scope current)
        {
            Context = current;
            Scope son=new Scope(current,new(),new());
            Source.SetScope(son);
            //Single(son);
            Predicate?.SetScope(son);
        }

        public override Scope.DataType CheckSemantic()
        {
            if (Source.CheckSemantic() != Scope.DataType.String)
            {
                EngineCompiler.CreateError(ErrorCode.SemanticError, "Source is not string");
            }
            if (Source.ToString() != "hand" || Source.ToString() != "deck" || Source.ToString() != "graveyard" || Source.ToString() != "parents" || Source.ToString() != "board"
            || Source.ToString() != "otherGraveyard" || Source.ToString() != "otherHand" || Source.ToString() != "otherDeck")
            {
                EngineCompiler.CreateError(ErrorCode.SemanticError, "Source is not a valid source");
            }
            if (Predicate is not null)
            {
                if (Predicate.CheckSemantic() != Scope.DataType.Boolean)
                {
                    EngineCompiler.CreateError(ErrorCode.SemanticError, "Predicate is not boolean");
                }
            }
            return Scope.DataType.String;
        }
        public override object Evaluate()
        {
            //(string,bool,object?) result = (Source.Evaluate()?.ToString()!,Single.Evaluate(),Predicate?.Evaluate());
            (string,bool,object?) result = (Source.Evaluate()?.ToString()!,Single,Predicate?.Evaluate());
            return result;
        }
    }
}