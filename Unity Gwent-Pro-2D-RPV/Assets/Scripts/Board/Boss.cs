using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Boss : MonoBehaviour
{
    public GameObject GameManager;
    public GameObject deck;
    // private GameObject card;
    void Start()
    {
        if(gameObject.tag == "Boss Zone1")
        {
            deck = GameManager.GetComponent<GameManager>().player1.deck.gameObject;
        }
        else
        {
            deck = GameManager.GetComponent<GameManager>().player2.deck.gameObject;
        }
        //card= deck.GetComponent<Decks>().deck[0];
        //card.transform.SetParent(this.transform, false);
        // Instantiate(card,card.position, Quaternion.identity);

        GameObject newcard = Instantiate(deck.GetComponent<Decks>().deck[0], new Vector3(0, 0, 0), Quaternion.identity);
        newcard.transform.SetParent(this.transform, false);
        //Debug.Log("Listo");
    }

    void OnMouseDown()
    {
        Effects.ActivateEffect(gameObject);
        if (GameObject.Find("GameManager").GetComponent<GameManager>().player1.isPlaying)
        {
            GameObject.Find("GameManager").GetComponent<GameManager>().activeboss1 = true;
        }
        else
        {
            GameObject.Find("GameManager").GetComponent<GameManager>().activeboss2 = true;
        }

    }
}
