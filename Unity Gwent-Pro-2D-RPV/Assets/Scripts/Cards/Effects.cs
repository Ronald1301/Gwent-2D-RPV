using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Effects : MonoBehaviour
{
    //Active Effects
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
                Delete_Card_with_Max_Power_on_the_field(gameObject);
                break;
            case Card.TypeEffects.Delete_Card_with_Min_Power_on_the_field:
                Delete_Card_with_Min_Power_on_the_field_opponent(gameObject);
                break;
            case Card.TypeEffects.Draw_Card_from_Deck:
                Draw_Card_from_Deck();
                break;
            case Card.TypeEffects.Clear_file:
                Clear_the_row_with_fewer_cards_on_the_field(gameObject);
                break;
            case Card.TypeEffects.Average_Power_on_the_field:
                Average_Power_on_the_field(gameObject);
                break;
            case Card.TypeEffects.Decreases_one_Point:
                Decreases_one_Point(gameObject);
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
            for (int i = 0; i < GameManager.GetComponent<GameManager>().player1.hand.CardsInHand.Count; i++)
            {
                if (GameManager.GetComponent<GameManager>().player1.hand.CardsInHand[i].GetComponent<CardDisplay>().card.TypeSpecialCard == Card.SubTypeSpecialCard.Increase)
                {
                    GameManager.GetComponent<GameManager>().player1.hand.CardsInHand[i].GetComponent<MoveCard>().Move();
                    break;
                }
            }
            return;
        }
        //player 2
        else
        {
            for (int i = 0; i < GameManager.GetComponent<GameManager>().player2.hand.CardsInHand.Count; i++)
            {
                if (GameManager.GetComponent<GameManager>().player2.hand.CardsInHand[i].GetComponent<CardDisplay>().card.TypeSpecialCard == Card.SubTypeSpecialCard.Increase)
                {
                    GameManager.GetComponent<GameManager>().player2.hand.CardsInHand[i].GetComponent<MoveCard>().Move();
                    break;
                }
            }
            return;
        }
    }
    public static void Put_Climate()
    {
        GameObject GameManager = GameObject.Find("GameManager");

        Debug.Log("Put_Climate Effect");

        //player 1
        if (GameManager.GetComponent<GameManager>().player1.isPlaying)
        {
            for (int i = 0; i < GameManager.GetComponent<GameManager>().player1.hand.CardsInHand.Count; i++)
            {
                if (GameManager.GetComponent<GameManager>().player1.hand.CardsInHand[i].GetComponent<CardDisplay>().card.TypeSpecialCard == Card.SubTypeSpecialCard.Climate)
                {
                    GameManager.GetComponent<GameManager>().player1.hand.CardsInHand[i].GetComponent<MoveCard>().Move();
                    break;
                }
            }
            return;
        }
        //player 2
        else
        {
            for (int i = 0; i < GameManager.GetComponent<GameManager>().player2.hand.CardsInHand.Count; i++)
            {
                if (GameManager.GetComponent<GameManager>().player2.hand.CardsInHand[i].GetComponent<CardDisplay>().card.TypeSpecialCard == Card.SubTypeSpecialCard.Climate)
                {
                    GameManager.GetComponent<GameManager>().player2.hand.CardsInHand[i].GetComponent<MoveCard>().Move();
                    break;
                }
            }
            return;
        }
    }
    public static void Delete_Card_with_Max_Power_on_the_field(GameObject gameObject)
    {
        if (IsEmptyField(gameObject)) return;

        GameObject GameManager = GameObject.Find("GameManager");

        Debug.Log("Delete_Card_with_Max_Power_on_the_field Effect");
        GameObject cardTarget = new GameObject();

        //player 1
        // if (GameManager.GetComponent<GameManager>().player1.isPlaying)
        {
            if (GameManager.GetComponent<GameManager>().player2.board.M.GetComponent<MeleeZone>().melee.Count == 0)
            {
                if (GameManager.GetComponent<GameManager>().player2.board.R.GetComponent<RangedZone>().ranged.Count == 0)
                {
                    if (GameManager.GetComponent<GameManager>().player2.board.S.GetComponent<SiegeZone>().siege.Count == 0)
                    {
                        if (GameManager.GetComponent<GameManager>().player1.board.M.GetComponent<MeleeZone>().melee.Count == 0)
                        {
                            if (GameManager.GetComponent<GameManager>().player1.board.R.GetComponent<RangedZone>().ranged.Count == 0)
                            {
                                if (GameManager.GetComponent<GameManager>().player1.board.S.GetComponent<SiegeZone>().siege.Count == 0)
                                {
                                    return;
                                }
                                else
                                    cardTarget = GameManager.GetComponent<GameManager>().player1.board.S.GetComponent<SiegeZone>().siege[0];
                            }
                            else
                                cardTarget = GameManager.GetComponent<GameManager>().player1.board.R.GetComponent<RangedZone>().ranged[0];
                        }
                        else
                            cardTarget = GameManager.GetComponent<GameManager>().player1.board.M.GetComponent<MeleeZone>().melee[0];
                    }
                    else
                        cardTarget = GameManager.GetComponent<GameManager>().player2.board.S.GetComponent<SiegeZone>().siege[0];
                }
                else
                    cardTarget = GameManager.GetComponent<GameManager>().player2.board.R.GetComponent<RangedZone>().ranged[0];
            }

            else
            {
                cardTarget = GameManager.GetComponent<GameManager>().player2.board.M.GetComponent<MeleeZone>().melee[0];
            }
            /*
                        //Player 1
                        if (GameManager.GetComponent<GameManager>().player1.isPlaying)
                        {
                            cardTarget = GameManager.GetComponent<GameManager>().player2.board.M.GetComponent<MeleeZone>().melee[0];
            */
            for (int i = 0; i < GameManager.GetComponent<GameManager>().player2.board.M.GetComponent<MeleeZone>().melee.Count; i++)
            {
                if (cardTarget.GetComponent<CardDisplay>().card.Power < GameManager.GetComponent<GameManager>().player2.board.M.GetComponent<MeleeZone>().melee[i].GetComponent<CardDisplay>().card.Power && GameManager.GetComponent<GameManager>().player2.board.M.GetComponent<MeleeZone>().melee[i] != gameObject)
                {
                    cardTarget = GameManager.GetComponent<GameManager>().player2.board.M.GetComponent<MeleeZone>().melee[i];
                }
            }
            for (int i = 0; i < GameManager.GetComponent<GameManager>().player2.board.R.GetComponent<RangedZone>().ranged.Count; i++)
            {
                if (cardTarget.GetComponent<CardDisplay>().card.Power < GameManager.GetComponent<GameManager>().player2.board.R.GetComponent<RangedZone>().ranged[i].GetComponent<CardDisplay>().card.Power && GameManager.GetComponent<GameManager>().player2.board.R.GetComponent<RangedZone>().ranged[i] != gameObject)
                {
                    cardTarget = GameManager.GetComponent<GameManager>().player2.board.R.GetComponent<RangedZone>().ranged[i];
                }
            }
            for (int i = 0; i < GameManager.GetComponent<GameManager>().player2.board.S.GetComponent<SiegeZone>().siege.Count; i++)
            {
                if (cardTarget.GetComponent<CardDisplay>().card.Power < GameManager.GetComponent<GameManager>().player2.board.S.GetComponent<SiegeZone>().siege[i].GetComponent<CardDisplay>().card.Power && GameManager.GetComponent<GameManager>().player2.board.S.GetComponent<SiegeZone>().siege[i] != gameObject)
                {
                    cardTarget = GameManager.GetComponent<GameManager>().player2.board.S.GetComponent<SiegeZone>().siege[i];
                }
            }
            /*
                        }
                        //Player 2
                        else
                        {
                            cardTarget = GameManager.GetComponent<GameManager>().player1.board.M.GetComponent<MeleeZone>().melee[0];
            */
            for (int i = 0; i < GameManager.GetComponent<GameManager>().player1.board.M.GetComponent<MeleeZone>().melee.Count; i++)
            {
                if (cardTarget.GetComponent<CardDisplay>().card.Power < GameManager.GetComponent<GameManager>().player1.board.M.GetComponent<MeleeZone>().melee[i].GetComponent<CardDisplay>().card.Power && GameManager.GetComponent<GameManager>().player1.board.M.GetComponent<MeleeZone>().melee[i] != gameObject)
                {
                    cardTarget = GameManager.GetComponent<GameManager>().player1.board.M.GetComponent<MeleeZone>().melee[i];
                }
            }
            for (int i = 0; i < GameManager.GetComponent<GameManager>().player1.board.R.GetComponent<RangedZone>().ranged.Count; i++)
            {
                if (cardTarget.GetComponent<CardDisplay>().card.Power < GameManager.GetComponent<GameManager>().player1.board.R.GetComponent<RangedZone>().ranged[i].GetComponent<CardDisplay>().card.Power && GameManager.GetComponent<GameManager>().player1.board.R.GetComponent<RangedZone>().ranged[i] != gameObject)
                {
                    cardTarget = GameManager.GetComponent<GameManager>().player1.board.R.GetComponent<RangedZone>().ranged[i];
                }
            }
            for (int i = 0; i < GameManager.GetComponent<GameManager>().player1.board.S.GetComponent<SiegeZone>().siege.Count; i++)
            {
                if (cardTarget.GetComponent<CardDisplay>().card.Power < GameManager.GetComponent<GameManager>().player1.board.S.GetComponent<SiegeZone>().siege[i].GetComponent<CardDisplay>().card.Power && GameManager.GetComponent<GameManager>().player1.board.S.GetComponent<SiegeZone>().siege[i] != gameObject)
                {
                    cardTarget = GameManager.GetComponent<GameManager>().player1.board.S.GetComponent<SiegeZone>().siege[i];
                }
            }
                 //}

        if (cardTarget != gameObject)
        {
            return;
        }
        else
            cardTarget.GetComponent<MoveCard>().MoveToCemetery();
    }
}
public static void Delete_Card_with_Min_Power_on_the_field_opponent(GameObject gameObject)
{
    if (IsEmptyField(gameObject)) return;

    GameObject GameManager = GameObject.Find("GameManager");

    Debug.Log("Delete_Card_with_Min_Power_on_the_field Effect");
    _ = new
    GameObject();
    GameObject cardTarget;
    //player 1
    if (GameManager.GetComponent<GameManager>().player1.isPlaying)
    {
        if (GameManager.GetComponent<GameManager>().player2.board.M.GetComponent<MeleeZone>().melee.Count == 0)
        {
            if (GameManager.GetComponent<GameManager>().player2.board.R.GetComponent<RangedZone>().ranged.Count == 0)
            {
                if (GameManager.GetComponent<GameManager>().player2.board.S.GetComponent<SiegeZone>().siege.Count == 0)
                {
                    return;
                }
                else
                    cardTarget = GameManager.GetComponent<GameManager>().player2.board.S.GetComponent<SiegeZone>().siege[0];
            }
            else
                cardTarget = GameManager.GetComponent<GameManager>().player2.board.R.GetComponent<RangedZone>().ranged[0];
        }
        else
        {
            cardTarget = GameManager.GetComponent<GameManager>().player2.board.M.GetComponent<MeleeZone>().melee[0];
        }

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
        if (GameManager.GetComponent<GameManager>().player1.board.M.GetComponent<MeleeZone>().melee.Count == 0)
        {
            if (GameManager.GetComponent<GameManager>().player1.board.R.GetComponent<RangedZone>().ranged.Count == 0)
            {
                if (GameManager.GetComponent<GameManager>().player1.board.S.GetComponent<SiegeZone>().siege.Count == 0)
                {
                    return;
                }
                else
                    cardTarget = GameManager.GetComponent<GameManager>().player1.board.S.GetComponent<SiegeZone>().siege[0];
            }
            else
                cardTarget = GameManager.GetComponent<GameManager>().player1.board.R.GetComponent<RangedZone>().ranged[0];
        }
        else
        {
            cardTarget = GameManager.GetComponent<GameManager>().player1.board.M.GetComponent<MeleeZone>().melee[0];
        }

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
        GameManager.GetComponent<GameManager>().player1.hand.DrawCard(1);
        /*
        int indexCard = Random.Range(1, GameManager.GetComponent<GameManager>().player1.hand.CardsInDeck.Count - 1);
        GameObject drawCard = Instantiate(GameManager.GetComponent<GameManager>().player1.hand.CardsInDeck[indexCard], new Vector3(0, 0, 0), Quaternion.identity);
        drawCard.transform.localScale = new Vector3(0.4f, 0.6f, 0);
        drawCard.transform.SetParent(GameManager.GetComponent<GameManager>().player1.hand.transform, false);
        GameManager.GetComponent<GameManager>().player1.hand.CardsInHand.Add(drawCard);
        GameManager.GetComponent<GameManager>().player1.hand.CardsInDeck.RemoveAt(indexCard);
        */
    }
    else
    {
        GameManager.GetComponent<GameManager>().player2.hand.DrawCard(1);
        /*
        int indexCard = Random.Range(1, GameManager.GetComponent<GameManager>().player2.hand.CardsInDeck.Count - 1);
        GameObject drawCard = Instantiate(GameManager.GetComponent<GameManager>().player2.hand.CardsInDeck[indexCard], new Vector3(0, 0, 0), Quaternion.identity);
        drawCard.transform.localScale = new Vector3(0.4f, 0.6f, 0);
        drawCard.transform.SetParent(GameManager.GetComponent<GameManager>().player2.hand.transform, false);
        GameManager.GetComponent<GameManager>().player2.hand.CardsInHand.Add(drawCard);
        GameManager.GetComponent<GameManager>().player2.hand.CardsInDeck.RemoveAt(indexCard);
        */
    }
}
public static void Clear_the_row_with_fewer_cards_on_the_field(GameObject gameObject)
{
    if (IsEmptyField(gameObject)) return;

    GameObject GameManager = GameObject.Find("GameManager");
    int countM = int.MaxValue;
    int countR = int.MaxValue;
    int countS = int.MaxValue;

    Debug.Log("Clear_the_row_with_fewer_cards_on_the_field Effect");

    if (GameManager.GetComponent<GameManager>().player1.isPlaying)
    {
        if (GameManager.GetComponent<GameManager>().player2.board.M.GetComponent<MeleeZone>().melee.Count != 0) countM = GameManager.GetComponent<GameManager>().player2.board.M.GetComponent<MeleeZone>().melee.Count;
        if (GameManager.GetComponent<GameManager>().player2.board.R.GetComponent<RangedZone>().ranged.Count != 0) countR = GameManager.GetComponent<GameManager>().player2.board.R.GetComponent<RangedZone>().ranged.Count;
        if (GameManager.GetComponent<GameManager>().player2.board.S.GetComponent<SiegeZone>().siege.Count != 0) countS = GameManager.GetComponent<GameManager>().player2.board.S.GetComponent<SiegeZone>().siege.Count;

        if (countM < countR && countM < countS)
        {
            for (int i = 0; i < GameManager.GetComponent<GameManager>().player2.board.M.GetComponent<MeleeZone>().melee.Count; i++)
            {
                GameManager.GetComponent<GameManager>().player2.board.M.GetComponent<MeleeZone>().melee[i].GetComponent<MoveCard>().MoveToCemetery();
            }
            GameManager.GetComponent<GameManager>().player2.board.M.GetComponent<MeleeZone>().melee.Clear();
        }
        else if (countR < countM && countR < countS)
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
            GameManager.GetComponent<GameManager>().player2.board.S.GetComponent<SiegeZone>().siege.Clear();
        }
    }
    else
    {
        if (GameManager.GetComponent<GameManager>().player1.board.M.GetComponent<MeleeZone>().melee.Count != 0) countM = GameManager.GetComponent<GameManager>().player1.board.M.GetComponent<MeleeZone>().melee.Count;
        if (GameManager.GetComponent<GameManager>().player1.board.R.GetComponent<RangedZone>().ranged.Count != 0) countR = GameManager.GetComponent<GameManager>().player1.board.R.GetComponent<RangedZone>().ranged.Count;
        if (GameManager.GetComponent<GameManager>().player1.board.S.GetComponent<SiegeZone>().siege.Count != 0) countS = GameManager.GetComponent<GameManager>().player1.board.S.GetComponent<SiegeZone>().siege.Count;

        if (countM < countR && countM < countS)
        {
            for (int i = 0; i < GameManager.GetComponent<GameManager>().player1.board.M.GetComponent<MeleeZone>().melee.Count; i++)
            {
                GameManager.GetComponent<GameManager>().player1.board.M.GetComponent<MeleeZone>().melee[i].GetComponent<MoveCard>().MoveToCemetery();
            }
            GameManager.GetComponent<GameManager>().player1.board.M.GetComponent<MeleeZone>().melee.Clear();
        }
        else if (countR < countM && countR < countS)
        {
            for (int i = 0; i < GameManager.GetComponent<GameManager>().player1.board.R.GetComponent<RangedZone>().ranged.Count; i++)
            {
                GameManager.GetComponent<GameManager>().player1.board.R.GetComponent<RangedZone>().ranged[i].GetComponent<MoveCard>().MoveToCemetery();
            }
            GameManager.GetComponent<GameManager>().player1.board.R.GetComponent<RangedZone>().ranged.Clear();
        }
        else
        {
            for (int i = 0; i < GameManager.GetComponent<GameManager>().player1.board.R.GetComponent<RangedZone>().ranged.Count; i++)
            {
                GameManager.GetComponent<GameManager>().player1.board.R.GetComponent<RangedZone>().ranged[i].GetComponent<MoveCard>().MoveToCemetery();
            }
            GameManager.GetComponent<GameManager>().player1.board.R.GetComponent<RangedZone>().ranged.Clear();
        }
    }
}
public static void Average_Power_on_the_field(GameObject gameObject)
{
    if (Effects.IsEmptyField(gameObject)) return;

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
public static void Decreases_one_Point(GameObject gameObject)
{
    if (IsEmptyField(gameObject)) return;

    GameObject GameManager = GameObject.Find("GameManager");

    Debug.Log("Decreases_one_Point Effect");

    int indextype = Random.Range(1, 3);
    // if (!IsEmptyField(gameObject))
    {
        while (IsRowEMpty(indextype))
        {
            indextype = Random.Range(1, 3);
        }
        if (!IsRowEMpty(indextype))
        {
            if (GameManager.GetComponent<GameManager>().player1.isPlaying)
            {
                if (indextype == 1)
                {
                    int index = Random.Range(0, GameManager.GetComponent<GameManager>().player2.board.M.GetComponent<MeleeZone>().melee.Count - 1);
                    if (GameManager.GetComponent<GameManager>().player2.board.M.GetComponent<MeleeZone>().melee[index] != null)
                    {
                        GameManager.GetComponent<GameManager>().player2.board.M.GetComponent<MeleeZone>().melee[index].GetComponent<CardDisplay>().card.Power--;
                    }
                }
                if (indextype == 2)
                {
                    int index = Random.Range(0, GameManager.GetComponent<GameManager>().player2.board.R.GetComponent<RangedZone>().ranged.Count - 1);
                    if (GameManager.GetComponent<GameManager>().player2.board.R.GetComponent<RangedZone>().ranged[index] != null)
                    {
                        GameManager.GetComponent<GameManager>().player2.board.R.GetComponent<RangedZone>().ranged[index].GetComponent<CardDisplay>().card.Power--;
                    }
                }
                if (indextype == 3)
                {
                    int index = Random.Range(0, GameManager.GetComponent<GameManager>().player2.board.S.GetComponent<SiegeZone>().siege.Count - 1);
                    if (GameManager.GetComponent<GameManager>().player2.board.S.GetComponent<SiegeZone>().siege[index] != null)
                    {
                        GameManager.GetComponent<GameManager>().player2.board.S.GetComponent<SiegeZone>().siege[index].GetComponent<CardDisplay>().card.Power--;
                    }
                }
            }
            else
            {
                if (indextype == 1)
                {
                    int index = Random.Range(0, GameManager.GetComponent<GameManager>().player1.board.M.GetComponent<MeleeZone>().melee.Count - 1);
                    if (GameManager.GetComponent<GameManager>().player1.board.M.GetComponent<MeleeZone>().melee[index] != null)
                    {
                        GameManager.GetComponent<GameManager>().player1.board.M.GetComponent<MeleeZone>().melee[index].GetComponent<CardDisplay>().card.Power--;
                    }
                }
                if (indextype == 2)
                {
                    int index = Random.Range(0, GameManager.GetComponent<GameManager>().player1.board.R.GetComponent<RangedZone>().ranged.Count - 1);
                    if (GameManager.GetComponent<GameManager>().player1.board.R.GetComponent<RangedZone>().ranged[index] != null)
                    {
                        GameManager.GetComponent<GameManager>().player1.board.R.GetComponent<RangedZone>().ranged[index].GetComponent<CardDisplay>().card.Power--;
                    }

                }
                if (indextype == 3)
                {
                    int index = Random.Range(0, GameManager.GetComponent<GameManager>().player1.board.S.GetComponent<SiegeZone>().siege.Count - 1);
                    if (GameManager.GetComponent<GameManager>().player1.board.S.GetComponent<SiegeZone>().siege[index] != null)
                    {
                        GameManager.GetComponent<GameManager>().player1.board.S.GetComponent<SiegeZone>().siege[index].GetComponent<CardDisplay>().card.Power--;
                    }

                }
            }
        }


    }
    // else return;

}

//Special effects
public static void Clearance(GameObject gameObject)
{
    GameObject GameManager = GameObject.Find("GameManager");
    GameObject climateCard = GameObject.FindWithTag("ClimateCard");

    Debug.Log("Clearance Effect");

    if (GameManager.GetComponent<GameManager>().player1.isPlaying)
    {
        if (GameManager.GetComponent<GameManager>().player2.board.Climate.GetComponent<ClimateZone>().climate != climateCard)
        {
            DisableEffectClimate(GameManager.GetComponent<GameManager>().player2.board.Climate.GetComponent<ClimateZone>().climate);
            GameManager.GetComponent<GameManager>().player2.board.Climate.GetComponent<ClimateZone>().climate.GetComponent<MoveCard>().MoveToCemetery();
        }
        gameObject.transform.position = GameManager.GetComponent<GameManager>().player2.board.Climate.GetComponent<ClimateZone>().climate.transform.position;
    }
    else
    {
        if (GameManager.GetComponent<GameManager>().player1.board.Climate.GetComponent<ClimateZone>().climate != climateCard)
        {
            DisableEffectClimate(GameManager.GetComponent<GameManager>().player1.board.Climate.GetComponent<ClimateZone>().climate);
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
        int index = Random.Range(0, GameManager.GetComponent<GameManager>().player1.board.M.GetComponent<MeleeZone>().melee.Count - 1);
        GameManager.GetComponent<GameManager>().player2.board.M.GetComponent<MeleeZone>().melee[index].GetComponent<CardDisplay>().card.stayintheField = true;
    }
    else if (indextype == 2)
    {
        int index = Random.Range(0, GameManager.GetComponent<GameManager>().player1.board.R.GetComponent<RangedZone>().ranged.Count - 1);
        GameManager.GetComponent<GameManager>().player2.board.R.GetComponent<RangedZone>().ranged[index].GetComponent<CardDisplay>().card.stayintheField = true;
    }
    else
    {
        int index = Random.Range(0, GameManager.GetComponent<GameManager>().player1.board.S.GetComponent<SiegeZone>().siege.Count - 1);
        GameManager.GetComponent<GameManager>().player2.board.S.GetComponent<SiegeZone>().siege[index].GetComponent<CardDisplay>().card.stayintheField = true;
    }
}

//Without effect
public static void None()
{
    Debug.Log("No effect");
    return;
}

// Path: Assets/Scripts/Cards/Effects.cs
//Aux effects
public static void DisableEffectClimate(GameObject gameObject)
{
    GameObject GameManager = GameObject.FindGameObjectWithTag("GameManager");

    Debug.Log("DisableEffectClimate");

    if (GameManager.GetComponent<GameManager>().player2.board.Climate.GetComponent<ClimateZone>().climate == gameObject)
    {
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

    else if (GameManager.GetComponent<GameManager>().player1.board.Climate.GetComponent<ClimateZone>().climate == gameObject)
    {
        if (GameManager.GetComponent<GameManager>().player1.board.Climate.GetComponent<ClimateZone>().climate.GetComponent<CardDisplay>().card.TypeField == 'M')
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
        else if (GameManager.GetComponent<GameManager>().player1.board.Climate.GetComponent<ClimateZone>().climate.GetComponent<CardDisplay>().card.TypeField == 'R')
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
}

public static bool IsEmptyField(GameObject gameObject)
{
    GameObject GameManager = GameObject.FindGameObjectWithTag("GameManager");

    Debug.Log("IsEmptyField");
    int count = 6;
    if (GameManager.GetComponent<GameManager>().player1.board.M.GetComponent<MeleeZone>().melee.Count != 0)
    {
        if (GameManager.GetComponent<GameManager>().player1.board.M.GetComponent<MeleeZone>().melee[0] == gameObject)
        {
            count--;
        }
    }
    else
        count--;

    if (GameManager.GetComponent<GameManager>().player1.board.R.GetComponent<RangedZone>().ranged.Count != 0)
    {
        if (GameManager.GetComponent<GameManager>().player1.board.R.GetComponent<RangedZone>().ranged[0] == gameObject)
        {
            count--;
        }
    }
    else
        count--;

    if (GameManager.GetComponent<GameManager>().player1.board.S.GetComponent<SiegeZone>().siege.Count != 0)
    {
        if (GameManager.GetComponent<GameManager>().player1.board.S.GetComponent<SiegeZone>().siege[0] == gameObject)
        {
            count--;
        }
    }
    else
        count--;

    if (GameManager.GetComponent<GameManager>().player2.board.M.GetComponent<MeleeZone>().melee.Count != 0)
    {
        if (GameManager.GetComponent<GameManager>().player2.board.M.GetComponent<MeleeZone>().melee[0] == gameObject)
        {
            count--;
        }
    }
    else
        count--;

    if (GameManager.GetComponent<GameManager>().player2.board.R.GetComponent<RangedZone>().ranged.Count != 0)
    {
        if (GameManager.GetComponent<GameManager>().player2.board.R.GetComponent<RangedZone>().ranged[0] == gameObject)
        {
            count--;
        }
    }
    else
        count--;

    if (GameManager.GetComponent<GameManager>().player2.board.S.GetComponent<SiegeZone>().siege.Count != 0)
    {
        if (GameManager.GetComponent<GameManager>().player2.board.S.GetComponent<SiegeZone>().siege[0] == gameObject)
        {
            count--;
        }
    }
    else
        count--;


    if (count == 0)
    {
        return true;
    }
    else
    {
        return false;
    }
}

public static bool IsRowEMpty(int n)
{
    GameObject GameManager = GameObject.FindGameObjectWithTag("GameManager");

    //Debug.Log("IsRowEMpty");
    if (GameManager.GetComponent<GameManager>().player1.isPlaying)
    {
        if (n == 1)
        {
            if (GameManager.GetComponent<GameManager>().player2.board.M.GetComponent<MeleeZone>().melee.Count == 0)
            {
                return true;
            }
            else return false;
        }
        else if (n == 2)
        {
            if (GameManager.GetComponent<GameManager>().player2.board.R.GetComponent<RangedZone>().ranged.Count == 0)
            {
                return true;
            }
            else return false;
        }
        else
        {
            if (GameManager.GetComponent<GameManager>().player2.board.S.GetComponent<SiegeZone>().siege.Count == 0)
            {
                return true;
            }
            else return false;
        }
    }
    else if (GameManager.GetComponent<GameManager>().player2.isPlaying)
    {
        if (n == 1)
        {
            if (GameManager.GetComponent<GameManager>().player1.board.M.GetComponent<MeleeZone>().melee.Count == 0)
            {
                return true;
            }
            else return false;
        }
        else if (n == 2)
        {
            if (GameManager.GetComponent<GameManager>().player1.board.R.GetComponent<RangedZone>().ranged.Count == 0)
            {
                return true;
            }
            else return false;
        }
        else
        {
            if (GameManager.GetComponent<GameManager>().player1.board.S.GetComponent<SiegeZone>().siege.Count == 0)
            {
                return true;
            }
            else return false;
        }
    }
    return false;

}


    /*
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
    */
}
