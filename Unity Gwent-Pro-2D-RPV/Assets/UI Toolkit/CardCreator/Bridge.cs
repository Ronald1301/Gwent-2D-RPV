using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gwent;
using System;

public static class Bridge
{
    static readonly GameObject dataBase = GameObject.Find("DataBase");
    static readonly GameObject StructCardNew = GameObject.Find("SkeletonCard");

    public static List<CardData> cardsDataCreated = new();

    static Player player1;
    static Player player2;
    static bool Playing;
    internal static void AssignmentPlayer(Player num1, Player num2)
    {
        player1 = num1;
        player2 = num2;
    }
    internal static void UpdatePlayer(bool playing) { Playing = playing; }
    internal static void CopyCardsAndEffects()
    {
        foreach (var item in EngineCompiler.cards)
        {
            dataBase.GetComponent<DataBase>().dataBaseData.cardsCompiled.Add(item.Key, item.Value);
        }
        foreach (var item in EngineCompiler.effects)
        {
            dataBase.GetComponent<DataBase>().dataBaseData.effectsCompiled.Add(item.Key, item.Value);
        }
    }
    public static void CreateCardsTheDictionary()
    {
        foreach (var item in dataBase.GetComponent<DataBase>().dataBaseData.cardsCompiled.Values)
        {
            CreateCard(item);
        }
    }
    internal static void CreateCard(DataCardComplete card)
    {
        GameObject cardNew = GameObject.Instantiate(StructCardNew, new Vector3(1, 4, -65), Quaternion.identity);

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
            case "Lure":
            case "Señuelo":
                type = CardData.CardType.Special;
                subTypeSpecialCard = CardData.SubTypeSpecialCard.Lure;
                subTypeUnitCard = CardData.SubTypeUnitCard.None;
                break;
            case "Despeje":
            case "Clearance":
                type = CardData.CardType.Special;
                subTypeSpecialCard = CardData.SubTypeSpecialCard.Clearance;
                subTypeUnitCard = CardData.SubTypeUnitCard.None;
                break;
            case "Leader":
            case "Jefe":
            default:
                type = CardData.CardType.Boss;
                subTypeSpecialCard = CardData.SubTypeSpecialCard.None;
                subTypeUnitCard = CardData.SubTypeUnitCard.None;
                break;
        }
        if (type == CardData.CardType.Boss)
        {
            GameObject.DestroyImmediate(cardNew.GetComponent<MoveCard>());
        }
        else GameObject.DestroyImmediate(cardNew.GetComponent<ScriptBoss>());

        string power = card.Power.ToString();
        string auxRange = "";
        for (int i = 0; i < card.Range.Length; i++)
        {
            if (card.Range[i])
            {
                switch (i)
                {
                    case 0:
                        auxRange += "M";
                        break;
                    case 1:
                        auxRange += "R";
                        break;
                    case 2:
                        auxRange += "S";
                        break;
                    default:
                        break;
                }
            }
        }
        CardData.EnumRange range;
        switch (auxRange)
        {
            case "M":
                range = CardData.EnumRange.M;
                break;
            case "R":
                range = CardData.EnumRange.R;
                break;
            case "S":
                range = CardData.EnumRange.S;
                break;
            case "MR":
                range = CardData.EnumRange.MR;
                break;
            case "MS":
                range = CardData.EnumRange.MS;
                break;
            case "RS":
                range = CardData.EnumRange.RS;
                break;
            default:
                range = CardData.EnumRange.MRS;
                break;
        }

        string description = "";
        foreach (var item in card.NamesAbility)
        {
            description += "\n" + item.ToString();
        }
        var listEffect = card.NamesAbility;

        var list1 = dataBase.GetComponent<DataBase>().dataBaseData.frontImages;
        var list2 = dataBase.GetComponent<DataBase>().dataBaseData.backImages;
        int IndexFront = new System.Random().Next(1, list1.Count);
        int IndexBack = new System.Random().Next(1, list2.Count);

        CardData cardData = new CardData(name, faction, description, power, range, type, subTypeUnitCard, subTypeSpecialCard, listEffect, list1[IndexFront], list2[IndexBack]);

        cardNew.GetComponent<CardDisplay>().cardData = cardData;

        bool add = false;
        foreach (var item in dataBase.GetComponent<DataBase>().dataBaseData.Decks)
        {
            if (item.GetComponent<Decks>().Name == faction)
            {
                add = true;
                // if(cardData.Type == CardData.CardType.Boss)
                {
                  //  item.GetComponent<Decks>().deck[0] = cardNew;
                }
                //else
                item.GetComponent<Decks>().deck.Add(cardNew);
               
                cardNew.transform.SetParent(item.transform, false);
                break;
            }
        }
        if (!add)
        {
            // es neutral y dar la opcion de elegir donde guardarla
            /*
            GameObject original = Resources.Load<GameObject>("Prefab/Decks/Deck");
            GameObject gameObjectDeck = GameObject.Instantiate(original, dataBase.transform, false);
            gameObjectDeck.GetComponent<Decks>().deck.Add(cardNew);
            break;
            */
        }

