using UnityEngine;
using UnityEngine.UIElements;
using Gwent;
using UnityEngine.SceneManagement;
using UnityEditor.PackageManager;
using UnityEditor;
using System;
public class ScriptCardCreator : MonoBehaviour
{
    UIDocument CardCreator;
    // [SerializeField] private GameObject StartMenu;

    [SerializeField] private SaveLoadSystem saveLoadSystem;

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

        //saveLoadSystem.DataLoaded += OnDataLoaded;
    }

    private void ExportCode(ClickEvent evt)
    {
        saveLoadSystem.infoCode.Code = BoxCode.value;
        saveLoadSystem.SaveCode();
    }

    private void ImportCode(ClickEvent evt)
    {
        saveLoadSystem.LoadCode();
        BoxCode.value = saveLoadSystem.infoCode.Code;
    }

    private void OnDataLoaded()
    {
        BoxCode.value = saveLoadSystem.infoCode.Code;
    }

    private void CompileCode(ClickEvent evt)
    {
        InfoCode infoCode = new InfoCode(BoxCode.value);

        try
        {
             EngineCompiler.CompileCode(infoCode.Code);
        }
        catch (System.Exception e)
        {
            EngineCompiler.error.argument = e.Message;
            PrintError(EngineCompiler.error);
            return;
        }
       
        BoxResult.value = EngineCompiler.PrintResult();
        BoxResult.style.visibility = Visibility.Visible;
    }

    private void BackToStartMenu(ClickEvent evt)
    {
        SceneManager.LoadScene("StartMenuScene");
        // StartMenu.SetActive(true);
        //gameObject.SetActive(false);
    }

    public void UpdateJson()
    {
        saveLoadSystem.infoCode.Code = BoxCode.value;
        string aux = saveLoadSystem.LoadJson();
        saveLoadSystem.SaveJson(aux);
    }
    public void LoadJson()
    {
        saveLoadSystem.LoadCode();
        BoxCode.value = saveLoadSystem.infoCode.Code;
    }
    public void PrintError(Gwent.Error error)
    {
        EditorUtility.DisplayDialog("Error", error.Text(), "Ok");

    }
}
