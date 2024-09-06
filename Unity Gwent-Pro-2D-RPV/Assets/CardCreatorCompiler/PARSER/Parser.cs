using System;
using System.Collections.Generic;

namespace Gwent
{
    public class Parser
    {
        readonly List<Token> Tokens;
        int index = 0;
        private readonly List<Token.TokenType> IDPropitiatesDotExpression = new List<Token.TokenType>
        {
            Token.TokenType.Token_Name,
            Token.TokenType.Token_Type,
            Token.TokenType.Token_Range,
            Token.TokenType.Token_Faction,
            Token.TokenType.Token_Power,
            Token.TokenType.Token_Owner,
            Token.TokenType.Token_TriggerPlayer,
            Token.TokenType.Token_Board,
            Token.TokenType.Token_Hand,
            Token.TokenType.Token_Field,
            Token.TokenType.Token_Graveyard,
            Token.TokenType.Token_Deck
        };
        private readonly List<Token.TokenType> IDMethodsDotExpression = new List<Token.TokenType>
        {
            Token.TokenType.Token_Find,
            Token.TokenType.Token_Push,
            Token.TokenType.Token_SendBottom,
            Token.TokenType.Token_Remove,
            Token.TokenType.Token_Add,
            Token.TokenType.Token_Pop,
            Token.TokenType.Token_Shuffle,
            Token.TokenType.Token_HandOfPlayer,
            Token.TokenType.Token_FieldOfPlayer,
            Token.TokenType.Token_GraveyardOfPlayer,
            Token.TokenType.Token_DeckOfPlayer,
            // Token.TokenType.Token_TriggerPlayer,
        };

        public Parser(List<Token> list)
        {
            Tokens = list;
        }

        public Expression Parsing()
        {
            return L(new Statement());
        }
        private Expression L(Statement statement)
        {
            var result = M();
            if (result is not null)
                statement.Expressions.Enqueue(result);
            if (Tokens[index].Type == Token.TokenType.PointAndComma)
            {
                index++;
                return L(statement);
            }
            if (index == Tokens.Count - 1 && Tokens[index].Type == Token.TokenType.EndProgram)
            {
                return statement;
            }
            EngineCompiler.error = new(ErrorCode.SyntacticError); throw new("Where is ; ?");
        }
        private Expression M(Expression last = null!)
        {
            if (Tokens[index].Type == Token.TokenType.Token_card)
            {
                if (Tokens[index + 1].Type == Token.TokenType.Open_Key)
                {
                    index += 2;
                    var cardExpression = ParsingCard(last, new CardExpression());
                    if (Tokens[index++].Type == Token.TokenType.Close_Key)
                    {
                        return cardExpression;
                    }
                    EngineCompiler.error = new(ErrorCode.SyntacticError);

                    throw new(" Where is } ?");
                }
                EngineCompiler.error = new(ErrorCode.SyntacticError);
                throw new("Where is { ?");
            }

            if (Tokens[index].Type == Token.TokenType.Token_effect_Declaration)
            {
                if (Tokens[index + 1].Type == Token.TokenType.Open_Key)
                {
                    index += 2;
                    var effectExpression = ParsingEffectDeclaration(last, new EffectDeclarationExpression());
                    if (Tokens[index++].Type == Token.TokenType.Close_Key)
                    {
                        return effectExpression;
                    }
                    EngineCompiler.error = new(ErrorCode.SyntacticError);

                    throw new(" Where is } ?");
                }
                EngineCompiler.error = new(ErrorCode.SyntacticError);
                throw new(" Where is { ?");
            }

            return C(last);
        }

