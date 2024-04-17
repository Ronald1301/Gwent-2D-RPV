using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCard : MonoBehaviour
{
    SubBoard subBoard;
    GameObject Zone;
 
    void Start()
    {
        subBoard = GameObject.FindGameObjectWithTag("SubBoard1").GetComponent<SubBoard>();
    }
    void GetMouseDown()
    {
        if (GetComponent<CardDisplay>().card.Type == Card.CardType.Unit)
        {
            if(GetComponent<CardDisplay>().card.TypeField == 'M')
            {
                MoveToM();
            }
            else if (GetComponent<CardDisplay>().card.TypeField == 'R')
            {
                MoveToR();
            }
            else if (GetComponent<CardDisplay>().card.TypeField == 'S')
            {
                MoveToS();
            }
        }
        else
        {
            if (gameObject.GetComponent<CardDisplay>().card.TypeSpecialCard == Card.SubTypeSpecialCard.Climate)
            {
                MoveToClimate();
            }
            else if (gameObject.GetComponent<CardDisplay>().card.TypeSpecialCard == Card.SubTypeSpecialCard.Increase)
            {
                MoveToIncrease();
            }
            else if (gameObject.GetComponent<CardDisplay>().card.TypeSpecialCard == Card.SubTypeSpecialCard.Clearance)
            {
                MoveToCemetery();
            }
            else if (gameObject.GetComponent<CardDisplay>().card.TypeSpecialCard == Card.SubTypeSpecialCard.Lure)
            {
                MoveToCemetery();
            }
        }
    }
    public void MoveToCemetery()
    {
        subBoard.Cemetery.Add(this.gameObject);

        this.gameObject.transform.SetParent(subBoard.transform, false);
    }
    public void MoveToM()
    {
        subBoard.M.Add(this.gameObject);
        this.gameObject.transform.SetParent(subBoard.transform, false);
    }
    public void MoveToR()
    {
        subBoard.R.Add(this.gameObject);
        this.gameObject.transform.SetParent(subBoard.transform, false);
    }
    public void MoveToS()
    {
        subBoard.S.Add(this.gameObject);
        this.gameObject.transform.SetParent(subBoard.transform, false);
    }
    public void MoveToIncrease()
    {
        subBoard.Increase[0] = this.gameObject;
        this.gameObject.transform.SetParent(subBoard.transform, false);
    }
    public void MoveToClimate()
    {
        subBoard.Climate = this.gameObject;
        this.gameObject.transform.SetParent(subBoard.transform, false);
    }
}
