using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Gwent;
using Unity.VisualScripting;
using UnityEngine;

public class Effects : MonoBehaviour
{
    private static readonly Dictionary<CardData.TypeEffects, Action<GameObject>> PredeterminateEffects = new()
    {
        [CardData.TypeEffects.Put_Increase] = (GameObject gameObject) => Put_Increase(),
        [CardData.TypeEffects.Put_Climate] = (GameObject gameObject) => Put_Climate(),
        [CardData.TypeEffects.Delete_Card_with_Max_Power_on_the_field] = (GameObject gameObject) => Delete_Card_with_Max_Power_on_the_field(gameObject),
        [CardData.TypeEffects.Delete_Card_with_Min_Power_on_the_field] = (GameObject gameObject) => Delete_Card_with_Min_Power_on_the_field_opponent(),
        [CardData.TypeEffects.Draw_Card_from_Deck] = (GameObject gameObject) => Draw_Card_from_Deck(),
        [CardData.TypeEffects.Clear_file] = (GameObject gameObject) => Clear_the_row_with_fewer_cards_on_the_field(gameObject),
        [CardData.TypeEffects.Average_Power_on_the_field] = (GameObject gameObject) => Average_Power_on_the_field(gameObject),
        [CardData.TypeEffects.Decreases_one_Point] = (GameObject gameObject) => Decreases_one_Point(),
        [CardData.TypeEffects.Multiply_the_attack_of_the_card_by_the_number_of_identical_cards_on_the_field] = (GameObject gameObject) => Multiply_the_attack_of_the_card_by_the_number_of_identical_cards_on_the_field(gameObject),

        //Special effects
        [CardData.TypeEffects.Climate] = (GameObject gameObject) => Climate(gameObject),
        [CardData.TypeEffects.Clearance] = (GameObject gameObject) => Clearance(gameObject),
        [CardData.TypeEffects.Increase] = (GameObject gameObject) => Increase(gameObject),
        [CardData.TypeEffects.Lure] = (GameObject gameObject) => None(),

        //Boss effects
        [CardData.TypeEffects.StayintheField] = (GameObject gameObject) => StayintheField(),

        [CardData.TypeEffects.effectCardCompiler] = (GameObject gameObject) => EffectCompiler(gameObject),

        [CardData.TypeEffects.None] = (GameObject gameObject) => None(),
    };

    private static void EffectCompiler(GameObject gameObject)
    {
        var nameEffects = gameObject.GetComponent<CardDisplay>().cardData.ListEffect;

        foreach (var nameeffect in nameEffects)
        {
            if (EngineCompiler.effects.TryGetValue(nameeffect, out (EffectComplete, SelectorExpression) effect))
            {
                var resultSelector = effect.Item2.Evaluate();
                if (resultSelector is (string, bool, object))
                {
                    var tuple = ((string, bool, object))resultSelector;

                    var targets = Bridge.GetSource(Bridge.GetTriggerPlayer(), tuple.Item1);

                    if (tuple.Item3 is Predicate<GameObject> predicate)
                    {
                        if (targets is not null)
                        {
                            targets = targets.FindAll(predicate);
                            if (tuple.Item2)
                            {
                                List<GameObject> list = new();
                                list.Add(targets[0]);
                                targets = list;
                            }
                        }
                    }
                    foreach (var item in effect.Item1.ContextCard!.Items.Keys)
                    {
                        if (item.token.Value == effect.Item1.Body!.Params[0].token.Value)
                        {
                            effect.Item1.ContextCard.Items[item] = targets!;
                        }
                    }
                    var action = (Action<GameObject[]>)effect.Item1.Body.Evaluate();
                    action.Invoke(targets.ToArray());
                }


            }
        }

    }

