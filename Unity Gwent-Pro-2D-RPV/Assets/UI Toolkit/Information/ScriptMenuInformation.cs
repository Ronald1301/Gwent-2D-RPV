using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ScriptMenuInformation : MonoBehaviour
{
    UIDocument MenuInformation;
    public GameObject StartMenu;


    private VisualElement LanguageSpanish;
    private VisualElement LanguageEnglish;
    private Button Spanish;
    private Button English;
    private Button back;
    private void OnEnable()
    {
        MenuInformation = GetComponent<UIDocument>();
        VisualElement root = MenuInformation.rootVisualElement;

        //References to the buttons
        LanguageSpanish = root.Q<VisualElement>("LanguageSpanish");
        LanguageEnglish = root.Q<VisualElement>("LanguageEnglish");
        Spanish = root.Q<Button>("Spanish");
        English = root.Q<Button>("English");
        back = root.Q<Button>("Back");

        //Callbacks
        Spanish.RegisterCallback<ClickEvent>(ChangeLanguageSpanish);
        English.RegisterCallback<ClickEvent>(ChangeLanguageEnglish);
        back.RegisterCallback<ClickEvent>(BackToStartMenu);
    }

    private void ChangeLanguageSpanish(ClickEvent evt)
    {
        LanguageEnglish.style.display = DisplayStyle.None;
        LanguageSpanish.style.display = DisplayStyle.Flex;
    }
    private void ChangeLanguageEnglish(ClickEvent evt)
    {
        LanguageSpanish.style.display = DisplayStyle.None;
        LanguageEnglish.style.display = DisplayStyle.Flex;
    }

    private void BackToStartMenu(ClickEvent evt)
    {
        StartMenu.SetActive(true);
        gameObject.SetActive(false);
    }
}
