using System.Collections;
using System.Collections.Generic;
using UnityEditor.U2D;
using UnityEngine;

public class CemeteryZone : MonoBehaviour
{
    public List<GameObject> Cemetery;
    // Start is called before the first frame update
    void Start()
    {
        //gameObject.GetComponent<SpriteRenderer>().enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Cemetery.Count>0)
        {
            //gameObject.GetComponent<SpriteRenderer>().enabled = true;
            gameObject.GetComponent<SpriteRenderer>().sprite=Cemetery[Cemetery.Count].GetComponent<SpriteRenderer>().sprite;
        }
    }
}
