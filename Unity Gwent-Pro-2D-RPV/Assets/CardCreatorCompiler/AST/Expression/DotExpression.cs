//using UnityEngine;

using System;
using System.Collections.Generic;
using UnityEngine;

namespace Gwent
{

    public class DotExpression : Expression
    {
        public DotExpression(Expression left, Token.TokenType type, Expression right)
        {
            Left = left;
            Type = type;
            if (right is DotCall dotCall)
            {
                Right = dotCall;
            }
            else Right = null!;
        }

        protected override Scope? Context { get; set; }
        public Expression Left { get; set; }
        public Token.TokenType Type { get; }
        public DotCall Right { get; set; }

        public override void SetScope(Scope current)
        {
            Context = current;
            Scope son = new Scope(current, new(), new());
            Left.SetScope(son);
            Right.SetScope(son);
        }

        public override Scope.DataType CheckSemantic()
        {
            throw new NotImplementedException();
        }

        public override object Evaluate()
        {
            var valueDotExpression = Left.Evaluate();
            var typeDot = Right.Type;
            return Right.Evaluate(valueDotExpression, typeDot);
        }
    }

    public class DotCall : Expression
    {
        public DotCall(Token.TokenType type, Expression arguments)
        {
            Arguments = arguments;
            Type = type;
        }

        protected override Scope? Context { get; set; }
        public Token.TokenType Type { get; }//actual type
        Expression Arguments { get; }

        public override void SetScope(Scope current)
        {
            Context = current;
            Arguments.SetScope(current);
        }

        public override Scope.DataType CheckSemantic()
        {
            throw new NotImplementedException();
        }

        public override object Evaluate()
        {
            throw new NotImplementedException();
        }

        public object Evaluate(object valueDotExpression, Token.TokenType typeDot)//typeDot es de la anterior
        {
            if (valueDotExpression is string str)
            {
                if (str == "context")
                {
                    return this.Type switch
                    {
                        Token.TokenType.Token_TriggerPlayer => Bridge.GetTriggerPlayer(),
                        Token.TokenType.Token_DeckOfPlayer => Bridge.GetSource((int)Arguments.Evaluate(), "deck"),
                        Token.TokenType.Token_HandOfPlayer => Bridge.GetSource((int)Arguments.Evaluate(), "hand"),
                        Token.TokenType.Token_FieldOfPlayer => Bridge.GetSource((int)Arguments.Evaluate(), "field"),
                        Token.TokenType.Token_GraveyardOfPlayer => Bridge.GetSource((int)Arguments.Evaluate(), "graveyard"),
                        Token.TokenType.Token_Board => Bridge.GetSource(Bridge.GetTriggerPlayer(), "board"),
                        Token.TokenType.Token_Deck => Bridge.GetSource(Bridge.GetTriggerPlayer(), "deck"),
                        Token.TokenType.Token_Hand => Bridge.GetSource(Bridge.GetTriggerPlayer(), "hand"),
                        Token.TokenType.Token_Field => Bridge.GetSource(Bridge.GetTriggerPlayer(), "field"),
                        Token.TokenType.Token_Graveyard => Bridge.GetSource(Bridge.GetTriggerPlayer(), "graveyard"),
                        _ => throw new NotImplementedException(),
                    };
                }
            }
            if (valueDotExpression is List<GameObject> list)
            {
                return (this.Type) switch
                {
                    Token.TokenType.Token_Add => Bridge.AddCard(list, typeDot, Arguments.Evaluate()),
                    Token.TokenType.Token_Shuffle => Bridge.ShuffleList(list, typeDot, Arguments.Evaluate()),
                    Token.TokenType.Token_Pop => Bridge.PopCard(list, typeDot),
                    Token.TokenType.Token_Push => Bridge.PushCard(list, typeDot, Arguments.Evaluate()),
                    Token.TokenType.Token_Remove => Bridge.RemoveCard(list, typeDot, Arguments.Evaluate()),
                    Token.TokenType.Token_SendBottom => Bridge.SendBottom(list, typeDot, Arguments.Evaluate()),
                    Token.TokenType.Token_Find => Bridge.FindCards(list, Arguments.Evaluate()),
                    _ => throw new NotImplementedException(),
                };
            }

            if (valueDotExpression is CardData card)
            {
                return this.Type switch
                {
                    Token.TokenType.Token_Owner => card.owner,
                    Token.TokenType.Token_Name => card.CardName,
                    Token.TokenType.Token_Faction => card.Faction,
                    Token.TokenType.Token_Type => card.Type,
                    Token.TokenType.Token_Power => card.Power,
                    Token.TokenType.Token_Range => card.TypeField,
                    _ => throw new NotImplementedException(),
                };
            }
            throw new NotImplementedException();
        }

    }

}