      cardsDataCreated.Add(cardData);
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
            if (!Effects.IsRowEmpty(player1new, 1))
            {
                cards = player1new.subBoard.M.GetComponent<MeleeZone>().melee;
                for (int i = 0; i < cards.Count; i++)
                {
                    result.Add(cards[i]);
                }
            }

            if (!Effects.IsRowEmpty(player1new, 2))
            {
                cards = player1new.subBoard.R.GetComponent<RangedZone>().ranged;
                for (int i = 0; i < cards.Count; i++)
                {
                    result.Add(cards[i]);
                }
            }

            if (!Effects.IsRowEmpty(player1new, 3))
            {
                cards = player1new.subBoard.S.GetComponent<SiegeZone>().siege;
                for (int i = 0; i < cards.Count; i++)
                {
                    result.Add(cards[i]);
                }
            }

            if (!Effects.IsRowEmpty(player2new, 1))
            {

                cards = player2new.subBoard.M.GetComponent<MeleeZone>().melee;
                for (int i = 0; i < cards.Count; i++)
                {
                    result.Add(cards[i]);
                }
            }

            if (!Effects.IsRowEmpty(player2new, 2))
            {
                cards = player2new.subBoard.R.GetComponent<RangedZone>().ranged;
                for (int i = 0; i < cards.Count; i++)
                {
                    result.Add(cards[i]);
                }
            }
            if (!Effects.IsRowEmpty(player2new, 3))
            {
                cards = player2new.subBoard.S.GetComponent<SiegeZone>().siege;
                for (int i = 0; i < cards.Count; i++)
                {
                    result.Add(cards[i]);
                }
            }
        }

        else if (Source == "field")
        {
            if (!Effects.IsRowEmpty(player1new, 1))
            {
                cards = player1new.subBoard.M.GetComponent<MeleeZone>().melee;
                for (int i = 0; i < cards.Count; i++)
                {
                    result.Add(cards[i]);
                }
            }
            if (!Effects.IsRowEmpty(player1new, 2))
            {
                cards = player1new.subBoard.R.GetComponent<RangedZone>().ranged;
                for (int i = 0; i < cards.Count; i++)
                {
                    result.Add(cards[i]);
                }
            }
            if (!Effects.IsRowEmpty(player1new, 3))
            {
                cards = player1new.subBoard.S.GetComponent<SiegeZone>().siege;
                for (int i = 0; i < cards.Count; i++)
                {
                    result.Add(cards[i]);
                }
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
            if (player1new.subBoard.Cemetery.GetComponent<CemeteryZone>().Cemetery.Count != 0)
            {
                cards = player1new.subBoard.Cemetery.GetComponent<CemeteryZone>().Cemetery;
                for (int i = 0; i < cards.Count; i++)
                {
                    result.Add(cards[i]);
                }
            }
        }

        else if (Source == "otherField")
        {
            if (!Effects.IsRowEmpty(player2new, 1))
            {
                cards = player2new.subBoard.M.GetComponent<MeleeZone>().melee;
                for (int i = 0; i < cards.Count; i++)
                {
                    result.Add(cards[i]);
                }
            }
            if (!Effects.IsRowEmpty(player2new, 2))
            {
                cards = player2new.subBoard.R.GetComponent<RangedZone>().ranged;
                for (int i = 0; i < cards.Count; i++)
                {
                    result.Add(cards[i]);
                }
            }
            if (!Effects.IsRowEmpty(player2new, 3))
            {
                cards = player2new.subBoard.S.GetComponent<SiegeZone>().siege;
                for (int i = 0; i < cards.Count; i++)
                {
                    result.Add(cards[i]);
                }
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
            if (player2new.subBoard.Cemetery.GetComponent<CemeteryZone>().Cemetery.Count != 0)
            {
                cards = player2new.subBoard.Cemetery.GetComponent<CemeteryZone>().Cemetery;
                for (int i = 0; i < cards.Count; i++)
                {
                    result.Add(cards[i]);
                }
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
        GameObject card = list[^1]; //list.Count - 1
        list.RemoveAt(0);
        return card;
    }

    internal static GameObject PushCard(List<GameObject> list, Token.TokenType typeDot, object v) //arreglar
    {
        if (v is GameObject card)
        {
            list.Add(card);

            switch (typeDot)
            {
                //  default:
            }
            return card;
        }
        return null;
    }

    internal static List<GameObject> RemoveCard(List<GameObject> list, Token.TokenType typeDot, object v)//arreglar
    {
        if (v is GameObject card)
        {
            list.Remove(card);
           // card.GetComponent<MoveCard>().MoveToCemetery();
        }
        return list;
    }

    internal static List<GameObject> SendBottom(List<GameObject> list, Token.TokenType typeDot, object v)//arreglar
    {
        if (v is GameObject card)
        {
            list.Insert(0, card);
            card.GetComponent<MoveCard>().MoveToDeck();
        }
        return list;
    }

    internal static List<GameObject> AddCard(List<GameObject> list, Token.TokenType typeDot, object v)//arreglar
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
