namespace Gwent
{
    public class TypeError : Error
    {
        public ErrorCode Code { get; }

        public string argument { get; }

        public 
        TypeError(ErrorCode code, string argument)
        {
            this.Code = code;
            this.argument = argument;
        }

        public override string Text()
        {
            switch (this.Code)
            {
                case ErrorCode.LexicalError:
                    return "! Lexical Error : " + argument;
                case ErrorCode.SyntacticError:
                    return "!! Syntactic Error : " + argument;
                case ErrorCode.SemanticError:
                    return "!!! Semantic Error : " + argument;
                default:
                    return "!!!! Unknown Error : " + argument;
            }
            throw new NotImplementedException();
        }
    }

    public enum ErrorCode
    {
        LexicalError,
        SyntacticError,
        SemanticError,
        Unknown,
    }
}
