using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gwent;
using System;
using System.Security.Cryptography;

public static class Bridge
{
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
        string faction = card.Faction.ToString();
        bool[] range = card.Range;
        string description = "";
        foreach (var item in card.Ability)
        {
            description += "\n" + item.ToString();
        }
        CardData cardData = ScriptableObject.CreateInstance<CardData>();
        //CardData cardData = new CardData(name, faction, description, power, range,type, subTypeUnitCard, subTypeSpecialCard);

        cardNew.GetComponent<CardDisplay>().cardData = cardData;

        if (DataBase.instance.Decks.ContainsKey(faction))
        {
            DataBase.instance.Decks[faction].Add(cardNew);
        }
        else DataBase.instance.Decks.Add(faction, new List<GameObject> { cardNew });

    }
    internal static void CreateEffect()
    {
        //Effect effect = new Effect();
        //CardCreator.instance.CreateEffect(effect);
    }
    internal static int GetTriggerPlayer() { return Playing ? 1 : 2; }

    internal static List<(GameObject, CardData)> GetSource(int IDPlayer, string Source)
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
        List<(GameObject, CardData)> result = new List<(GameObject, CardData)>();
        List<GameObject> cards = new();

        if (Source == "board")
        {
            cards = player1new.board.M.GetComponent<MeleeZone>().melee;
            for (int i = 0; i < cards.Count; i++)
            {
                result.Add((cards[i], cards[i].GetComponent<CardDisplay>().cardData));
            }

            cards = player1new.board.R.GetComponent<RangedZone>().ranged;
            for (int i = 0; i < cards.Count; i++)
            {
                result.Add((cards[i], cards[i].GetComponent<CardDisplay>().cardData));
            }

            cards = player1new.board.S.GetComponent<SiegeZone>().siege;
            for (int i = 0; i < cards.Count; i++)
            {
                result.Add((cards[i], cards[i].GetComponent<CardDisplay>().cardData));
            }

            cards = player2new.board.M.GetComponent<MeleeZone>().melee;
            for (int i = 0; i < cards.Count; i++)
            {
                result.Add((cards[i], cards[i].GetComponent<CardDisplay>().cardData));
            }

            cards = player2new.board.R.GetComponent<RangedZone>().ranged;
            for (int i = 0; i < cards.Count; i++)
            {
                result.Add((cards[i], cards[i].GetComponent<CardDisplay>().cardData));
            }

            cards = player2new.board.S.GetComponent<SiegeZone>().siege;
            for (int i = 0; i < cards.Count; i++)
            {
                result.Add((cards[i], cards[i].GetComponent<CardDisplay>().cardData));
            }
        }

        else if (Source == "field")
        {

            cards = player1new.board.M.GetComponent<MeleeZone>().melee;
            for (int i = 0; i < cards.Count; i++)
            {
                result.Add((cards[i], cards[i].GetComponent<CardDisplay>().cardData));
            }

            cards = player1new.board.R.GetComponent<RangedZone>().ranged;
            for (int i = 0; i < cards.Count; i++)
            {
                result.Add((cards[i], cards[i].GetComponent<CardDisplay>().cardData));
            }

            cards = player1new.board.S.GetComponent<SiegeZone>().siege;
            for (int i = 0; i < cards.Count; i++)
            {
                result.Add((cards[i], cards[i].GetComponent<CardDisplay>().cardData));
            }
        }

        else if (Source == "deck")
        {
            cards = player1new.deck.deck;
            for (int i = 0; i < cards.Count; i++)
            {
                result.Add((cards[i], cards[i].GetComponent<CardDisplay>().cardData));
            }
        }

        else if (Source == "hand")
        {
            cards = player1new.hand.CardsInHand;
            for (int i = 0; i < cards.Count; i++)
            {
                result.Add((cards[i], cards[i].GetComponent<CardDisplay>().cardData));
            }
        }

        else if (Source == "graveyard")
        {
            cards = player1new.board.Cemetery.GetComponent<CemeteryZone>().Cemetery;
            for (int i = 0; i < cards.Count; i++)
            {
                result.Add((cards[i], cards[i].GetComponent<CardDisplay>().cardData));
            }
        }

        else if (Source == "otherField")
        {
            cards = player2new.board.M.GetComponent<MeleeZone>().melee;
            for (int i = 0; i < cards.Count; i++)
            {
                result.Add((cards[i], cards[i].GetComponent<CardDisplay>().cardData));
            }

            cards = player2new.board.R.GetComponent<RangedZone>().ranged;
            for (int i = 0; i < cards.Count; i++)
            {
                result.Add((cards[i], cards[i].GetComponent<CardDisplay>().cardData));
            }

            cards = player2new.board.S.GetComponent<SiegeZone>().siege;
            for (int i = 0; i < cards.Count; i++)
            {
                result.Add((cards[i], cards[i].GetComponent<CardDisplay>().cardData));
            }
        }

        else if (Source == "otherDeck")
        {
            cards = player2new.deck.deck;

            for (int i = 0; i < cards.Count; i++)
            {
                result.Add((cards[i], cards[i].GetComponent<CardDisplay>().cardData));
            }
        }

        else if (Source == "otherHand")
        {
            cards = player2new.hand.CardsInHand;

            for (int i = 0; i < cards.Count; i++)
            {
                result.Add((cards[i], cards[i].GetComponent<CardDisplay>().cardData));
            }
        }

        else if (Source == "otherGraveyard")
        {
            cards = player2new.board.Cemetery.GetComponent<CemeteryZone>().Cemetery;

            for (int i = 0; i < cards.Count; i++)
            {
                result.Add((cards[i], cards[i].GetComponent<CardDisplay>().cardData));
            }
        }

        return result;
    }

    internal static List<GameObject> FindCards(List<GameObject> list, object v)
    {
        if (v is Predicate<GameObject> predicate)
        {
            return list.FindAll(predicate);
        }
        return list;
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

    internal static List<GameObject> PopCard(List<GameObject> list, Token.TokenType typeDot)
    {
        GameObject card = list[^1];//list.Count - 1
        list.RemoveAt(0);
        card.GetComponent<MoveCard>().MoveToHand();
        return list;
    }

    internal static List<GameObject> PushCard(List<GameObject> list, Token.TokenType typeDot, object v)
    {
        if (v is GameObject card)
        {
            //list.Add(card);
            card.GetComponent<MoveCard>().MoveToDeck();
        }
        return list;
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
