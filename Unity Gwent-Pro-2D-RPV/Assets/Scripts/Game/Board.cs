using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    public Decks deck;
    public List<Card> Cemetery;
    public List<Card> Body_To_Body;
    public List<Card> Distance;
    public List<Card> Siege;

    //public Card Leader { get; set; }
    public Board(Decks decks)
    {
        this.deck = decks;
        Cemetery = new();
        Body_To_Body = new();
        Distance = new();
        Siege = new();
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
