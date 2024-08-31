using System.Linq.Expressions;
using System.Runtime.InteropServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gwent
{
    public class Parser
    {
        readonly List<Token> Tokens;
        int index = 0;
        private List<Token.TokenType> IDPropitiatesDotExpression = new List<Token.TokenType>
        {
        Token.TokenType.Token_Name,
        Token.TokenType.Token_Type,
        Token.TokenType.Token_Faction,
        Token.TokenType.Token_Power,
        // Token.TokenType.Token_TriggerPlayer,
        Token.TokenType.Token_Board,
        Token.TokenType.Token_Hand,
        Token.TokenType.Token_Field,
        Token.TokenType.Token_Graveyard,
        Token.TokenType.Token_Deck,
        Token.TokenType.Token_Owner,
        Token.TokenType.Token_Range
        };
        private List<Token.TokenType> IDMethodsDotExpression = new List<Token.TokenType>
        {
            Token.TokenType.Token_Find,
            Token.TokenType.Token_Push,
            Token.TokenType.Token_SendBottom,
            Token.TokenType.Token_Pop,
            Token.TokenType.Token_Remove,
            Token.TokenType.Token_Shuffle,
            Token.TokenType.Token_HandOfPlayer,
            Token.TokenType.Token_FieldOfPlayer,
            Token.TokenType.Token_GraveyardOfPlayer,
            Token.TokenType.Token_DeckOfPlayer,

            Token.TokenType.Token_TriggerPlayer,
        };

        public Parser(List<Token> list)
        {
            Tokens=list;
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
            Additional.errors.Add(new TypeError(ErrorCode.SyntacticError, " Where is ; ?"));
            return null!;
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
                    Additional.errors.Add(new TypeError(ErrorCode.SyntacticError, " Where is } ?"));
                }
                Additional.errors.Add(new TypeError(ErrorCode.SyntacticError, " Where is { ?"));
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
                    Additional.errors.Add(new TypeError(ErrorCode.SyntacticError, " Where is } ?"));
                }
                Additional.errors.Add(new TypeError(ErrorCode.SyntacticError, " Where is { ?"));
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
                    Additional.errors.Add(new TypeError(ErrorCode.SyntacticError, " Where is : ?"));
                }
                Additional.errors.Add(new TypeError(ErrorCode.SyntacticError, "Effect already has a name"));
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
                        Additional.errors.Add(new TypeError(ErrorCode.SyntacticError, " Where is { ?"));
                    }
                    Additional.errors.Add(new TypeError(ErrorCode.SyntacticError, " Where is : ?"));
                }
                Additional.errors.Add(new TypeError(ErrorCode.SyntacticError, "Effect already has a params"));
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
                    Additional.errors.Add(new TypeError(ErrorCode.SyntacticError, "Effect already has a body"));
                }
                Additional.errors.Add(new TypeError(ErrorCode.SyntacticError, "Effect already has a action"));
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
                    Additional.errors.Add(new TypeError(ErrorCode.SyntacticError, " Where is : ?"));
                }
                Additional.errors.Add(new TypeError(ErrorCode.SyntacticError, "Card already has a name"));
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
                    Additional.errors.Add(new TypeError(ErrorCode.SyntacticError, " Where is : ?"));
                }
                Additional.errors.Add(new TypeError(ErrorCode.SyntacticError, "Card already has a type"));
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
                    Additional.errors.Add(new TypeError(ErrorCode.SyntacticError, " Where is : ?"));
                }
                Additional.errors.Add(new TypeError(ErrorCode.SyntacticError, "Card already has a faction"));
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
                    Additional.errors.Add(new TypeError(ErrorCode.SyntacticError, " Where is : ?"));
                }
                Additional.errors.Add(new TypeError(ErrorCode.SyntacticError, "Card already has a power"));
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
                        Additional.errors.Add(new TypeError(ErrorCode.SyntacticError, " Where is ] ?"));
                    }
                    Additional.errors.Add(new TypeError(ErrorCode.SyntacticError, " Where is : ?"));
                }
                Additional.errors.Add(new TypeError(ErrorCode.SyntacticError, "Card already has a range"));
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
                        Additional.errors.Add(new TypeError(ErrorCode.SyntacticError, " Where is ] ?"));
                    }
                    Additional.errors.Add(new TypeError(ErrorCode.SyntacticError, " Where is [ ?"));
                }
                Additional.errors.Add(new TypeError(ErrorCode.SyntacticError, " Where is : ?"));
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
                Additional.errors.Add(new TypeError(ErrorCode.SyntacticError, " Where is } ?"));
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
                            Additional.errors.Add(new TypeError(ErrorCode.SyntacticError, " Where is } ?"));
                        }
                        else
                        {
                            effect.Name = C(null!);
                            return effect;
                        }
                    }
                    Additional.errors.Add(new TypeError(ErrorCode.SyntacticError, " Where is : ?"));
                }
                Additional.errors.Add(new TypeError(ErrorCode.SyntacticError, "Effect already has a name"));
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
                            Additional.errors.Add(new TypeError(ErrorCode.SyntacticError, " Where is } ?"));
                        }
                        Additional.errors.Add(new TypeError(ErrorCode.SyntacticError, " Where is : ?"));
                    }
                    Additional.errors.Add(new TypeError(ErrorCode.SyntacticError, "Effect already has a selector"));
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
                        Additional.errors.Add(new TypeError(ErrorCode.SyntacticError, " Where is } ?"));
                    }
                    Additional.errors.Add(new TypeError(ErrorCode.SyntacticError, " Where is : ?"));
                }
                Additional.errors.Add(new TypeError(ErrorCode.SyntacticError, "Effect already has a postAction"));
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
                    Additional.errors.Add(new TypeError(ErrorCode.SyntacticError, " Where is : ?"));
                }
                Additional.errors.Add(new TypeError(ErrorCode.SyntacticError, "Effect already has a name"));
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
                    Additional.errors.Add(new TypeError(ErrorCode.SyntacticError, " Where is : ?"));
                }
                Additional.errors.Add(new TypeError(ErrorCode.SyntacticError, "Selector already has a source"));
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
                Additional.errors.Add(new TypeError(ErrorCode.SyntacticError, " Where is : ?"));
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
                    Additional.errors.Add(new TypeError(ErrorCode.SyntacticError, " Where is : ?"));
                }
                Additional.errors.Add(new TypeError(ErrorCode.SyntacticError, "Selector already has a predicate"));
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
                    Additional.errors.Add(new TypeError(ErrorCode.SyntacticError, " Where is : ?"));
                }
                Additional.errors.Add(new TypeError(ErrorCode.SyntacticError, "PostAction already has a type"));
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
                            Additional.errors.Add(new TypeError(ErrorCode.SyntacticError, " Where is } ?"));
                        }
                        Additional.errors.Add(new TypeError(ErrorCode.SyntacticError, " Where is : ?"));
                    }
                    Additional.errors.Add(new TypeError(ErrorCode.SyntacticError, " Where is : ?"));
                }
                Additional.errors.Add(new TypeError(ErrorCode.SyntacticError, "PostAction already has a selector"));
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
                    Additional.errors.Add(new TypeError(ErrorCode.SyntacticError, " Where is : ?"));
                }
                Additional.errors.Add(new TypeError(ErrorCode.SyntacticError, "PostAction already has a postAction"));
            }

            return postAction;
        }
        private LambdaExpression ParsingLambda()
        {
            if (Tokens[index++].Type == Token.TokenType.Open_Paren)
            {
                var paramsLambda = ParsingParams(new List<Expression>(), new bool[] { true, false, false });

                if (Tokens[index++].Type == Token.TokenType.Token_Lambda)
                {
                    if (Tokens[index++].Type == Token.TokenType.Open_Key)
                    {
                        var lambdaBody = C(null!);
                        if (Tokens[index++].Type == Token.TokenType.Close_Key)
                        {
                            return new LambdaExpression(paramsLambda, lambdaBody);
                        }
                        Additional.errors.Add(new TypeError(ErrorCode.SyntacticError, " Where is } ?"));
                    }
                    Additional.errors.Add(new TypeError(ErrorCode.SyntacticError, " Where is { ?"));
                }
                Additional.errors.Add(new TypeError(ErrorCode.SyntacticError, " Where is -> ?"));
            }
            Additional.errors.Add(new TypeError(ErrorCode.SyntacticError, " Where is ( ?"));
            return null!;
        }
        private Expression ParsingDotExpression()
        {
            if (IDPropitiatesDotExpression.Contains(Tokens[index].Type))
            {
                if (Tokens[index + 1].Type == Token.TokenType.Open_Block)
                {
                    index += 2;
                    var result = W(null!);
                    if (Tokens[index + 1].Type == Token.TokenType.Close_Block)
                    {
                        index++;
                        return new DotCall(Tokens[index - 2].Type, Tokens[index - 3], result);
                    }
                    Additional.errors.Add(new TypeError(ErrorCode.SyntacticError, " Where is ] ?"));
                }
                return new DotCall(Tokens[index].Type, Tokens[index - 1], null!);
            }
            else if (IDMethodsDotExpression.Contains(Tokens[index].Type))
            {
                if (Tokens[index + 1].Type == Token.TokenType.Open_Paren)
                {
                    if (Tokens[index - 1].Type == Token.TokenType.Token_Find)
                    {
                        index += 2;
                        var result = ParsingLambda();
                        if (Tokens[index].Type == Token.TokenType.Close_Paren)
                        {
                            return new DotCall(Tokens[index - 1].Type, Tokens[index - 2], result);
                        }
                        Additional.errors.Add(new TypeError(ErrorCode.SyntacticError, " Where is ) ?"));
                    }
                    else if (Tokens[index - 1].Type == Token.TokenType.Token_Push || Tokens[index - 1].Type == Token.TokenType.Token_SendBottom || Tokens[index - 1].Type == Token.TokenType.Token_Remove || Tokens[index - 1].Type == Token.TokenType.Token_Shuffle || Tokens[index - 1].Type == Token.TokenType.Token_Pop)
                    {
                        index += 2;
                        if (Tokens[index].Type == Token.TokenType.Close_Paren)
                        {
                            return new DotCall(Tokens[index - 1].Type, Tokens[index - 2], null!);
                        }
                        Additional.errors.Add(new TypeError(ErrorCode.SyntacticError, " Where is ) ?"));
                    }
                    else if (Tokens[index - 1].Type == Token.TokenType.Token_HandOfPlayer || Tokens[index - 1].Type == Token.TokenType.Token_GraveyardOfPlayer || Tokens[index - 1].Type == Token.TokenType.Token_FieldOfPlayer || Tokens[index - 1].Type == Token.TokenType.Token_DeckOfPlayer)
                    {
                        index += 2;
                        var param = ParsingParams(new List<Expression>(), new bool[] { true, false, false })[0];

                        if (Tokens[index].Type == Token.TokenType.Close_Paren)
                        {
                            return new DotCall(Tokens[index - 1].Type, Tokens[index - 2], param);
                        }
                        Additional.errors.Add(new TypeError(ErrorCode.SyntacticError, " Where is ) ?"));
                    }
                    else
                    {
                        Additional.errors.Add(new TypeError(ErrorCode.SyntacticError, "Invalid Expression"));
                    }
                }
            }
            Additional.errors.Add(new TypeError(ErrorCode.SyntacticError, "Invalid Expression"));
            return null!;
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
                            Additional.errors.Add(new TypeError(ErrorCode.SyntacticError, " Where is a value ?"));
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
                default:
                    break;
            }
            Additional.errors.Add(new TypeError(ErrorCode.SyntacticError, "Invalid Expression"));

            return null!;
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
                                else Additional.errors.Add(new TypeError(ErrorCode.SyntacticError, " Where is ; or } ?"));
                            }
                            index++;
                            return new WhileExpression(conditional!, statement);
                        }
                        Additional.errors.Add(new TypeError(ErrorCode.SyntacticError, " Where is { ?"));
                    }
                    Additional.errors.Add(new TypeError(ErrorCode.SyntacticError, " Where is ) ?"));
                }
                Additional.errors.Add(new TypeError(ErrorCode.SyntacticError, " Where is ( ?"));
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
                                    else Additional.errors.Add(new TypeError(ErrorCode.SyntacticError, " Where is ; or } ?"));
                                }
                                index++;
                                return new ForExpression(new InExpression(item, collection), statement);
                            }
                            Additional.errors.Add(new TypeError(ErrorCode.SyntacticError, " Where is { ?"));
                        }
                        Additional.errors.Add(new TypeError(ErrorCode.SyntacticError, " Where is a variable ?"));
                    }
                    Additional.errors.Add(new TypeError(ErrorCode.SyntacticError, " Where is in ?"));
                }
                Additional.errors.Add(new TypeError(ErrorCode.SyntacticError, " Where is a variable ?"));
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
                                else Additional.errors.Add(new TypeError(ErrorCode.SyntacticError, " Where is ; or } ?"));
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
                                        else Additional.errors.Add(new TypeError(ErrorCode.SyntacticError, " Where is ; or } ?"));
                                    }
                                    index++;
                                    return new ConditionalExpression(conditional!, statement, statement_else);
                                }
                                Additional.errors.Add(new TypeError(ErrorCode.SyntacticError, " Where is { ?"));
                            }
                            else
                                return new ConditionalExpression(conditional!, statement, null!);
                        }
                        Additional.errors.Add(new TypeError(ErrorCode.SyntacticError, " Where is { ?"));
                    }
                    Additional.errors.Add(new TypeError(ErrorCode.SyntacticError, " Where is ) ?"));
                }
                Additional.errors.Add(new TypeError(ErrorCode.SyntacticError, " Where is ( ?"));
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
                    Additional.errors.Add(new TypeError(ErrorCode.SyntacticError, " Where is ) ?"));
                }

                Additional.errors.Add(new TypeError(ErrorCode.SyntacticError, " Where is ( ?"));
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
                    Additional.errors.Add(new TypeError(ErrorCode.SyntacticError, " Where is ) ?"));
                }

                Additional.errors.Add(new TypeError(ErrorCode.SyntacticError, " Where is ( ?"));
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
                    Additional.errors.Add(new TypeError(ErrorCode.SyntacticError, " Where is ) ?"));
                }

                Additional.errors.Add(new TypeError(ErrorCode.SyntacticError, " Where is ( ?"));
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
                    Additional.errors.Add(new TypeError(ErrorCode.SyntacticError, " Where is ) ?"));
                }
                Additional.errors.Add(new TypeError(ErrorCode.SyntacticError, " Where is ( ?"));
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
                    Additional.errors.Add(new TypeError(ErrorCode.SyntacticError, " Where is ) ?"));
                }
                Additional.errors.Add(new TypeError(ErrorCode.SyntacticError, " Where is ( ?"));
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
                    Additional.errors.Add(new TypeError(ErrorCode.SyntacticError, " Where is ) ?"));
                }
                Additional.errors.Add(new TypeError(ErrorCode.SyntacticError, " Where is ( ?"));
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
                Additional.errors.Add(new TypeError(ErrorCode.SyntacticError, " Where is a variable ?"));
                //var result_W = W(last);
                //return new Unary(result_W, Unary.Operators.SumSumLeft);
            }

            if (Tokens[index].Type == Token.TokenType.Token_DifDif)
            {
                index++;
                if (Tokens[index].Type == Token.TokenType.Identifier)
                {
                    return new Unary(new IDExpression(Tokens[index], null!), Unary.Operators.DifDifLeft);
                }
                //var result_W = W(last);
                //return new Unary(result_W, Unary.Operators.DifDifLeft);
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
                Additional.errors.Add(new TypeError(ErrorCode.SyntacticError, " Where is ) ?"));
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
                Additional.errors.Add(new TypeError(ErrorCode.SyntacticError, " Where is } ?"));
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
                    else Additional.errors.Add(new TypeError(ErrorCode.SyntacticError, " Where is ; ?"));
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
                    var left = new IDExpression(Tokens[index - 2], null!);
                    return new DotExpression(left, Tokens[index - 1], ParsingDotExpression());
                }
                index++;
                return id;
            }

            if (Tokens[index].Type == Token.TokenType.Close_Paren ||
            Tokens[index].Type == Token.TokenType.Close_Block ||
            Tokens[index].Type == Token.TokenType.Close_Key ||
            Tokens[index].Type == Token.TokenType.Comma ||
            Tokens[index].Type == Token.TokenType.PointAndComma ||
            Tokens[index].Type == Token.TokenType.EndLine)
            {
                index++;
                return last;
            }
            if (Tokens[index].Type == Token.TokenType.EndProgram)
            {
                return last;
            }

            Additional.errors.Add(new TypeError(ErrorCode.Unknown, "This token that I parse in the parser is invalid"));
            return null!;
        }
    }
}