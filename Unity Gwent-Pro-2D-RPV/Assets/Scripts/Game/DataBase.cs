using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataBase : MonoBehaviour
{
    public  List<Sprite> frontImages;
    public  List<Sprite> backImages;
    //public Dictionary<string, CardData> cards = new Dictionary<string, CardData>();
    public Dictionary<string, List<GameObject>> Decks;

    void Awake()
    {
        DontDestroyOnLoad(this);

        frontImages = new List<Sprite>();
        backImages = new List<Sprite>();
        Decks = new Dictionary<string, List<GameObject>>();
       //Decks.Add("Pirates", GameObject.FindGameObjectWithTag("Deck Pirates").GetComponent<Decks>().deck);
       //Decks.Add("Resistance", GameObject.FindGameObjectWithTag("Deck Resistance").GetComponent<Decks>().deck);
    }
}

