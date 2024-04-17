using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class ScrpiptSelectDecks : MonoBehaviour
{
    UIDocument SelectDecks;
   // public GameObject StartMenu;
    GameObject DeckPirates;
    GameObject DeckResistance;

    private Button deck1;
    private Button deck2;
    private Button back;

    private void Start()
    {
        DeckPirates = GameObject.Find("Deck Pirates");
        DeckResistance = GameObject.FindGameObjectWithTag("Deck Resistance");
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
        throw new NotImplementedException();
    }

    private void BackToStartMenu(ClickEvent evt)
    {
        SceneManager.LoadScene("StartMenuScene");
       //StartMenu.SetActive(true);
       //gameObject.SetActive(false);
    }
}
