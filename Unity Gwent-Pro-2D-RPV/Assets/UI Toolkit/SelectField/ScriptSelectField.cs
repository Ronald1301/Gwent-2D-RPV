using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ScriptSelectField : MonoBehaviour
{
    UIDocument UIDocument;
    public Button M;
    public Button R;
    public Button S;

    public char selected = '\0';

    void OnEnable()
    {
        UIDocument = GetComponent<UIDocument>();
        VisualElement root = UIDocument.rootVisualElement;

        M = root.Q<Button>("M");
        R = root.Q<Button>("R");
        S = root.Q<Button>("S");

        //Callbacks
        M.RegisterCallback<ClickEvent>(MClick);
        R.RegisterCallback<ClickEvent>(RClick);
        S.RegisterCallback<ClickEvent>(SClick);
    }

    private void SClick(ClickEvent evt)
    {
        selected = 'S';
        R.style.display = DisplayStyle.Flex;
        M.style.display = DisplayStyle.Flex;
        this.gameObject.SetActive(false);
    }

    private void RClick(ClickEvent evt)
    {
        selected = 'R';
        S.style.display = DisplayStyle.Flex;
        M.style.display = DisplayStyle.Flex;
        this.gameObject.SetActive(false);
    }

    private void MClick(ClickEvent evt)
    {
        selected = 'M';
        S.style.display = DisplayStyle.Flex;
        R.style.display = DisplayStyle.Flex;
        this.gameObject.SetActive(false);
    }
    public void LoadSelectField(CardData.EnumRange range)
    {
        if (range == CardData.EnumRange.MR)
        {
           S.style.display = DisplayStyle.None;
        }
        else if (range == CardData.EnumRange.MS)
        {
            R.style.display = DisplayStyle.None;
        }
        else if (range == CardData.EnumRange.RS)
        {
            M.style.display = DisplayStyle.None;
        }
    }
   
}
