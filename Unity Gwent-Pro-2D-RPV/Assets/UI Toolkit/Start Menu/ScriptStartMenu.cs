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
    [SerializeField] GameObject SceneStartMenu;
    [SerializeField] GameObject SceneCardCreator;
    [SerializeField] GameObject SceneRuntime;
    public GameObject Options;
    public GameObject Information;
    public GameObject SoundMenu;


    private Button startGame;
    private Button cardCreator;
    private Button options;
    private Button information;
    private Button exitGame;


    void Awake()
    {
        SceneStartMenu.SetActive(true);
        SceneCardCreator.SetActive(false);
        SceneRuntime.SetActive(false);

        Options.SetActive(false);
        Information.SetActive(false);
        SoundMenu.SetActive(true);
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
        SceneRuntime.SetActive(true);
        SoundMenu.SetActive(false);
        SceneStartMenu.SetActive(false);
    }

    private void OpenCardCreator(ClickEvent evt)
    {
        // this.gameObject.GetComponent<AudioSource>().Play();
        SceneCardCreator.SetActive(true);
        SceneStartMenu.SetActive(false);
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
