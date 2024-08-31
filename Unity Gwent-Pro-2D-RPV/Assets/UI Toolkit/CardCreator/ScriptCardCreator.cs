using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework.Internal;
using UnityEngine;
using UnityEngine.UIElements;
using Gwent;

public class ScriptCardCreator : MonoBehaviour
{
    UIDocument CardCreator;
    public GameObject StartMenu;
    SaveLoadSystem saveLoadSystem;

    private TextField BoxCode;
    private TextField BoxResult;
    private Button compile;
    private Button import;
    private Button export;
    private Button back;

    private void OnEnable()
    {
        CardCreator = GetComponent<UIDocument>();
        VisualElement root = CardCreator.rootVisualElement;

        BoxCode = root.Q<TextField>("Code");
        BoxResult = root.Q<TextField>("BoxResult");

        //References to the buttons
        compile = root.Q<Button>("Compile");
        import = root.Q<Button>("Import");
        export = root.Q<Button>("Export");
        back = root.Q<Button>("Back");

        //Callbacks
        compile.RegisterCallback<ClickEvent>(CompileCode);
        import.RegisterCallback<ClickEvent>(ImportCode);
        export.RegisterCallback<ClickEvent>(ExportCode);
        back.RegisterCallback<ClickEvent>(BackToStartMenu);

        saveLoadSystem = new SaveLoadSystem();
        saveLoadSystem.DataLoaded += OnDataLoaded;
        saveLoadSystem.LoadCode();

    }

    private void ExportCode(ClickEvent evt)
    {
        // this.gameObject.GetComponent<AudioSource>().Play();
        InfoCode infoCode = new InfoCode(BoxCode.value);
        saveLoadSystem.InfoCode.code = infoCode.code;
        saveLoadSystem.SaveCode();
    }

    private void ImportCode(ClickEvent evt)
    {
        // this.gameObject.GetComponent<AudioSource>().Play();
        saveLoadSystem.LoadCode();
    }

    private void OnDataLoaded()
    {
        BoxCode.value = saveLoadSystem.InfoCode.code;
    }

    private void CompileCode(ClickEvent evt)
    {
        // this.gameObject.GetComponent<AudioSource>().Play();
        Program program = new Program(BoxCode.value);
        program.CompileCode();
        BoxResult.value =program.PrintResult();
        BoxResult.style.visibility = Visibility.Visible;
    }

    private void BackToStartMenu(ClickEvent evt)
    {
        // this.gameObject.GetComponent<AudioSource>().Play();
        StartMenu.SetActive(true);
        gameObject.SetActive(false);
    }
}
