using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR;

public class Decks : MonoBehaviour
{
    //[SerializeField] private Sprite ImageDeck;
    [SerializeField] public List<GameObject> deck;
    //public GameObject Hand;
    //public List<GameObject> Deck { get; set; }

    public GameObject this[int index]
    {
        get => deck[index];
        set => deck[index] = value;
    }

    // Start is called before the first frame update
    void Start()
    {
        //deck = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
