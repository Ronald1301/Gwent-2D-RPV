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
        deckref = this.gameObject.CompareTag("Deck Player1") ? 
        GameManager.GetComponent<GameManager>().player1.deck.gameObject : 
        GameManager.GetComponent<GameManager>().player2.deck.gameObject;

        GetComponent<SpriteRenderer>().sprite = deckref.GetComponent<SpriteRenderer>().sprite;
    }

    // Update is called once per frame
    void Update()
    {
        //GetComponent<SpriteRenderer>().sprite = deckref.GetComponent<SpriteRenderer>().sprite;
    }
}
