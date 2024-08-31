using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gwent;

public static class Bridge
{
    static Player player1;
    static Player player2;

public static void  UpdatePlayer(Player player1,Player player2)
    {
        Bridge.player1 = player1;
        Bridge.player2 = player2;
    }
    public static void CreateCard(DataCardComplete card)
    {
        string name = card.Name.ToString();
        string faction = card.Faction.ToString();
        string type = card.Type.ToString();
        string description = card.Ability.ToString();
        string cardImageForehead = card.Image.ToString();
    }
    public static void CreateEffect()
    {
        //Effect effect = new Effect();
        //CardCreator.instance.CreateEffect(effect);
    }

    public static Player GetTriggerPlayer(int ID)
    {
        if (ID == 1)
        {
            return player1;
        }
        else
        {
            return player2;
        }
    }
    public static Player GetPlayer()
    {
        return player1;
    }
    public static List<(GameObject, CardData)> GetSource(Player playingPlayer, string Source)
    {
        if (player1 != playingPlayer)
        {
            player2 = player1;
            player1 = playingPlayer;
        }
        List<(GameObject, CardData)> result = new List<(GameObject, CardData)>();
        List<GameObject> cards = new();
        if (Source == "board")
        {
            cards = player1.board.M.GetComponent<MeleeZone>().melee;
            for (int i = 0; i < cards.Count; i++)
            {
                result.Add((cards[i], cards[i].GetComponent<CardDisplay>().cardData));
            }

            cards = player1.board.R.GetComponent<RangedZone>().ranged;
            for (int i = 0; i < cards.Count; i++)
            {
                result.Add((cards[i], cards[i].GetComponent<CardDisplay>().cardData));
            }

            cards = player1.board.S.GetComponent<SiegeZone>().siege;
            for (int i = 0; i < cards.Count; i++)
            {
                result.Add((cards[i], cards[i].GetComponent<CardDisplay>().cardData));
            }

            cards = player2.board.M.GetComponent<MeleeZone>().melee;
            for (int i = 0; i < cards.Count; i++)
            {
                result.Add((cards[i], cards[i].GetComponent<CardDisplay>().cardData));
            }

            cards = player2.board.R.GetComponent<RangedZone>().ranged;
            for (int i = 0; i < cards.Count; i++)
            {
                result.Add((cards[i], cards[i].GetComponent<CardDisplay>().cardData));
            }

            cards = player2.board.S.GetComponent<SiegeZone>().siege;
            for (int i = 0; i < cards.Count; i++)
            {
                result.Add((cards[i], cards[i].GetComponent<CardDisplay>().cardData));
            }
        }
        else if (Source == "Field")
        {

            cards = player1.board.M.GetComponent<MeleeZone>().melee;
            for (int i = 0; i < cards.Count; i++)
            {
                result.Add((cards[i], cards[i].GetComponent<CardDisplay>().cardData));
            }

            cards = player1.board.R.GetComponent<RangedZone>().ranged;
            for (int i = 0; i < cards.Count; i++)
            {
                result.Add((cards[i], cards[i].GetComponent<CardDisplay>().cardData));
            }

            cards = player1.board.S.GetComponent<SiegeZone>().siege;
            for (int i = 0; i < cards.Count; i++)
            {
                result.Add((cards[i], cards[i].GetComponent<CardDisplay>().cardData));
            }
        }
        else if (Source == "deck")
        {
            cards = player1.deck.deck;
            for (int i = 0; i < cards.Count; i++)
            {
                result.Add((cards[i], cards[i].GetComponent<CardDisplay>().cardData));
            }
        }

        else if (Source == "hand")
        {
            cards = player1.hand.CardsInHand;
            for (int i = 0; i < cards.Count; i++)
            {
                result.Add((cards[i], cards[i].GetComponent<CardDisplay>().cardData));
            }
        }

        else if (Source == "graveyard")
        {
            cards = player1.board.Cemetery.GetComponent<CemeteryZone>().Cemetery;
            for (int i = 0; i < cards.Count; i++)
            {
                result.Add((cards[i], cards[i].GetComponent<CardDisplay>().cardData));
            }
        }
        else if (Source == "otherField")
        {
            cards = player2.board.M.GetComponent<MeleeZone>().melee;
            for (int i = 0; i < cards.Count; i++)
            {
                result.Add((cards[i], cards[i].GetComponent<CardDisplay>().cardData));
            }

            cards = player2.board.R.GetComponent<RangedZone>().ranged;
            for (int i = 0; i < cards.Count; i++)
            {
                result.Add((cards[i], cards[i].GetComponent<CardDisplay>().cardData));
            }

            cards = player2.board.S.GetComponent<SiegeZone>().siege;
            for (int i = 0; i < cards.Count; i++)
            {
                result.Add((cards[i], cards[i].GetComponent<CardDisplay>().cardData));
            }
        }

        else if (Source == "otherDeck")
        {
            cards = player2.deck.deck;

            for (int i = 0; i < cards.Count; i++)
            {
                result.Add((cards[i], cards[i].GetComponent<CardDisplay>().cardData));
            }
        }
        else if (Source == "otherHand")
        {
            cards = player2.hand.CardsInHand;

            for (int i = 0; i < cards.Count; i++)
            {
                result.Add((cards[i], cards[i].GetComponent<CardDisplay>().cardData));
            }
        }
        else if (Source == "otherGraveyard")
        {
            cards = player2.board.Cemetery.GetComponent<CemeteryZone>().Cemetery;

            for (int i = 0; i < cards.Count; i++)
            {
                result.Add((cards[i], cards[i].GetComponent<CardDisplay>().cardData));
            }
        }
        return result;
    }

}
