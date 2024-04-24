using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SubBoard : MonoBehaviour
{
    public GameObject Boss;
    public GameObject DeckField;
    public GameObject Hand;
    public GameObject M;
    public GameObject R;
    public GameObject S;
    public GameObject Increase;
    public GameObject Climate;
    public GameObject Cemetery;


    internal int UpdatePoints()
    {
        int points = 0;
        if (!Effects.IsRowEMpty(1))
        {
            for (int i = 0; i < M.GetComponent<MeleeZone>().melee.Count; i++)
            {
                points += M.GetComponent<MeleeZone>().melee[i].GetComponent<CardDisplay>().card.Power;
            }
        }
        if (!Effects.IsRowEMpty(2))
        {
            for (int i = 0; i < R.GetComponent<RangedZone>().ranged.Count; i++)
            {
                points += R.GetComponent<RangedZone>().ranged[i].GetComponent<CardDisplay>().card.Power;
            }
        }
        if (!Effects.IsRowEMpty(3))
        {
            for (int i = 0; i < S.GetComponent<SiegeZone>().siege.Count; i++)
            {
                points += S.GetComponent<SiegeZone>().siege[i].GetComponent<CardDisplay>().card.Power;
            }
        }
        return points;
    }


    //public Card Leader { get; set; }
    /*
    public Board(Decks decks)
    {
        this.deck = decks;
        Cemetery = new();
        M = new();
        R = new();
        S = new();
    }
    */
}
