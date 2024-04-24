using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardDisplay : MonoBehaviour
{
    public Card card;
    //[SerializeField] GameObject GameManager;
    [SerializeField] UICardDescription UI;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<SpriteRenderer>().sprite = card.CardImageForehead;
        //UI = GameObject.FindGameObjectWithTag("UI Card Description");
    }
    void Update()
    {
        /*
        if (card.inTheField)
        {
            GetComponent<SpriteRenderer>().sprite = card.CardImageForehead;
        }
        else if (!card.inTheField && GameManager.GetComponent<GameManager>().player1.isPlaying && GetComponent<GameManager>().player2.hand.CardsInHand.Contains(this.gameObject))
        {
            GetComponent<SpriteRenderer>().sprite = card.CardImageBack;
        }
        else if (!card.inTheField && GameManager.GetComponent<GameManager>().player2.isPlaying && GetComponent<GameManager>().player1.hand.CardsInHand.Contains(this.gameObject))
        {
            GetComponent<SpriteRenderer>().sprite = card.CardImageBack;
        }
        else
        {
            GetComponent<SpriteRenderer>().sprite = card.CardImageForehead;
        }
        */
    }

    public void OnMouseEnter()
    {
        if(GetComponent<SpriteRenderer>().sprite != card.CardImageForehead)
        {
            UI.gameObject.SetActive(false);
        }
        else
        {
            UI.gameObject.SetActive(true);
            UI.UIUpdateCardDescription(card);
        }

        //UI.gameObject.SetActive(true);
        //UI.UIUpdateCardDescription(card);
    }
    public void OnMouseExit()
    {
        UI.gameObject.SetActive(false);
    }


    /*
    public void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(1))
        {
            Destroy(gameObject);
        }
    }
    */
    /*
    private void OnMouseUpAsButton()
    {
        Debug.Log("Click");
    }
    */
}
