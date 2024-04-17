using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckField : MonoBehaviour
{
    [SerializeField] GameObject deckref;
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<SpriteRenderer>().sprite = deckref.GetComponent<SpriteRenderer>().sprite;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
