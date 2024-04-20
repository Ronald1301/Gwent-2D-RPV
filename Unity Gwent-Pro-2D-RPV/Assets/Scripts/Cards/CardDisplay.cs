using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardDisplay : MonoBehaviour
{
    public Card card;
    //public GameObject UI;
    [SerializeField] UICardDescription UI;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<SpriteRenderer>().sprite = card.CardImageForehead;
        //UI = GameObject.FindGameObjectWithTag("UI Card Description");
    }

    public void OnMouseEnter()
    {
        //if ((GetComponent<GameManager>().player1.isPlaying && GetComponent<GameManager>().player2.hand.CardsInHand.Contains(gameObject)) || (GetComponent<GameManager>().player2.isPlaying && GetComponent<GameManager>().player1.hand.CardsInHand.Contains(gameObject)))
        
          //  UI.gameObject.SetActive(false);
        
        
        
            //UI.SetActive(true);
            // UI.SendMessageUpwards("UIUpdate", card);   
            //var newposition = new Vector3(transform.position.x+1, transform.position.y, transform.position.z - 1);
            //gameObject.transform.position = newposition;
            UI.gameObject.SetActive(true);
            UI.UIUpdateCardDescription(card);
        
    }
    public void OnMouseExit()
    {
        //UI.SetActive(false);
        //UI.SendMessageUpwards("UIUpdate", null);

        UI.gameObject.SetActive(false);
        //gameObject.UIUpdateCardDescription(null);
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
