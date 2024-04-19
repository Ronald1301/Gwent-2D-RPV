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
    GameObject DeckPirates;
    GameObject DeckResistance;
    GameObject Board1;
    GameObject Board2;
    GameObject Hand1;
    GameObject Hand2;
    [SerializeField] GameObject Game;
    [SerializeField] GameObject UIRuntime;

    private Button deck1;
    private Button deck2;
    private Button back;

    private void Start()
    {
        GameManager = GameObject.FindGameObjectWithTag("GameManager");
        DeckPirates = GameObject.FindGameObjectWithTag("Deck Pirates");
        DeckResistance = GameObject.FindGameObjectWithTag("Deck Resistance");
        Board1 = GameObject.FindGameObjectWithTag("SubBoard1");
        Board2 = GameObject.FindGameObjectWithTag("SubBoard2");
        Hand1 = GameObject.FindGameObjectWithTag("Hand1");
        Hand2 = GameObject.FindGameObjectWithTag("Hand2");
       Game = GameObject.FindGameObjectWithTag("Game");
        UIRuntime= GameObject.FindGameObjectWithTag("UIRuntime");
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
        deck1.RegisterCallback<ClickEvent>(OpenGame);
        deck2.RegisterCallback<ClickEvent>(OpenGame);
        back.RegisterCallback<ClickEvent>(BackToStartMenu);
    }

    private void OpenGame(ClickEvent evt)
    {
        GameManager.GetComponent<GameManager>().player1 = gameObject.AddComponent<Player>();
        GameManager.GetComponent<GameManager>().player2 = gameObject.AddComponent<Player>();
        
        Game.SetActive(true);
        UIRuntime.SetActive(true);
        gameObject.SetActive(false);
    }

    private void OpenGameDeck1(ClickEvent evt)
    {
        GetComponent<GameManager>().player1.deck = DeckPirates.GetComponent<Decks>();
        GetComponent<GameManager>().player1.board = Board1.GetComponent<SubBoard>();
        GetComponent<GameManager>().player1.hand =  Hand1.GetComponent<Hand>();
        GetComponent<GameManager>().player2.deck= DeckResistance.GetComponent<Decks>();
        GetComponent<GameManager>().player2.board = Board2.GetComponent<SubBoard>();
        GetComponent<GameManager>().player2.hand = Hand2.GetComponent<Hand>();
        gameObject.SetActive(false);
        Game.SetActive(true);
        UIRuntime.SetActive(true);
    }
    private void OpenGameDeck2(ClickEvent evt)
    {
       
        GetComponent<GameManager>().player1.deck = DeckResistance.GetComponent<Decks>();
        GetComponent<GameManager>().player1.board = Board1.GetComponent<SubBoard>();
        GetComponent<GameManager>().player1.hand= Hand1.GetComponent<Hand>();
        GetComponent<GameManager>().player2.deck = DeckPirates.GetComponent<Decks>();
        GetComponent<GameManager>().player2.board = Board2.GetComponent<SubBoard>();
        GetComponent<GameManager>().player2.hand= Hand2.GetComponent<Hand>();
         GameManager.GetComponent<GameManager>().player1 = gameObject.AddComponent<Player>();
        GameManager.GetComponent<GameManager>().player2 = gameObject.AddComponent<Player>();
        gameObject.SetActive(false);
        Game.SetActive(true);
        UIRuntime.SetActive(true);
    }

    private void BackToStartMenu(ClickEvent evt)
    {
        SceneManager.LoadScene("StartMenuScene");
       //StartMenu.SetActive(true);
       //gameObject.SetActive(false);
    }
}
