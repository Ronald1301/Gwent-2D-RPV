using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCard : MonoBehaviour
{
    GameObject subBoard;
    public GameObject GameManager;
    public ScriptUIRuntime UIRuntime;
    //GameObject Zone;

    private bool isClicked = false;
    private bool clickLure = false;

    void Start()
    {
//        StartCoroutine(WaitForClick());
        GameManager = GameObject.Find("GameManager");
        //UIRuntime = GameObject.FindGameObjectWithTag("UI Runtime").GetComponent<ScriptUIRuntime>();

        
                if (GameManager.GetComponent<GameManager>().player1.isPlaying)
                {
                   // subBoard = GameObject.FindGameObjectWithTag("SubBoard1");
                   subBoard=GameManager.GetComponent<GameManager>().player1.board.gameObject;
                    Debug.Log("SubBoard1");
                }
                else
                {
                    //subBoard = GameObject.FindGameObjectWithTag("SubBoard2");
                    subBoard=GameManager.GetComponent<GameManager>().player2.board.gameObject;
                    Debug.Log("SubBoard2");
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
                if (subBoard.GetComponent<SubBoard>().Climate.GetComponent<ClimateZone>().climate != null)
                {
                    Effects.DisableEffectClimate();
                    GameManager.GetComponent<GameManager>().player1.board.Climate.GetComponent<ClimateZone>().climate.GetComponent<MoveCard>().MoveToCemetery();
                    //this.gameObject.transform.position = GameManager.GetComponent<GameManager>().player1.board.Climate.GetComponent<ClimateZone>().climate.transform.position;
                }
                MoveToClimate();
                Debug.Log("Move to Climate");
            }
            else if (gameObject.GetComponent<CardDisplay>().card.TypeSpecialCard == Card.SubTypeSpecialCard.Increase)
            {
                if (subBoard.GetComponent<SubBoard>().Increase.GetComponent<IncreaseZone>().increase[0] == null && GetComponent<CardDisplay>().card.TypeField == 'M')
                {
                    MoveToIncrease(0);
                    Debug.Log("Move to Increase en 0");
                }
                else if (subBoard.GetComponent<SubBoard>().Increase.GetComponent<IncreaseZone>().increase[1] == null && GetComponent<CardDisplay>().card.TypeField == 'R')
                {
                    MoveToIncrease(1);
                    Debug.Log("Move to Increase en 1");
                }
                else if (subBoard.GetComponent<SubBoard>().Increase.GetComponent<IncreaseZone>().increase[2] == null && GetComponent<CardDisplay>().card.TypeField == 'S')
                {
                    MoveToIncrease(2);
                    Debug.Log("Move to Increase en 2");
                }
            }
            else if (gameObject.GetComponent<CardDisplay>().card.TypeSpecialCard == Card.SubTypeSpecialCard.Clearance)
            {
                if (subBoard.GetComponent<SubBoard>().Climate.GetComponent<ClimateZone>().climate != null)
                {
                    Effects.DisableEffectClimate();
                    subBoard.GetComponent<SubBoard>().Climate.GetComponent<ClimateZone>().climate.GetComponent<MoveCard>().MoveToCemetery();
                    Debug.Log("Move to Cemetery card climate");
                }
                MoveToClimate();
                Debug.Log("Move to Climate card clearance");
            }
            else if (gameObject.GetComponent<CardDisplay>().card.TypeSpecialCard == Card.SubTypeSpecialCard.Lure)
            {
                if (subBoard.GetComponent<SubBoard>().M.GetComponent<MeleeZone>().melee.Count == 0)
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
    }

    private void OnMouseDown()
    {
        if (GameManager.GetComponent<GameManager>().player1.isPlaying && GameManager.GetComponent<GameManager>().player1.board.GetComponent<SubBoard>().Hand.GetComponent<Hand>().CardsInHand.Contains(this.gameObject)
            || GameManager.GetComponent<GameManager>().player2.isPlaying && GameManager.GetComponent<GameManager>().player2.board.GetComponent<SubBoard>().Hand.GetComponent<Hand>().CardsInHand.Contains(this.gameObject))
        {
            Move();
            Effects.ActivateEffect(gameObject);
            GameManager.GetComponent<GameManager>().UpdatePoints(gameObject.GetComponent<CardDisplay>().card);

            /*
            GameManager.GetComponent<GameManager>().player1.board.UpdatePoints();
            GameManager.GetComponent<GameManager>().player2.board.UpdatePoints();
            GameManager.GetComponent<GameManager>().UpdatePoints();
            UIRuntime.UIUpdate();
            //Update points
            GameManager.GetComponent<GameManager>().player1.board.UpdatePoints();
            GameManager.GetComponent<GameManager>().player2.board.UpdatePoints();
            //Update UI
            */
            UIRuntime.UIUpdate();

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
    }
    public void MoveToCemetery()
    {
        subBoard.GetComponent<SubBoard>().Cemetery.GetComponent<CemeteryZone>().Cemetery.Add(this.gameObject);
        this.gameObject.transform.SetParent(subBoard.GetComponent<SubBoard>().Cemetery.transform, false);
    }
    public void MoveToHand()
    {
        subBoard.GetComponent<SubBoard>().Hand.GetComponent<Hand>().CardsInHand.Add(this.gameObject);
        this.gameObject.transform.SetParent(subBoard.GetComponent<SubBoard>().Hand.transform, false);
    }
    /*
    public void MoveToCemetery(GameObject card)
    {
        subBoard.GetComponent<SubBoard>().Cemetery.GetComponent<CemeteryZone>().Cemetery.Add(card);
        card.transform.SetParent(subBoard.GetComponent<SubBoard>().Cemetery.transform, false);
    }
    */
}