        private Expression ParsingEffectDeclaration(Expression last, EffectDeclarationExpression effect)
        {
            if (Tokens[index].Type == Token.TokenType.Token_Name)
            {
                if (effect.Name is null)
                {
                    if (Tokens[index + 1].Type == Token.TokenType.TwoPoint)
                    {
                        index += 2;
                        effect.Name = C(last);
                        if (Tokens[index].Type == Token.TokenType.Comma)
                        {
                            index++;
                            return ParsingEffectDeclaration(last, effect);
                        }
                        else return effect;
                    }
                    EngineCompiler.error = new(ErrorCode.SyntacticError);
                    throw new(" Where is : ?");
                }
                EngineCompiler.error = new(ErrorCode.SyntacticError);
                throw new("Effect already has a name");
            }

            if (Tokens[index].Type == Token.TokenType.Token_Params)
            {
                if (effect.Params is null)
                {
                    if (Tokens[index + 1].Type == Token.TokenType.TwoPoint)
                    {
                        if (Tokens[index + 2].Type == Token.TokenType.Open_Key)
                        {
                            index += 3;
                            effect.Params = ParsingParams(new List<Expression>(), new bool[] { false, false, true });
                            if (Tokens[index].Type == Token.TokenType.Comma)
                            {
                                index++;
                                return ParsingEffectDeclaration(last, effect);
                            }
                            else return last;
                        }
                        EngineCompiler.error = new(ErrorCode.SyntacticError);

                        throw new("Where is { ?");
                    }
                    EngineCompiler.error = new(ErrorCode.SyntacticError);

                    throw new(" Where is : ?");
                }
                EngineCompiler.error = new(ErrorCode.SyntacticError);
                throw new("Effect already has a params");
            }

            if (Tokens[index].Type == Token.TokenType.Token_Action)
            {
                if (effect.Body is null)
                {
                    if (Tokens[index + 1].Type == Token.TokenType.TwoPoint)
                    {
                        effect.Body = ParsingLambda();
                        if (Tokens[index].Type == Token.TokenType.Comma)
                        {
                            index++;
                            return ParsingEffectDeclaration(last, effect);
                        }
                        else return effect;

                    }
                    EngineCompiler.error = new(ErrorCode.SyntacticError);
                    throw new(" Where is : ?");
                }
                EngineCompiler.error = new(ErrorCode.SyntacticError);
                throw new("Effect already has a body");
            }

            return effect;
        }
        private Expression ParsingCard(Expression last, CardExpression card)
        {
            if (Tokens[index].Type == Token.TokenType.Token_Name)
            {
                if (card.Name is null)
                {
                    if (Tokens[index + 1].Type == Token.TokenType.TwoPoint)
                    {
                        index += 2;
                        card.Name = C(last);
                        if (Tokens[index].Type == Token.TokenType.Comma)
                        {
                            index++;
                            return ParsingCard(last, card);
                        }
                        else return card;
                    }
                    EngineCompiler.error = new(ErrorCode.SyntacticError);
                    throw new(" Where is : ?");
                }
                EngineCompiler.error = new(ErrorCode.SyntacticError);
                throw new("Card already has a name");
            }

            if (Tokens[index].Type == Token.TokenType.Token_Type)
            {
                if (card.Type is null)
                {
                    if (Tokens[index + 1].Type == Token.TokenType.TwoPoint)
                    {
                        index += 2;
                        card.Type = C(last);
                        if (Tokens[index].Type == Token.TokenType.Comma)
                        {
                            index++;
                            return ParsingCard(last, card);
                        }
                        else return card;
                    }
                    EngineCompiler.error = new(ErrorCode.SyntacticError);

                    throw new(" Where is , ?");
                }
                EngineCompiler.error = new(ErrorCode.SyntacticError);
                throw new("Card already has a type");
            }

            if (Tokens[index].Type == Token.TokenType.Token_Faction)
            {
                if (card.Faction is null)
                {
                    if (Tokens[index + 1].Type == Token.TokenType.TwoPoint)
                    {
                        index += 2;
                        card.Faction = C(last);
                        if (Tokens[index].Type == Token.TokenType.Comma)
                        {
                            index++;
                            return ParsingCard(last, card);
                        }
                        else return card;
                    }
                    EngineCompiler.error = new(ErrorCode.SyntacticError);

                    throw new(" Where is , ?");
                }
                EngineCompiler.error = new(ErrorCode.SyntacticError);
                throw new("Card already has a faction");
            }

            if (Tokens[index].Type == Token.TokenType.Token_Power)
            {
                if (card.Power is null)
                {
                    if (Tokens[index + 1].Type == Token.TokenType.TwoPoint)
                    {
                        index += 2;
                        card.Power = C(last);
                        if (Tokens[index].Type == Token.TokenType.Comma)
                        {
                            index++;
                            return ParsingCard(last, card);
                        }
                        else return card;
                    }
                    EngineCompiler.error = new(ErrorCode.SyntacticError);

                    throw new(" Where is , ?");
                }
                EngineCompiler.error = new(ErrorCode.SyntacticError);
                throw new();
            }

            if (Tokens[index].Type == Token.TokenType.Token_Range)
            {
                if (card.Range is null)
                {
                    if (Tokens[index + 1].Type == Token.TokenType.TwoPoint)
                    {
                        if (Tokens[index + 1].Type == Token.TokenType.Open_Block)
                        {
                            index += 2;
                            card.Range = ParsingParams(new List<Expression>(), new bool[] { false, true, false });
                            if (Tokens[index].Type == Token.TokenType.Comma)
                            {
                                index++;
                                return ParsingCard(last, card);
                            }
                            else return card;
                        }
                        EngineCompiler.error = new(ErrorCode.SyntacticError);

                        throw new(" Where is , ?");
                    }
                    EngineCompiler.error = new(ErrorCode.SyntacticError);

                    throw new(" Where is : ?");
                }
                EngineCompiler.error = new(ErrorCode.SyntacticError);
                throw new("Card already has a range");
            }

            if (Tokens[index].Type == Token.TokenType.Token_OnActivation)
            {
                if (Tokens[index + 1].Type == Token.TokenType.TwoPoint)
                {
                    if (Tokens[index + 2].Type == Token.TokenType.Open_Block)
                    {
                        index += 3;
                        var result_OnActivation = StatementParsingOnActivation(new OnActivationExpression());
                        if (Tokens[index].Type == Token.TokenType.Close_Block)
                        {
                            index++;
                            card.OnActivation = result_OnActivation;
                            if (Tokens[index].Type == Token.TokenType.Comma)
                            {
                                index++;
                                return ParsingCard(last, card);
                            }
                            else return card;
                        }
                        EngineCompiler.error = new(ErrorCode.SyntacticError);

                        throw new(" Where is , ?");
                    }
                    EngineCompiler.error = new(ErrorCode.SyntacticError);

                    throw new(" Where is { ?");
                }
                EngineCompiler.error = new(ErrorCode.SyntacticError);
                throw new(" Where is : ?");
            }

            return card;
        }
        private OnActivationExpression StatementParsingOnActivation(OnActivationExpression onActivationExpression)
        {
            if (Tokens[index].Type == Token.TokenType.Open_Key)
            {
                index++;
                var effect = ParsingOnActivation(new EffectCardExpression());
                if (Tokens[index].Type == Token.TokenType.Close_Key)
                {
                    index++;
                    onActivationExpression.Body.Enqueue(effect);
                    return onActivationExpression;
                }
                EngineCompiler.error = new(ErrorCode.SyntacticError);
                throw new(" Where is } ?");
            }

            var result = ParsingOnActivation(new EffectCardExpression());
            index++;
            onActivationExpression.Body.Enqueue(result);
            return onActivationExpression;
        }
        private EffectCardExpression ParsingOnActivation(EffectCardExpression effect)
        {
            if (Tokens[index].Type == Token.TokenType.Token_Effect)
            {
                if (effect.Name is null)
                {
                    if (Tokens[index + 1].Type == Token.TokenType.TwoPoint)
                    {
                        if (Tokens[index + 2].Type == Token.TokenType.Open_Key)
                        {
                            index += 3;
                            ParsingEffectAssignmentCard(effect);
                            if (Tokens[index++].Type == Token.TokenType.Close_Key)
                            {
                                if (Tokens[index].Type == Token.TokenType.Comma)
                                {
                                    index++;
                                    return ParsingOnActivation(effect);
                                }
                                else return effect;
                            }
                            EngineCompiler.error = new(ErrorCode.SyntacticError);

                            throw new(" Where is , ?");
                        }
                        else
                        {
                            effect.Name = C(null!);
                            return effect;
                        }
                    }
                    EngineCompiler.error = new(ErrorCode.SyntacticError);

                    throw new(" Where is { ?");
                }
                EngineCompiler.error = new(ErrorCode.SyntacticError);
                throw new("Effect already has a name");
            }

            if (Tokens[index].Type == Token.TokenType.Token_Selector)
            {
                if (effect.Selector is null)
                {
                    if (Tokens[index + 1].Type == Token.TokenType.TwoPoint)
                    {
                        if (Tokens[index + 2].Type == Token.TokenType.Open_Key)
                        {
                            index += 3;
                            effect.Selector = ParsingSelector(new SelectorExpression());
                            if (Tokens[index++].Type == Token.TokenType.Close_Key)
                            {
                                if (Tokens[index].Type == Token.TokenType.Comma)
                                {
                                    index++;
                                    return ParsingOnActivation(effect);
                                }
                                else return effect;
                            }
                            EngineCompiler.error = new(ErrorCode.SyntacticError);

                            throw new(" Where is , ?");
                        }
                        EngineCompiler.error = new(ErrorCode.SyntacticError);

                        throw new(" Where is { ?");
                    }
                    EngineCompiler.error = new(ErrorCode.SyntacticError);

                    throw new(" Where is : ?");
                }
            }

            if (Tokens[index].Type == Token.TokenType.Token_PostAction)
            {
                if (Tokens[index + 1].Type == Token.TokenType.TwoPoint)
                {
                    if (Tokens[index + 2].Type == Token.TokenType.Open_Key)
                    {
                        index += 3;
                        effect.PostAction = ParsingPostAction(null!, new PostActionExpression());
                        if (Tokens[index++].Type == Token.TokenType.Close_Key)
                        {
                            if (Tokens[index].Type == Token.TokenType.Comma)
                            {
                                index++;
                                return ParsingOnActivation(effect);
                            }
                            else return effect;
                        }
                        EngineCompiler.error = new(ErrorCode.SyntacticError);

                        throw new(" Where is , ?");
                    }
                    EngineCompiler.error = new(ErrorCode.SyntacticError);

                    throw new(" Where is { ?");
                }
                EngineCompiler.error = new(ErrorCode.SyntacticError);
                throw new(" Where is : ?");
            }

            return effect;
        }
        private void ParsingEffectAssignmentCard(EffectCardExpression effect)
        {
            if (Tokens[index].Type == Token.TokenType.Token_Name)
            {
                if (effect.Name is null)
                {
                    if (Tokens[index + 1].Type == Token.TokenType.TwoPoint)
                    {
                        index += 2;
                        var nameExpression = C(null!);
                        effect.Name = nameExpression;
                        if (Tokens[index].Type == Token.TokenType.Comma)
                        {
                            index++;
                            ParsingEffectAssignmentCard(effect);
                            return;
                        }
                        else return;
                    }
                    EngineCompiler.error = new(ErrorCode.SyntacticError);

                    throw new(" Where is , ?");
                }
                EngineCompiler.error = new(ErrorCode.SyntacticError);
                throw new("Effect already has a name");
            }

            if (Tokens[index].Type == Token.TokenType.Identifier)
            {
                index++;
                var paramsExpression = ParsingParams(new List<Expression>(), new bool[] { false, false, true });
                effect.Params = paramsExpression;
                if (Tokens[index].Type == Token.TokenType.Comma)
                {
                    index++;
                    ParsingEffectAssignmentCard(effect);
                    return;
                }
                else return;
            }
        }
        private SelectorExpression ParsingSelector(SelectorExpression selector)
        {
            if (Tokens[index].Type == Token.TokenType.Token_Source)
            {
                if (selector.Source is null)
                {
                    if (Tokens[index + 1].Type == Token.TokenType.TwoPoint)
                    {
                        index += 2;
                        var sourceExpression = C(null!);
                        selector.Source = (Expression)sourceExpression;
                        if (Tokens[index].Type == Token.TokenType.Comma)
                        {
                            index++;
                            return ParsingSelector(selector);
                        }
                        else return selector;
                    }
                    EngineCompiler.error = new(ErrorCode.SyntacticError);

                    throw new(" Where is , ?");
                }
                EngineCompiler.error = new(ErrorCode.SyntacticError);
                throw new("Selector already has a source");
            }

            if (Tokens[index].Type == Token.TokenType.Token_Single)
            {
                if (Tokens[index + 1].Type == Token.TokenType.TwoPoint)
                {
                    index += 2;
                    var singleExpression = T(null!);
                    if (singleExpression is Atomic atomic)
                    {
                        if (atomic.token.Type == Token.TokenType.Token_True) selector.Single = true;
                        else if (atomic.token.Type == Token.TokenType.Token_False) selector.Single = false;
                    }
                    if (Tokens[index].Type == Token.TokenType.Comma)
                    {
                        index++;
                        return ParsingSelector(selector);
                    }
                    else return selector;
                }
                EngineCompiler.error = new(ErrorCode.SyntacticError);
                throw new(" Where is : ?");
            }

            if (Tokens[index].Type == Token.TokenType.Token_Predicate)
            {
                if (selector.Predicate is null)
                {
                    if (Tokens[index++].Type == Token.TokenType.TwoPoint)
                    {
                        selector.Predicate = ParsingLambda();
                        if (Tokens[index].Type == Token.TokenType.Comma)
                        {
                            index++;
                            return ParsingSelector(selector);
                        }
                        else return selector;
                    }
                    EngineCompiler.error = new(ErrorCode.SyntacticError);

                    throw new(" Where is , ?");
                }
                EngineCompiler.error = new(ErrorCode.SyntacticError);
                throw new("Selector already has a predicate");
            }
            return selector;
        }
        private PostActionExpression ParsingPostAction(SelectorExpression selector, PostActionExpression postAction)
        {
            if (Tokens[index].Type == Token.TokenType.Token_Type)
            {
                if (postAction.EffectPostAction.Name is null)
                {
                    if (Tokens[index + 1].Type == Token.TokenType.TwoPoint)
                    {
                        index += 2;
                        var nameExpression = C(null!);
                        postAction.EffectPostAction.Name = (Expression)nameExpression;
                        if (Tokens[index].Type == Token.TokenType.Comma)
                        {
                            index++;
                            return ParsingPostAction(selector, postAction);
                        }
                        else return postAction;
                    }
                    EngineCompiler.error = new(ErrorCode.SyntacticError);

                    throw new(" Where is , ?");
                }
                EngineCompiler.error = new(ErrorCode.SyntacticError);
                throw new("PostAction already has a name");
            }

            if (Tokens[index].Type == Token.TokenType.Token_Selector)
            {
                if (postAction.EffectPostAction.Selector is null)
                {
                    if (Tokens[index + 1].Type == Token.TokenType.TwoPoint)
                    {
                        if (Tokens[index + 2].Type == Token.TokenType.Open_Key)
                        {
                            index += 3;
                            postAction.EffectPostAction.Selector = ParsingSelector(new SelectorExpression());
                            if (Tokens[index].Type == Token.TokenType.Close_Key)
                            {
                                if (Tokens[index].Type == Token.TokenType.Comma)
                                {
                                    index++;
                                    return ParsingPostAction(selector, postAction);
                                }
                                else return postAction;
                            }
                            EngineCompiler.error = new(ErrorCode.SyntacticError);

                            throw new(" Where is , ?");
                        }
                        EngineCompiler.error = new(ErrorCode.SyntacticError);

                        throw new(" Where is { ?");
                    }
                    EngineCompiler.error = new(ErrorCode.SyntacticError);

                    throw new(" Where is : ?");
                }
                EngineCompiler.error = new(ErrorCode.SyntacticError);
                throw new("PostAction already has a selector");
            }

            if (postAction.EffectPostAction.Selector is null)
            {
                postAction.EffectPostAction.Selector = selector;
            }

            if (Tokens[index].Type == Token.TokenType.Token_PostAction)
            {
                if (postAction.EffectPostAction.PostAction is null)
                {
                    if (Tokens[index + 1].Type == Token.TokenType.TwoPoint)
                    {
                        index += 2;
                        postAction.EffectPostAction.PostAction = ParsingPostAction(postAction.EffectPostAction.Selector, new PostActionExpression());
                        if (Tokens[index].Type == Token.TokenType.Comma)
                        {
                            index++;
                            return ParsingPostAction(selector, postAction);
                        }
                        else return postAction;
                    }
                    EngineCompiler.error = new(ErrorCode.SyntacticError);

                    throw new(" Where is , ?");
                }
                EngineCompiler.error = new(ErrorCode.SyntacticError);
                throw new("PostAction already has a postAction");
            }

            return postAction;
        }
        private LambdaExpression ParsingLambda()
        {
            if (Tokens[index++].Type == Token.TokenType.Open_Paren)
            {
                var paramsResult = ParsingParams(new List<Expression>(), new bool[] { true, false, false });

                List<IDExpression> paramsLambda = new();
                foreach (var item in paramsResult)
                {
                    if (item is IDExpression iD)
                    {
                        paramsLambda.Add(iD);
                    }
                }

                if (Tokens[index++].Type == Token.TokenType.Token_Lambda)
                {
                    if (Tokens[index++].Type == Token.TokenType.Open_Key)
                    {
                        var lambdaBody = C(null!);
                        if (Tokens[index++].Type == Token.TokenType.Close_Key)
                        {
                            return new LambdaExpression(paramsLambda, lambdaBody);
                        }
                        EngineCompiler.error = new(ErrorCode.SyntacticError);

                        throw new(" Where is } ?");
                    }
                    EngineCompiler.error = new(ErrorCode.SyntacticError);

                    throw new("Where is { ?");
                }
                EngineCompiler.error = new(ErrorCode.SyntacticError);
                throw new("Where is => ?");
            }
            EngineCompiler.error = new(ErrorCode.SyntacticError); throw new();
            throw new("Where is ( ?");
        }
        private Expression ParsingDotExpression(DotExpression dotExpression)//debo crearlas recursivas hacia la derecha
        {
            if (Tokens[index].Type == Token.TokenType.Open_Block)
            {
                index++;
                var result = W(null!);
                if (Tokens[index + 1].Type == Token.TokenType.Close_Block)
                {
                    if (Tokens[index + 1].Type == Token.TokenType.Point)
                    {
                        index++;
                        return ParsingDotExpression(new DotExpression(dotExpression, Tokens[index - 1].Type, null!));
                    }
                    else
                    {
                        index++;
                        dotExpression.Right = new DotCall(Tokens[index - 1].Type, result);
                        return ParsingDotExpression(dotExpression);
                    }
                }
                EngineCompiler.error = new(ErrorCode.SyntacticError);
                throw new(" Where is } ?");
            }

            else if (IDPropitiatesDotExpression.Contains(Tokens[index].Type))
            {
                dotExpression.Right = new DotCall(Tokens[index].Type, null!);
                index++;
                if (Tokens[index].Type == Token.TokenType.Point)
                {
                    index++;
                    return ParsingDotExpression(new DotExpression(dotExpression, Tokens[index - 1].Type, null!));
                }
                else
                {
                    return ParsingDotExpression(dotExpression);
                }
            }

            else if (IDMethodsDotExpression.Contains(Tokens[index].Type))
            {
                if (Tokens[index + 1].Type == Token.TokenType.Open_Paren)
                {
                    int indexResult = index;
                    index += 2;
                    Expression result = null!;
                    Expression param = null!;
                    if (Tokens[index - 2].Type == Token.TokenType.Token_Find)
                    {
                        result = ParsingLambda();
                    }
                    else if (Tokens[index - 2].Type == Token.TokenType.Token_Push ||
                             Tokens[index - 2].Type == Token.TokenType.Token_SendBottom ||
                             Tokens[index - 2].Type == Token.TokenType.Token_Remove ||
                             Tokens[index - 2].Type == Token.TokenType.Token_Add)
                    {
                        param = T(null!);
                    }
                    else if (Tokens[index - 2].Type == Token.TokenType.Token_Pop ||
                             Tokens[index - 2].Type == Token.TokenType.Token_Shuffle)
                    {
                        //no hace falta un parametro
                    }
                    else if (Tokens[index - 2].Type == Token.TokenType.Token_HandOfPlayer ||
                             Tokens[index - 2].Type == Token.TokenType.Token_GraveyardOfPlayer ||
                             Tokens[index - 2].Type == Token.TokenType.Token_FieldOfPlayer ||
                             Tokens[index - 2].Type == Token.TokenType.Token_DeckOfPlayer)
                    {
                        param = W(null!);
                    }
                    else
                    {
                        EngineCompiler.error = new(ErrorCode.SyntacticError);

                        throw new("Where is the parameter ?");
                    }

                    if (Tokens[index].Type == Token.TokenType.Close_Paren)
                    {
                        index++;
                        if (result is not null)
                        {
                            dotExpression.Right = new DotCall(Tokens[indexResult].Type, result);
                        }
                        else if (param is not null)
                        {
                            dotExpression.Right = new DotCall(Tokens[indexResult].Type, param);
                        }
                        else
                        {
                            dotExpression.Right = new DotCall(Tokens[indexResult].Type, null!);
                        }

                        if (Tokens[index].Type == Token.TokenType.Point)
                        {
                            index++;
                            return ParsingDotExpression(new DotExpression(dotExpression, Tokens[indexResult].Type, null!));
                        }
                        else
                        {
                            return ParsingDotExpression(dotExpression);
                        }
                    }
                    EngineCompiler.error = new(ErrorCode.SyntacticError);

                    throw new(" Where is ) ?");
                }
                EngineCompiler.error = new(ErrorCode.SyntacticError);
                throw new(" Where is ( ?");
            }
            return dotExpression;
        }
        private List<Expression> ParsingParams(List<Expression> arg, bool[] options)
        {
            if (Tokens[index].Type == Token.TokenType.Comma)
            {
                index++;
                return ParsingParams(arg, options);
            }

            switch (options)
            {
                case { } when options[0]: //Params del Action de effect Declaration  and params del predicate
                    if (Tokens[index].Type == Token.TokenType.Identifier)
                    {
                        var result = new IDExpression(Tokens[index], null!);
                        arg.Add(result);
                        index++;
                        return ParsingParams(arg, options);
                    }
                    else if (options[0] && Tokens[index].Type == Token.TokenType.Close_Paren)
                    {
                        return arg;
                    }
                    break;

                case { } when options[1]: // Parsing el range de la card
                    if (Tokens[index].Value.Equals("melee", StringComparison.CurrentCultureIgnoreCase) || Tokens[index].Value.Equals("range", StringComparison.CurrentCultureIgnoreCase) || Tokens[index].Value.Equals("siege", StringComparison.CurrentCultureIgnoreCase))
                    {
                        var result = new Atomic(Tokens[index]);
                        arg.Add(result);
                        index++;
                        return ParsingParams(arg, options);
                    }
                    else if (!options[1] && Tokens[index].Type == Token.TokenType.Close_Block)
                    {
                        return arg;
                    }
                    break;

                case { } when options[2]://Parsing de los params de effectDeclaration and params del effect de OnActivation

                    if (Tokens[index].Type == Token.TokenType.Identifier)
                    {
                        if (Tokens[index + 1].Type == Token.TokenType.TwoPoint)
                        {
                            index += 2;
                            if (Tokens[index].Type == Token.TokenType.Number || Tokens[index].Type == Token.TokenType.String || Tokens[index].Type == Token.TokenType.Boolean)
                            {
                                Assignment result = new(new IDExpression(Tokens[index - 2], Tokens[index].Type, null!), Assignment.Operators.Equal, new Atomic(Tokens[index]));
                                arg.Add(result);
                                index++;
                                foreach (var item in arg)
                                {
                                    if (item is Assignment assignment)
                                    {
                                        if (assignment.Argument is null)
                                        {
                                            assignment.Argument = assignment.ID.Value = result.Argument;
                                            assignment.ID.Type = result.ID.Type;
                                        }
                                    }
                                }
                                return ParsingParams(arg, options);
                            }
                            EngineCompiler.error = new(ErrorCode.SyntacticError);
                            throw new("Where is the value ?");
                        }
                        else if (Tokens[index + 1].Type == Token.TokenType.Comma)
                        {
                            Assignment assignment = new(new IDExpression(Tokens[index], null!), Assignment.Operators.Equal);
                            arg.Add(assignment);
                            index += 2;
                            return ParsingParams(arg, options);
                        }
                    }
                    if (!options[2] && Tokens[index].Type == Token.TokenType.Close_Key)
                    {
                        return arg;
                    }
                    break;

                default: break;
            }
            EngineCompiler.error = new(ErrorCode.SyntacticError);
            throw new("Where is the parameter ?");
        }
        private Expression C(Expression last)
        {
            if (Tokens[index].Type == Token.TokenType.Token_While)
            {
                if (Tokens[index + 1].Type == Token.TokenType.Open_Paren)
                {
                    index += 2;
                    var result_A = A(last);
                    var conditional = result_A as BoolExpression;
                    if (Tokens[index].Type == Token.TokenType.Close_Paren)
                    {
                        index++;
                        Statement statement = new();
                        if (Tokens[index].Type == Token.TokenType.Open_Key)
                        {
                            index++;
                            while (Tokens[index].Type != Token.TokenType.Close_Key)
                            {
                                var result_M = M();
                                if (Tokens[index].Type == Token.TokenType.PointAndComma)
                                {
                                    index++;
                                    statement.Expressions.Enqueue(result_M);
                                }
                                else
                                {
                                    EngineCompiler.error = new(ErrorCode.SyntacticError);

                                    throw new("Where is ; ?");
                                }
                            }
                        }
                        else
                        {
                            var result_M = M();
                            if (Tokens[index].Type == Token.TokenType.PointAndComma)
                            {
                                statement.Expressions.Enqueue(result_M);
                            }
                            else
                            {
                                EngineCompiler.error = new(ErrorCode.SyntacticError);

                                throw new("Where is ; ?");
                            }
                        }
                        index++;
                        return new WhileExpression(conditional!, statement);
                    }
                    EngineCompiler.error = new(ErrorCode.SyntacticError);

                    throw new("Where is ) ?");
                }
                EngineCompiler.error = new(ErrorCode.SyntacticError);
                throw new("Where is ( ?");
            }

            if (Tokens[index].Type == Token.TokenType.Token_For)
            {
                if (Tokens[index + 1].Type == Token.TokenType.Identifier)
                {
                    Assignment item = new(new IDExpression(Tokens[index + 1], null!), Assignment.Operators.Equal, last);
                    index += 2;
                    if (Tokens[index].Type == Token.TokenType.Token_In)
                    {
                        if (Tokens[index + 1].Type == Token.TokenType.Identifier)
                        {
                            Assignment collection = new(new IDExpression(Tokens[index], null!), Assignment.Operators.Equal, last);// comprobar si es un IEnumerable
                            index += 2;
                            if (Tokens[index++].Type == Token.TokenType.Open_Key)
                            {
                                index += 2;
                                Statement statement = new();
                                while (Tokens[index].Type != Token.TokenType.Close_Key)
                                {
                                    var result_M = M();
                                    if (Tokens[index].Type == Token.TokenType.PointAndComma)
                                    {
                                        index++;
                                        statement.Expressions.Enqueue(result_M);
                                    }
                                    else
                                    {
                                        EngineCompiler.error = new(ErrorCode.SyntacticError);

                                        throw new("Where is ; ?");
                                    }
                                }
                                index++;
                                return new ForExpression(new InExpression(item, collection), statement);
                            }
                            EngineCompiler.error = new(ErrorCode.SyntacticError);

                            throw new("Where is { ?");
                        }
                        EngineCompiler.error = new(ErrorCode.SyntacticError);

                        throw new("Where is the collection ?");
                    }
                    EngineCompiler.error = new(ErrorCode.SyntacticError);

                    throw new("Where is in ?");
                }
                EngineCompiler.error = new(ErrorCode.SyntacticError);
                throw new("Where is the item ?");
            }

            if (Tokens[index].Type == Token.TokenType.Token_If)
            {
                if (Tokens[index + 1].Type == Token.TokenType.Open_Paren)
                {
                    index += 2;
                    var result_A = A(last);
                    var conditional = result_A as BoolExpression;
                    if (Tokens[index].Type == Token.TokenType.Close_Paren)
                    {
                        if (Tokens[index + 1].Type == Token.TokenType.Open_Key)
                        {
                            index += 2;
                            Statement statement = new();
                            while (Tokens[index].Type != Token.TokenType.Close_Key)
                            {
                                var result_M = M();
                                if (Tokens[index].Type == Token.TokenType.PointAndComma)
                                {
                                    index++;
                                    statement.Expressions.Enqueue(result_M);
                                }
                                else
                                {
                                    EngineCompiler.error = new(ErrorCode.SyntacticError);

                                    throw new("Where is ; ?");
                                }
                            }

                            index++;

                            if (Tokens[index].Type == Token.TokenType.Token_Else)
                            {
                                if (Tokens[index + 2].Type == Token.TokenType.Open_Key)
                                {
                                    index += 3;
                                    Statement statement_else = new();
                                    while (Tokens[index].Type != Token.TokenType.Close_Key)
                                    {
                                        var result_M = M();
                                        if (Tokens[index].Type == Token.TokenType.PointAndComma)
                                        {
                                            index++;
                                            statement.Expressions.Enqueue(result_M);
                                        }
                                        else
                                        {
                                            EngineCompiler.error = new(ErrorCode.SyntacticError);

                                            throw new("Where is ; ?");
                                        }
                                    }
                                    index++;
                                    return new ConditionalExpression(conditional!, statement, statement_else);
                                }
                                EngineCompiler.error = new(ErrorCode.SyntacticError);

                                throw new("Where is { ?");
                            }
                            else
                                return new ConditionalExpression(conditional!, statement, null!);
                        }
                        EngineCompiler.error = new(ErrorCode.SyntacticError);

                        throw new("Where is { ?");
                    }
                    EngineCompiler.error = new(ErrorCode.SyntacticError);

                    throw new("Where is ) ?");
                }
                EngineCompiler.error = new(ErrorCode.SyntacticError);
                throw new("Where is ( ?");
            }

            return A(last);
        }
        private Expression A(Expression last)
        {
            var result_Y = Y(last);
            var result_Z = Z(result_Y);
            return result_Z;
        }
        private Expression Z(Expression last)
        {
            if (Tokens[index].Type == Token.TokenType.Token_OrOr)
            {
                index++;
                var result_A = A(last);
                Expression OrOrExpressions = new BoolExpression(last, result_A, BoolExpression.OperatorsLogic.OrOr);
                return OrOrExpressions;
            }

            if (Tokens[index].Type == Token.TokenType.Token_Or)
            {
                index++;
                var result_A = A(last);
                Expression orExpressions = new BoolExpression(last, result_A, BoolExpression.OperatorsLogic.Or);
                return orExpressions;
            }

            return last;
        }
        private Expression Y(Expression last)
        {
            var result_B = B(last);
            var result_N = N(result_B);
            return result_N;
        }
        private Expression N(Expression last)
        {
            if (Tokens[index].Type == Token.TokenType.Token_AndAnd)
            {
                index++;
                var result_Y = Y(last);
                Expression AndAndExpressions = new BoolExpression(last, result_Y, BoolExpression.OperatorsLogic.AndAnd);
                return AndAndExpressions;
            }

            if (Tokens[index].Type == Token.TokenType.Token_And)
            {
                index++;
                var result_Y = Y(last);
                Expression andExpressions = new BoolExpression(last, result_Y, BoolExpression.OperatorsLogic.And);
                return andExpressions;
            }

            return last;
        }
        private Expression B(Expression last)
        {
            var result_W = W(last);
            var result_E = E(result_W);
            return result_E;
        }
        private Expression E(Expression last)
        {
            if (Tokens[index].Type == Token.TokenType.Token_DoubleEqual)
            {
                index++;
                var result_B = B(last);
                Expression doubleEqualExpression = new BoolExpression(last, result_B, BoolExpression.OperatorsComparison.DoubleEqual);
                return doubleEqualExpression;
            }

            if (Tokens[index].Type == Token.TokenType.Token_LessOrEqual)
            {
                index++;
                var result_B = B(last);
                Expression LessOrEqualExpression = new BoolExpression(last, result_B, BoolExpression.OperatorsComparison.LessOrEqual);
                return LessOrEqualExpression;
            }

            if (Tokens[index].Type == Token.TokenType.Token_MoreOrEqual)
            {
                index++;
                var result_B = B(last);
                Expression MoreOrEqualExpression = new BoolExpression(last, result_B, BoolExpression.OperatorsComparison.MoreOrEqual);
                return MoreOrEqualExpression;
            }

            if (Tokens[index].Type == Token.TokenType.Token_NotEqual)
            {
                index++;
                var result_B = B(last);
                Expression NoEqualExpression = new BoolExpression(last, result_B, BoolExpression.OperatorsComparison.NoEqual);
                return NoEqualExpression;
            }

            if (Tokens[index].Type == Token.TokenType.Token_Less)
            {
                index++;
                var result_B = B(last);
                Expression LessExpression = new BoolExpression(last, result_B, BoolExpression.OperatorsComparison.Less);
                return LessExpression;
            }

            if (Tokens[index].Type == Token.TokenType.Token_More)
            {
                index++;
                var result_B = B(last);
                Expression MoreExpression = new BoolExpression(last, result_B, BoolExpression.OperatorsComparison.More);
                return MoreExpression;
            }

            return last;
        }
        private Expression W(Expression last)
        {
            var result_F = F(last);
            var result_X = X(result_F);
            return result_X;
        }
        private Expression X(Expression last)
        {
            if (Tokens[index].Type == Token.TokenType.Token_Sum)
            {
                index++;
                var result_W = W(last);
                Expression sumExpression = new ArithmeticBinary(last, result_W, ArithmeticBinary.Operators.add);
                return sumExpression;
            }

            if (Tokens[index].Type == Token.TokenType.Token_Dif)
            {
                index++;
                var result_W = W(last);
                Expression difExpression = new ArithmeticBinary(last, result_W, ArithmeticBinary.Operators.dif);
                return difExpression;
            }

            if (Tokens[index].Type == Token.TokenType.Token_Concat)
            {
                index++;
                var result_W = W(last);
                Expression concatExpression = new ArithmeticBinary(last, result_W, ArithmeticBinary.Operators.Concat);
                return concatExpression;
            }

            if (Tokens[index].Type == Token.TokenType.Token_DoubleConcat)
            {
                index++;
                var result_W = W(last);
                Expression concatExpression = new ArithmeticBinary(last, result_W, ArithmeticBinary.Operators.DoubleConcat);
                return concatExpression;
            }

            return last;
        }
        private Expression F(Expression last)
        {
            var result_T = T(last);
            var result_P = P(result_T);
            return result_P;
        }
        private Expression P(Expression last)
        {
            if (Tokens[index].Type == Token.TokenType.Token_Multi)
            {
                index++;
                var result_F = F(last);
                Expression multiExpression = new ArithmeticBinary(last, result_F, ArithmeticBinary.Operators.multi);
                return multiExpression;
            }

            if (Tokens[index].Type == Token.TokenType.Token_Div)
            {
                index++;
                var result_F = F(last);
                Expression divExpression = new ArithmeticBinary(last, result_F, ArithmeticBinary.Operators.div);
                return divExpression;
            }

            if (Tokens[index].Type == Token.TokenType.Token_Pow)
            {
                index++;
                var result_F = F(last);
                Expression powExpression = new ArithmeticBinary(last, result_F, ArithmeticBinary.Operators.Pow);
                return powExpression;
            }

            if (Tokens[index].Type == Token.TokenType.Token_Mod)
            {
                index++;
                var result_F = F(last);
                Expression percentExpression = new ArithmeticBinary(last, result_F, ArithmeticBinary.Operators.Mod);
                return percentExpression;
            }

            return last;
        }
        private Expression T(Expression last)
        {
            if (Tokens[index].Type == Token.TokenType.Number_Literal)
            {
                var atomic = new Atomic(Tokens[index++]);
                return atomic;
            }

            if (Tokens[index].Type == Token.TokenType.Chain_Literals)
            {
                var atomic = new Atomic(Tokens[index++]);
                return atomic;
            }

            if (Tokens[index].Type == Token.TokenType.Token_False)
            {
                var atomic = new Atomic(Tokens[index++]);
                return atomic;
            }

            if (Tokens[index].Type == Token.TokenType.Token_True)
            {
                var atomic = new Atomic(Tokens[index++]);
                return atomic;
            }

            if (Tokens[index].Type == Token.TokenType.Token_PI)
            {
                var atomic = new Atomic(Tokens[index++]);
                return atomic;
            }

            if (Tokens[index].Type == Token.TokenType.Token_Sen)
            {
                if (Tokens[index + 1].Type == Token.TokenType.Open_Paren)
                {
                    index += 2;
                    var result_W = W(last);
                    if (Tokens[index++].Type == Token.TokenType.Close_Paren)
                    {
                        return new Unary(result_W, Unary.Operators.Sen);
                    }
                    EngineCompiler.error = new(ErrorCode.SyntacticError);

                    throw new(" Where is ) ?");
                }

                EngineCompiler.error = new(ErrorCode.SyntacticError);
                throw new(" Where is ( ?");
            }

            if (Tokens[index].Type == Token.TokenType.Token_Cos)
            {
                if (Tokens[index + 1].Type == Token.TokenType.Open_Paren)
                {
                    index += 2;
                    var result_W = W(last);
                    if (Tokens[index++].Type == Token.TokenType.Close_Paren)
                    {
                        return new Unary(result_W, Unary.Operators.Sen);
                    }
                    EngineCompiler.error = new(ErrorCode.SyntacticError);

                    throw new(" Where is ) ?");
                }

                EngineCompiler.error = new(ErrorCode.SyntacticError);
                throw new(" Where is ( ?");
            }

            if (Tokens[index].Type == Token.TokenType.Token_Tan)
            {
                if (Tokens[index + 1].Type == Token.TokenType.Open_Paren)
                {
                    index += 2;
                    var result_W = W(last);
                    if (Tokens[index++].Type == Token.TokenType.Close_Paren)
                    {
                        return new Unary(result_W, Unary.Operators.Sen);
                    }
                    EngineCompiler.error = new(ErrorCode.SyntacticError);

                    throw new(" Where is ) ?");
                }

                EngineCompiler.error = new(ErrorCode.SyntacticError);
                throw new(" Where is ( ?");
            }

            if (Tokens[index].Type == Token.TokenType.Token_Cot)
            {
                if (Tokens[index + 1].Type == Token.TokenType.Open_Paren)
                {
                    index += 2;
                    var result_W = W(last);
                    if (Tokens[index++].Type == Token.TokenType.Close_Paren)
                    {
                        return new Unary(result_W, Unary.Operators.Sen);
                    }
                    EngineCompiler.error = new(ErrorCode.SyntacticError);

                    throw new(" Where is ) ?");
                }
                EngineCompiler.error = new(ErrorCode.SyntacticError);
                throw new(" Where is ( ?");
            }

            if (Tokens[index].Type == Token.TokenType.Token_Sqrt)
            {
                if (Tokens[index + 1].Type == Token.TokenType.Open_Paren)
                {
                    index += 2;
                    var result_W = W(last);
                    if (Tokens[index++].Type == Token.TokenType.Close_Paren)
                    {
                        return new Unary(result_W, Unary.Operators.Sen);
                    }
                    EngineCompiler.error = new(ErrorCode.SyntacticError);

                    throw new(" Where is ) ?");
                }
                EngineCompiler.error = new(ErrorCode.SyntacticError);
                throw new(" Where is ( ?");
            }

            if (Tokens[index].Type == Token.TokenType.Token_Log)
            {
                if (Tokens[index + 1].Type == Token.TokenType.Open_Paren)
                {
                    index += 2;
                    var result_W = W(last);
                    if (Tokens[index++].Type == Token.TokenType.Close_Paren)
                    {
                        return new Unary(result_W, Unary.Operators.Sen);
                    }
                    EngineCompiler.error = new(ErrorCode.SyntacticError);

                    throw new(" Where is ) ?");
                }
                EngineCompiler.error = new(ErrorCode.SyntacticError);
                throw new(" Where is ( ?");
            }

            if (Tokens[index].Type == Token.TokenType.Token_Sum)
            {
                index++;
                var result_W = W(last);
                return new Unary(result_W, Unary.Operators.Sum);
            }

            if (Tokens[index].Type == Token.TokenType.Token_Dif)
            {
                index++;
                var result_W = W(last);
                return new Unary(result_W, Unary.Operators.Dif);
            }

            if (Tokens[index].Type == Token.TokenType.Token_SumSum)
            {
                if (Tokens[index++].Type == Token.TokenType.Identifier)
                {
                    return new Unary(new IDExpression(Tokens[index], null!), Unary.Operators.SumSumLeft);
                }
                EngineCompiler.error = new(ErrorCode.SyntacticError);
                throw new(" Where is the identifier ?");
            }

            if (Tokens[index].Type == Token.TokenType.Token_DifDif)
            {
                index++;
                if (Tokens[index].Type == Token.TokenType.Identifier)
                {
                    return new Unary(new IDExpression(Tokens[index], null!), Unary.Operators.DifDifLeft);
                }
                EngineCompiler.error = new(ErrorCode.SyntacticError);
                throw new(" Where is the identifier ?");
            }

            if (Tokens[index].Type == Token.TokenType.Token_Not)
            {
                index++;
                var result_W = W(last);
                return new Unary(result_W, Unary.Operators.Not);
            }

            if (Tokens[index].Type == Token.TokenType.Open_Paren)
            {
                index++;
                var result_M = M(last);
                if (Tokens[index++].Type == Token.TokenType.Close_Paren)
                {
                    return result_M;
                }
                EngineCompiler.error = new(ErrorCode.SyntacticError);
                throw new(" Where is ) ?");
            }

            if (Tokens[index].Type == Token.TokenType.Open_Block)
            {
                index++;
                var result_M = M(last);
                //Statement statement = new Statement(result_M);
                if (Tokens[index++].Type == Token.TokenType.Close_Block)
                {
                    return result_M;
                }
                EngineCompiler.error = new(ErrorCode.SyntacticError);
                throw new(" Where is } ?");
            }

            if (Tokens[index].Type == Token.TokenType.Open_Key)
            {
                index++;
                Queue<Expression> expressions = new();
                while (Tokens[index].Type != Token.TokenType.Close_Key)
                {
                    var result_M = M();
                    if (Tokens[index].Type == Token.TokenType.PointAndComma)
                    {
                        index++;
                        expressions.Enqueue(result_M);
                    }
                    else
                    {
                        EngineCompiler.error = new(ErrorCode.SyntacticError);

                        throw new("Where is ; ?");
                    }
                }
                index++;
                return new Statement(expressions.ToArray());
            }

            if (Tokens[index].Type == Token.TokenType.Identifier)
            {
                var id = new IDExpression(Tokens[index], null!);
                if (Tokens[index + 1].Type == Token.TokenType.Token_SumSum)
                {
                    index += 2;
                    return new Unary(id, Unary.Operators.SumSumRight);
                }
                if (Tokens[index + 1].Type == Token.TokenType.Token_DifDif)
                {
                    index += 2;
                    return new Unary(id, Unary.Operators.DifDifRight);
                }
                if (Tokens[index + 1].Type == Token.TokenType.Token_Equal)
                {
                    index += 2;
                    id.Value = W(last);
                    return new Assignment(id, Assignment.Operators.Equal, id.Value);
                }
                if (Tokens[index + 1].Type == Token.TokenType.Token_SumEqual)
                {
                    index += 2;
                    id.Value = W(last);
                    return new Assignment(id, Assignment.Operators.SumEqual, id.Value);
                }
                if (Tokens[index + 1].Type == Token.TokenType.Token_DifEqual)
                {
                    index += 2;
                    id.Value = W(last);
                    return new Assignment(id, Assignment.Operators.DifEqual, id.Value);
                }
                if (Tokens[index + 1].Type == Token.TokenType.Token_MultiEqual)
                {
                    index += 2;
                    id.Value = W(last);
                    return new Assignment(id, Assignment.Operators.MulEqual, id.Value);
                }
                if (Tokens[index + 1].Type == Token.TokenType.Token_DivEqual)
                {
                    index += 2;
                    id.Value = W(last);
                    return new Assignment(id, Assignment.Operators.DivEqual, id.Value);
                }
                if (Tokens[index + 1].Type == Token.TokenType.TwoPoint)
                {
                    index += 2;
                    id.Value = W(last);
                    return new Assignment(id, Assignment.Operators.TwoPoint, id.Value);
                }
                if (Tokens[index + 1].Type == Token.TokenType.Point)
                {
                    index += 2;
                    return ParsingDotExpression(new(id, Tokens[index - 2].Type, null!));
                }
                index++;
                return id;
            }

            if (Tokens[index].Type == Token.TokenType.Close_Paren ||
            Tokens[index].Type == Token.TokenType.Close_Block ||
            Tokens[index].Type == Token.TokenType.Close_Key ||
            Tokens[index].Type == Token.TokenType.Comma ||
            Tokens[index].Type == Token.TokenType.PointAndComma)
            {
                index++;
                return last;
            }
            if (Tokens[index].Type == Token.TokenType.EndProgram)
            {
                return last;
            }

            EngineCompiler.error = new(ErrorCode.SyntacticError); 
            throw new("Where is the value ?");
        }
    }
}