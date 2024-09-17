using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR;

public class Decks : MonoBehaviour
{
    //[SerializeField] private Sprite ImageDeck;
    public string Name{get=>deck[0].GetComponent<CardDisplay>().cardData.Faction;}
    public List<GameObject> deck;
    //public List<GameObject> deckBoss;
    //public GameObject Hand;
    //public List<GameObject> Deck { get; set; }

    public GameObject this[int index]
    {
        get => deck[index];
        set => deck[index] = value;
    }
}
