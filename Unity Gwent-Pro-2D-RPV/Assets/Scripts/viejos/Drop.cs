using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Drop : MonoBehaviour,IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        GameObject Dropcard = eventData.pointerDrag;
        Dropcard.transform.SetParent( transform);
        Dropcard.transform.position = transform.position;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
