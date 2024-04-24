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
    public GameObject GameManager;


    void Start()
    {

        if (this.gameObject.CompareTag("Hand1") && ((GameManager.GetComponent<GameManager>().player1 == GameObject.FindGameObjectWithTag("Player1").GetComponent<Player>()) || (GameManager.GetComponent<GameManager>().player1 == GameObject.FindGameObjectWithTag("Player4").GetComponent<Player>())))
        {
            deck = GameObject.FindGameObjectWithTag("Deck Pirates");
        }
        else if (this.gameObject.CompareTag("Hand1") && ((GameManager.GetComponent<GameManager>().player1 == GameObject.FindGameObjectWithTag("Player2").GetComponent<Player>()) || (GameManager.GetComponent<GameManager>().player1 == GameObject.FindGameObjectWithTag("Player3").GetComponent<Player>())))
        {
            deck = GameObject.FindGameObjectWithTag("Deck Resistance");
        }
        else if (this.gameObject.CompareTag("Hand2") && ((GameManager.GetComponent<GameManager>().player2 == GameObject.FindGameObjectWithTag("Player2").GetComponent<Player>()) || (GameManager.GetComponent<GameManager>().player2 == GameObject.FindGameObjectWithTag("Player3").GetComponent<Player>())))
        {
            deck = GameObject.FindGameObjectWithTag("Deck Resistance");
        }
        else //if (this.gameObject.CompareTag("Hand2") && ((GameManager.GetComponent<GameManager>().player2 == GameObject.FindGameObjectWithTag("Player1").GetComponent<Player>()) || (GameManager.GetComponent<GameManager>().player2 == GameObject.FindGameObjectWithTag("Player4").GetComponent<Player>())))
        {
            deck = GameObject.FindGameObjectWithTag("Deck Pirates");
        }

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
    /*
        public void DrawCard(int count)
        {
            for (int i = 0; i < count; i++)
            {
                //System.Random index = new System.Random();
                //long indexCard = index.Next(1, CardsInDeck.Count);
                long indexCard= UnityEngine.Random.Range(1, CardsInDeck.Count-1);
                GameObject drawCard = Instantiate(CardsInDeck[Convert.ToInt32(indexCard)], new Vector3(i - 4.8f, 1, 0), Quaternion.identity);
                //Mask[i] = true;
                drawCard.transform.localScale = new Vector3(0.4f, 0.6f, 0);
                //GameObject drawCard = CardsInDeck[indexCard];
                drawCard.transform.SetParent(transform, false);
                CardsInHand.Add(drawCard);
                CardsInDeck.RemoveAt(Convert.ToInt32(indexCard));
            }
        }
        */

    public void DrawCard(int count)
    {
        for (int i = 0; i < count; i++)
        {
            if (CardsInDeck.Count > 0)
            {
                int randomIndex = new System.Random().Next(1, CardsInDeck.Count);
                GameObject drawCard = Instantiate(CardsInDeck[Convert.ToInt32(randomIndex)], new Vector3(i - 4.8f, 1, 0), Quaternion.identity);
                drawCard.transform.localScale = new Vector3(0.4f, 0.6f, 0);
                drawCard.transform.SetParent(transform, false);
                CardsInHand.Add(drawCard);
                CardsInDeck.RemoveAt(Convert.ToInt32(randomIndex));
            }
            else
            {
                Debug.Log("El mazo está vacío. No se pueden sacar más cartas.");
                break;
            }
        }
    }


    internal bool CheckHand()
    {
        if (CardsInHand.Count == 0) return true;
        /*
        for(int i = 0; i < CardsInHand.Count; i++)
        {
            if (CardsInHand[i] == null) return false;
        }
        */
        return false;
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
                CardsInHand.Add(drawCard);
                CardsInDeck.RemoveAt(indexCardDraw);
                CardsInDeck.Add(card);
                Destroy(card);
            }
        }
    }

}
