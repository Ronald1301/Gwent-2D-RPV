using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ScriptMenuInformation : MonoBehaviour
{
    UIDocument MenuInformation;
    public GameObject StartMenu;

    private Button back;

    private void OnEnable()
    {
        MenuInformation = GetComponent<UIDocument>();
        VisualElement root = MenuInformation.rootVisualElement;

        //References to the buttons
        back = root.Q<Button>("Back");

        //Callbacks
        back.RegisterCallback<ClickEvent>(BackToStartMenu);
    }

    private void BackToStartMenu(ClickEvent evt)
    {
        StartMenu.SetActive(true);
        gameObject.SetActive(false);
    }
}
