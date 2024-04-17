using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardDisplay : MonoBehaviour
{
    [SerializeField] Card card;
    //public GameObject UI;
    [SerializeField] new UICardDescription gameObject;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<SpriteRenderer>().sprite = card.CardImageForehead;
        //UI = GameObject.FindGameObjectWithTag("UI Card Description");
    }

    public void OnMouseEnter()
    {  
        //UI.SetActive(true);
       // UI.SendMessageUpwards("UIUpdate", card);   
        
        gameObject.gameObject.SetActive(true);
        gameObject.UIUpdate(card);
    }
    public void OnMouseExit()
    {
        //UI.SetActive(false);
        //UI.SendMessageUpwards("UIUpdate", null);
        
        gameObject.gameObject.SetActive(false);
        gameObject.UIUpdate(null);
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
