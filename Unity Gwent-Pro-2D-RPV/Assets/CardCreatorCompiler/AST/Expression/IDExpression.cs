
using System.Runtime.InteropServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Gwent
{
    public class IDExpression : Expression  //, Expression value) : Expression
    {
        public Token token;
        public Expression Value;
        public Token.TokenType Type { get; set; }
        protected override Scope? Context { get; set; }
        public Scope ContextPublic { get => Context!; }

        public IDExpression(Token token, Expression value)//, Expression value) : base(token, value
        {
            this.token = token;
            this.Value = value;
            this.Type = token.Type;
        }
        public IDExpression(Token token, Token.TokenType type, Expression value)//, Expression value) : base(token, value
        {
            this.token = token;
            this.Value = value;
            this.Type = type;
        }
        /*
        public override string ToString()
        {
            return token.Value;
        }
        */
        public override Scope.DataType CheckSemantic()
        {
            foreach (var item in Context!.datatype.Keys)
            {
                if (item.Value == token.Value)
                {
                    return Context.datatype[item];
                }
            }
            Additional.errors.Add(new TypeError(ErrorCode.SemanticError, "The variable is not declared"));
            throw new();
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
            Additional.errors.Add(new TypeError(ErrorCode.SemanticError, "The variable is not declared"));
            throw new();
            */

            foreach (var item in Context!.Items.Keys)
            {
                if (item.token.Value == token.Value)
                {
                    return Context.Items[item];
                }
            }
            Additional.errors.Add(new TypeError(ErrorCode.SemanticError, "The variable is not declared"));
            throw new();

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
                Additional.errors.Add(new TypeError(ErrorCode.SemanticError, "The variable is not declared"));
                throw new();
            }
        }
    }
}