using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using System;
using UnityEngine.SceneManagement;

public class ScriptUIRuntime : MonoBehaviour
{
    UIDocument UIRuntime;

    private Label currentRound;
    private Label playerTurn;

    private Label powerPlayer1;
    private ProgressBar roundsWonPlayer1;
     private Label powerPlayer2;
    private ProgressBar roundsWonPlayer2;

    private Button changeTurn;
    private Button exit;

    private void OnEnable()
    {
        UIRuntime = GetComponent<UIDocument>();
        VisualElement root = UIRuntime.rootVisualElement;

        //References
        currentRound = root.Q<Label>("NumCurrentRound");
        playerTurn = root.Q<Label>("PlayerTurn");

        powerPlayer1 = root.Q<Label>("Power1");
        roundsWonPlayer1 = root.Q<ProgressBar>("RoundsWon1");
        powerPlayer2 = root.Q<Label>("Power2");
        roundsWonPlayer2 = root.Q<ProgressBar>("RoundsWon2");

        changeTurn = root.Q<Button>("ChangeTurn");
        exit = root.Q<Button>("Exit");

        //Callbacks
        changeTurn.RegisterCallback<ClickEvent>(ChangeTurn);
        exit.RegisterCallback<ClickEvent>(ExitToStartMenu);

    }

    private void Start()
    {
        /*
        currentRound.text = GameManager.currentRound.ToString();
        playerTurn.text = GameManager.currentPlayer.ToString();
        powerPlayer1.text = GameManager.player1.Power.ToString();
        roundsWonPlayer1.value = GameManager.player1.RoundsWon;
        powerPlayer2.text = GameManager.player2.Power.ToString();
        roundsWonPlayer2.value = GameManager.player2.RoundsWon;
        */
    }

    public void Update()
    {
        /*
        //currentRound.text = GameManager.currentRound.ToString();
        //playerTurn.text = GameManager.currentPlayer.ToString();
        //powerPlayer1.text = GameManager.player1.Power.ToString();
        //roundsWonPlayer1.value = GameManager.player1.RoundsWon;
        //powerPlayer2.text = GameManager.player2.Power.ToString();
        //roundsWonPlayer2.value = GameManager.player2.RoundsWon;
        */
    }


    private void ChangeTurn(ClickEvent evt)
    {
        //GameManager.ChangeTurn();
        Debug.Log("Change Turn");
    }

    private void ExitToStartMenu(ClickEvent evt)
    {
        SceneManager.LoadScene("StartMenuScene");
    }
}
