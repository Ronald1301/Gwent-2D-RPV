using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCard : MonoBehaviour
{
    GameObject subBoard;
    GameObject GameManager;
    [SerializeField] ScriptUIRuntime UIRuntime;
        //GameObject Zone;

        void Start()
    {
        GameManager = GameObject.Find("GameManager");
        UIRuntime = GameObject.Find("UIRuntime").GetComponent<ScriptUIRuntime>();
        /*
        if(subBoard.Hand.tag == "Hand1")
        {             
            subBoard = GameObject.FindGameObjectWithTag("SubBoard1").GetComponent<SubBoard>();
        }
        else
        {
            subBoard = GameObject.FindGameObjectWithTag("SubBoard2").GetComponent<SubBoard>();
        }
        */
        subBoard = GameObject.FindGameObjectWithTag("SubBoard1");
        Debug.Log("subBoard cargado");

    }

    private void OnMouseDown()
    {
        //Move card to the corresponding zone
        if (GetComponent<CardDisplay>().card.Type == Card.CardType.Unit)
        {
            if(GetComponent<CardDisplay>().card.TypeField == 'M')
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
                if (subBoard.GetComponent<SubBoard>().Climate.GetComponent<ClimateZone>().climate == null)
                {
                    MoveToClimate();
                    Debug.Log("Move to Climate");
                }   
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
                if(subBoard.GetComponent<SubBoard>().Climate.GetComponent<ClimateZone>().climate != null)
                {
                    MoveToCemetery(subBoard.GetComponent<SubBoard>().Climate.GetComponent<ClimateZone>().climate);
                    Debug.Log("Move to Cemetery card climate");
                    MoveToClimate();
                    Debug.Log("Move to Climate card clearance");
                }
            }
            else if (gameObject.GetComponent<CardDisplay>().card.TypeSpecialCard == Card.SubTypeSpecialCard.Lure)
            {
               // MoveToCemetery();
               Debug.Log("Move card lure");
            }
        }

        //Update points
        GameManager.GetComponent<GameManager>().UpdatePoints(GetComponent<CardDisplay>().card);
        //Update UI
        UIRuntime.UIUpdate();
        //Change turn
        GameManager.GetComponent<GameManager>().ChangeTurn();

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
        this.gameObject.transform.SetParent(subBoard.GetComponent<SubBoard>().Increase.transform, false);
    }
    public void MoveToClimate()
    {
        subBoard.GetComponent<SubBoard>().Climate.GetComponent<ClimateZone>().climate = this.gameObject;
        this.gameObject.transform.SetParent(subBoard.GetComponent<SubBoard>().Climate.transform, false);
    }
    public void MoveToCemetery()
    {
        subBoard.GetComponent<SubBoard>().Cementery.GetComponent<CementeryZone>().Cemetery.Add(this.gameObject);
        this.gameObject.transform.SetParent(subBoard.GetComponent<SubBoard>().Cementery.transform, false);
    }
    public void MoveToCemetery( GameObject card)
    {
        subBoard.GetComponent<SubBoard>().Cementery.GetComponent<CementeryZone>().Cemetery.Add(card);
       card.transform.SetParent(subBoard.GetComponent<SubBoard>().Cementery.transform, false);
    }
}
