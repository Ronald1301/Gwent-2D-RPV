using System;

namespace Gwent
{
    public class TypeError : Error
    {
        public ErrorCode Code { get; }

        public string argument { get; set;}

        public LocationError? Location { get; set; }
        public TypeError(ErrorCode code)
        {
            this.Code = code;
        }
        /*
        public TypeError(ErrorCode code, string argument)
        {
            this.Code = code;
            this.argument = argument;
        }
        */
        public TypeError(ErrorCode code, string argument, int line, int column)
        {
            this.Code = code;
            this.argument = argument;
            this.Location = new LocationError(line, column);
        }

        public override string Text()
        {
            switch (this.Code)
            {
                case ErrorCode.LexicalError:
                    return "! Lexical Error : " + argument + "en " + (Location is not null ? Location.ToString() : "");
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
        EvaluateError,
        Unknown,
    }
}
