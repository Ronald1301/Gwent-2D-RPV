using System.Collections;
using System.Collections.Generic;
//using UnityEditor.U2D;
using UnityEngine;

public class CemeteryZone : MonoBehaviour
{
    public List<GameObject> Cemetery;
    // Start is called before the first frame update
    void Start()
    {
        Cemetery = new List<GameObject>();
        //gameObject.GetComponent<SpriteRenderer>().enabled = false;

    }

    // Update is called once per frame
    void Update()
    {
        if (Cemetery.Count > 0)
        {
            //gameObject.GetComponent<SpriteRenderer>().enabled = true;
            Cemetery[Cemetery.Count-1].transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z - 0.1f);
            gameObject.GetComponent<SpriteRenderer>().sprite = Cemetery[Cemetery.Count-1].GetComponent<SpriteRenderer>().sprite;

        }
    }
}
