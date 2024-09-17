using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class ScriptSelectDecks : MonoBehaviour
{
    UIDocument SelectDecks;
    [SerializeField] GameObject GameManager;
    [SerializeField] GameObject SceneStartMenu;
    [SerializeField] GameObject SceneRuntime;
    [SerializeField] GameObject SoundRuntime;

    GameManager gameManager;
    public GameObject Game;
    [SerializeField] GameObject UIRuntime;

    private Button deck1;
    private Button deck2;
    private Button back;

    private void Awake()
    {
        gameManager = GameManager.GetComponent<GameManager>();
        //Game = GameObject.Find("Game");
        //UIRuntime = GameObject.Find("UIRuntime");
        //SoundRuntime = GameObject.Find("SoundRuntime");
        GameManager.SetActive(true);
        SoundRuntime.SetActive(true);
        SceneStartMenu.SetActive(false);
        SceneRuntime.SetActive(true);
    }
    private void OnEnable()
    {
        SelectDecks = GetComponent<UIDocument>();
        VisualElement root = SelectDecks.rootVisualElement;

        //References to the buttons
        deck1 = root.Q<Button>("Deck1");
        deck2 = root.Q<Button>("Deck2");
        back = root.Q<Button>("Back");

        //Callbacks
        deck1.RegisterCallback<ClickEvent>(OpenGameDeck1);
        deck2.RegisterCallback<ClickEvent>(OpenGameDeck2);
        back.RegisterCallback<ClickEvent>(BackToStartMenu);
    }
    private void OpenGameDeck1(ClickEvent evt)
    {
        gameManager.player1 = GameObject.FindGameObjectWithTag("Player1").GetComponent<Player>();
        gameManager.player2 = GameObject.FindGameObjectWithTag("Player2").GetComponent<Player>();

        GameObject deck1 = GameObject.Find("Deck Pirates");
        GameObject deck2 = GameObject.Find("Deck Resistance");

        gameManager.player1.hand.deck = deck1;
        gameManager.player1.deck = deck1.GetComponent<Decks>();
        gameManager.player1.subBoard.Boss.GetComponent<BossZone>().deck = deck1;

        gameManager.player2.hand.deck = deck2;
        gameManager.player2.deck = deck2.GetComponent<Decks>();
        gameManager.player2.subBoard.Boss.GetComponent<BossZone>().deck = deck2;

        gameManager.player1.isPlaying = true;
        gameManager.player2.isPlaying = false;

        Game.SetActive(true);
        UIRuntime.SetActive(true);
        gameObject.SetActive(false);

        UIRuntime.GetComponent<ScriptUIRuntime>().ShowMessage("Start Game");
        gameManager.startGame = true;
    }
    private void OpenGameDeck2(ClickEvent evt)
    {
        gameManager.player1 = GameObject.FindGameObjectWithTag("Player1").GetComponent<Player>();
        gameManager.player2 = GameObject.FindGameObjectWithTag("Player2").GetComponent<Player>();

        GameObject deck1 = GameObject.Find("Deck Resistance");
        GameObject deck2 = GameObject.Find("Deck Pirates");

        gameManager.player1.hand.deck = deck1;
        gameManager.player1.deck = deck1.GetComponent<Decks>();
        gameManager.player1.subBoard.Boss.GetComponent<BossZone>().deck = deck1;

        gameManager.player2.hand.deck = deck2;
        gameManager.player2.deck = deck2.GetComponent<Decks>();
        gameManager.player2.subBoard.Boss.GetComponent<BossZone>().deck = deck2;

        gameManager.player1.isPlaying = true;
        gameManager.player2.isPlaying = false;

        Game.SetActive(true);
        UIRuntime.SetActive(true);
        gameObject.SetActive(false);

        UIRuntime.GetComponent<ScriptUIRuntime>().ShowMessage("Start Game");
        gameManager.startGame = true;
    }
    private void BackToStartMenu(ClickEvent evt)
    {
        SceneStartMenu.SetActive(true);
        SoundRuntime.SetActive(false);
        SceneRuntime.SetActive(false);
    }
}
