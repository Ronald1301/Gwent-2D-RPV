using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckField : MonoBehaviour
{
    public GameObject deckref;
    public GameObject GameManager;
    // Start is called before the first frame update
    void Start()
    {
        /*
        if (this.gameObject.tag == "Deck Player1")
        {
            deckref = GameManager.GetComponent<GameManager>().player1.hand.deck;
        }
        else
        {
            deckref = GameManager.GetComponent<GameManager>().player2.hand.deck;
        }
        GetComponent<SpriteRenderer>().sprite = deckref.GetComponent<SpriteRenderer>().sprite;
        */
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
