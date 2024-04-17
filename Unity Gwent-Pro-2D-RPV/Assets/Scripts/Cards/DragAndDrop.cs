using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragAndDrop : MonoBehaviour
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

    // Update is called once per frame
    void Update()
    {
        
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

    private void OnMouseUp()
    {
        if (isOverDropZone)
        {
            transform.SetParent(meleeZone.transform, false);
            inTheField = true;
        }
        else
        {
            transform.position = startPosition;
        }
        //transform.position = new Vector3(0, 0, 0);

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        isOverDropZone = true;
        meleeZone = collision.gameObject;
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        isOverDropZone = false;
        meleeZone = null;
    }




}
