using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effects : MonoBehaviour
{
    public static void ActivateEffect(GameObject gameObject)
    {
        switch (gameObject.GetComponent<CardDisplay>().card.Effect)
        {
            //Unit effects
            case Card.TypeEffects.Put_Increase:
                Put_Increase();
                break;
            case Card.TypeEffects.Put_Climate:
                Put_Climate();
                break;
            case Card.TypeEffects.Delete_Card_with_Max_Power_on_the_field:
                Delete_Card_with_Max_Power_on_the_field();
                break;
            case Card.TypeEffects.Delete_Card_with_Min_Power_on_the_field:
                Delete_Card_with_Min_Power_on_the_field();
                break;
            case Card.TypeEffects.Draw_Card_from_Deck:
                Draw_Card_from_Deck();
                break;
            case Card.TypeEffects.Clear_file:
                Clear_the_row_with_fewer_cards_on_the_field();
                break;
            case Card.TypeEffects.Average_Power_on_the_field:
                Average_Power_on_the_field(gameObject);
                break;
            case Card.TypeEffects.Decreases_one_Point:
                Decreases_one_Point();
                break;

            //Special effects
            case Card.TypeEffects.Climate:
                Climate(gameObject);
                break;
            case Card.TypeEffects.Clearance:
                Clearance(gameObject);
                break;
            case Card.TypeEffects.Increase:
                Increase(gameObject);
                break;
            case Card.TypeEffects.Lure:
                None();
                break;

            //Boss effects
            case Card.TypeEffects.StayintheField:
                StayintheField();
                break;

            //Without effect
            default:
                None();
                break;
        }
    }

    //Unit effects
    public static void Put_Increase()
    {
        GameObject GameManager = GameObject.Find("GameManager");

        Debug.Log("Put_IncreaseEffect");
        //player 1
        if (GameManager.GetComponent<GameManager>().player1.isPlaying)
        {
            for (int i = 0; i < GameManager.GetComponent<GameManager>().player1.hand.CardsInDeck.Count; i++)
            {
                if (GameManager.GetComponent<GameManager>().player1.hand.CardsInDeck[i].GetComponent<CardDisplay>().card.TypeSpecialCard == Card.SubTypeSpecialCard.Increase)
                {
                    GameManager.GetComponent<GameManager>().player1.hand.CardsInDeck[i].GetComponent<MoveCard>().Move();
                }
            }
        }
        //player 2
        else
        {
            for (int i = 0; i < GameManager.GetComponent<GameManager>().player2.hand.CardsInDeck.Count; i++)
            {
                if (GameManager.GetComponent<GameManager>().player2.hand.CardsInDeck[i].GetComponent<CardDisplay>().card.TypeSpecialCard == Card.SubTypeSpecialCard.Increase)
                {
                    GameManager.GetComponent<GameManager>().player2.hand.CardsInDeck[i].GetComponent<MoveCard>().Move();
                }
            }
        }
    }
    public static void Put_Climate()
    {
        GameObject GameManager = GameObject.Find("GameManager");

        Debug.Log("Put_Climate Effect");

        //player 1
        if (GameManager.GetComponent<GameManager>().player1.isPlaying)
        {
            for (int i = 0; i < GameManager.GetComponent<GameManager>().player1.hand.CardsInDeck.Count; i++)
            {
                if (GameManager.GetComponent<GameManager>().player1.hand.CardsInDeck[i].GetComponent<CardDisplay>().card.TypeSpecialCard == Card.SubTypeSpecialCard.Climate)
                {
                    GameManager.GetComponent<GameManager>().player1.hand.CardsInDeck[i].GetComponent<MoveCard>().Move();
                }
            }
        }
        //player 2
        else
        {
            for (int i = 0; i < GameManager.GetComponent<GameManager>().player2.hand.CardsInDeck.Count; i++)
            {
                if (GameManager.GetComponent<GameManager>().player2.hand.CardsInDeck[i].GetComponent<CardDisplay>().card.TypeSpecialCard == Card.SubTypeSpecialCard.Climate)
                {
                    GameManager.GetComponent<GameManager>().player2.hand.CardsInDeck[i].GetComponent<MoveCard>().Move();
                }
            }
        }
    }
    public static void Delete_Card_with_Max_Power_on_the_field()
    {
        GameObject GameManager = GameObject.Find("GameManager");

        Debug.Log("Delete_Card_with_Max_Power_on_the_field Effect");

        GameObject cardTarget = new();

        //player 1
        if (GameManager.GetComponent<GameManager>().player1.isPlaying)
        {
            cardTarget = GameManager.GetComponent<GameManager>().player2.board.M.GetComponent<MeleeZone>().melee[0];

            for (int i = 0; i < GameManager.GetComponent<GameManager>().player2.board.M.GetComponent<MeleeZone>().melee.Count; i++)
            {
                if (cardTarget.GetComponent<CardDisplay>().card.Power < GameManager.GetComponent<GameManager>().player2.board.M.GetComponent<MeleeZone>().melee[i].GetComponent<CardDisplay>().card.Power)
                {
                    cardTarget = GameManager.GetComponent<GameManager>().player2.board.M.GetComponent<MeleeZone>().melee[i];
                }
            }
            for (int i = 0; i < GameManager.GetComponent<GameManager>().player2.board.R.GetComponent<RangedZone>().ranged.Count; i++)
            {
                if (cardTarget.GetComponent<CardDisplay>().card.Power < GameManager.GetComponent<GameManager>().player2.board.R.GetComponent<RangedZone>().ranged[i].GetComponent<CardDisplay>().card.Power)
                {
                    cardTarget = GameManager.GetComponent<GameManager>().player2.board.R.GetComponent<RangedZone>().ranged[i];
                }
            }
            for (int i = 0; i < GameManager.GetComponent<GameManager>().player2.board.S.GetComponent<SiegeZone>().siege.Count; i++)
            {
                if (cardTarget.GetComponent<CardDisplay>().card.Power < GameManager.GetComponent<GameManager>().player2.board.S.GetComponent<SiegeZone>().siege[i].GetComponent<CardDisplay>().card.Power)
                {
                    cardTarget = GameManager.GetComponent<GameManager>().player2.board.S.GetComponent<SiegeZone>().siege[i];
                }
            }
        }
        //player 2
        else
        {
            cardTarget = GameManager.GetComponent<GameManager>().player1.board.M.GetComponent<MeleeZone>().melee[0];

            for (int i = 0; i < GameManager.GetComponent<GameManager>().player1.board.M.GetComponent<MeleeZone>().melee.Count; i++)
            {
                if (cardTarget.GetComponent<CardDisplay>().card.Power < GameManager.GetComponent<GameManager>().player1.board.M.GetComponent<MeleeZone>().melee[i].GetComponent<CardDisplay>().card.Power)
                {
                    cardTarget = GameManager.GetComponent<GameManager>().player1.board.M.GetComponent<MeleeZone>().melee[i];
                }
            }
            for (int i = 0; i < GameManager.GetComponent<GameManager>().player1.board.R.GetComponent<RangedZone>().ranged.Count; i++)
            {
                if (cardTarget.GetComponent<CardDisplay>().card.Power < GameManager.GetComponent<GameManager>().player1.board.R.GetComponent<RangedZone>().ranged[i].GetComponent<CardDisplay>().card.Power)
                {
                    cardTarget = GameManager.GetComponent<GameManager>().player1.board.R.GetComponent<RangedZone>().ranged[i];
                }
            }
            for (int i = 0; i < GameManager.GetComponent<GameManager>().player1.board.S.GetComponent<SiegeZone>().siege.Count; i++)
            {
                if (cardTarget.GetComponent<CardDisplay>().card.Power < GameManager.GetComponent<GameManager>().player1.board.S.GetComponent<SiegeZone>().siege[i].GetComponent<CardDisplay>().card.Power)
                {
                    cardTarget = GameManager.GetComponent<GameManager>().player1.board.S.GetComponent<SiegeZone>().siege[i];
                }
            }
        }

        cardTarget.GetComponent<MoveCard>().MoveToCemetery();
    }
    public static void Delete_Card_with_Min_Power_on_the_field()
    {
        GameObject GameManager = GameObject.Find("GameManager");

        Debug.Log("Delete_Card_with_Min_Power_on_the_field Effect");

        GameObject cardTarget = new();

        //player 1
        if (GameManager.GetComponent<GameManager>().player1.isPlaying)
        {
            cardTarget = GameManager.GetComponent<GameManager>().player2.board.M.GetComponent<MeleeZone>().melee[0];

            for (int i = 0; i < GameManager.GetComponent<GameManager>().player2.board.M.GetComponent<MeleeZone>().melee.Count; i++)
            {
                if (cardTarget.GetComponent<CardDisplay>().card.Power > GameManager.GetComponent<GameManager>().player2.board.M.GetComponent<MeleeZone>().melee[i].GetComponent<CardDisplay>().card.Power)
                {
                    cardTarget = GameManager.GetComponent<GameManager>().player2.board.M.GetComponent<MeleeZone>().melee[i];
                }
            }
            for (int i = 0; i < GameManager.GetComponent<GameManager>().player2.board.R.GetComponent<RangedZone>().ranged.Count; i++)
            {
                if (cardTarget.GetComponent<CardDisplay>().card.Power > GameManager.GetComponent<GameManager>().player2.board.R.GetComponent<RangedZone>().ranged[i].GetComponent<CardDisplay>().card.Power)
                {
                    cardTarget = GameManager.GetComponent<GameManager>().player2.board.R.GetComponent<RangedZone>().ranged[i];
                }
            }
            for (int i = 0; i < GameManager.GetComponent<GameManager>().player2.board.S.GetComponent<SiegeZone>().siege.Count; i++)
            {
                if (cardTarget.GetComponent<CardDisplay>().card.Power > GameManager.GetComponent<GameManager>().player2.board.S.GetComponent<SiegeZone>().siege[i].GetComponent<CardDisplay>().card.Power)
                {
                    cardTarget = GameManager.GetComponent<GameManager>().player2.board.S.GetComponent<SiegeZone>().siege[i];
                }
            }
        }

        //player 2
        else
        {
            cardTarget = GameManager.GetComponent<GameManager>().player1.board.M.GetComponent<MeleeZone>().melee[0];

            for (int i = 0; i < GameManager.GetComponent<GameManager>().player1.board.M.GetComponent<MeleeZone>().melee.Count; i++)
            {
                if (cardTarget.GetComponent<CardDisplay>().card.Power > GameManager.GetComponent<GameManager>().player1.board.M.GetComponent<MeleeZone>().melee[i].GetComponent<CardDisplay>().card.Power)
                {
                    cardTarget = GameManager.GetComponent<GameManager>().player1.board.M.GetComponent<MeleeZone>().melee[i];
                }
            }
            for (int i = 0; i < GameManager.GetComponent<GameManager>().player1.board.R.GetComponent<RangedZone>().ranged.Count; i++)
            {
                if (cardTarget.GetComponent<CardDisplay>().card.Power > GameManager.GetComponent<GameManager>().player1.board.R.GetComponent<RangedZone>().ranged[i].GetComponent<CardDisplay>().card.Power)
                {
                    cardTarget = GameManager.GetComponent<GameManager>().player1.board.R.GetComponent<RangedZone>().ranged[i];
                }
            }
            for (int i = 0; i < GameManager.GetComponent<GameManager>().player1.board.S.GetComponent<SiegeZone>().siege.Count; i++)
            {
                if (cardTarget.GetComponent<CardDisplay>().card.Power > GameManager.GetComponent<GameManager>().player1.board.S.GetComponent<SiegeZone>().siege[i].GetComponent<CardDisplay>().card.Power)
                {
                    cardTarget = GameManager.GetComponent<GameManager>().player1.board.S.GetComponent<SiegeZone>().siege[i];
                }
            }
        }

        cardTarget.GetComponent<MoveCard>().MoveToCemetery();
    }
    public static void Draw_Card_from_Deck()
    {
        GameObject GameManager = GameObject.Find("GameManager");

        Debug.Log("Draw_Card_from_Deck Effect");

        if (GameManager.GetComponent<GameManager>().player1.isPlaying)
        {
            int indexCard = Random.Range(1, GameManager.GetComponent<GameManager>().player1.hand.CardsInDeck.Count - 1);
            GameObject drawCard = Instantiate(GameManager.GetComponent<GameManager>().player1.hand.CardsInDeck[indexCard], new Vector3(0, 0, 0), Quaternion.identity);
            drawCard.transform.SetParent(GameManager.GetComponent<GameManager>().player1.hand.transform, false);
            GameManager.GetComponent<GameManager>().player1.hand.CardsInHand.Add(drawCard);
            GameManager.GetComponent<GameManager>().player1.hand.CardsInDeck.RemoveAt(indexCard);
        }
        else
        {
            int indexCard = Random.Range(1, GameManager.GetComponent<GameManager>().player2.hand.CardsInDeck.Count - 1);
            GameObject drawCard = Instantiate(GameManager.GetComponent<GameManager>().player2.hand.CardsInDeck[indexCard], new Vector3(0, 0, 0), Quaternion.identity);
            drawCard.transform.SetParent(GameManager.GetComponent<GameManager>().player2.hand.transform, false);
            GameManager.GetComponent<GameManager>().player2.hand.CardsInHand.Add(drawCard);
            GameManager.GetComponent<GameManager>().player2.hand.CardsInDeck.RemoveAt(indexCard);
        }
    }
    public static void Clear_the_row_with_fewer_cards_on_the_field()
    {
        GameObject GameManager = GameObject.Find("GameManager");

        Debug.Log("Clear_the_row_with_fewer_cards_on_the_field Effect");

        if (GameManager.GetComponent<GameManager>().player1.isPlaying)
        {
            if (GameManager.GetComponent<GameManager>().player2.board.M.GetComponent<MeleeZone>().melee.Count < GameManager.GetComponent<GameManager>().player2.board.R.GetComponent<RangedZone>().ranged.Count && GameManager.GetComponent<GameManager>().player2.board.M.GetComponent<MeleeZone>().melee.Count < GameManager.GetComponent<GameManager>().player2.board.S.GetComponent<SiegeZone>().siege.Count)
            {
                for (int i = 0; i < GameManager.GetComponent<GameManager>().player2.board.M.GetComponent<MeleeZone>().melee.Count; i++)
                {
                    GameManager.GetComponent<GameManager>().player2.board.M.GetComponent<MeleeZone>().melee[i].GetComponent<MoveCard>().MoveToCemetery();
                }
                GameManager.GetComponent<GameManager>().player2.board.M.GetComponent<MeleeZone>().melee.Clear();
            }
            else if (GameManager.GetComponent<GameManager>().player2.board.R.GetComponent<RangedZone>().ranged.Count < GameManager.GetComponent<GameManager>().player2.board.M.GetComponent<MeleeZone>().melee.Count && GameManager.GetComponent<GameManager>().player2.board.R.GetComponent<RangedZone>().ranged.Count < GameManager.GetComponent<GameManager>().player2.board.S.GetComponent<SiegeZone>().siege.Count)
            {
                for (int i = 0; i < GameManager.GetComponent<GameManager>().player2.board.R.GetComponent<RangedZone>().ranged.Count; i++)
                {
                    GameManager.GetComponent<GameManager>().player2.board.R.GetComponent<RangedZone>().ranged[i].GetComponent<MoveCard>().MoveToCemetery();
                }
                GameManager.GetComponent<GameManager>().player2.board.R.GetComponent<RangedZone>().ranged.Clear();
            }
            else
            {
                for (int i = 0; i < GameManager.GetComponent<GameManager>().player2.board.S.GetComponent<SiegeZone>().siege.Count; i++)
                {
                    GameManager.GetComponent<GameManager>().player2.board.S.GetComponent<SiegeZone>().siege[i].GetComponent<MoveCard>().MoveToCemetery();
                }
                GameManager.GetComponent<GameManager>().player1.board.S.GetComponent<SiegeZone>().siege.Clear();
            }
        }
        else
        {
            if (GameManager.GetComponent<GameManager>().player1.board.M.GetComponent<MeleeZone>().melee.Count < GameManager.GetComponent<GameManager>().player1.board.R.GetComponent<RangedZone>().ranged.Count && GameManager.GetComponent<GameManager>().player1.board.M.GetComponent<MeleeZone>().melee.Count < GameManager.GetComponent<GameManager>().player1.board.S.GetComponent<SiegeZone>().siege.Count)
            {
                for (int i = 0; i < GameManager.GetComponent<GameManager>().player1.board.M.GetComponent<MeleeZone>().melee.Count; i++)
                {
                    GameManager.GetComponent<GameManager>().player1.board.M.GetComponent<MeleeZone>().melee[i].GetComponent<MoveCard>().MoveToCemetery();
                }
                GameManager.GetComponent<GameManager>().player1.board.M.GetComponent<MeleeZone>().melee.Clear();
            }
            else if (GameManager.GetComponent<GameManager>().player1.board.R.GetComponent<RangedZone>().ranged.Count < GameManager.GetComponent<GameManager>().player1.board.M.GetComponent<MeleeZone>().melee.Count && GameManager.GetComponent<GameManager>().player1.board.R.GetComponent<RangedZone>().ranged.Count < GameManager.GetComponent<GameManager>().player1.board.S.GetComponent<SiegeZone>().siege.Count)
            {
                for (int i = 0; i < GameManager.GetComponent<GameManager>().player1.board.R.GetComponent<RangedZone>().ranged.Count; i++)
                {
                    GameManager.GetComponent<GameManager>().player1.board.R.GetComponent<RangedZone>().ranged[i].GetComponent<MoveCard>().MoveToCemetery();
                }
                GameManager.GetComponent<GameManager>().player1.board.R.GetComponent<RangedZone>().ranged.Clear();
            }
            else
            {
                for (int i = 0; i < GameManager.GetComponent<GameManager>().player1.board.S.GetComponent<SiegeZone>().siege.Count; i++)
                {
                    GameManager.GetComponent<GameManager>().player1.board.S.GetComponent<SiegeZone>().siege[i].GetComponent<MoveCard>().MoveToCemetery();
                }
                GameManager.GetComponent<GameManager>().player1.board.S.GetComponent<SiegeZone>().siege.Clear();
            }
        }
    }
    public static void Average_Power_on_the_field(GameObject gameObject)
    {
        GameObject GameManager = GameObject.Find("GameManager");

        Debug.Log("Average_Power_on_the_field Effect");

        int average = 0;

        for (int i = 0; i < GameManager.GetComponent<GameManager>().player2.board.M.GetComponent<MeleeZone>().melee.Count; i++)
        {
            average += GameManager.GetComponent<GameManager>().player2.board.M.GetComponent<MeleeZone>().melee[i].GetComponent<CardDisplay>().card.Power;
        }
        for (int i = 0; i < GameManager.GetComponent<GameManager>().player2.board.R.GetComponent<RangedZone>().ranged.Count; i++)
        {
            average += GameManager.GetComponent<GameManager>().player2.board.R.GetComponent<RangedZone>().ranged[i].GetComponent<CardDisplay>().card.Power;
        }
        for (int i = 0; i < GameManager.GetComponent<GameManager>().player2.board.S.GetComponent<SiegeZone>().siege.Count; i++)
        {
            average += GameManager.GetComponent<GameManager>().player2.board.S.GetComponent<SiegeZone>().siege[i].GetComponent<CardDisplay>().card.Power;
        }

        for (int i = 0; i < GameManager.GetComponent<GameManager>().player1.board.M.GetComponent<MeleeZone>().melee.Count; i++)
        {
            average += GameManager.GetComponent<GameManager>().player1.board.M.GetComponent<MeleeZone>().melee[i].GetComponent<CardDisplay>().card.Power;
        }
        for (int i = 0; i < GameManager.GetComponent<GameManager>().player1.board.R.GetComponent<RangedZone>().ranged.Count; i++)
        {
            average += GameManager.GetComponent<GameManager>().player1.board.R.GetComponent<RangedZone>().ranged[i].GetComponent<CardDisplay>().card.Power;
        }
        for (int i = 0; i < GameManager.GetComponent<GameManager>().player1.board.S.GetComponent<SiegeZone>().siege.Count; i++)
        {
            average += GameManager.GetComponent<GameManager>().player1.board.S.GetComponent<SiegeZone>().siege[i].GetComponent<CardDisplay>().card.Power;
        }

        average /= GameManager.GetComponent<GameManager>().player1.board.M.GetComponent<MeleeZone>().melee.Count
             + GameManager.GetComponent<GameManager>().player1.board.R.GetComponent<RangedZone>().ranged.Count
             + GameManager.GetComponent<GameManager>().player1.board.S.GetComponent<SiegeZone>().siege.Count
             + GameManager.GetComponent<GameManager>().player2.board.M.GetComponent<MeleeZone>().melee.Count
             + GameManager.GetComponent<GameManager>().player2.board.R.GetComponent<RangedZone>().ranged.Count
             + GameManager.GetComponent<GameManager>().player2.board.S.GetComponent<SiegeZone>().siege.Count;

        gameObject.GetComponent<CardDisplay>().card.Power = average;
    }
    public static void Decreases_one_Point()
    {
        GameObject GameManager = GameObject.Find("GameManager");

        Debug.Log("Decreases_one_Point Effect");

        int indextype = Random.Range(1, 3);

        if (GameManager.GetComponent<GameManager>().player1.isPlaying)
        {
            if (indextype == 1)
            {
                int index = Random.Range(1, GameManager.GetComponent<GameManager>().player2.board.M.GetComponent<MeleeZone>().melee.Count - 1);
                GameManager.GetComponent<GameManager>().player2.board.M.GetComponent<MeleeZone>().melee[index].GetComponent<CardDisplay>().card.Power--;
            }
            else if (indextype == 2)
            {
                int index = Random.Range(1, GameManager.GetComponent<GameManager>().player2.board.R.GetComponent<RangedZone>().ranged.Count - 1);
                GameManager.GetComponent<GameManager>().player2.board.R.GetComponent<RangedZone>().ranged[index].GetComponent<CardDisplay>().card.Power--;
            }
            else
            {
                int index = Random.Range(1, GameManager.GetComponent<GameManager>().player2.board.S.GetComponent<SiegeZone>().siege.Count - 1);
                GameManager.GetComponent<GameManager>().player2.board.S.GetComponent<SiegeZone>().siege[index].GetComponent<CardDisplay>().card.Power--;
            }
        }
        else
        {
            if (indextype == 1)
            {
                int index = Random.Range(1, GameManager.GetComponent<GameManager>().player1.board.M.GetComponent<MeleeZone>().melee.Count - 1);
                GameManager.GetComponent<GameManager>().player1.board.M.GetComponent<MeleeZone>().melee[index].GetComponent<CardDisplay>().card.Power--;

            }
            else if (indextype == 2)
            {
                int index = Random.Range(1, GameManager.GetComponent<GameManager>().player1.board.R.GetComponent<RangedZone>().ranged.Count - 1);
                GameManager.GetComponent<GameManager>().player1.board.R.GetComponent<RangedZone>().ranged[index].GetComponent<CardDisplay>().card.Power--;
            }
            else
            {
                int index = Random.Range(1, GameManager.GetComponent<GameManager>().player1.board.S.GetComponent<SiegeZone>().siege.Count - 1);
                GameManager.GetComponent<GameManager>().player1.board.S.GetComponent<SiegeZone>().siege[index].GetComponent<CardDisplay>().card.Power--;
            }
        }
    }

    //Special effects
    public static void Clearance(GameObject gameObject)
    {
        GameObject GameManager = GameObject.Find("GameManager");

        Debug.Log("Clearance Effect");

        if (GameManager.GetComponent<GameManager>().player1.isPlaying)
        {
            if (GameManager.GetComponent<GameManager>().player2.board.Climate.GetComponent<ClimateZone>().climate != null)
            {
                GameManager.GetComponent<GameManager>().player2.board.Climate.GetComponent<ClimateZone>().climate.GetComponent<MoveCard>().MoveToCemetery();
            }
            gameObject.transform.position = GameManager.GetComponent<GameManager>().player2.board.Climate.GetComponent<ClimateZone>().climate.transform.position;
        }
        else
        {
            if (GameManager.GetComponent<GameManager>().player1.board.Climate.GetComponent<ClimateZone>().climate != null)
            {
                GameManager.GetComponent<GameManager>().player1.board.Climate.GetComponent<ClimateZone>().climate.GetComponent<MoveCard>().MoveToCemetery();
            }
            gameObject.transform.position = GameManager.GetComponent<GameManager>().player1.board.Climate.GetComponent<ClimateZone>().climate.transform.position;
        }
        gameObject.GetComponent<MoveCard>().MoveToCemetery();
    }
    public static void Climate(GameObject gameObject)
    {
        GameObject GameManager = GameObject.Find("GameManager");

        Debug.Log("Climate Effect");

        if (gameObject.GetComponent<CardDisplay>().card.TypeField == 'M')
        {
            for (int i = 0; i < GameManager.GetComponent<GameManager>().player1.board.M.GetComponent<MeleeZone>().melee.Count; i++)
            {
                if (GameManager.GetComponent<GameManager>().player1.board.M.GetComponent<MeleeZone>().melee[i].GetComponent<CardDisplay>().card.TypeUnitCard != Card.SubTypeUnitCard.Gold)
                {
                    GameManager.GetComponent<GameManager>().player1.board.M.GetComponent<MeleeZone>().melee[i].GetComponent<CardDisplay>().card.Power = 0;
                }
            }
            for (int i = 0; i < GameManager.GetComponent<GameManager>().player2.board.M.GetComponent<MeleeZone>().melee.Count; i++)
            {
                if (GameManager.GetComponent<GameManager>().player2.board.M.GetComponent<MeleeZone>().melee[i].GetComponent<CardDisplay>().card.TypeUnitCard != Card.SubTypeUnitCard.Gold)
                {
                    GameManager.GetComponent<GameManager>().player2.board.M.GetComponent<MeleeZone>().melee[i].GetComponent<CardDisplay>().card.Power = 0;
                }
            }
        }
        else if (gameObject.GetComponent<CardDisplay>().card.TypeField == 'R')
        {
            for (int i = 0; i < GameManager.GetComponent<GameManager>().player1.board.R.GetComponent<RangedZone>().ranged.Count; i++)
            {
                if (GameManager.GetComponent<GameManager>().player1.board.R.GetComponent<RangedZone>().ranged[i].GetComponent<CardDisplay>().card.TypeUnitCard != Card.SubTypeUnitCard.Gold)
                {
                    GameManager.GetComponent<GameManager>().player1.board.R.GetComponent<RangedZone>().ranged[i].GetComponent<CardDisplay>().card.Power = 0;
                }
            }
            for (int i = 0; i < GameManager.GetComponent<GameManager>().player2.board.R.GetComponent<RangedZone>().ranged.Count; i++)
            {
                if (GameManager.GetComponent<GameManager>().player2.board.R.GetComponent<RangedZone>().ranged[i].GetComponent<CardDisplay>().card.TypeUnitCard != Card.SubTypeUnitCard.Gold)
                {
                    GameManager.GetComponent<GameManager>().player2.board.R.GetComponent<RangedZone>().ranged[i].GetComponent<CardDisplay>().card.Power = 0;
                }
            }
        }
        else
        {
            for (int i = 0; i < GameManager.GetComponent<GameManager>().player1.board.S.GetComponent<SiegeZone>().siege.Count; i++)
            {
                if (GameManager.GetComponent<GameManager>().player1.board.S.GetComponent<SiegeZone>().siege[i].GetComponent<CardDisplay>().card.TypeUnitCard != Card.SubTypeUnitCard.Gold)
                {
                    GameManager.GetComponent<GameManager>().player1.board.S.GetComponent<SiegeZone>().siege[i].GetComponent<CardDisplay>().card.Power--;
                }
            }
            for (int i = 0; i < GameManager.GetComponent<GameManager>().player2.board.S.GetComponent<SiegeZone>().siege.Count; i++)
            {
                if (GameManager.GetComponent<GameManager>().player2.board.S.GetComponent<SiegeZone>().siege[i].GetComponent<CardDisplay>().card.TypeUnitCard != Card.SubTypeUnitCard.Gold)
                {
                    GameManager.GetComponent<GameManager>().player2.board.S.GetComponent<SiegeZone>().siege[i].GetComponent<CardDisplay>().card.Power--;
                }
            }
        }
    }

    public static void Increase(GameObject gameObject)
    {
        GameObject GameManager = GameObject.Find("GameManager");

        Debug.Log("Increase Effect");

        if (GameManager.GetComponent<GameManager>().player1.isPlaying)
        {
            if (gameObject.GetComponent<CardDisplay>().card.TypeField == 'M')
            {
                for (int i = 0; i < GameManager.GetComponent<GameManager>().player1.board.M.GetComponent<MeleeZone>().melee.Count; i++)
                {
                    if (GameManager.GetComponent<GameManager>().player1.board.M.GetComponent<MeleeZone>().melee[i].GetComponent<CardDisplay>().card.TypeUnitCard != Card.SubTypeUnitCard.Gold)
                    {
                        GameManager.GetComponent<GameManager>().player1.board.M.GetComponent<MeleeZone>().melee[i].GetComponent<CardDisplay>().card.Power += 2;
                        GameManager.GetComponent<GameManager>().player1.board.M.GetComponent<MeleeZone>().melee[i].GetComponent<CardDisplay>().card.StartPower += 2;
                    }
                }
            }
            else if (gameObject.GetComponent<CardDisplay>().card.TypeField == 'R')
            {
                for (int i = 0; i < GameManager.GetComponent<GameManager>().player1.board.R.GetComponent<RangedZone>().ranged.Count; i++)
                {
                    if (GameManager.GetComponent<GameManager>().player1.board.R.GetComponent<RangedZone>().ranged[i].GetComponent<CardDisplay>().card.TypeUnitCard != Card.SubTypeUnitCard.Gold)
                    {
                        GameManager.GetComponent<GameManager>().player1.board.R.GetComponent<RangedZone>().ranged[i].GetComponent<CardDisplay>().card.Power += 2;
                        GameManager.GetComponent<GameManager>().player1.board.R.GetComponent<RangedZone>().ranged[i].GetComponent<CardDisplay>().card.StartPower += 2;
                    }
                }
            }
            else
            {
                for (int i = 0; i < GameManager.GetComponent<GameManager>().player1.board.S.GetComponent<SiegeZone>().siege.Count; i++)
                {
                    if (GameManager.GetComponent<GameManager>().player1.board.S.GetComponent<SiegeZone>().siege[i].GetComponent<CardDisplay>().card.TypeUnitCard != Card.SubTypeUnitCard.Gold)
                    {
                        GameManager.GetComponent<GameManager>().player1.board.S.GetComponent<SiegeZone>().siege[i].GetComponent<CardDisplay>().card.Power += 2;
                        GameManager.GetComponent<GameManager>().player1.board.S.GetComponent<SiegeZone>().siege[i].GetComponent<CardDisplay>().card.StartPower += 2;
                    }
                }
            }
        }
        else
        {
            if (gameObject.GetComponent<CardDisplay>().card.TypeField == 'M')
            {
                for (int i = 0; i < GameManager.GetComponent<GameManager>().player2.board.M.GetComponent<MeleeZone>().melee.Count; i++)
                {
                    if (GameManager.GetComponent<GameManager>().player2.board.M.GetComponent<MeleeZone>().melee[i].GetComponent<CardDisplay>().card.TypeUnitCard != Card.SubTypeUnitCard.Gold)
                    {
                        GameManager.GetComponent<GameManager>().player2.board.M.GetComponent<MeleeZone>().melee[i].GetComponent<CardDisplay>().card.Power += 2;
                        GameManager.GetComponent<GameManager>().player2.board.M.GetComponent<MeleeZone>().melee[i].GetComponent<CardDisplay>().card.StartPower += 2;
                    }
                }
            }
            else if (gameObject.GetComponent<CardDisplay>().card.TypeField == 'R')
            {
                for (int i = 0; i < GameManager.GetComponent<GameManager>().player2.board.R.GetComponent<RangedZone>().ranged.Count; i++)
                {
                    if (GameManager.GetComponent<GameManager>().player2.board.R.GetComponent<RangedZone>().ranged[i].GetComponent<CardDisplay>().card.TypeUnitCard != Card.SubTypeUnitCard.Gold)
                    {
                        GameManager.GetComponent<GameManager>().player2.board.R.GetComponent<RangedZone>().ranged[i].GetComponent<CardDisplay>().card.Power += 2;
                        GameManager.GetComponent<GameManager>().player2.board.R.GetComponent<RangedZone>().ranged[i].GetComponent<CardDisplay>().card.StartPower += 2;
                    }
                }
            }
            else
            {
                for (int i = 0; i < GameManager.GetComponent<GameManager>().player2.board.S.GetComponent<SiegeZone>().siege.Count; i++)
                {
                    if (GameManager.GetComponent<GameManager>().player2.board.S.GetComponent<SiegeZone>().siege[i].GetComponent<CardDisplay>().card.TypeUnitCard != Card.SubTypeUnitCard.Gold)
                    {
                        GameManager.GetComponent<GameManager>().player2.board.S.GetComponent<SiegeZone>().siege[i].GetComponent<CardDisplay>().card.Power += 2;
                        GameManager.GetComponent<GameManager>().player2.board.S.GetComponent<SiegeZone>().siege[i].GetComponent<CardDisplay>().card.StartPower += 2;
                    }
                }
            }
        }
    }

    //Boss effects
    public static void StayintheField()
    {
        GameObject GameManager = GameObject.Find("GameManager");

        int indextype = Random.Range(1, 3);

        if (indextype == 1)
        {
            int index = Random.Range(1, GameManager.GetComponent<GameManager>().player1.board.M.GetComponent<MeleeZone>().melee.Count - 1);
            GameManager.GetComponent<GameManager>().player2.board.M.GetComponent<MeleeZone>().melee[index].GetComponent<CardDisplay>().card.stayintheField = true;
        }
        else if (indextype == 2)
        {
            int index = Random.Range(1, GameManager.GetComponent<GameManager>().player1.board.R.GetComponent<RangedZone>().ranged.Count - 1);
            GameManager.GetComponent<GameManager>().player2.board.R.GetComponent<RangedZone>().ranged[index].GetComponent<CardDisplay>().card.stayintheField = true;
        }
        else
        {
            int index = Random.Range(1, GameManager.GetComponent<GameManager>().player1.board.S.GetComponent<SiegeZone>().siege.Count - 1);
            GameManager.GetComponent<GameManager>().player2.board.S.GetComponent<SiegeZone>().siege[index].GetComponent<CardDisplay>().card.stayintheField = true;
        }
    }

    //Without effect
    public static void None()
    {
        Debug.Log("No effect");
    }

    // Path: Assets/Scripts/Cards/Effects.cs
    //Aux effects
    public static void DisableEffectClimate()
    {
        GameObject GameManager = GameObject.FindGameObjectWithTag("GameManager");

        Debug.Log("DisableEffectClimate");
        if (GameManager.GetComponent<GameManager>().player2.board.Climate.GetComponent<ClimateZone>().climate.GetComponent<CardDisplay>().card.TypeField == 'M')
        {
            for (int i = 0; i < GameManager.GetComponent<GameManager>().player1.board.M.GetComponent<MeleeZone>().melee.Count; i++)
            {
                GameManager.GetComponent<GameManager>().player1.board.M.GetComponent<MeleeZone>().melee[i].GetComponent<CardDisplay>().card.Power = GameManager.GetComponent<GameManager>().player1.board.M.GetComponent<MeleeZone>().melee[i].GetComponent<CardDisplay>().card.StartPower;
            }
            for (int i = 0; i < GameManager.GetComponent<GameManager>().player2.board.M.GetComponent<MeleeZone>().melee.Count; i++)
            {
                GameManager.GetComponent<GameManager>().player2.board.M.GetComponent<MeleeZone>().melee[i].GetComponent<CardDisplay>().card.Power = GameManager.GetComponent<GameManager>().player2.board.M.GetComponent<MeleeZone>().melee[i].GetComponent<CardDisplay>().card.StartPower;
            }
        }
        else if (GameManager.GetComponent<GameManager>().player2.board.Climate.GetComponent<ClimateZone>().climate.GetComponent<CardDisplay>().card.TypeField == 'R')
        {
            for (int i = 0; i < GameManager.GetComponent<GameManager>().player1.board.R.GetComponent<RangedZone>().ranged.Count; i++)
            {
                GameManager.GetComponent<GameManager>().player1.board.R.GetComponent<RangedZone>().ranged[i].GetComponent<CardDisplay>().card.Power = GameManager.GetComponent<GameManager>().player1.board.R.GetComponent<RangedZone>().ranged[i].GetComponent<CardDisplay>().card.StartPower;
            }
            for (int i = 0; i < GameManager.GetComponent<GameManager>().player2.board.R.GetComponent<RangedZone>().ranged.Count; i++)
            {
                GameManager.GetComponent<GameManager>().player2.board.R.GetComponent<RangedZone>().ranged[i].GetComponent<CardDisplay>().card.Power = GameManager.GetComponent<GameManager>().player2.board.R.GetComponent<RangedZone>().ranged[i].GetComponent<CardDisplay>().card.StartPower;
            }
        }
        else
        {
            for (int i = 0; i < GameManager.GetComponent<GameManager>().player1.board.S.GetComponent<SiegeZone>().siege.Count; i++)
            {
                GameManager.GetComponent<GameManager>().player1.board.S.GetComponent<SiegeZone>().siege[i].GetComponent<CardDisplay>().card.Power++;
            }
            for (int i = 0; i < GameManager.GetComponent<GameManager>().player2.board.S.GetComponent<SiegeZone>().siege.Count; i++)
            {
                GameManager.GetComponent<GameManager>().player2.board.S.GetComponent<SiegeZone>().siege[i].GetComponent<CardDisplay>().card.Power++;
            }
        }
    }


    #region 
    public static void Lure(GameObject gameObject)
    {
        GameObject GameManager = GameObject.Find("GameManager");

        Debug.Log("Lure Effect");

        if (GameManager.GetComponent<GameManager>().player1.isPlaying)
        {
            if (gameObject.GetComponent<CardDisplay>().card.TypeField == 'M')
            {
                for (int i = 0; i < GameManager.GetComponent<GameManager>().player2.board.M.GetComponent<MeleeZone>().melee.Count; i++)
                {
                    GameManager.GetComponent<GameManager>().player2.board.M.GetComponent<MeleeZone>().melee[i].GetComponent<CardDisplay>().card.Power = 0;
                }
            }
            else if (gameObject.GetComponent<CardDisplay>().card.TypeField == 'R')
            {
                for (int i = 0; i < GameManager.GetComponent<GameManager>().player2.board.R.GetComponent<RangedZone>().ranged.Count; i++)
                {
                    GameManager.GetComponent<GameManager>().player2.board.R.GetComponent<RangedZone>().ranged[i].GetComponent<CardDisplay>().card.Power = 0;
                }
            }
            else
            {
                for (int i = 0; i < GameManager.GetComponent<GameManager>().player2.board.S.GetComponent<SiegeZone>().siege.Count; i++)
                {
                    GameManager.GetComponent<GameManager>().player2.board.S.GetComponent<SiegeZone>().siege[i].GetComponent<CardDisplay>().card.Power = 0;
                }
            }
        }
        else
        {
            if (gameObject.GetComponent<CardDisplay>().card.TypeField == 'M')
            {
                for (int i = 0; i < GameManager.GetComponent<GameManager>().player1.board.M.GetComponent<MeleeZone>().melee.Count; i++)
                {
                    GameManager.GetComponent<GameManager>().player1.board.M.GetComponent<MeleeZone>().melee[i].GetComponent<CardDisplay>().card.Power = 0;
                }
            }
            else if (gameObject.GetComponent<CardDisplay>().card.TypeField == 'R')
            {
                for (int i = 0; i < GameManager.GetComponent<GameManager>().player1.board.R.GetComponent<RangedZone>().ranged.Count; i++)
                {
                    GameManager.GetComponent<GameManager>().player1.board.R.GetComponent<RangedZone>().ranged[i].GetComponent<CardDisplay>().card.Power = 0;
                }
            }
            else
            {
                for (int i = 0; i < GameManager.GetComponent<GameManager>().player1.board.S.GetComponent<SiegeZone>().siege.Count; i++)
                {
                    GameManager.GetComponent<GameManager>().player1.board.S.GetComponent<SiegeZone>().siege[i].GetComponent<CardDisplay>().card.Power = 0;
                }
            }
        }
    }
    #endregion

}