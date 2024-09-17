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
            Left.SetScope(current);
            Right.SetScope(current);
            /*
            Scope son = new Scope(current, new(), new());
            Left.SetScope(son);
            Right.SetScope(son);
            */
        }

        public override Scope.DataType CheckSemantic()
        {
            if(Right is null) EngineCompiler.CreateError(ErrorCode.SemanticError,"Invalid property or method");
            return Scope.DataType.Void;
        }

        public override object Evaluate()
        {
            var valueDotExpression = Left.Evaluate();
            ///var typeDot = Right.Type;
            return Right.Evaluate(valueDotExpression);
            //return null!;
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
            if (Arguments is not null) Arguments.SetScope(current);
        }

        public override Scope.DataType CheckSemantic()
        {
            throw new NotImplementedException();
        }

        public override object Evaluate()
        {
            return null!;
        }

        public object Evaluate(object valueDotExpression)//typeDot es de la anterior
        {
            try
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
                            _ => throw new("Invalid method")
                        };
                    }
                }
                if (valueDotExpression is List<GameObject> list)
                {
                    return (this.Type) switch
                    {
                        Token.TokenType.Token_Add => Bridge.AddCard(list, Type, Arguments.Evaluate()),
                        Token.TokenType.Token_Shuffle => Bridge.ShuffleList(list, Type, Arguments.Evaluate()),
                        Token.TokenType.Token_Pop => Bridge.PopCard(list, Type),
                        Token.TokenType.Token_Push => Bridge.PushCard(list, Type, Arguments.Evaluate()),
                        Token.TokenType.Token_Remove => Bridge.RemoveCard(list, Type, Arguments.Evaluate()),
                        Token.TokenType.Token_SendBottom => Bridge.SendBottom(list, Type, Arguments.Evaluate()),
                        Token.TokenType.Token_Find => Bridge.FindCards(list, (LambdaExpression)Arguments),
                        _ => throw new("Invalid property")
                    };
                }

                if (valueDotExpression is GameObject card)
                {
                    CardData cardData = card.GetComponent<CardDisplay>().cardData;
                    return this.Type switch
                    {
                        Token.TokenType.Token_Owner => cardData.owner,
                        Token.TokenType.Token_Name => cardData.CardName,
                        Token.TokenType.Token_Faction => cardData.Faction,
                        Token.TokenType.Token_Type => cardData.Type,
                        Token.TokenType.Token_Power => cardData.Power,
                        Token.TokenType.Token_Range => cardData.TypeField,
                        _ => throw new("Invalid property")
                    };
                }
                //EngineCompiler.CreateError(ErrorCode.SemanticError, "Is not a card");
                throw new("Is not a card");
            }
            catch (System.Exception e)
            {
                EngineCompiler.CreateError(ErrorCode.EvaluateError, e.Message);
                return null!;
            }
        }


    }

}