using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
     public GameObject deck;
   // private GameObject card;
    void Start()
    {
        //card= deck.GetComponent<Decks>().deck[0];
        //card.transform.SetParent(this.transform, false);
        // Instantiate(card,card.position, Quaternion.identity);

        GameObject newcard = Instantiate(deck.GetComponent<Decks>().deck[0], new Vector3(0, 0, 0), Quaternion.identity);
       newcard.transform.SetParent(this.transform, false);
        Debug.Log("Listo");
    }

    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            Debug.Log("Click");
        }
    }
}
