using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataBase : MonoBehaviour
{
    public static DataBase instance;
    //public Dictionary<string, CardData> cards = new Dictionary<string, CardData>();
    public Dictionary<string, List<GameObject>> Decks = new Dictionary<string, List<GameObject>>
    {
        ["Pirates"] = GameObject.FindGameObjectWithTag("Deck Pirates").GetComponent<Decks>().deck,
        ["Resistance"] = GameObject.FindGameObjectWithTag("Deck Resistance").GetComponent<Decks>().deck
    };

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }
}
