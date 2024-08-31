using System.Data.Common;

using System.Runtime.InteropServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Gwent
{
    public class Atomic : Expression
    {
        public Token token { get; set; }

        protected override Scope? Context { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public Atomic(Token token)
        {
            this.token = token;
        }
        public override Scope.DataType CheckSemantic()
        {
            if (token.Type == Token.TokenType.Number_Literal ||
             token.Type == Token.TokenType.Token_Cos ||
             token.Type == Token.TokenType.Token_Cot ||
             token.Type == Token.TokenType.Token_Dif ||
             token.Type == Token.TokenType.Token_Div ||
             token.Type == Token.TokenType.Token_Log ||
             token.Type == Token.TokenType.Token_Mod ||
             token.Type == Token.TokenType.Token_Multi ||
              token.Type == Token.TokenType.Token_PI ||
              token.Type == Token.TokenType.Token_Pow ||
              token.Type == Token.TokenType.Token_Sen ||
              token.Type == Token.TokenType.Token_Sqrt ||
              token.Type == Token.TokenType.Token_Sum ||
              token.Type == Token.TokenType.Token_Tan) return Scope.DataType.Number;

            if (token.Type == Token.TokenType.Chain_Literals ||
             token.Type == Token.TokenType.String ||
              token.Type == Token.TokenType.Token_Concat) return Scope.DataType.Number;

            if (token.Type == Token.TokenType.Identifier) return Scope.DataType.Unknown;

            if (token.Type == Token.TokenType.Token_False ||
            token.Type == Token.TokenType.Token_True ||
             token.Type == Token.TokenType.Token_And ||
             token.Type == Token.TokenType.Token_Or ||
                 token.Type == Token.TokenType.Token_Not ||
                 token.Type == Token.TokenType.Token_NotEqual ||
               token.Type == Token.TokenType.Token_DoubleEqual ||
               token.Type == Token.TokenType.Token_Less ||
               token.Type == Token.TokenType.Token_LessOrEqual ||
               token.Type == Token.TokenType.Token_More ||
               token.Type == Token.TokenType.Token_MoreOrEqual) return Scope.DataType.Number;
            throw new();
        }
        public override object Evaluate()
        {
            //id.Evaluate();
            if (token.Type == Token.TokenType.Token_PI) return Math.PI;
            return token.Value;
        }

        public override void SetScope(Scope current)
        {
            /*
            if (current.Father == null)
            {
                Additional.errors.Add(new TypeError(ErrorCode.SemanticError, "The variable is not declared"));
                throw new();
            }
            foreach (var item in current.Father!.datatype.Keys)
            {
                if (item.Value == token.Value)
                {
                    Context = current.Father;
                    return;
                }
            }
            GetScope(current.Father); 
            */
        }
    }
}
