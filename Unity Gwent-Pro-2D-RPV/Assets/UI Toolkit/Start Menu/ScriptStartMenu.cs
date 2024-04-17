using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class ScriptStartMenu : MonoBehaviour
{
    UIDocument StartMenu;
    //public GameObject SelectDecks;
    public GameObject Options;
    public GameObject Information;

    private Button startGame;
    private Button options;
    private Button information;
    private Button exitGame;

    void Awake()
    {
       // SelectDecks.SetActive(false);
        Options.SetActive(false);
        Information.SetActive(false);
    }
    private void OnEnable()
    {
        StartMenu = GetComponent<UIDocument>();
        VisualElement root = StartMenu.rootVisualElement;

        //References to the buttons
        startGame = root.Q<Button>("StartGame");
        options = root.Q<Button>("Options");
        information = root.Q<Button>("Information");
        exitGame = root.Q<Button>("Exit");

        //Callbacks
        startGame.RegisterCallback<ClickEvent>(StartGame);
        options.RegisterCallback<ClickEvent>(OpenOptions);
        information.RegisterCallback<ClickEvent>(OpenInformation);
        exitGame.RegisterCallback<ClickEvent>(ExitGame);        
        
    }

    private void StartGame(ClickEvent evt)
    {
        SceneManager.LoadScene("GameScene");
    }

    /*
    private void OpenSelectDecks(ClickEvent evt)
    {
        SelectDecks.SetActive(true);
        gameObject.SetActive(false);
    }
    */

    private void OpenOptions(ClickEvent evt)
    {
        Options.SetActive(true);
        gameObject.SetActive(false);
    }

    private void OpenInformation(ClickEvent evt)
    {
        Information.SetActive(true);
        gameObject.SetActive(false);
    }

    private void ExitGame(ClickEvent evt)
    {
        Debug.Log("Game is exiting");
        Application.Quit();
    }
}