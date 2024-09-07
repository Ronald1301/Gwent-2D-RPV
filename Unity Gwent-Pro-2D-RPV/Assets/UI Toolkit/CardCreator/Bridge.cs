using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gwent;
using System;
using System.Security.Cryptography;

public static class Bridge
{
    static GameObject DataBase = GameObject.Find("DataBase");
    static GameObject StructCardNew = GameObject.Find("SkeletonCard");
    static Player player1;
    static Player player2;
    static bool Playing;
    internal static void AssignmentPlayer(Player num1, Player num2)
    {
        player1 = num1;
        player2 = num2;
    }
    internal static void UpdatePlayer(bool playing) { Playing = playing; }

    internal static void CreateCard(DataCardComplete card)
    {
        GameObject cardNew = GameObject.Instantiate(StructCardNew, StructCardNew.transform.position, Quaternion.identity);
        string faction = card.Faction.ToString();

        var name = card.Name.ToString();
        CardData.CardType type;
        CardData.SubTypeSpecialCard subTypeSpecialCard;
        CardData.SubTypeUnitCard subTypeUnitCard;
        switch (card.Type)
        {
            case "Gold":
            case "Oro":
                type = CardData.CardType.Unit;
                subTypeUnitCard = CardData.SubTypeUnitCard.Gold;
                subTypeSpecialCard = CardData.SubTypeSpecialCard.None;
                break;
            case "Silver":
            case "Plata":
                type = CardData.CardType.Unit;
                subTypeUnitCard = CardData.SubTypeUnitCard.Silver;
                subTypeSpecialCard = CardData.SubTypeSpecialCard.None;
                break;
            case "Climate":
            case "Weather":
            case "Clima":
                type = CardData.CardType.Special;
                subTypeSpecialCard = CardData.SubTypeSpecialCard.Climate;
                subTypeUnitCard = CardData.SubTypeUnitCard.None;
                break;
            case "Increase":
            case "Incremento":
            case "Aumento":
                type = CardData.CardType.Special;
                subTypeSpecialCard = CardData.SubTypeSpecialCard.Increase;
                subTypeUnitCard = CardData.SubTypeUnitCard.None;
                break;
            case "Leader":
            default:
                type = CardData.CardType.Boss;
                subTypeSpecialCard = CardData.SubTypeSpecialCard.None;
                subTypeUnitCard = CardData.SubTypeUnitCard.None;
                break;
        }
        string power = card.Power.ToString();
        bool[] range = card.Range;
        string description = "";
        foreach (var item in card.NamesAbility)
        {
            description += "\n" + item.ToString();
        }
        var listEffect = card.NamesAbility;

        var list1 = DataBase.GetComponent<DataBase>().frontImages;
        var list2 = DataBase.GetComponent<DataBase>().backImages;
        int IndexFront = new System.Random().Next(1, list1.Count);
        int IndexBack = new System.Random().Next(1, list2.Count);

        CardData cardData = new CardData(name, faction, description, power, range, type, subTypeUnitCard, subTypeSpecialCard, listEffect, list1[IndexFront], list2[IndexBack]);


        // CardData cardData = ScriptableObject.CreateInstance<CardData>();//lo comentado es equivalente a esta linea

        cardNew.GetComponent<CardDisplay>().cardData = cardData;

        if (DataBase.GetComponent<DataBase>().Decks.ContainsKey(faction))
        {
            DataBase.GetComponent<DataBase>().Decks[faction].Add(cardNew);
        }
        else DataBase.GetComponent<DataBase>().Decks.Add(faction, new List<GameObject> { cardNew });

    }
    internal static int GetTriggerPlayer() { return Playing ? 1 : 2; }

