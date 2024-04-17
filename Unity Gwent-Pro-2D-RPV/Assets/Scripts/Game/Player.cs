using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    public Board board;
    public List<Card> Hand;
    public int Points_for_round = 0;
    public int Points_for_game = 0;
    public bool isPlaying ;

    public Player(Board board)
    {
        this.board = board;
        Hand = new();
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
