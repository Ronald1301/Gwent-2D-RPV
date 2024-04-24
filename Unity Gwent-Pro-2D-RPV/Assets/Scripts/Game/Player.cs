using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
  public Decks deck;
  public SubBoard board;
  public Hand hand;
  public int Points_for_game;
  public int Points_for_round;
  public int RoundsWon;
 // public bool[] RoundsWon_for_game;
  public bool isPlaying;
  public bool PlayedACard;
  public bool passTurn; 
  public bool youWin;

  public Player(Decks decks, SubBoard board, Hand hand)
  {
    this.deck = decks;
    this.board = board;
    this.hand = hand;
    Points_for_game = 0;
    Points_for_round = 0;
    RoundsWon = 0;
   // RoundsWon_for_game = new bool[3];
    isPlaying = false;
    PlayedACard = false;
    passTurn = false;
    youWin = false;
  }
}
