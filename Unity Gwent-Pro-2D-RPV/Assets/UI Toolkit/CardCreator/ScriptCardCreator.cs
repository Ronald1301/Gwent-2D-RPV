using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework.Internal;
using UnityEngine;
using UnityEngine.UIElements;
//using Gwent++;

public class ScriptCardCreator : MonoBehaviour
{
    UIDocument CardCreator;
    public GameObject StartMenu;

    private TextField code;
    private Button compile;
    private Button button;
    private Button import;
    private Button export;
    private Button back;

    private void OnEnable()
    {
        CardCreator = GetComponent<UIDocument>();
        VisualElement root = CardCreator.rootVisualElement;

        code = root.Q<TextField>("Code");

        //References to the buttons
        compile = root.Q<Button>("Compile");
        button = root.Q<Button>("Button");
        import = root.Q<Button>("Import");
        export = root.Q<Button>("Export");
        back = root.Q<Button>("Back");

        //Callbacks
        compile.RegisterCallback<ClickEvent>(CompileCode);
        import.RegisterCallback<ClickEvent>(ImportCode);
        export.RegisterCallback<ClickEvent>(ExportCode);
        back.RegisterCallback<ClickEvent>(BackToStartMenu);

    }

    private void ExportCode(ClickEvent evt)
    {

    }

    private void ImportCode(ClickEvent evt)
    {

    }

    private void CompileCode(ClickEvent evt)
    {
        Debug.Log(code.text);
    }

    private void BackToStartMenu(ClickEvent evt)
    {
        StartMenu.SetActive(true);
        gameObject.SetActive(false);
    }
}