    //Active Effects
    public static void ActivateEffect(GameObject gameObject)
    {
        CardData.TypeEffects effect = gameObject.GetComponent<CardDisplay>().cardData.Effect;

        if (PredeterminateEffects.TryGetValue(effect, out Action<GameObject> func))
            func(gameObject);
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
                if (GameManager.GetComponent<GameManager>().player1.hand.CardsInHand[i].GetComponent<CardDisplay>().cardData.TypeSpecialCard == CardData.SubTypeSpecialCard.Increase)
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
                if (GameManager.GetComponent<GameManager>().player2.hand.CardsInHand[i].GetComponent<CardDisplay>().cardData.TypeSpecialCard == CardData.SubTypeSpecialCard.Increase)
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
                if (GameManager.GetComponent<GameManager>().player1.hand.CardsInHand[i].GetComponent<CardDisplay>().cardData.TypeSpecialCard == CardData.SubTypeSpecialCard.Climate)
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
                if (GameManager.GetComponent<GameManager>().player2.hand.CardsInHand[i].GetComponent<CardDisplay>().cardData.TypeSpecialCard == CardData.SubTypeSpecialCard.Climate)
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
        if (GameManager.GetComponent<GameManager>().player1.isPlaying)
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
                cardTarget = GameManager.GetComponent<GameManager>().player2.board.M.GetComponent<MeleeZone>().melee[0];
        }
        else
        {
            if (GameManager.GetComponent<GameManager>().player1.board.M.GetComponent<MeleeZone>().melee.Count == 0)
            {
                if (GameManager.GetComponent<GameManager>().player1.board.R.GetComponent<RangedZone>().ranged.Count == 0)
                {
                    if (GameManager.GetComponent<GameManager>().player1.board.S.GetComponent<SiegeZone>().siege.Count == 0)
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
                            cardTarget = GameManager.GetComponent<GameManager>().player2.board.M.GetComponent<MeleeZone>().melee[0];
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

        for (int i = 0; i < GameManager.GetComponent<GameManager>().player2.board.M.GetComponent<MeleeZone>().melee.Count; i++)
        {
            if (cardTarget.GetComponent<CardDisplay>().cardData.Power < GameManager.GetComponent<GameManager>().player2.board.M.GetComponent<MeleeZone>().melee[i].GetComponent<CardDisplay>().cardData.Power && GameManager.GetComponent<GameManager>().player2.board.M.GetComponent<MeleeZone>().melee[i] != gameObject)
            {
                cardTarget = GameManager.GetComponent<GameManager>().player2.board.M.GetComponent<MeleeZone>().melee[i];
            }
        }
        for (int i = 0; i < GameManager.GetComponent<GameManager>().player2.board.R.GetComponent<RangedZone>().ranged.Count; i++)
        {
            if (cardTarget.GetComponent<CardDisplay>().cardData.Power < GameManager.GetComponent<GameManager>().player2.board.R.GetComponent<RangedZone>().ranged[i].GetComponent<CardDisplay>().cardData.Power && GameManager.GetComponent<GameManager>().player2.board.R.GetComponent<RangedZone>().ranged[i] != gameObject)
            {
                cardTarget = GameManager.GetComponent<GameManager>().player2.board.R.GetComponent<RangedZone>().ranged[i];
            }
        }
        for (int i = 0; i < GameManager.GetComponent<GameManager>().player2.board.S.GetComponent<SiegeZone>().siege.Count; i++)
        {
            if (cardTarget.GetComponent<CardDisplay>().cardData.Power < GameManager.GetComponent<GameManager>().player2.board.S.GetComponent<SiegeZone>().siege[i].GetComponent<CardDisplay>().cardData.Power && GameManager.GetComponent<GameManager>().player2.board.S.GetComponent<SiegeZone>().siege[i] != gameObject)
            {
                cardTarget = GameManager.GetComponent<GameManager>().player2.board.S.GetComponent<SiegeZone>().siege[i];
            }
        }

        for (int i = 0; i < GameManager.GetComponent<GameManager>().player1.board.M.GetComponent<MeleeZone>().melee.Count; i++)
        {
            if (cardTarget.GetComponent<CardDisplay>().cardData.Power < GameManager.GetComponent<GameManager>().player1.board.M.GetComponent<MeleeZone>().melee[i].GetComponent<CardDisplay>().cardData.Power && GameManager.GetComponent<GameManager>().player1.board.M.GetComponent<MeleeZone>().melee[i] != gameObject)
            {
                cardTarget = GameManager.GetComponent<GameManager>().player1.board.M.GetComponent<MeleeZone>().melee[i];
            }
        }
        for (int i = 0; i < GameManager.GetComponent<GameManager>().player1.board.R.GetComponent<RangedZone>().ranged.Count; i++)
        {
            if (cardTarget.GetComponent<CardDisplay>().cardData.Power < GameManager.GetComponent<GameManager>().player1.board.R.GetComponent<RangedZone>().ranged[i].GetComponent<CardDisplay>().cardData.Power && GameManager.GetComponent<GameManager>().player1.board.R.GetComponent<RangedZone>().ranged[i] != gameObject)
            {
                cardTarget = GameManager.GetComponent<GameManager>().player1.board.R.GetComponent<RangedZone>().ranged[i];
            }
        }
        for (int i = 0; i < GameManager.GetComponent<GameManager>().player1.board.S.GetComponent<SiegeZone>().siege.Count; i++)
        {
            if (cardTarget.GetComponent<CardDisplay>().cardData.Power < GameManager.GetComponent<GameManager>().player1.board.S.GetComponent<SiegeZone>().siege[i].GetComponent<CardDisplay>().cardData.Power && GameManager.GetComponent<GameManager>().player1.board.S.GetComponent<SiegeZone>().siege[i] != gameObject)
            {
                cardTarget = GameManager.GetComponent<GameManager>().player1.board.S.GetComponent<SiegeZone>().siege[i];
            }
        }


        if (cardTarget == gameObject)
        {
            return;
        }
        else
            cardTarget.GetComponent<MoveCard>().MoveToCemetery();

    }
    public static void Delete_Card_with_Min_Power_on_the_field_opponent()
    {
        GameObject GameManager = GameObject.Find("GameManager");

        Debug.Log("Delete_Card_with_Min_Power_on_the_field Effect");
        GameObject cardTarget;
        //player 1
        if (GameManager.GetComponent<GameManager>().player1.isPlaying)
        {
            if (IsRowEmptyEnemy(1))
            {
                if (IsRowEmptyEnemy(2))
                {
                    if (IsRowEmptyEnemy(3))
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
                cardTarget = GameManager.GetComponent<GameManager>().player2.board.M.GetComponent<MeleeZone>().melee[0];


            for (int i = 0; i < GameManager.GetComponent<GameManager>().player2.board.M.GetComponent<MeleeZone>().melee.Count; i++)
            {
                if (cardTarget.GetComponent<CardDisplay>().cardData.Power > GameManager.GetComponent<GameManager>().player2.board.M.GetComponent<MeleeZone>().melee[i].GetComponent<CardDisplay>().cardData.Power)
                {
                    cardTarget = GameManager.GetComponent<GameManager>().player2.board.M.GetComponent<MeleeZone>().melee[i];
                }
            }
            for (int i = 0; i < GameManager.GetComponent<GameManager>().player2.board.R.GetComponent<RangedZone>().ranged.Count; i++)
            {
                if (cardTarget.GetComponent<CardDisplay>().cardData.Power > GameManager.GetComponent<GameManager>().player2.board.R.GetComponent<RangedZone>().ranged[i].GetComponent<CardDisplay>().cardData.Power)
                {
                    cardTarget = GameManager.GetComponent<GameManager>().player2.board.R.GetComponent<RangedZone>().ranged[i];
                }
            }
            for (int i = 0; i < GameManager.GetComponent<GameManager>().player2.board.S.GetComponent<SiegeZone>().siege.Count; i++)
            {
                if (cardTarget.GetComponent<CardDisplay>().cardData.Power > GameManager.GetComponent<GameManager>().player2.board.S.GetComponent<SiegeZone>().siege[i].GetComponent<CardDisplay>().cardData.Power)
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
                if (cardTarget.GetComponent<CardDisplay>().cardData.Power > GameManager.GetComponent<GameManager>().player1.board.M.GetComponent<MeleeZone>().melee[i].GetComponent<CardDisplay>().cardData.Power)
                {
                    cardTarget = GameManager.GetComponent<GameManager>().player1.board.M.GetComponent<MeleeZone>().melee[i];
                }
            }
            for (int i = 0; i < GameManager.GetComponent<GameManager>().player1.board.R.GetComponent<RangedZone>().ranged.Count; i++)
            {
                if (cardTarget.GetComponent<CardDisplay>().cardData.Power > GameManager.GetComponent<GameManager>().player1.board.R.GetComponent<RangedZone>().ranged[i].GetComponent<CardDisplay>().cardData.Power)
                {
                    cardTarget = GameManager.GetComponent<GameManager>().player1.board.R.GetComponent<RangedZone>().ranged[i];
                }
            }
            for (int i = 0; i < GameManager.GetComponent<GameManager>().player1.board.S.GetComponent<SiegeZone>().siege.Count; i++)
            {
                if (cardTarget.GetComponent<CardDisplay>().cardData.Power > GameManager.GetComponent<GameManager>().player1.board.S.GetComponent<SiegeZone>().siege[i].GetComponent<CardDisplay>().cardData.Power)
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
            int indexCard = UnityEngine.Random.Range(1, GameManager.GetComponent<GameManager>().player1.hand.CardsInDeck.Count - 1);
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
            int indexCard = UnityEngine.Random.Range(1, GameManager.GetComponent<GameManager>().player2.hand.CardsInDeck.Count - 1);
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
        int k = 0;

        Debug.Log("Clear_the_row_with_fewer_cards_on_the_field Effect");

        if (GameManager.GetComponent<GameManager>().player1.isPlaying)
        {
            if (!IsRowEmptyEnemy(1)) countM = GameManager.GetComponent<GameManager>().player2.board.M.GetComponent<MeleeZone>().melee.Count;
            if (!IsRowEmptyEnemy(2)) countR = GameManager.GetComponent<GameManager>().player2.board.R.GetComponent<RangedZone>().ranged.Count;
            if (!IsRowEmptyEnemy(3)) countS = GameManager.GetComponent<GameManager>().player2.board.S.GetComponent<SiegeZone>().siege.Count;

            if (countM <= countR && countM <= countS)
            {
                while (GameManager.GetComponent<GameManager>().player2.board.M.GetComponent<MeleeZone>().melee.Count != 0)
                {
                    GameManager.GetComponent<GameManager>().player2.board.M.GetComponent<MeleeZone>().melee[k].GetComponent<MoveCard>().MoveToCemetery(GameManager.GetComponent<GameManager>().player2.board.M.GetComponent<MeleeZone>().melee[k], GameManager.GetComponent<GameManager>().player2.board);
                }
                /*
                for (int i = 0; i < GameManager.GetComponent<GameManager>().player2.board.M.GetComponent<MeleeZone>().melee.Count; i++)
                {
                    GameManager.GetComponent<GameManager>().player2.board.M.GetComponent<MeleeZone>().melee[i].GetComponent<MoveCard>().MoveToCemetery(GameManager.GetComponent<GameManager>().player2.board.M.GetComponent<MeleeZone>().melee[i], GameManager.GetComponent<GameManager>().player2.board);
                    //GameManager.GetComponent<GameManager>().player2.board.M.GetComponent<MeleeZone>().melee[i].GetComponent<MoveCard>().MoveToCemetery();
                }
                */
                GameManager.GetComponent<GameManager>().player2.board.M.GetComponent<MeleeZone>().melee = new();
            }
            else if (countR <= countM && countR <= countS)
            {
                while (GameManager.GetComponent<GameManager>().player2.board.R.GetComponent<RangedZone>().ranged.Count != 0)
                {
                    GameManager.GetComponent<GameManager>().player2.board.R.GetComponent<RangedZone>().ranged[k].GetComponent<MoveCard>().MoveToCemetery(GameManager.GetComponent<GameManager>().player2.board.R.GetComponent<RangedZone>().ranged[k], GameManager.GetComponent<GameManager>().player2.board);
                }
                /*
                for (int i = 0; i < GameManager.GetComponent<GameManager>().player2.board.R.GetComponent<RangedZone>().ranged.Count; i++)
                {
                    GameManager.GetComponent<GameManager>().player2.board.R.GetComponent<RangedZone>().ranged[i].GetComponent<MoveCard>().MoveToCemetery(GameManager.GetComponent<GameManager>().player2.board.R.GetComponent<RangedZone>().ranged[i], GameManager.GetComponent<GameManager>().player2.board);
                    //GameManager.GetComponent<GameManager>().player2.board.R.GetComponent<RangedZone>().ranged[i].GetComponent<MoveCard>().MoveToCemetery();
                }
                */
                GameManager.GetComponent<GameManager>().player2.board.R.GetComponent<RangedZone>().ranged = new();
            }
            else if (countS <= countM && countS <= countR)
            {

                while (GameManager.GetComponent<GameManager>().player2.board.S.GetComponent<SiegeZone>().siege.Count != 0)
                {
                    GameManager.GetComponent<GameManager>().player2.board.S.GetComponent<SiegeZone>().siege[k].GetComponent<MoveCard>().MoveToCemetery(GameManager.GetComponent<GameManager>().player2.board.S.GetComponent<SiegeZone>().siege[k], GameManager.GetComponent<GameManager>().player2.board);
                }
                /*
                for (int i = 0; i < GameManager.GetComponent<GameManager>().player2.board.S.GetComponent<SiegeZone>().siege.Count; i++)
                {
                    GameManager.GetComponent<GameManager>().player2.board.S.GetComponent<SiegeZone>().siege[i].GetComponent<MoveCard>().MoveToCemetery(GameManager.GetComponent<GameManager>().player2.board.S.GetComponent<SiegeZone>().siege[i], GameManager.GetComponent<GameManager>().player2.board);
                    //GameManager.GetComponent<GameManager>().player2.board.S.GetComponent<SiegeZone>().siege[i].GetComponent<MoveCard>().MoveToCemetery();
                }
                */
                GameManager.GetComponent<GameManager>().player2.board.S.GetComponent<SiegeZone>().siege = new();
            }
            else Debug.Log("Error effect Clear File");
        }
        else
        {
            if (GameManager.GetComponent<GameManager>().player1.board.M.GetComponent<MeleeZone>().melee.Count != 0) countM = GameManager.GetComponent<GameManager>().player1.board.M.GetComponent<MeleeZone>().melee.Count;
            if (GameManager.GetComponent<GameManager>().player1.board.R.GetComponent<RangedZone>().ranged.Count != 0) countR = GameManager.GetComponent<GameManager>().player1.board.R.GetComponent<RangedZone>().ranged.Count;
            if (GameManager.GetComponent<GameManager>().player1.board.S.GetComponent<SiegeZone>().siege.Count != 0) countS = GameManager.GetComponent<GameManager>().player1.board.S.GetComponent<SiegeZone>().siege.Count;

            if (countM < countR && countM < countS)
            {
                while (GameManager.GetComponent<GameManager>().player1.board.M.GetComponent<MeleeZone>().melee.Count != 0)
                {
                    GameManager.GetComponent<GameManager>().player1.board.M.GetComponent<MeleeZone>().melee[k].GetComponent<MoveCard>().MoveToCemetery(GameManager.GetComponent<GameManager>().player1.board.M.GetComponent<MeleeZone>().melee[k], GameManager.GetComponent<GameManager>().player1.board);
                }
                /*
                for (int i = 0; i < GameManager.GetComponent<GameManager>().player1.board.M.GetComponent<MeleeZone>().melee.Count; i++)
                {
                    GameManager.GetComponent<GameManager>().player1.board.M.GetComponent<MeleeZone>().melee[i].GetComponent<MoveCard>().MoveToCemetery(GameManager.GetComponent<GameManager>().player1.board.M.GetComponent<MeleeZone>().melee[i], GameManager.GetComponent<GameManager>().player1.board);
                    //GameManager.GetComponent<GameManager>().player1.board.M.GetComponent<MeleeZone>().melee[i].GetComponent<MoveCard>().MoveToCemetery();
                }
                */
                GameManager.GetComponent<GameManager>().player1.board.M.GetComponent<MeleeZone>().melee = new();
            }
            else if (countR < countM && countR < countS)
            {
                while (GameManager.GetComponent<GameManager>().player1.board.R.GetComponent<RangedZone>().ranged.Count != 0)
                {
                    GameManager.GetComponent<GameManager>().player1.board.R.GetComponent<RangedZone>().ranged[k].GetComponent<MoveCard>().MoveToCemetery(GameManager.GetComponent<GameManager>().player1.board.R.GetComponent<RangedZone>().ranged[k], GameManager.GetComponent<GameManager>().player1.board);
                }
                /*
                for (int i = 0; i < GameManager.GetComponent<GameManager>().player1.board.R.GetComponent<RangedZone>().ranged.Count; i++)
                {
                    GameManager.GetComponent<GameManager>().player1.board.R.GetComponent<RangedZone>().ranged[i].GetComponent<MoveCard>().MoveToCemetery(GameManager.GetComponent<GameManager>().player1.board.R.GetComponent<RangedZone>().ranged[i], GameManager.GetComponent<GameManager>().player1.board);
                    //GameManager.GetComponent<GameManager>().player1.board.R.GetComponent<RangedZone>().ranged[i].GetComponent<MoveCard>().MoveToCemetery();
                }
                */
                GameManager.GetComponent<GameManager>().player1.board.R.GetComponent<RangedZone>().ranged = new();
            }
            else
            {
                while (GameManager.GetComponent<GameManager>().player1.board.R.GetComponent<RangedZone>().ranged.Count != 0)
                {
                    GameManager.GetComponent<GameManager>().player1.board.R.GetComponent<RangedZone>().ranged[k].GetComponent<MoveCard>().MoveToCemetery(GameManager.GetComponent<GameManager>().player1.board.R.GetComponent<RangedZone>().ranged[k], GameManager.GetComponent<GameManager>().player1.board);
                }
                /*
                for (int i = 0; i < GameManager.GetComponent<GameManager>().player1.board.R.GetComponent<RangedZone>().ranged.Count; i++)
                {
                    GameManager.GetComponent<GameManager>().player1.board.R.GetComponent<RangedZone>().ranged[i].GetComponent<MoveCard>().MoveToCemetery(GameManager.GetComponent<GameManager>().player1.board.R.GetComponent<RangedZone>().ranged[i], GameManager.GetComponent<GameManager>().player1.board);
                    //GameManager.GetComponent<GameManager>().player1.board.R.GetComponent<RangedZone>().ranged[i].GetComponent<MoveCard>().MoveToCemetery();
                }
                */
                GameManager.GetComponent<GameManager>().player1.board.R.GetComponent<RangedZone>().ranged = new();
            }
        }
    }
    public static void Average_Power_on_the_field(GameObject gameObject)
    {
        if (IsEmptyField(gameObject)) return;

        GameObject GameManager = GameObject.Find("GameManager");

        Debug.Log("Average_Power_on_the_field Effect");

        int average = 0;

        for (int i = 0; i < GameManager.GetComponent<GameManager>().player2.board.M.GetComponent<MeleeZone>().melee.Count; i++)
        {
            average += GameManager.GetComponent<GameManager>().player2.board.M.GetComponent<MeleeZone>().melee[i].GetComponent<CardDisplay>().cardData.Power;
        }
        for (int i = 0; i < GameManager.GetComponent<GameManager>().player2.board.R.GetComponent<RangedZone>().ranged.Count; i++)
        {
            average += GameManager.GetComponent<GameManager>().player2.board.R.GetComponent<RangedZone>().ranged[i].GetComponent<CardDisplay>().cardData.Power;
        }
        for (int i = 0; i < GameManager.GetComponent<GameManager>().player2.board.S.GetComponent<SiegeZone>().siege.Count; i++)
        {
            average += GameManager.GetComponent<GameManager>().player2.board.S.GetComponent<SiegeZone>().siege[i].GetComponent<CardDisplay>().cardData.Power;
        }

        for (int i = 0; i < GameManager.GetComponent<GameManager>().player1.board.M.GetComponent<MeleeZone>().melee.Count; i++)
        {
            average += GameManager.GetComponent<GameManager>().player1.board.M.GetComponent<MeleeZone>().melee[i].GetComponent<CardDisplay>().cardData.Power;
        }
        for (int i = 0; i < GameManager.GetComponent<GameManager>().player1.board.R.GetComponent<RangedZone>().ranged.Count; i++)
        {
            average += GameManager.GetComponent<GameManager>().player1.board.R.GetComponent<RangedZone>().ranged[i].GetComponent<CardDisplay>().cardData.Power;
        }
        for (int i = 0; i < GameManager.GetComponent<GameManager>().player1.board.S.GetComponent<SiegeZone>().siege.Count; i++)
        {
            average += GameManager.GetComponent<GameManager>().player1.board.S.GetComponent<SiegeZone>().siege[i].GetComponent<CardDisplay>().cardData.Power;
        }

        average /= GameManager.GetComponent<GameManager>().player1.board.M.GetComponent<MeleeZone>().melee.Count
             + GameManager.GetComponent<GameManager>().player1.board.R.GetComponent<RangedZone>().ranged.Count
             + GameManager.GetComponent<GameManager>().player1.board.S.GetComponent<SiegeZone>().siege.Count
             + GameManager.GetComponent<GameManager>().player2.board.M.GetComponent<MeleeZone>().melee.Count
             + GameManager.GetComponent<GameManager>().player2.board.R.GetComponent<RangedZone>().ranged.Count
             + GameManager.GetComponent<GameManager>().player2.board.S.GetComponent<SiegeZone>().siege.Count;

        gameObject.GetComponent<CardDisplay>().cardData.Power = average;

        for (int i = 0; i < GameManager.GetComponent<GameManager>().player2.board.M.GetComponent<MeleeZone>().melee.Count; i++)
        {
            GameManager.GetComponent<GameManager>().player2.board.M.GetComponent<MeleeZone>().melee[i].GetComponent<CardDisplay>().cardData.Power = average;
            GameManager.GetComponent<GameManager>().player2.board.M.GetComponent<MeleeZone>().melee[i].GetComponent<CardDisplay>().cardData.affectedByIncrease = false;
            GameManager.GetComponent<GameManager>().player2.board.M.GetComponent<MeleeZone>().melee[i].GetComponent<CardDisplay>().cardData.affectedByClimate = false;
        }
        for (int i = 0; i < GameManager.GetComponent<GameManager>().player2.board.R.GetComponent<RangedZone>().ranged.Count; i++)
        {
            GameManager.GetComponent<GameManager>().player2.board.R.GetComponent<RangedZone>().ranged[i].GetComponent<CardDisplay>().cardData.Power = average;
            GameManager.GetComponent<GameManager>().player2.board.R.GetComponent<RangedZone>().ranged[i].GetComponent<CardDisplay>().cardData.affectedByIncrease = false;
            GameManager.GetComponent<GameManager>().player2.board.R.GetComponent<RangedZone>().ranged[i].GetComponent<CardDisplay>().cardData.affectedByClimate = false;
        }
        for (int i = 0; i < GameManager.GetComponent<GameManager>().player2.board.S.GetComponent<SiegeZone>().siege.Count; i++)
        {
            GameManager.GetComponent<GameManager>().player2.board.S.GetComponent<SiegeZone>().siege[i].GetComponent<CardDisplay>().cardData.Power = average;
            GameManager.GetComponent<GameManager>().player2.board.S.GetComponent<SiegeZone>().siege[i].GetComponent<CardDisplay>().cardData.affectedByIncrease = false;
            GameManager.GetComponent<GameManager>().player2.board.S.GetComponent<SiegeZone>().siege[i].GetComponent<CardDisplay>().cardData.affectedByClimate = false;
        }

        for (int i = 0; i < GameManager.GetComponent<GameManager>().player1.board.M.GetComponent<MeleeZone>().melee.Count; i++)
        {
            GameManager.GetComponent<GameManager>().player1.board.M.GetComponent<MeleeZone>().melee[i].GetComponent<CardDisplay>().cardData.Power = average;
            GameManager.GetComponent<GameManager>().player1.board.M.GetComponent<MeleeZone>().melee[i].GetComponent<CardDisplay>().cardData.affectedByIncrease = false;
            GameManager.GetComponent<GameManager>().player1.board.M.GetComponent<MeleeZone>().melee[i].GetComponent<CardDisplay>().cardData.affectedByClimate = false;
        }
        for (int i = 0; i < GameManager.GetComponent<GameManager>().player1.board.R.GetComponent<RangedZone>().ranged.Count; i++)
        {
            GameManager.GetComponent<GameManager>().player1.board.R.GetComponent<RangedZone>().ranged[i].GetComponent<CardDisplay>().cardData.Power = average;
            GameManager.GetComponent<GameManager>().player1.board.R.GetComponent<RangedZone>().ranged[i].GetComponent<CardDisplay>().cardData.affectedByIncrease = false;
            GameManager.GetComponent<GameManager>().player1.board.R.GetComponent<RangedZone>().ranged[i].GetComponent<CardDisplay>().cardData.affectedByClimate = false;
        }
        for (int i = 0; i < GameManager.GetComponent<GameManager>().player1.board.S.GetComponent<SiegeZone>().siege.Count; i++)
        {
            GameManager.GetComponent<GameManager>().player1.board.S.GetComponent<SiegeZone>().siege[i].GetComponent<CardDisplay>().cardData.Power = average;
            GameManager.GetComponent<GameManager>().player1.board.S.GetComponent<SiegeZone>().siege[i].GetComponent<CardDisplay>().cardData.affectedByIncrease = false;
            GameManager.GetComponent<GameManager>().player1.board.S.GetComponent<SiegeZone>().siege[i].GetComponent<CardDisplay>().cardData.affectedByClimate = false;
        }
    }
    public static void Decreases_one_Point()
    {
        if (IsEmptyFieldEnemy()) return;

        GameObject GameManager = GameObject.Find("GameManager");

        Debug.Log("Decreases_one_Point Effect");

        int indextype = UnityEngine.Random.Range(1, 4);

        while (IsRowEmptyEnemy(indextype))
        {
            indextype = UnityEngine.Random.Range(1, 4);
        }
        if (!IsRowEmptyEnemy(indextype))
        {
            if (GameManager.GetComponent<GameManager>().player1.isPlaying)
            {
                if (indextype == 1)
                {
                    int index = UnityEngine.Random.Range(0, GameManager.GetComponent<GameManager>().player2.board.M.GetComponent<MeleeZone>().melee.Count);
                    if (GameManager.GetComponent<GameManager>().player2.board.M.GetComponent<MeleeZone>().melee[index] != null)
                    {
                        GameManager.GetComponent<GameManager>().player2.board.M.GetComponent<MeleeZone>().melee[index].GetComponent<CardDisplay>().cardData.Power--;
                    }
                }
                if (indextype == 2)
                {
                    int index = UnityEngine.Random.Range(0, GameManager.GetComponent<GameManager>().player2.board.R.GetComponent<RangedZone>().ranged.Count);
                    if (GameManager.GetComponent<GameManager>().player2.board.R.GetComponent<RangedZone>().ranged[index] != null)
                    {
                        GameManager.GetComponent<GameManager>().player2.board.R.GetComponent<RangedZone>().ranged[index].GetComponent<CardDisplay>().cardData.Power--;
                    }
                }
                if (indextype == 3)
                {
                    int index = UnityEngine.Random.Range(0, GameManager.GetComponent<GameManager>().player2.board.S.GetComponent<SiegeZone>().siege.Count);
                    if (GameManager.GetComponent<GameManager>().player2.board.S.GetComponent<SiegeZone>().siege[index] != null)
                    {
                        GameManager.GetComponent<GameManager>().player2.board.S.GetComponent<SiegeZone>().siege[index].GetComponent<CardDisplay>().cardData.Power--;
                    }
                }
            }
            else
            {
                if (indextype == 1)
                {
                    int index = UnityEngine.Random.Range(0, GameManager.GetComponent<GameManager>().player1.board.M.GetComponent<MeleeZone>().melee.Count);
                    if (GameManager.GetComponent<GameManager>().player1.board.M.GetComponent<MeleeZone>().melee[index] != null)
                    {
                        GameManager.GetComponent<GameManager>().player1.board.M.GetComponent<MeleeZone>().melee[index].GetComponent<CardDisplay>().cardData.Power--;
                    }
                }
                if (indextype == 2)
                {
                    int index = UnityEngine.Random.Range(0, GameManager.GetComponent<GameManager>().player1.board.R.GetComponent<RangedZone>().ranged.Count);
                    if (GameManager.GetComponent<GameManager>().player1.board.R.GetComponent<RangedZone>().ranged[index] != null)
                    {
                        GameManager.GetComponent<GameManager>().player1.board.R.GetComponent<RangedZone>().ranged[index].GetComponent<CardDisplay>().cardData.Power--;
                    }

                }
                if (indextype == 3)
                {
                    int index = UnityEngine.Random.Range(0, GameManager.GetComponent<GameManager>().player1.board.S.GetComponent<SiegeZone>().siege.Count);
                    if (GameManager.GetComponent<GameManager>().player1.board.S.GetComponent<SiegeZone>().siege[index] != null)
                    {
                        GameManager.GetComponent<GameManager>().player1.board.S.GetComponent<SiegeZone>().siege[index].GetComponent<CardDisplay>().cardData.Power--;
                    }

                }
            }
        }
    }
    public static void Multiply_the_attack_of_the_card_by_the_number_of_identical_cards_on_the_field(GameObject gameObject)
    {
        GameObject GameManager = GameObject.Find("GameManager");

        Debug.Log("Multiply_the_attack_of_the_card_by_the_number_of_identical_cards_on_the_field Effect");

        int count = 1;

        if (GameManager.GetComponent<GameManager>().player1.isPlaying)
        {
            for (int i = 0; i < GameManager.GetComponent<GameManager>().player1.board.M.GetComponent<MeleeZone>().melee.Count; i++)
            {
                if (GameManager.GetComponent<GameManager>().player1.board.M.GetComponent<MeleeZone>().melee[i].GetComponent<CardDisplay>().cardData.CardName == gameObject.GetComponent<CardDisplay>().cardData.CardName && GameManager.GetComponent<GameManager>().player1.board.M.GetComponent<MeleeZone>().melee[i] != gameObject)
                {
                    count++;
                }
            }
            for (int i = 0; i < GameManager.GetComponent<GameManager>().player1.board.R.GetComponent<RangedZone>().ranged.Count; i++)
            {
                if (GameManager.GetComponent<GameManager>().player1.board.R.GetComponent<RangedZone>().ranged[i].GetComponent<CardDisplay>().cardData.CardName == gameObject.GetComponent<CardDisplay>().cardData.CardName && GameManager.GetComponent<GameManager>().player1.board.R.GetComponent<RangedZone>().ranged[i] != gameObject)
                {
                    count++;
                }
            }
            for (int i = 0; i < GameManager.GetComponent<GameManager>().player1.board.S.GetComponent<SiegeZone>().siege.Count; i++)
            {
                if (GameManager.GetComponent<GameManager>().player1.board.S.GetComponent<SiegeZone>().siege[i].GetComponent<CardDisplay>().cardData.CardName == gameObject.GetComponent<CardDisplay>().cardData.CardName && GameManager.GetComponent<GameManager>().player1.board.S.GetComponent<SiegeZone>().siege[i] != gameObject)
                {
                    count++;
                }
            }
        }

        else
        {
            for (int i = 0; i < GameManager.GetComponent<GameManager>().player2.board.M.GetComponent<MeleeZone>().melee.Count; i++)
            {
                if (GameManager.GetComponent<GameManager>().player2.board.M.GetComponent<MeleeZone>().melee[i].GetComponent<CardDisplay>().cardData.CardName == gameObject.GetComponent<CardDisplay>().cardData.CardName && GameManager.GetComponent<GameManager>().player2.board.M.GetComponent<MeleeZone>().melee[i] != gameObject)
                {
                    count++;
                }
            }
            for (int i = 0; i < GameManager.GetComponent<GameManager>().player2.board.R.GetComponent<RangedZone>().ranged.Count; i++)
            {
                if (GameManager.GetComponent<GameManager>().player2.board.R.GetComponent<RangedZone>().ranged[i].GetComponent<CardDisplay>().cardData.CardName == gameObject.GetComponent<CardDisplay>().cardData.CardName && GameManager.GetComponent<GameManager>().player2.board.R.GetComponent<RangedZone>().ranged[i] != gameObject)
                {
                    count++;
                }
            }
            for (int i = 0; i < GameManager.GetComponent<GameManager>().player2.board.S.GetComponent<SiegeZone>().siege.Count; i++)
            {
                if (GameManager.GetComponent<GameManager>().player2.board.S.GetComponent<SiegeZone>().siege[i].GetComponent<CardDisplay>().cardData.CardName == gameObject.GetComponent<CardDisplay>().cardData.CardName && GameManager.GetComponent<GameManager>().player2.board.S.GetComponent<SiegeZone>().siege[i] != gameObject)
                {
                    count++;
                }
            }
        }

        gameObject.GetComponent<CardDisplay>().cardData.Power *= count;
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

        //Debug.Log("Climate Effect");

        if (gameObject.GetComponent<CardDisplay>().cardData.TypeField == 'M')
        {
            for (int i = 0; i < GameManager.GetComponent<GameManager>().player1.board.M.GetComponent<MeleeZone>().melee.Count; i++)
            {
                if (GameManager.GetComponent<GameManager>().player1.board.M.GetComponent<MeleeZone>().melee[i].GetComponent<CardDisplay>().cardData.TypeUnitCard != CardData.SubTypeUnitCard.Gold && !GameManager.GetComponent<GameManager>().player1.board.M.GetComponent<MeleeZone>().melee[i].GetComponent<CardDisplay>().cardData.affectedByClimate)
                {
                    GameManager.GetComponent<GameManager>().player1.board.M.GetComponent<MeleeZone>().melee[i].GetComponent<CardDisplay>().cardData.Power = 0;
                    GameManager.GetComponent<GameManager>().player1.board.M.GetComponent<MeleeZone>().melee[i].GetComponent<CardDisplay>().cardData.affectedByClimate = true;
                    GameManager.GetComponent<GameManager>().player1.board.M.GetComponent<MeleeZone>().melee[i].GetComponent<CardDisplay>().cardData.affectedByIncrease = false;
                }
            }
            for (int i = 0; i < GameManager.GetComponent<GameManager>().player2.board.M.GetComponent<MeleeZone>().melee.Count; i++)
            {
                if (GameManager.GetComponent<GameManager>().player2.board.M.GetComponent<MeleeZone>().melee[i].GetComponent<CardDisplay>().cardData.TypeUnitCard != CardData.SubTypeUnitCard.Gold && !GameManager.GetComponent<GameManager>().player2.board.M.GetComponent<MeleeZone>().melee[i].GetComponent<CardDisplay>().cardData.affectedByClimate)
                {
                    GameManager.GetComponent<GameManager>().player2.board.M.GetComponent<MeleeZone>().melee[i].GetComponent<CardDisplay>().cardData.Power = 0;
                    GameManager.GetComponent<GameManager>().player2.board.M.GetComponent<MeleeZone>().melee[i].GetComponent<CardDisplay>().cardData.affectedByClimate = true;
                    GameManager.GetComponent<GameManager>().player2.board.M.GetComponent<MeleeZone>().melee[i].GetComponent<CardDisplay>().cardData.affectedByIncrease = false;
                }
            }
        }
        else if (gameObject.GetComponent<CardDisplay>().cardData.TypeField == 'R')
        {
            for (int i = 0; i < GameManager.GetComponent<GameManager>().player1.board.R.GetComponent<RangedZone>().ranged.Count; i++)
            {
                if (GameManager.GetComponent<GameManager>().player1.board.R.GetComponent<RangedZone>().ranged[i].GetComponent<CardDisplay>().cardData.TypeUnitCard != CardData.SubTypeUnitCard.Gold && !GameManager.GetComponent<GameManager>().player1.board.R.GetComponent<RangedZone>().ranged[i].GetComponent<CardDisplay>().cardData.affectedByClimate)
                {
                    GameManager.GetComponent<GameManager>().player1.board.R.GetComponent<RangedZone>().ranged[i].GetComponent<CardDisplay>().cardData.Power = 0;
                    GameManager.GetComponent<GameManager>().player1.board.R.GetComponent<RangedZone>().ranged[i].GetComponent<CardDisplay>().cardData.affectedByClimate = true;
                    GameManager.GetComponent<GameManager>().player1.board.R.GetComponent<RangedZone>().ranged[i].GetComponent<CardDisplay>().cardData.affectedByIncrease = false;
                }
            }
            for (int i = 0; i < GameManager.GetComponent<GameManager>().player2.board.R.GetComponent<RangedZone>().ranged.Count; i++)
            {
                if (GameManager.GetComponent<GameManager>().player2.board.R.GetComponent<RangedZone>().ranged[i].GetComponent<CardDisplay>().cardData.TypeUnitCard != CardData.SubTypeUnitCard.Gold && !GameManager.GetComponent<GameManager>().player2.board.R.GetComponent<RangedZone>().ranged[i].GetComponent<CardDisplay>().cardData.affectedByClimate)
                {
                    GameManager.GetComponent<GameManager>().player2.board.R.GetComponent<RangedZone>().ranged[i].GetComponent<CardDisplay>().cardData.Power = 0;
                    GameManager.GetComponent<GameManager>().player2.board.R.GetComponent<RangedZone>().ranged[i].GetComponent<CardDisplay>().cardData.affectedByClimate = true;
                    GameManager.GetComponent<GameManager>().player2.board.R.GetComponent<RangedZone>().ranged[i].GetComponent<CardDisplay>().cardData.affectedByIncrease = false;
                }
            }
        }
        else
        {
            for (int i = 0; i < GameManager.GetComponent<GameManager>().player1.board.S.GetComponent<SiegeZone>().siege.Count; i++)
            {
                if (GameManager.GetComponent<GameManager>().player1.board.S.GetComponent<SiegeZone>().siege[i].GetComponent<CardDisplay>().cardData.TypeUnitCard != CardData.SubTypeUnitCard.Gold && !GameManager.GetComponent<GameManager>().player1.board.S.GetComponent<SiegeZone>().siege[i].GetComponent<CardDisplay>().cardData.affectedByClimate)
                {
                    GameManager.GetComponent<GameManager>().player1.board.S.GetComponent<SiegeZone>().siege[i].GetComponent<CardDisplay>().cardData.Power = 1;
                    GameManager.GetComponent<GameManager>().player1.board.S.GetComponent<SiegeZone>().siege[i].GetComponent<CardDisplay>().cardData.affectedByClimate = true;
                    GameManager.GetComponent<GameManager>().player1.board.S.GetComponent<SiegeZone>().siege[i].GetComponent<CardDisplay>().cardData.affectedByIncrease = false;
                }
            }
            for (int i = 0; i < GameManager.GetComponent<GameManager>().player2.board.S.GetComponent<SiegeZone>().siege.Count; i++)
            {
                if (GameManager.GetComponent<GameManager>().player2.board.S.GetComponent<SiegeZone>().siege[i].GetComponent<CardDisplay>().cardData.TypeUnitCard != CardData.SubTypeUnitCard.Gold && !GameManager.GetComponent<GameManager>().player2.board.S.GetComponent<SiegeZone>().siege[i].GetComponent<CardDisplay>().cardData.affectedByClimate)
                {
                    GameManager.GetComponent<GameManager>().player2.board.S.GetComponent<SiegeZone>().siege[i].GetComponent<CardDisplay>().cardData.Power = 1;
                    GameManager.GetComponent<GameManager>().player2.board.S.GetComponent<SiegeZone>().siege[i].GetComponent<CardDisplay>().cardData.affectedByClimate = true;
                    GameManager.GetComponent<GameManager>().player2.board.S.GetComponent<SiegeZone>().siege[i].GetComponent<CardDisplay>().cardData.affectedByIncrease = false;
                }
            }
        }
    }
    public static void Increase(GameObject gameObject)
    {
        GameObject GameManager = GameObject.Find("GameManager");

        //Debug.Log("Increase Effect");

        if (GameManager.GetComponent<GameManager>().player1.board.GetComponent<SubBoard>().Increase.GetComponent<IncreaseZone>().increase.Contains(gameObject))
        {
            if (gameObject.GetComponent<CardDisplay>().cardData.TypeField == 'M')
            {
                for (int i = 0; i < GameManager.GetComponent<GameManager>().player1.board.M.GetComponent<MeleeZone>().melee.Count; i++)
                {
                    if (GameManager.GetComponent<GameManager>().player1.board.M.GetComponent<MeleeZone>().melee[i].GetComponent<CardDisplay>().cardData.TypeUnitCard != CardData.SubTypeUnitCard.Gold &&
                    !GameManager.GetComponent<GameManager>().player1.board.M.GetComponent<MeleeZone>().melee[i].GetComponent<CardDisplay>().cardData.affectedByIncrease)
                    {
                        GameManager.GetComponent<GameManager>().player1.board.M.GetComponent<MeleeZone>().melee[i].GetComponent<CardDisplay>().cardData.Power += 2;
                        //GameManager.GetComponent<GameManager>().player1.board.M.GetComponent<MeleeZone>().melee[i].GetComponent<CardDisplay>().card.StartPower += 2;
                        GameManager.GetComponent<GameManager>().player1.board.M.GetComponent<MeleeZone>().melee[i].GetComponent<CardDisplay>().cardData.affectedByIncrease = true;
                    }
                }
            }
            else if (gameObject.GetComponent<CardDisplay>().cardData.TypeField == 'R')
            {
                for (int i = 0; i < GameManager.GetComponent<GameManager>().player1.board.R.GetComponent<RangedZone>().ranged.Count; i++)
                {
                    if (GameManager.GetComponent<GameManager>().player1.board.R.GetComponent<RangedZone>().ranged[i].GetComponent<CardDisplay>().cardData.TypeUnitCard != CardData.SubTypeUnitCard.Gold &&
                    !GameManager.GetComponent<GameManager>().player1.board.R.GetComponent<RangedZone>().ranged[i].GetComponent<CardDisplay>().cardData.affectedByIncrease)
                    {
                        GameManager.GetComponent<GameManager>().player1.board.R.GetComponent<RangedZone>().ranged[i].GetComponent<CardDisplay>().cardData.Power += 2;
                        //GameManager.GetComponent<GameManager>().player1.board.R.GetComponent<RangedZone>().ranged[i].GetComponent<CardDisplay>().card.StartPower += 2;
                        GameManager.GetComponent<GameManager>().player1.board.R.GetComponent<RangedZone>().ranged[i].GetComponent<CardDisplay>().cardData.affectedByIncrease = true;
                    }
                }
            }
            else
            {
                for (int i = 0; i < GameManager.GetComponent<GameManager>().player1.board.S.GetComponent<SiegeZone>().siege.Count; i++)
                {
                    if (GameManager.GetComponent<GameManager>().player1.board.S.GetComponent<SiegeZone>().siege[i].GetComponent<CardDisplay>().cardData.TypeUnitCard != CardData.SubTypeUnitCard.Gold &&
                    !GameManager.GetComponent<GameManager>().player1.board.S.GetComponent<SiegeZone>().siege[i].GetComponent<CardDisplay>().cardData.affectedByIncrease)
                    {
                        GameManager.GetComponent<GameManager>().player1.board.S.GetComponent<SiegeZone>().siege[i].GetComponent<CardDisplay>().cardData.Power += 2;
                        //GameManager.GetComponent<GameManager>().player1.board.S.GetComponent<SiegeZone>().siege[i].GetComponent<CardDisplay>().card.StartPower += 2;
                        GameManager.GetComponent<GameManager>().player1.board.S.GetComponent<SiegeZone>().siege[i].GetComponent<CardDisplay>().cardData.affectedByIncrease = true;
                    }
                }
            }
        }
        else if (GameManager.GetComponent<GameManager>().player2.board.GetComponent<SubBoard>().Increase.GetComponent<IncreaseZone>().increase.Contains(gameObject))
        {
            if (gameObject.GetComponent<CardDisplay>().cardData.TypeField == 'M')
            {
                for (int i = 0; i < GameManager.GetComponent<GameManager>().player2.board.M.GetComponent<MeleeZone>().melee.Count; i++)
                {
                    if (GameManager.GetComponent<GameManager>().player2.board.M.GetComponent<MeleeZone>().melee[i].GetComponent<CardDisplay>().cardData.TypeUnitCard != CardData.SubTypeUnitCard.Gold &&
                    !GameManager.GetComponent<GameManager>().player2.board.M.GetComponent<MeleeZone>().melee[i].GetComponent<CardDisplay>().cardData.affectedByIncrease)
                    {
                        GameManager.GetComponent<GameManager>().player2.board.M.GetComponent<MeleeZone>().melee[i].GetComponent<CardDisplay>().cardData.Power += 2;
                        //GameManager.GetComponent<GameManager>().player2.board.M.GetComponent<MeleeZone>().melee[i].GetComponent<CardDisplay>().card.StartPower += 2;
                        GameManager.GetComponent<GameManager>().player2.board.M.GetComponent<MeleeZone>().melee[i].GetComponent<CardDisplay>().cardData.affectedByIncrease = true;
                    }
                }
            }
            else if (gameObject.GetComponent<CardDisplay>().cardData.TypeField == 'R')
            {
                for (int i = 0; i < GameManager.GetComponent<GameManager>().player2.board.R.GetComponent<RangedZone>().ranged.Count; i++)
                {
                    if (GameManager.GetComponent<GameManager>().player2.board.R.GetComponent<RangedZone>().ranged[i].GetComponent<CardDisplay>().cardData.TypeUnitCard != CardData.SubTypeUnitCard.Gold &&
                    !GameManager.GetComponent<GameManager>().player2.board.R.GetComponent<RangedZone>().ranged[i].GetComponent<CardDisplay>().cardData.affectedByIncrease)
                    {
                        GameManager.GetComponent<GameManager>().player2.board.R.GetComponent<RangedZone>().ranged[i].GetComponent<CardDisplay>().cardData.Power += 2;
                        //GameManager.GetComponent<GameManager>().player2.board.R.GetComponent<RangedZone>().ranged[i].GetComponent<CardDisplay>().card.StartPower += 2;
                        GameManager.GetComponent<GameManager>().player2.board.R.GetComponent<RangedZone>().ranged[i].GetComponent<CardDisplay>().cardData.affectedByIncrease = true;
                    }
                }
            }
            else
            {
                for (int i = 0; i < GameManager.GetComponent<GameManager>().player2.board.S.GetComponent<SiegeZone>().siege.Count; i++)
                {
                    if (GameManager.GetComponent<GameManager>().player2.board.S.GetComponent<SiegeZone>().siege[i].GetComponent<CardDisplay>().cardData.TypeUnitCard != CardData.SubTypeUnitCard.Gold &&
                    !GameManager.GetComponent<GameManager>().player2.board.S.GetComponent<SiegeZone>().siege[i].GetComponent<CardDisplay>().cardData.affectedByIncrease)
                    {
                        GameManager.GetComponent<GameManager>().player2.board.S.GetComponent<SiegeZone>().siege[i].GetComponent<CardDisplay>().cardData.Power += 2;
                        //GameManager.GetComponent<GameManager>().player2.board.S.GetComponent<SiegeZone>().siege[i].GetComponent<CardDisplay>().card.StartPower += 2;
                        GameManager.GetComponent<GameManager>().player2.board.S.GetComponent<SiegeZone>().siege[i].GetComponent<CardDisplay>().cardData.affectedByIncrease = true;
                    }
                }
            }
        }
    }

    //Boss effects
    public static void StayintheField()
    {
        if (IsEmptyMyField()) return;

        GameObject GameManager = GameObject.Find("GameManager");

        int indextype = UnityEngine.Random.Range(1, 4);

        while (IsRowEmpty(indextype))
        {
            indextype = UnityEngine.Random.Range(1, 4);
        }
        //if (!IsRowEmpty(indextype))
        if (GameManager.GetComponent<GameManager>().player1.deck.CompareTag("Deck Pirates"))
        {
            if (indextype == 1)
            {
                int index = UnityEngine.Random.Range(0, GameManager.GetComponent<GameManager>().player1.board.M.GetComponent<MeleeZone>().melee.Count);
                GameManager.GetComponent<GameManager>().player1.board.M.GetComponent<MeleeZone>().melee[index].GetComponent<CardDisplay>().cardData.stayintheField = true;
                Debug.Log("StayintheField activado en melee 1");
            }
            else if (indextype == 2)
            {
                int index = UnityEngine.Random.Range(0, GameManager.GetComponent<GameManager>().player1.board.R.GetComponent<RangedZone>().ranged.Count);
                GameManager.GetComponent<GameManager>().player1.board.R.GetComponent<RangedZone>().ranged[index].GetComponent<CardDisplay>().cardData.stayintheField = true;
                Debug.Log("StayintheField activado en ranged 1");
            }
            else
            {
                int index = UnityEngine.Random.Range(0, GameManager.GetComponent<GameManager>().player1.board.S.GetComponent<SiegeZone>().siege.Count);
                GameManager.GetComponent<GameManager>().player1.board.S.GetComponent<SiegeZone>().siege[index].GetComponent<CardDisplay>().cardData.stayintheField = true;
                Debug.Log("StayintheField activado en siege 1");
            }
        }
        else if (GameManager.GetComponent<GameManager>().player2.deck.CompareTag("Deck Pirates"))
        {
            if (indextype == 1)
            {
                int index = UnityEngine.Random.Range(0, GameManager.GetComponent<GameManager>().player2.board.M.GetComponent<MeleeZone>().melee.Count);
                GameManager.GetComponent<GameManager>().player2.board.M.GetComponent<MeleeZone>().melee[index].GetComponent<CardDisplay>().cardData.stayintheField = true;
                Debug.Log("StayintheField activado en melee 2");
            }
            else if (indextype == 2)
            {
                int index = UnityEngine.Random.Range(0, GameManager.GetComponent<GameManager>().player2.board.R.GetComponent<RangedZone>().ranged.Count);
                GameManager.GetComponent<GameManager>().player2.board.R.GetComponent<RangedZone>().ranged[index].GetComponent<CardDisplay>().cardData.stayintheField = true;
            }
            else
            {
                int index = UnityEngine.Random.Range(0, GameManager.GetComponent<GameManager>().player2.board.S.GetComponent<SiegeZone>().siege.Count);
                GameManager.GetComponent<GameManager>().player2.board.S.GetComponent<SiegeZone>().siege[index].GetComponent<CardDisplay>().cardData.stayintheField = true;
            }
        }
    }

    //Without effect
    public static void None() { return; }

    //Aux effects
    public static void DisableEffectClimate(GameObject gameObject)
    {
        GameObject GameManager = GameObject.FindGameObjectWithTag("GameManager");

        Debug.Log("DisableEffectClimate");

        if (GameManager.GetComponent<GameManager>().player2.board.Climate.GetComponent<ClimateZone>().climate == gameObject)
        {
            if (GameManager.GetComponent<GameManager>().player2.board.Climate.GetComponent<ClimateZone>().climate.GetComponent<CardDisplay>().cardData.TypeField == 'M')
            {
                for (int i = 0; i < GameManager.GetComponent<GameManager>().player1.board.M.GetComponent<MeleeZone>().melee.Count; i++)
                {
                    GameManager.GetComponent<GameManager>().player1.board.M.GetComponent<MeleeZone>().melee[i].GetComponent<CardDisplay>().cardData.Power = GameManager.GetComponent<GameManager>().player1.board.M.GetComponent<MeleeZone>().melee[i].GetComponent<CardDisplay>().cardData.StartPower;
                }
                for (int i = 0; i < GameManager.GetComponent<GameManager>().player2.board.M.GetComponent<MeleeZone>().melee.Count; i++)
                {
                    GameManager.GetComponent<GameManager>().player2.board.M.GetComponent<MeleeZone>().melee[i].GetComponent<CardDisplay>().cardData.Power = GameManager.GetComponent<GameManager>().player2.board.M.GetComponent<MeleeZone>().melee[i].GetComponent<CardDisplay>().cardData.StartPower;
                }
            }
            else if (GameManager.GetComponent<GameManager>().player2.board.Climate.GetComponent<ClimateZone>().climate.GetComponent<CardDisplay>().cardData.TypeField == 'R')
            {
                for (int i = 0; i < GameManager.GetComponent<GameManager>().player1.board.R.GetComponent<RangedZone>().ranged.Count; i++)
                {
                    GameManager.GetComponent<GameManager>().player1.board.R.GetComponent<RangedZone>().ranged[i].GetComponent<CardDisplay>().cardData.Power = GameManager.GetComponent<GameManager>().player1.board.R.GetComponent<RangedZone>().ranged[i].GetComponent<CardDisplay>().cardData.StartPower;
                }
                for (int i = 0; i < GameManager.GetComponent<GameManager>().player2.board.R.GetComponent<RangedZone>().ranged.Count; i++)
                {
                    GameManager.GetComponent<GameManager>().player2.board.R.GetComponent<RangedZone>().ranged[i].GetComponent<CardDisplay>().cardData.Power = GameManager.GetComponent<GameManager>().player2.board.R.GetComponent<RangedZone>().ranged[i].GetComponent<CardDisplay>().cardData.StartPower;
                }
            }
            else
            {
                for (int i = 0; i < GameManager.GetComponent<GameManager>().player1.board.S.GetComponent<SiegeZone>().siege.Count; i++)
                {
                    GameManager.GetComponent<GameManager>().player1.board.S.GetComponent<SiegeZone>().siege[i].GetComponent<CardDisplay>().cardData.Power++;
                }
                for (int i = 0; i < GameManager.GetComponent<GameManager>().player2.board.S.GetComponent<SiegeZone>().siege.Count; i++)
                {
                    GameManager.GetComponent<GameManager>().player2.board.S.GetComponent<SiegeZone>().siege[i].GetComponent<CardDisplay>().cardData.Power++;
                }
            }
        }

        else if (GameManager.GetComponent<GameManager>().player1.board.Climate.GetComponent<ClimateZone>().climate == gameObject)
        {
            if (GameManager.GetComponent<GameManager>().player1.board.Climate.GetComponent<ClimateZone>().climate.GetComponent<CardDisplay>().cardData.TypeField == 'M')
            {
                for (int i = 0; i < GameManager.GetComponent<GameManager>().player1.board.M.GetComponent<MeleeZone>().melee.Count; i++)
                {
                    GameManager.GetComponent<GameManager>().player1.board.M.GetComponent<MeleeZone>().melee[i].GetComponent<CardDisplay>().cardData.Power = GameManager.GetComponent<GameManager>().player1.board.M.GetComponent<MeleeZone>().melee[i].GetComponent<CardDisplay>().cardData.StartPower;
                }
                for (int i = 0; i < GameManager.GetComponent<GameManager>().player2.board.M.GetComponent<MeleeZone>().melee.Count; i++)
                {
                    GameManager.GetComponent<GameManager>().player2.board.M.GetComponent<MeleeZone>().melee[i].GetComponent<CardDisplay>().cardData.Power = GameManager.GetComponent<GameManager>().player2.board.M.GetComponent<MeleeZone>().melee[i].GetComponent<CardDisplay>().cardData.StartPower;
                }
            }
            else if (GameManager.GetComponent<GameManager>().player1.board.Climate.GetComponent<ClimateZone>().climate.GetComponent<CardDisplay>().cardData.TypeField == 'R')
            {
                for (int i = 0; i < GameManager.GetComponent<GameManager>().player1.board.R.GetComponent<RangedZone>().ranged.Count; i++)
                {
                    GameManager.GetComponent<GameManager>().player1.board.R.GetComponent<RangedZone>().ranged[i].GetComponent<CardDisplay>().cardData.Power = GameManager.GetComponent<GameManager>().player1.board.R.GetComponent<RangedZone>().ranged[i].GetComponent<CardDisplay>().cardData.StartPower;
                }
                for (int i = 0; i < GameManager.GetComponent<GameManager>().player2.board.R.GetComponent<RangedZone>().ranged.Count; i++)
                {
                    GameManager.GetComponent<GameManager>().player2.board.R.GetComponent<RangedZone>().ranged[i].GetComponent<CardDisplay>().cardData.Power = GameManager.GetComponent<GameManager>().player2.board.R.GetComponent<RangedZone>().ranged[i].GetComponent<CardDisplay>().cardData.StartPower;
                }
            }
            else
            {
                for (int i = 0; i < GameManager.GetComponent<GameManager>().player1.board.S.GetComponent<SiegeZone>().siege.Count; i++)
                {
                    GameManager.GetComponent<GameManager>().player1.board.S.GetComponent<SiegeZone>().siege[i].GetComponent<CardDisplay>().cardData.Power++;
                }
                for (int i = 0; i < GameManager.GetComponent<GameManager>().player2.board.S.GetComponent<SiegeZone>().siege.Count; i++)
                {
                    GameManager.GetComponent<GameManager>().player2.board.S.GetComponent<SiegeZone>().siege[i].GetComponent<CardDisplay>().cardData.Power++;
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
    public static bool IsEmptyMyField()
    {
        GameObject GameManager = GameObject.FindGameObjectWithTag("GameManager");
        int count = 3;

        if (GameManager.GetComponent<GameManager>().player1.isPlaying)
        {
            if (GameManager.GetComponent<GameManager>().player1.board.M.GetComponent<MeleeZone>().melee.Count == 0)
            {
                count--;
            }
            if (GameManager.GetComponent<GameManager>().player1.board.R.GetComponent<RangedZone>().ranged.Count == 0)
            {
                count--;
            }
            if (GameManager.GetComponent<GameManager>().player1.board.S.GetComponent<SiegeZone>().siege.Count == 0)
            {
                count--;
            }
        }
        else
        {
            if (GameManager.GetComponent<GameManager>().player2.board.M.GetComponent<MeleeZone>().melee.Count == 0)
            {
                count--;
            }
            if (GameManager.GetComponent<GameManager>().player2.board.R.GetComponent<RangedZone>().ranged.Count == 0)
            {
                count--;
            }
            if (GameManager.GetComponent<GameManager>().player2.board.S.GetComponent<SiegeZone>().siege.Count == 0)
            {
                count--;
            }
        }

        if (count == 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public static bool IsEmptyFieldEnemy()
    {
        GameObject GameManager = GameObject.FindGameObjectWithTag("GameManager");
        int count = 3;

        if (GameManager.GetComponent<GameManager>().player1.isPlaying)
        {
            if (GameManager.GetComponent<GameManager>().player2.board.M.GetComponent<MeleeZone>().melee.Count == 0)
            {
                count--;
            }
            if (GameManager.GetComponent<GameManager>().player2.board.R.GetComponent<RangedZone>().ranged.Count == 0)
            {
                count--;
            }
            if (GameManager.GetComponent<GameManager>().player2.board.S.GetComponent<SiegeZone>().siege.Count == 0)
            {
                count--;
            }
        }
        else
        {
            if (GameManager.GetComponent<GameManager>().player1.board.M.GetComponent<MeleeZone>().melee.Count == 0)
            {
                count--;
            }
            if (GameManager.GetComponent<GameManager>().player1.board.R.GetComponent<RangedZone>().ranged.Count == 0)
            {
                count--;
            }
            if (GameManager.GetComponent<GameManager>().player1.board.S.GetComponent<SiegeZone>().siege.Count == 0)
            {
                count--;
            }
        }

        if (count == 0)
        {
            return true;
        }
        else
        {
            return false;
        }


    }
    public static bool IsRowEmpty(int n)
    {
        GameObject GameManager = GameObject.FindGameObjectWithTag("GameManager");

        if (GameManager.GetComponent<GameManager>().player1.isPlaying)
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
        else if (GameManager.GetComponent<GameManager>().player2.isPlaying)
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
        return false;
    }
    public static bool IsRowEmptyEnemy(int n)
    {
        GameObject GameManager = GameObject.FindGameObjectWithTag("GameManager");

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
}
