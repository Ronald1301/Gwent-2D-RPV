using System.Globalization;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class ScriptStartMenu : MonoBehaviour
{
    UIDocument StartMenu;
    public GameObject CardCreator;
    public GameObject Options;
    public GameObject Information;

    private Button startGame;
    private Button cardCreator;
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
        cardCreator = root.Q<Button>("CardCreator");
        options = root.Q<Button>("Options");
        information = root.Q<Button>("Information");
        exitGame = root.Q<Button>("Exit");

        //Callbacks
        startGame.RegisterCallback<ClickEvent>(StartGame);
        cardCreator.RegisterCallback<ClickEvent>(OpenCardCreator);
        options.RegisterCallback<ClickEvent>(OpenOptions);
        information.RegisterCallback<ClickEvent>(OpenInformation);
        exitGame.RegisterCallback<ClickEvent>(ExitGame);

    }

    private void StartGame(ClickEvent evt)
    {
       // this.gameObject.GetComponent<AudioSource>().Play();
        SceneManager.LoadScene("GameScene");
    }

    private void OpenCardCreator(ClickEvent evt)
    {
       // this.gameObject.GetComponent<AudioSource>().Play();
        SceneManager.LoadScene("CardCreatorScene");
        //CardCreator.SetActive(true);
        //gameObject.SetActive(false);
    }

    private void OpenOptions(ClickEvent evt)
    {
       // this.gameObject.GetComponent<AudioSource>().Play();
        Options.SetActive(true);
        gameObject.SetActive(false);
    }

    private void OpenInformation(ClickEvent evt)
    {
        //this.gameObject.GetComponent<AudioSource>().Play();
        Information.SetActive(true);
        gameObject.SetActive(false);
    }

    private void ExitGame(ClickEvent evt)
    {
        //this.gameObject.GetComponent<AudioSource>().Play();
        Debug.Log("Game is exiting");
        Application.Quit();
    }
}
