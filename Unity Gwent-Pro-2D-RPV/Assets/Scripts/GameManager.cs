using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Player player1;
    public Player player2;
    public int currentRound;
    public bool activeboss1;
    public bool activeboss2;

    public bool Changecards1;
    public bool Changecards2;

    private int clickcount;
    GameObject UIRuntime;

    public GameObject MainBoard;


    void Start()
    {
        // UIRuntime = new ScriptUIRuntime();
        //UIRuntime = GameObject.Find("UI Runtime");
        UIRuntime = GameObject.FindGameObjectWithTag("UI Runtime");
        Debug.Log(UIRuntime);
        StartGame();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            clickcount++;
            if (clickcount <= 2)
            {
                if (player1.isPlaying && !Changecards1)
                {
                    player1.hand.ChangeCard();
                    if (clickcount == 2) Changecards1 = true;
                }
                else if (player2.isPlaying && !Changecards2)
                {
                    player2.hand.ChangeCard();
                    if (clickcount == 2) Changecards2 = true;
                }
            }
        }

        //ChangLayerCardsinHand();
        UIRuntime.GetComponent<ScriptUIRuntime>().UIUpdate();
        //gameObject.SendMessageUpwards("UIUpdate");
        UpdatePoints();
        EndRound2();

    }
    
    public void LateUpdate()
    {
        if (player1.passTurn && player2.passTurn)
        {
            EndRound();
        }
        if (currentRound == 4)
        {
            EndGame();
        }
    }


    /*
        private void ChangLayerCardsinHand()
        {
            if (player1.isPlaying)
            {
                foreach (GameObject card in player2.hand.CardsInHand)
                {
                    card.GetComponent<SpriteRenderer>().sortingOrder = 0;
                   // card.layer = 8;
                }
                foreach (GameObject card in player1.hand.CardsInHand)
                {
                    card.GetComponent<SpriteRenderer>().sortingOrder = 1000;
                    //card.layer = 1000;
                }
            }
            else
            {
                foreach (GameObject card in player1.hand.CardsInHand)
                {
                    card.GetComponent<SpriteRenderer>().sortingOrder = 0;
                    //card.layer = 8;
                }
                foreach (GameObject card in player2.hand.CardsInHand)
                {
                    card.GetComponent<SpriteRenderer>().sortingOrder = 1000;
                   // card.layer = 1000;
                }
            }
        }
        */

    public void StartGame()
    {
        /*
        player1 = gameObject.AddComponent<Player>();
        player2 = gameObject.AddComponent<Player>();
        */
        player1 = new Player(GameObject.FindGameObjectWithTag("Deck Pirates").GetComponent<Decks>(),
            GameObject.FindGameObjectWithTag("SubBoard1").GetComponent<SubBoard>(),
            GameObject.FindGameObjectWithTag("Hand1").GetComponent<Hand>());
        player2 = new Player(GameObject.FindGameObjectWithTag("Deck Resistance").GetComponent<Decks>(),
            GameObject.FindGameObjectWithTag("SubBoard2").GetComponent<SubBoard>(),
            GameObject.FindGameObjectWithTag("Hand2").GetComponent<Hand>());

        /*
        player1 = new Player();
        player2 = new Player();

        player1.deck = GameObject.FindGameObjectWithTag("Deck Pirates").GetComponent<Decks>();
        player1.board = GameObject.FindGameObjectWithTag("SubBoard1").GetComponent<SubBoard>();
        player1.hand = GameObject.FindGameObjectWithTag("Hand1").GetComponent<Hand>();
        player2.deck = GameObject.FindGameObjectWithTag("Deck Resistance").GetComponent<Decks>();
        player2.board = GameObject.FindGameObjectWithTag("SubBoard2").GetComponent<SubBoard>();
        player2.hand = GameObject.FindGameObjectWithTag("Hand2").GetComponent<Hand>();
        */
        currentRound = 1;
        player1.isPlaying = true;

    }
    

    public void UpdatePoints(Card card)
    {
        if (player1.isPlaying)
        {
            player1.Points_for_round[currentRound - 1] = card.Power;
        }
        else
        {
            player2.Points_for_round[currentRound - 1] = card.Power;
        }
    }

    public void UpdatePoints()
    {
        player1.Points_for_round[currentRound - 1] = player1.board.UpdatePoints();
        player2.Points_for_round[currentRound - 1] = player2.board.UpdatePoints();
    }


    public void ChangeTurn()
    {
        clickcount = 0;
        if (player1.isPlaying)
        {
            Changecards1 = true;
            player1.isPlaying = false;
            player2.isPlaying = true;
        }
        else
        {
            Changecards2 = true;
            player2.isPlaying = false;
            player1.isPlaying = true;
        }
        MainBoard.transform.Rotate(0, 0, 180);
    }

    public void EndRound()
    {
        if (player1.Points_for_round[currentRound - 1] > player2.Points_for_round[currentRound - 1])
        {
            player1.RoundsWon_for_game[currentRound - 1] = true;
            player1.RoundsWon++;
            currentRound++;

            player1.Points_for_game += player1.Points_for_round[currentRound - 1];
            player2.Points_for_game += player2.Points_for_round[currentRound - 1];

            player1.isPlaying = true;

            player1.passTurn = false;
            player2.passTurn = false;
        }
        else if (player1.Points_for_round[currentRound - 1] < player2.Points_for_round[currentRound - 1])
        {
            player2.RoundsWon_for_game[currentRound - 1] = true;
            player2.RoundsWon++;
            currentRound++;

            player1.Points_for_game += player1.Points_for_round[currentRound - 1];
            player2.Points_for_game += player2.Points_for_round[currentRound - 1];

            player2.isPlaying = true;

            player1.passTurn = false;
            player2.passTurn = false;
        }
        else
        {
            player1.RoundsWon_for_game[currentRound - 1] = true;
            player1.RoundsWon++;
            player2.RoundsWon_for_game[currentRound - 1] = true;
            player2.RoundsWon++;
            currentRound++;

            player1.Points_for_game += player1.Points_for_round[currentRound - 1];
            player2.Points_for_game += player2.Points_for_round[currentRound - 1];

            int index = Random.Range(1, 2);
            if (index == 1) player1.isPlaying = true;
            else player2.isPlaying = true;

            player1.passTurn = false;
            player2.passTurn = false;

        }
    }



    public void EndRound2()
    {
        if (player1.hand.CheckHand())
        {
            player1.passTurn = true;
        }
        if (player2.hand.CheckHand())
        {
            player2.passTurn = true;
        }
    }



    public void EndGame()
    {
        if (player1.RoundsWon == 2)
        {
            Debug.Log("Player 1 Wins");
        }
        else if (player2.RoundsWon == 2)
        {
            Debug.Log("Player 2 Wins");
        }
        else
        {
            Debug.Log("Tied Game");
        }
    }


    /*
    public void ResetGame()
    {
        player1 = new Player();
        player2 = new Player();
        currentRound = 1;
        player1.isPlaying = true;
    }
    */

}

