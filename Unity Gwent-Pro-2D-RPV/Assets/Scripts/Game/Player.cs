using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player 
{
    public Decks deck;
    public SubBoard board;
    public Hand hand;
    public int Points_for_round = 0;
    public int Points_for_game = 0;
    //public bool isPlaying ;

    public Player(Decks deck,SubBoard board,Hand hand)
    {
        this.deck = deck;
        this.board = board;
        this.hand = hand;
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
