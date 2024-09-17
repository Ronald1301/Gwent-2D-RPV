using System;

namespace Gwent
{
    public class Error
    {
        public ErrorCode Code { get; }

        public string argument { get; set; }

        public LocationError? Location { get; set; }
        /*
        public Error(ErrorCode code)
        {
            this.Code = code;
            argument = "";
        }
        */
        public Error(ErrorCode code, string argument)
        {
            this.Code = code;
            this.argument = argument;
        }
        public Error(ErrorCode code, string argument, int line, int column)
        {
            this.Code = code;
            this.argument = argument;
            this.Location = new LocationError(line, column);
        }

        public string Text()
        {
            switch (this.Code)
            {
                case ErrorCode.LexicalError:
                    return "! Lexical Error : " + argument + "in " + (Location is not null ? Location.ToString() : "");
                case ErrorCode.SyntacticError:
                    return "!! Syntactic Error : " + argument;
                case ErrorCode.SemanticError:
                    return "!!! Semantic Error : " + argument;
                case ErrorCode.EvaluateError:
                    return "!!!! Evaluate Error : " + argument;
                default:
                    return "!!!!! Unknown Error : " + argument;
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
        NoExist
    }

    public class LocationError
    {
        int Line { get; set; }
        int Column { get; set; }

        public LocationError(int line, int column)
        {
            this.Line = line;
            this.Column = column;
        }

        public override string ToString()
        {
            return "Line: " + Line + " Column: " + Column;
        }
    }
}