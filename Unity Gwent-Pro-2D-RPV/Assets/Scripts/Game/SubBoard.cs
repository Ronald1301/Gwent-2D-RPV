using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SubBoard : MonoBehaviour
{
    public GameObject Boss;
    public GameObject deckfield;
    public GameObject Hand;
    public GameObject M;
    public GameObject R;
    public GameObject S;
    public GameObject Increase;
    public GameObject Climate;
    public GameObject Cemetery;

    internal int UpdatePoints()
    {
        
        if (M.GetComponent<MeleeZone>().melee.Count == 0 ||
            R.GetComponent<RangedZone>().ranged.Count == 0 ||
            GetComponent<SiegeZone>().siege.Count == 0)
        {
            return 0;
        }
        
        int points = 0;
        for (int i = 0; i < M.GetComponent<MeleeZone>().melee.Count; i++)
        {
            points += M.GetComponent<MeleeZone>().melee[i].GetComponent<CardDisplay>().card.Power;
        }
        for (int i = 0; i < R.GetComponent<RangedZone>().ranged.Count; i++)
        {
            points += R.GetComponent<RangedZone>().ranged[i].GetComponent<CardDisplay>().card.Power;
        }
        for (int i = 0; i < S.GetComponent<SiegeZone>().siege.Count; i++)
        {
            points += S.GetComponent<SiegeZone>().siege[i].GetComponent<CardDisplay>().card.Power;
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
