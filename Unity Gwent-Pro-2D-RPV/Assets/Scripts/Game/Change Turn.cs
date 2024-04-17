using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeTurn : MonoBehaviour
{
    /*
    GameObject camera1;
    GameObject camera2;

    bool active1 = false;
    bool active2 = false;
    */
    // Start is called before the first frame update
    void Start()
    {
        /*
        camera1 = GameObject.FindGameObjectWithTag("Camera Player1");
        camera2 = GameObject.FindGameObjectWithTag("Camera Player2");
        active1 = true;
        */
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnClick()
    {
        Camera.main.transform.Rotate(0, 180, 0);
        /*
        if (active1)
        {
            camera2.SetActive(true);
            camera1.SetActive(false);
        }
        else 
        camera1.SetActive(true);
        camera2.SetActive(false);
            */
    }
}
