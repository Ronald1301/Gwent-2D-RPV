//using UnityEngine;

namespace Gwent
{
    public class IDExpression : Expression  //, Expression value) : Expression
    {
        public Token token;
        public Expression Value;
        public Scope.DataType Type { get; set; }
        protected override Scope? Context { get; set; }
        public Scope ContextPublic { get => Context!; }

        public IDExpression(Token token, Expression value)//, Expression value) : base(token, value
        {
            this.token = token;
            this.Value = value;
            switch (token.Type)
            {
                case Token.TokenType.Number:
                case Token.TokenType.Number_Literal:
                    Type = Scope.DataType.Number;
                    break;
                case Token.TokenType.String:
                    Type = Scope.DataType.String;
                    break;
                case Token.TokenType.Token_True:
                case Token.TokenType.Token_False:
                    Type = Scope.DataType.Boolean;
                    break;
                default:
                    Type = Scope.DataType.Unknown;
                    break;
            }
        }
        public IDExpression(Token token, Token.TokenType type, Expression value)//, Expression value) : base(token, value
        {
            this.token = token;
            this.Value = value;
            switch (type)
            {
                case Token.TokenType.Number:
                case Token.TokenType.Number_Literal:
                    Type = Scope.DataType.Number;
                    break;
                case Token.TokenType.String:
                    Type = Scope.DataType.String;
                    break;
                case Token.TokenType.Token_True:
                case Token.TokenType.Token_False:
                    Type = Scope.DataType.Boolean;
                    break;
                default:
                    Type = Scope.DataType.Unknown;
                    break;
            }
        }
        public override Scope.DataType CheckSemantic()
        {
            foreach (var item in Context!.datatype.Keys)
            {
                if (item.Value == token.Value)
                {
                    return Context.datatype[item];
                }
            }
          EngineCompiler.CreateError(ErrorCode.SemanticError, "The variable is not declared");
            return Scope.DataType.Unknown;
        }

        public override object Evaluate()
        {
            /*
            foreach (var item in Context!.datatype.Keys)
            {
                if (item.Value == token.Value)
                {
                    return Context.datatype[item];
                }
            }
            Additional.error =new TypeError(ErrorCode.SemanticError, "The variable is not declared");
            throw new();
            */

            foreach (var item in Context!.Items.Keys)
            {
                if (item.token.Value == token.Value)
                {
                    return Context.Items[item];
                }
            }

           EngineCompiler.CreateError(ErrorCode.SemanticError, "The variable is not declared");
            return null;
            //return Context!.Items.First(x => x.Key.token.Value == token.Value);
            //return Context!.Items.TryGetValue(new IDExpression(token, Value), out var value) ? value : Value.Evaluate();
        }

        public override void SetScope(Scope current)
        {
            Context = current;
            foreach (var item in current.datatype.Keys)
            {
                if (item.Value == token.Value)//revisar
                {
                    //Context = current.Father;
                    return;
                }
            }
            if (current.Father is not null)
            {
                SetScope(current.Father);
            }
            else
            {
               EngineCompiler.CreateError(ErrorCode.SemanticError, "The variable is not declared");
            }
        }
    }
}