using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeCamera : MonoBehaviour
{
    Camera mainCamera;
    public Camera camera1;
    public Camera camera2;

    GameObject GameManager;


    // Start is called before the first frame update
    void Start()
    {
        GameManager = GameObject.Find("GameManager");
        mainCamera = Camera.main;
        mainCamera = camera1;
        /*
        camera1 = GameManager.GetComponent<GameManager>().camera1;
        camera2 = GameManager.GetComponent<GameManager>().camera2;
        */
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            OnClick();
        }
    }

    public void OnClick()
    {
        if (mainCamera == camera1 )
        {
            GameManager.GetComponentInParent<GameManager>().player1.isPlaying = false;
            GameManager.GetComponentInParent<GameManager>().player2.isPlaying = true;
            mainCamera = camera2;
        }
        else
        {
            GameManager.GetComponentInParent<GameManager>().player2.isPlaying = false;
            GameManager.GetComponentInParent<GameManager>().player1.isPlaying = true;
            mainCamera = camera1;
        }
    }
}
