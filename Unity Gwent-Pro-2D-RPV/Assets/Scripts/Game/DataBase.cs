using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gwent;

public class DataBase : MonoBehaviour
{
    public DataBaseData dataBaseData;

    //public static DataBase instance;

    void Awake()
    {
        dataBaseData.Decks = new List<GameObject>();

        dataBaseData.Decks.Add(GameObject.Find("Deck Pirates"));
        dataBaseData.Decks.Add(GameObject.Find("Deck Resistance"));
        dataBaseData.effectsCompiled = new Dictionary<(string, string), (EffectComplete, SelectorExpression)>();
        dataBaseData.cardsCompiled = new Dictionary<string, DataCardComplete>();
        /*
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        */
    }

}
