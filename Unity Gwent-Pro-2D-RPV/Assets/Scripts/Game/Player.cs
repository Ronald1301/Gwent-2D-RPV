using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player 
{
    public Decks deck;
    public SubBoard board;
    public Hand hand;
    public int Points_for_game;
    public int[] Points_for_round ;
    public int RoundsWon;
    public bool[] RoundsWon_for_game;
    public bool isPlaying;
    public bool passTurn ;

    public Player(Decks deck,SubBoard board,Hand hand)
    {
        this.deck = deck;
        this.board = board;
        this.hand = hand;
        Points_for_game = 0;
        Points_for_round = new int[3];
        RoundsWon =0;
        RoundsWon_for_game = new bool[3];
        isPlaying = false;
        passTurn = false;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