    internal static List<GameObject> GetSource(int IDPlayer, string Source)
    {
        Player player1new;
        Player player2new;
        if (IDPlayer == 1)
        {
            player1new = player1;
            player2new = player2;
        }
        else
        {
            player1new = player2;
            player2new = player1;
        }
        List<GameObject> result = new List<GameObject>();
        List<GameObject> cards = new();

        if (Source == "board")
        {
            cards = player1new.board.M.GetComponent<MeleeZone>().melee;
            for (int i = 0; i < cards.Count; i++)
            {
                result.Add(cards[i]);
            }

            cards = player1new.board.R.GetComponent<RangedZone>().ranged;
            for (int i = 0; i < cards.Count; i++)
            {
                result.Add(cards[i]);
            }

            cards = player1new.board.S.GetComponent<SiegeZone>().siege;
            for (int i = 0; i < cards.Count; i++)
            {
                result.Add(cards[i]);
            }

            cards = player2new.board.M.GetComponent<MeleeZone>().melee;
            for (int i = 0; i < cards.Count; i++)
            {
                result.Add(cards[i]);
            }

            cards = player2new.board.R.GetComponent<RangedZone>().ranged;
            for (int i = 0; i < cards.Count; i++)
            {
                result.Add(cards[i]);
            }

            cards = player2new.board.S.GetComponent<SiegeZone>().siege;
            for (int i = 0; i < cards.Count; i++)
            {
                result.Add(cards[i]);
            }
        }

        else if (Source == "field")
        {

            cards = player1new.board.M.GetComponent<MeleeZone>().melee;
            for (int i = 0; i < cards.Count; i++)
            {
                result.Add(cards[i]);
            }

            cards = player1new.board.R.GetComponent<RangedZone>().ranged;
            for (int i = 0; i < cards.Count; i++)
            {
                result.Add(cards[i]);
            }

            cards = player1new.board.S.GetComponent<SiegeZone>().siege;
            for (int i = 0; i < cards.Count; i++)
            {
                result.Add(cards[i]);
            }
        }

        else if (Source == "deck")
        {
            cards = player1new.deck.deck;
            for (int i = 0; i < cards.Count; i++)
            {
                result.Add(cards[i]);
            }
        }

        else if (Source == "hand")
        {
            cards = player1new.hand.CardsInHand;
            for (int i = 0; i < cards.Count; i++)
            {
                result.Add(cards[i]);
            }
        }

        else if (Source == "graveyard")
        {
            cards = player1new.board.Cemetery.GetComponent<CemeteryZone>().Cemetery;
            for (int i = 0; i < cards.Count; i++)
            {
                result.Add(cards[i]);
            }
        }

        else if (Source == "otherField")
        {
            cards = player2new.board.M.GetComponent<MeleeZone>().melee;
            for (int i = 0; i < cards.Count; i++)
            {
                result.Add(cards[i]);
            }

            cards = player2new.board.R.GetComponent<RangedZone>().ranged;
            for (int i = 0; i < cards.Count; i++)
            {
                result.Add(cards[i]);
            }

            cards = player2new.board.S.GetComponent<SiegeZone>().siege;
            for (int i = 0; i < cards.Count; i++)
            {
                result.Add(cards[i]);
            }
        }

        else if (Source == "otherDeck")
        {
            cards = player2new.deck.deck;

            for (int i = 0; i < cards.Count; i++)
            {
                result.Add(cards[i]);
            }
        }

        else if (Source == "otherHand")
        {
            cards = player2new.hand.CardsInHand;

            for (int i = 0; i < cards.Count; i++)
            {
                result.Add(cards[i]);
            }
        }

        else if (Source == "otherGraveyard")
        {
            cards = player2new.board.Cemetery.GetComponent<CemeteryZone>().Cemetery;

            for (int i = 0; i < cards.Count; i++)
            {
                result.Add(cards[i]);
            }
        }

        return result;
    }

    internal static List<GameObject> FindCards(List<GameObject> list, LambdaExpression lambda)
    {
        return list.FindAll((Predicate<GameObject>)lambda.Evaluate());
        /*
        List<GameObject> result = new List<GameObject>();
        foreach (var item in list)
        {
            if (lambda.Evaluate() is Predicate<GameObject> predicate && predicate.Invoke(item))
            {
                result.Add(item);
            }
        }
        return result;
        */
    }
    internal static List<GameObject> ShuffleList(List<GameObject> list, Token.TokenType typeDot, object v)
    {
        var random = new System.Random();
        for (var i = list.Count - 1; i > 0; i--)
        {
            var j = random.Next(i + 1);
            (list[i], list[j]) = (list[j], list[i]);
        }
        return list;
    }

    internal static GameObject PopCard(List<GameObject> list, Token.TokenType typeDot)
    {
        GameObject card = list[^1];//list.Count - 1
        list.RemoveAt(0);
        return card;
    }

    internal static GameObject PushCard(List<GameObject> list, Token.TokenType typeDot, object v)
    {
        if (v is GameObject card)
        {
            list.Add(card);
            return card;
            //card.GetComponent<MoveCard>().MoveToDeck();
        }
        return null;
    }

    internal static List<GameObject> RemoveCard(List<GameObject> list, Token.TokenType typeDot, object v)
    {
        if (v is GameObject card)
        {
            //list.Remove(card);
            card.GetComponent<MoveCard>().MoveToCemetery();
        }
        return list;
    }

    internal static List<GameObject> SendBottom(List<GameObject> list, Token.TokenType typeDot, object v)
    {
        if (v is GameObject card)
        {
            list.Insert(0, card);
            card.GetComponent<MoveCard>().MoveToDeck();
        }
        return list;
    }

    internal static List<GameObject> AddCard(List<GameObject> list, Token.TokenType typeDot, object v)
    {
        if (v is GameObject card)
        {
            //list.Add(card);

            switch (typeDot)
            {
                case Token.TokenType.Token_Hand:
                    card.GetComponent<MoveCard>().MoveToHand();
                    break;
                case Token.TokenType.Token_Deck:
                    card.GetComponent<MoveCard>().MoveToDeck();
                    break;
                case Token.TokenType.Token_Graveyard:
                    card.GetComponent<MoveCard>().MoveToCemetery();
                    break;

                default:
                    break;
            }
        }
        return list;
    }
}
