using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dragnew : MonoBehaviour
{
    public bool inTheField;
    public bool isOverDropZone;
    public GameObject meleeZone;
    public Vector2 startPosition;
    // Start is called before the first frame update
    void Start()
    {
        inTheField = false;
        isOverDropZone = false;
    }
    private void OnMouseDrag()
    {
        if (!inTheField)
        {
            startPosition = transform.position;
            Vector3 mousePosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10);
            Vector3 objPosition = Camera.main.ScreenToWorldPoint(mousePosition);
            transform.position = objPosition;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
