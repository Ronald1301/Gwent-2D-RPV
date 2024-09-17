using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class CardDisplay : MonoBehaviour
{
    public CardData cardData;
    //[SerializeField] GameObject GameManager;
    public UICardDescription UI;

    // Start is called before the first frame update
    void Start()
    {
        if(UI is null)UI = GameObject.FindGameObjectWithTag("UI Card Description").GetComponent<UICardDescription>();
        GetComponent<SpriteRenderer>().sprite = cardData.CardImageForehead;
       // UI = GameObject.Find("UI Card Description").AddComponent<UICardDescription>();
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
        if (GetComponent<SpriteRenderer>().sprite != cardData.CardImageForehead)
        {
            UI.gameObject.SetActive(false);
        }
        else
        {
            UI.gameObject.SetActive(true);
            UI.UIUpdateCardDescription(cardData);
        }

        //UI.gameObject.SetActive(true);
        //UI.UIUpdateCardDescription(card);
    }
    public void OnMouseExit()
    {
        UI.gameObject.SetActive(false);
    }
}
