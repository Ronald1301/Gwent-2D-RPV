using System.Collections;
using System.Collections.Generic;
//using UnityEditor.U2D;
using UnityEngine;

public class CemeteryZone : MonoBehaviour
{
    public List<GameObject> Cemetery;

    void Start()
    {
        Cemetery = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        
        for (int i = 0; i < Cemetery.Count; i++)
        {
            Cemetery[i].transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        }
        
        
    }

}
