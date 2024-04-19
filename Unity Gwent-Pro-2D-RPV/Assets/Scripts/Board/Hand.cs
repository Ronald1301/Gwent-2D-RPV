using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand : MonoBehaviour
{
    public GameObject deck;
    public List<GameObject> CardsInHand;
    // public bool[] Mask = new bool[15];
    //public GameObject[] HandPosition = new GameObject[15];
    public List<GameObject> CardsInDeck;


    void Start()
    {
        CardsInHand = new List<GameObject>();
        //deck= GameObject.FindGameObjectWithTag("Deck Pirates");
        CardsInDeck = deck.GetComponent<Decks>().deck;
        //CardsInDeck = new List<GameObject>();
        for (int i = 0; i < 10; i++)
        {
            System.Random index = new System.Random();
            int indexCard = index.Next(1, CardsInDeck.Count);
            //int indexCard= Random.Range(1, CardsInDeck.Count-1);
            GameObject drawCard = Instantiate(CardsInDeck[indexCard], new Vector3(i - 4.8f, 1, 0), Quaternion.identity);
            //Mask[i] = true;
            drawCard.transform.localScale = new Vector3(0.4f, 0.6f, 0);
            //GameObject drawCard = CardsInDeck[indexCard];
            drawCard.transform.SetParent(this.transform, false);
            CardsInHand.Add(drawCard);
            CardsInDeck.RemoveAt(indexCard);
        }
    }

    public void DrawCard(int count)
    {
        for (int i = 0; i < count; i++)
        {
            System.Random index = new System.Random();
            int indexCard = index.Next(1, CardsInDeck.Count);
            //int indexCard= Random.Range(1, CardsInDeck.Count-1);
            GameObject drawCard = Instantiate(CardsInDeck[indexCard], new Vector3(i - 4.8f, 1, 0), Quaternion.identity);
            //Mask[i] = true;
            drawCard.transform.localScale = new Vector3(0.4f, 0.6f, 0);
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

    internal void ChangeCard()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);
        if (hit.collider != null)
        {
            GameObject card = hit.collider.gameObject;
            if (CardsInHand.Contains(card))
            {
                int index = CardsInHand.IndexOf(card);
                System.Random indexCard = new System.Random();
                int indexCardDraw = indexCard.Next(1, CardsInDeck.Count);
                //int indexCardDraw = Random.Range(1, CardsInDeck.Count - 1);
                GameObject drawCard = Instantiate(CardsInDeck[indexCardDraw], new Vector3(card.transform.position.x, card.transform.position.y, 0), Quaternion.identity);
                drawCard.transform.localScale = new Vector3(0.4f, 0.6f, 0);
                drawCard.transform.SetParent(this.transform, false);
                CardsInHand[index] = drawCard;
                CardsInDeck.RemoveAt(indexCardDraw);
                CardsInDeck.Add(card);
                Destroy(card);
            }
        }
    }

}
