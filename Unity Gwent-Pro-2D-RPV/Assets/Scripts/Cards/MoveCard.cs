using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCard : MonoBehaviour
{
    GameObject subBoard;
    public GameObject GameManager;
    public ScriptUIRuntime UIRuntime;
    private GameObject climateCard;

    private bool isClicked = false;
    private bool clickLure = false;

    void Start()
    {
        climateCard = GameObject.FindGameObjectWithTag("ClimateCard");
        //StartCoroutine(WaitForClick());
        //GameManager = GameObject.Find("GameManager");
        //UIRuntime = GameObject.FindGameObjectWithTag("UI Runtime").GetComponent<ScriptUIRuntime>();

        if (GameManager.GetComponent<GameManager>().player1.hand.CardsInHand.Contains(this.gameObject))
        {
            // subBoard = GameObject.FindGameObjectWithTag("SubBoard1");
            subBoard = GameManager.GetComponent<GameManager>().player1.board.gameObject;
            //Debug.Log("SubBoard1");
        }
        else if (GameManager.GetComponent<GameManager>().player2.hand.CardsInHand.Contains(this.gameObject))
        {
            //subBoard = GameObject.FindGameObjectWithTag("SubBoard2");
            subBoard = GameManager.GetComponent<GameManager>().player2.board.gameObject;
            //Debug.Log("SubBoard2");
        }
        // subBoard = GameObject.FindGameObjectWithTag("SubBoard1");
    }
    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            isClicked = true;
            clickLure = true;
        }
    }
    private void OnMouseDown()
    {
        if (GameManager.GetComponent<GameManager>().player1.isPlaying &&
        GameManager.GetComponent<GameManager>().player1.board.GetComponent<SubBoard>().Hand.GetComponent<Hand>().CardsInHand.Contains(this.gameObject)
            || GameManager.GetComponent<GameManager>().player2.isPlaying &&
            GameManager.GetComponent<GameManager>().player2.board.GetComponent<SubBoard>().Hand.GetComponent<Hand>().CardsInHand.Contains(this.gameObject))
        {
            Move();
            this.gameObject.GetComponent<Card>().inTheField = true;
        }
    }

    public void Move()
    {
        gameObject.transform.localScale = new Vector3(1, 1, 1);

        //Move card to the corresponding zone
        if (GetComponent<CardDisplay>().card.Type == Card.CardType.Unit)
        {
            if (GetComponent<CardDisplay>().card.TypeField == 'M')
            {
                MoveToM();
                Debug.Log("Move to M");
            }
            else if (GetComponent<CardDisplay>().card.TypeField == 'R')
            {
                MoveToR();
                Debug.Log("Move to R");
            }
            else if (GetComponent<CardDisplay>().card.TypeField == 'S')
            {
                MoveToS();
                Debug.Log("Move to S");
            }
        }
        else
        {
            if (gameObject.GetComponent<CardDisplay>().card.TypeSpecialCard == Card.SubTypeSpecialCard.Climate)
            {
                if (subBoard.GetComponent<SubBoard>().Climate.GetComponent<ClimateZone>().climate != climateCard)
                {
                    Effects.DisableEffectClimate(subBoard.GetComponent<SubBoard>().Climate.GetComponent<ClimateZone>().climate);
                    GameManager.GetComponent<GameManager>().player1.board.Climate.GetComponent<ClimateZone>().climate.GetComponent<MoveCard>().MoveToCemetery(subBoard.GetComponent<SubBoard>().Climate.GetComponent<ClimateZone>().climate, subBoard.GetComponent<SubBoard>().Cemetery);
                    //this.gameObject.transform.position = GameManager.GetComponent<GameManager>().player1.board.Climate.GetComponent<ClimateZone>().climate.transform.position;
                }
                MoveToClimate();
                Debug.Log("Move to Climate");
            }
            else if (gameObject.GetComponent<CardDisplay>().card.TypeSpecialCard == Card.SubTypeSpecialCard.Increase)
            {
                if (/*subBoard.GetComponent<SubBoard>().Increase.GetComponent<IncreaseZone>().increase[0] == null &&*/ GetComponent<CardDisplay>().card.TypeField == 'M')
                {
                    MoveToIncrease(0);
                    Debug.Log("Move to Increase en 0");
                }
                else if (/*subBoard.GetComponent<SubBoard>().Increase.GetComponent<IncreaseZone>().increase[1] == null && */GetComponent<CardDisplay>().card.TypeField == 'R')
                {
                    MoveToIncrease(1);
                    Debug.Log("Move to Increase en 1");
                }
                else if (/*subBoard.GetComponent<SubBoard>().Increase.GetComponent<IncreaseZone>().increase[2] == null &&*/ GetComponent<CardDisplay>().card.TypeField == 'S')
                {
                    MoveToIncrease(2);
                    Debug.Log("Move to Increase en 2");
                }
            }
            else if (gameObject.GetComponent<CardDisplay>().card.TypeSpecialCard == Card.SubTypeSpecialCard.Clearance)
            {
                if (subBoard == GameManager.GetComponent<GameManager>().player1.board.gameObject)
                {
                    if (GameManager.GetComponent<GameManager>().player2.board.Climate.GetComponent<ClimateZone>().climate != climateCard)
                    {
                        Effects.DisableEffectClimate(GameManager.GetComponent<GameManager>().player2.board.GetComponent<ClimateZone>().climate);
                        subBoard.GetComponent<SubBoard>().Climate.GetComponent<ClimateZone>().climate.GetComponent<MoveCard>().MoveToCemetery(GameManager.GetComponent<GameManager>().player2.board.GetComponent<ClimateZone>().climate, subBoard.GetComponent<SubBoard>().Cemetery);
                        Debug.Log("Move to Cemetery card climate");
                    }
                }
                else if (subBoard == GameManager.GetComponent<GameManager>().player2.board.gameObject)
                {
                    if (GameManager.GetComponent<GameManager>().player1.board.Climate.GetComponent<ClimateZone>().climate != climateCard)
                    {
                        Effects.DisableEffectClimate(GameManager.GetComponent<GameManager>().player1.board.Climate.GetComponent<ClimateZone>().climate);
                        subBoard.GetComponent<SubBoard>().Climate.GetComponent<ClimateZone>().climate.GetComponent<MoveCard>().MoveToCemetery(GameManager.GetComponent<GameManager>().player1.board.Climate.GetComponent<ClimateZone>().climate, subBoard.GetComponent<SubBoard>().Cemetery);
                        Debug.Log("Move to Cemetery card climate");
                    }
                }
                MoveToClimate();
                Debug.Log("Move to Climate card clearance");
            }
            else if (gameObject.GetComponent<CardDisplay>().card.TypeSpecialCard == Card.SubTypeSpecialCard.Lure)
            {
                if (Effects.IsRowEMpty(1) && Effects.IsRowEMpty(2) && Effects.IsRowEMpty(3))
                {
                    int indextype = UnityEngine.Random.Range(1, 3);
                    if (indextype == 1)
                    {
                        MoveToM();
                        Debug.Log("Move to M card lure");
                    }
                    else if (indextype == 2)
                    {
                        MoveToR();
                        Debug.Log("Move to R card lure");
                    }
                    else if (indextype == 3)
                    {
                        MoveToS();
                        Debug.Log("Move to S card lure");
                    }
                }
                else
                {
                    while (!clickLure)
                    {
                        StartCoroutine(WaitForClick());
                        return;
                    }
                }
                /*
                GameManager.GetComponent<GameManager>().player1.board.UpdatePoints();
                GameManager.GetComponent<GameManager>().player2.board.UpdatePoints();
                GameManager.GetComponent<GameManager>().UpdatePoints();
                //Update UI
                UIRuntime.UIUpdate();
                GameManager.GetComponent<GameManager>().ChangeTurn();
                */
            }
        }
        subBoard.GetComponent<SubBoard>().Hand.GetComponent<Hand>().CardsInHand.Remove(this.gameObject);

        //Here I activate the effect of the card, 
        //I add the points of the card if it has them to its corresponding player, 
        //I update the UI and change the turn
        {
            UIRuntime.UIUpdate();

            //Add points from the card to the player
            GameManager.GetComponent<GameManager>().UpdatePoints(this.gameObject.GetComponent<CardDisplay>().card);

            //Activate card effect
            Effects.ActivateEffect(gameObject);

            //Update points
            GameManager.GetComponent<GameManager>().player1.board.UpdatePoints();
            GameManager.GetComponent<GameManager>().player2.board.UpdatePoints();
            GameManager.GetComponent<GameManager>().UpdatePoints();

            //Update UI
            UIRuntime.UIUpdate();

            //Change Turn
            GameManager.GetComponent<GameManager>().ChangeTurn();
        }
    }
    public void MoveToM()
    {
        subBoard.GetComponent<SubBoard>().M.GetComponent<MeleeZone>().melee.Add(this.gameObject);
        this.gameObject.transform.SetParent(subBoard.GetComponent<SubBoard>().M.transform, false);
    }
    public void MoveToR()
    {
        subBoard.GetComponent<SubBoard>().R.GetComponent<RangedZone>().ranged.Add(this.gameObject);
        this.gameObject.transform.SetParent(subBoard.GetComponent<SubBoard>().R.transform, false);
    }
    public void MoveToS()
    {
        subBoard.GetComponent<SubBoard>().S.GetComponent<SiegeZone>().siege.Add(this.gameObject);
        this.gameObject.transform.SetParent(subBoard.GetComponent<SubBoard>().S.transform, false);
    }
    public void MoveToIncrease(int i)
    {
        subBoard.GetComponent<SubBoard>().Increase.GetComponent<IncreaseZone>().increase[i] = this.gameObject;
        //subBoard.GetComponent<SubBoard>().Increase.GetComponent<IncreaseZone>().increase[i].transform.SetParent(subBoard.GetComponent<SubBoard>().Increase.transform, false);
        this.gameObject.transform.SetParent(subBoard.GetComponent<SubBoard>().Increase.transform, false);
    }
    public void MoveToClimate()
    {
        subBoard.GetComponent<SubBoard>().Climate.GetComponent<ClimateZone>().climate = this.gameObject;
        this.gameObject.transform.SetParent(subBoard.GetComponent<SubBoard>().Climate.transform, false);
        this.gameObject.transform.position = new Vector3(transform.position.x - 7, transform.position.y, transform.position.z - 0.1f);
    }
    public void MoveToCemetery()
    {
        subBoard.GetComponent<SubBoard>().Cemetery.GetComponent<CemeteryZone>().Cemetery.Add(this.gameObject);
        this.gameObject.transform.SetParent(subBoard.GetComponent<SubBoard>().Cemetery.transform, false);
        this.gameObject.transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z - 0.1f);
    }
    public void MoveToCemetery(GameObject card, GameObject cemetery)
    {
        cemetery.GetComponent<CemeteryZone>().Cemetery.Add(card);
        card.transform.SetParent(cemetery.transform, false);
        card.transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z - 0.1f);
    }
    public void MoveToHand()
    {
        subBoard.GetComponent<SubBoard>().Hand.GetComponent<Hand>().CardsInHand.Add(this.gameObject);
        this.gameObject.transform.SetParent(subBoard.GetComponent<SubBoard>().Hand.transform, false);
        this.gameObject.transform.localScale = new Vector3(0.4f, 0.6f, 1);
    }

    //Card Lure
    public IEnumerator WaitForClick()
    {
        while (!isClicked)
        {
            yield return null;
        }
        if (Input.GetMouseButtonDown(1))
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);
            if (hit.collider != null)
            {
                GameObject card = hit.collider.gameObject;
                if (card.GetComponent<CardDisplay>().card.Type == Card.CardType.Unit)
                {
                    if (card.GetComponent<CardDisplay>().card.TypeField == 'M')
                    {
                        card.GetComponent<MoveCard>().MoveToHand();
                        MoveToM();
                    }
                    else if (card.GetComponent<CardDisplay>().card.TypeField == 'R')
                    {
                        card.GetComponent<MoveCard>().MoveToHand();
                        MoveToR();
                    }
                    else if (card.GetComponent<CardDisplay>().card.TypeField == 'S')
                    {
                        card.GetComponent<MoveCard>().MoveToHand();
                        MoveToS();
                    }
                }
            }
        }
        isClicked = false;
        clickLure = false;
        GameManager.GetComponent<GameManager>().UpdatePoints();
        UIRuntime.UIUpdate();
        GameManager.GetComponent<GameManager>().ChangeTurn();
    }



}
