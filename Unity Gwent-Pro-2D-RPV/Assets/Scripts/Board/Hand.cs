using System.Diagnostics;
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
    public List<GameObject> CardsPriority;
    public GameObject GameManager;


    void Start()
    {

        deck = this.gameObject.CompareTag("Hand1") ? GameManager.GetComponent<GameManager>().player1.deck.gameObject : GameManager.GetComponent<GameManager>().player2.deck.gameObject;

        CardsInHand = new List<GameObject>();
        CardsInDeck = deck.GetComponent<Decks>().deck;

        DrawCard(10);

        /*
        for (int i = 0; i < 10; i++)
        {
            System.Random index = new System.Random();
            int indexCard = index.Next(1, CardsInDeck.Count);
            //int indexCard2= UnityEngine.Random.Range(1, CardsInDeck.Count-1);
            GameObject drawCard = Instantiate(CardsInDeck[indexCard], new Vector3(i - 4.8f, 1, 0), Quaternion.identity);
            //Mask[i] = true;
            drawCard.transform.localScale = new Vector3(0.4f, 0.6f, 0);
            //GameObject drawCard = CardsInDeck[indexCard];
            drawCard.transform.SetParent(this.transform, false);
            CardsInHand.Add(drawCard);
            CardsInDeck.RemoveAt(indexCard);
        }
        */
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

                GameObject drawCard;
                if (CardsPriority.Count > 0)
                {
                    drawCard = Instantiate(CardsPriority[^1], new Vector3(i - 4.8f, 1, 0), Quaternion.identity);
                    CardsPriority.RemoveAt(CardsPriority.Count - 1);
                }
                else
                {
                    drawCard = Instantiate(CardsInDeck[Convert.ToInt32(randomIndex)], new Vector3(i - 4.8f, 1, 0), Quaternion.identity);
                    //drawCard = Instantiate(CardsInDeck[CardsInDeck.Count - 1], new Vector3(i - 4.8f, 1, 0), Quaternion.identity);
                }

                drawCard.transform.localScale = new Vector3(0.4f, 0.6f, 0);
                drawCard.transform.SetParent(transform, false);
                if (CardsInHand.Count >= 10)
                {
                    drawCard.transform.localScale = new Vector3(1, 1, 1);
                    if (this.gameObject.CompareTag("Hand1"))
                    {
                        GameObject subBoard = GameObject.FindGameObjectWithTag("SubBoard1");

                        subBoard.GetComponent<SubBoard>().Cemetery.GetComponent<CemeteryZone>().Cemetery.Add(drawCard);
                        drawCard.transform.SetParent(subBoard.GetComponent<SubBoard>().Cemetery.transform, false);

                        //subBoard.GetComponent<SubBoard>().Cemetery.GetComponent<SpriteRenderer>().sprite = drawCard.GetComponent<SpriteRenderer>().sprite;
                        //Destroy(drawCard);
                    }
                    else
                    {
                        GameObject subBoard = GameObject.FindGameObjectWithTag("SubBoard2");

                        subBoard.GetComponent<SubBoard>().Cemetery.GetComponent<CemeteryZone>().Cemetery.Add(drawCard);
                        drawCard.transform.SetParent(subBoard.GetComponent<SubBoard>().Cemetery.transform, false);

                        //subBoard.GetComponent<SubBoard>().Cemetery.GetComponent<SpriteRenderer>().sprite = drawCard.GetComponent<SpriteRenderer>().sprite;
                        //Destroy(drawCard);
                    }
                }
                else
                {
                    CardsInHand.Add(drawCard);
                }
                CardsInDeck.RemoveAt(Convert.ToInt32(randomIndex));
                //CardsInDeck.RemoveAt(CardsInDeck.Count - 1);
            }
            /*
            else
            {
                UnityEngine.Debug.Log("El mazo está vacío. No se pueden sacar más cartas.");
                break;
            }
            */
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
                GameManager.GetComponent<GameManager>().clickcount++;

                int index = CardsInHand.IndexOf(card);
                System.Random indexCard = new System.Random();
                int indexCardDraw = indexCard.Next(1, CardsInDeck.Count);
                //int indexCardDraw = Random.Range(1, CardsInDeck.Count - 1);
                GameObject drawCard = Instantiate(CardsInDeck[indexCardDraw], new Vector3(card.transform.position.x, card.transform.position.y, 0), Quaternion.identity);
                drawCard.transform.localScale = new Vector3(0.4f, 0.6f, 0);
                drawCard.transform.SetParent(this.transform, false);
                CardsInHand[index] = drawCard;
                //CardsInHand.Add(drawCard);
                CardsInDeck.RemoveAt(indexCardDraw);
                CardsInDeck.Add(card);
                card.transform.SetParent(deck.transform, false);
                //Destroy(card);

            }
        }
    }

}
