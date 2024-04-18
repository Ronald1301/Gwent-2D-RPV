using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class ScrpiptSelectDecks : MonoBehaviour
{
    UIDocument SelectDecks;
    GameObject GameManager;
    GameObject DeckPirates;
    GameObject DeckResistance;
    GameObject Board1;
    GameObject Board2;
    GameObject Hand1;
    GameObject Hand2;
    GameObject Game;
    GameObject UIRuntime;

    private Button deck1;
    private Button deck2;
    private Button back;

    private void Start()
    {
        GameManager = GameObject.Find("GameManager");
        DeckPirates = GameObject.FindGameObjectWithTag("Deck Pirates");
        DeckResistance = GameObject.FindGameObjectWithTag("Deck Resistance");
        Board1 = GameObject.FindGameObjectWithTag("SubBoard1");
        Board2 = GameObject.FindGameObjectWithTag("SubBoard2");
        Hand1 = GameObject.FindGameObjectWithTag("Hand1");
        Hand2 = GameObject.FindGameObjectWithTag("Hand2");
        Game = GameObject.FindGameObjectWithTag("GameController");
        UIRuntime= GameObject.Find("UIRuntime");
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
        GetComponent<GameManager>().player1 = new Player(DeckPirates.GetComponent<Decks>(),Board1.GetComponent<SubBoard>(),Hand1.GetComponent<Hand>());
        GetComponent<GameManager>().player2 = new Player(DeckResistance.GetComponent<Decks>(), Board2.GetComponent<SubBoard>(), Hand2.GetComponent<Hand>());
        gameObject.SetActive(false);
        Game.SetActive(true);
    }
    private void OpenGameDeck2(ClickEvent evt)
    {
        GetComponent<GameManager>().player1 = new Player(DeckResistance.GetComponent<Decks>(), Board1.GetComponent<SubBoard>(), Hand1.GetComponent<Hand>());
        GetComponent<GameManager>().player2 = new Player(DeckPirates.GetComponent<Decks>(), Board2.GetComponent<SubBoard>(), Hand2.GetComponent<Hand>());
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
