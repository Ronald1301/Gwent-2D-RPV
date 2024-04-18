using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand : MonoBehaviour
{
    public GameObject deck;
    public List<GameObject> CardsInHand;
    public List<GameObject> CardsInDeck;


    void Start()
    {
        CardsInHand = new List<GameObject>();
        //deck= GameObject.FindGameObjectWithTag("Deck Pirates");
        CardsInDeck= deck.GetComponent<Decks>().deck;
        //CardsInDeck = new List<GameObject>();

        for (int i = 0; i < 10; i++)
        {
         System.Random index = new System.Random();
        int indexCard = index.Next(1, CardsInDeck.Count);
        //int indexCard= Random.Range(1, CardsInDeck.Count-1);
        GameObject drawCard = Instantiate(CardsInDeck[indexCard], new Vector3(i-4.5f, 0, 0), Quaternion.identity);
        //GameObject drawCard = CardsInDeck[indexCard];
        drawCard.transform.SetParent(this.transform, false);
        CardsInHand.Add(drawCard);
        CardsInDeck.RemoveAt(indexCard);
        }
        
    }
    internal bool CheckHand()
    {
        if (CardsInHand.Count == 0) return true;
        return false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
