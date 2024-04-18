using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Player player1;
    public Player player2;
    public int currentRound;
    [SerializeField] ScriptUIRuntime UIRuntime;

    void Start()
    {
       // UIRuntime = new ScriptUIRuntime();
       UIRuntime = GameObject.Find("UIRuntime").GetComponent<ScriptUIRuntime>();
        StartGame();
    }
    // Update is called once per frame
    void Update()
    {
       // UIRuntime.UIUpdate();
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
    public void UpdatePoints(Card card)
    {
        if(player1.isPlaying)
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
        player2.Points_for_round[currentRound - 1] =player2.board.UpdatePoints();
    }

    public void StartGame()
    {
        player1 = new Player(new Decks(), new SubBoard(), new Hand());
        player2 = new Player(new Decks(), new SubBoard(), new Hand());
        currentRound = 1;
        player1.isPlaying = true;
    }

    public void ChangeTurn()
    {
        if (player1.isPlaying)
        {
            player1.isPlaying = false;
            player2.isPlaying = true;
        }
        else
        {
            player2.isPlaying = false;
            player1.isPlaying = true;
        }
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

            int index= Random.Range(1, 2);
            if (index == 1) player1.isPlaying = true;
            else player2.isPlaying = true;

            player1.passTurn = false;
            player2.passTurn = false;
            
        }
    }
    public void EndRound2()
    {
        if(player1.hand.CheckHand())
        {
            player1.passTurn = true;
        }
        if(player2.hand.CheckHand())
        {
            player2.passTurn = true;
        }
    }

    public void EndGame()
    {
        if (player1.RoundsWon ==2)
        {
            Debug.Log("Player 1 Wins");
        }
        else if (player2.RoundsWon==2)
        {
            Debug.Log("Player 2 Wins");
        }
        else
        {
            Debug.Log("Tied Game");
        }
    }

    public void ResetGame()
    {
        player1 = new Player(new Decks(), new SubBoard(), new Hand());
        player2 = new Player(new Decks(), new SubBoard(), new Hand());
        currentRound = 1;
        player1.isPlaying = true;
    }
}
