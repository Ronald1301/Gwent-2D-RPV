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
        
        if (this.gameObject.CompareTag("Deck Player1") && ((GameManager.GetComponent<GameManager>().player1 == GameObject.FindGameObjectWithTag("Player1").GetComponent<Player>()) || (GameManager.GetComponent<GameManager>().player1 == GameObject.FindGameObjectWithTag("Player4").GetComponent<Player>())))
        {
            deckref = GameObject.FindGameObjectWithTag("Deck Pirates");
        }
        else if (this.gameObject.CompareTag("Deck Player1") && ((GameManager.GetComponent<GameManager>().player1 == GameObject.FindGameObjectWithTag("Player2").GetComponent<Player>()) || (GameManager.GetComponent<GameManager>().player1 == GameObject.FindGameObjectWithTag("Player3").GetComponent<Player>())))
        {
            deckref = GameObject.FindGameObjectWithTag("Deck Resistance");
            
        }
        else if (this.gameObject.CompareTag("Deck Player2") && ((GameManager.GetComponent<GameManager>().player2 == GameObject.FindGameObjectWithTag("Player2").GetComponent<Player>()) || (GameManager.GetComponent<GameManager>().player2 == GameObject.FindGameObjectWithTag("Player3").GetComponent<Player>())))
        {
            deckref = GameObject.FindGameObjectWithTag("Deck Resistance");
            
        }
        else if (this.gameObject.CompareTag("Deck Player2") && ((GameManager.GetComponent<GameManager>().player2 == GameObject.FindGameObjectWithTag("Player1").GetComponent<Player>()) || (GameManager.GetComponent<GameManager>().player2 == GameObject.FindGameObjectWithTag("Player4").GetComponent<Player>())))
        {
            deckref = GameObject.FindGameObjectWithTag("Deck Pirates");
        }
        GetComponent<SpriteRenderer>().sprite = deckref.GetComponent<SpriteRenderer>().sprite;

    }

    // Update is called once per frame
    void Update()
    {
        //GetComponent<SpriteRenderer>().sprite = deckref.GetComponent<SpriteRenderer>().sprite;
    }
}
