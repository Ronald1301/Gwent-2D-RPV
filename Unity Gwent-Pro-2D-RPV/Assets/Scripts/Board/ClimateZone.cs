using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClimateZone : MonoBehaviour
{
    public GameObject climate;

    void Star()
    {
        //climate = new GameObject();
    }

    void Update()
    {
        if (climate == null || climate == false)
        {
            climate = GameObject.FindGameObjectWithTag("ClimateCard");
        }
    }
}
