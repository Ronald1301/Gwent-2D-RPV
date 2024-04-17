using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CementeryCards : MonoBehaviour
{    public List<GameObject> cementeryCards { get; set; }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //GetComponent<SpriteRenderer>().sprite = cementeryCards[cementeryCards.Count - 1].GetComponent<SpriteRenderer>().sprite;
    }
}
