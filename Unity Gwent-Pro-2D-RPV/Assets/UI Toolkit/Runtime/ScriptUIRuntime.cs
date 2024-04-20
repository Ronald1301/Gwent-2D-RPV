using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using System;
using UnityEngine.SceneManagement;

public class ScriptUIRuntime : MonoBehaviour
{
    UIDocument UIRuntime;
    public GameObject GameManager;

    private Label currentRound;
    private Label playerTurn;

    private Label powerPlayer1;
    private ProgressBar roundsWonPlayer1;
    private Label powerPlayer2;
    private ProgressBar roundsWonPlayer2;

    private Label TextWinner;
    private VisualElement Winner;
    private VisualElement Right;
    private VisualElement Left;

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

        TextWinner = root.Q<Label>("Text2");
        Winner = root.Q<VisualElement>("Winners");
        Right = root.Q<VisualElement>("Right");
        Left = root.Q<VisualElement>("Left");

        changeTurn = root.Q<Button>("ChangeTurn");
        exit = root.Q<Button>("Exit");

        //Callbacks
        changeTurn.RegisterCallback<ClickEvent>(ChangeTurn);
        exit.RegisterCallback<ClickEvent>(ExitToStartMenu);

    }

    public void UIUpdate()
    {
        currentRound.text = GameManager.GetComponent<GameManager>().currentRound.ToString();

        if (GameManager.GetComponent<GameManager>().player1.isPlaying) playerTurn.text = "Player 1";
        else playerTurn.text = "Player 2";

        powerPlayer1.text = GameManager.GetComponent<GameManager>().player1.Points_for_round.ToString();
        roundsWonPlayer1.value = GameManager.GetComponent<GameManager>().player1.RoundsWon;
        roundsWonPlayer1.title = GameManager.GetComponent<GameManager>().player1.RoundsWon.ToString();

        powerPlayer2.text = GameManager.GetComponent<GameManager>().player2.Points_for_round.ToString();
        roundsWonPlayer2.value = GameManager.GetComponent<GameManager>().player2.RoundsWon;
        roundsWonPlayer2.title = GameManager.GetComponent<GameManager>().player2.RoundsWon.ToString();

        if (GameManager.GetComponent<GameManager>().endgamebool) { ShowWinner(); }

    }

    private void ShowWinner()
    {
        if (GameManager.GetComponent<GameManager>().player1.RoundsWon == 2)
        {
            TextWinner.text = "Player 1 Wins!";
            Left.style.display = DisplayStyle.None;
            Right.style.display = DisplayStyle.None;
            Winner.style.display = DisplayStyle.Flex;
        }
        else if (GameManager.GetComponent<GameManager>().player2.RoundsWon == 2)
        {
            TextWinner.text = "Player 2 Wins!";
            Left.style.display = DisplayStyle.None;
            Right.style.display = DisplayStyle.None;
            Winner.style.display = DisplayStyle.Flex;
        }
    }

    private void ChangeTurn(ClickEvent evt)
    {
        if (GameManager.GetComponent<GameManager>().player1.isPlaying)
        {
            GameManager.GetComponent<GameManager>().player1.passTurn = true;
            Debug.Log("Player 1 pass turn");
            //GameManager.GetComponent<GameManager>().ChangeTurn();
            //Debug.Log("Change Turn");
        }
        else
        {
            GameManager.GetComponent<GameManager>().player2.passTurn = true;
            Debug.Log("Player 2 pass turn");
        }
        GameManager.GetComponent<GameManager>().ChangeTurn();

        GameManager.GetComponent<GameManager>().ChangeTurn();
    }

    private void ExitToStartMenu(ClickEvent evt)
    {
        SceneManager.LoadScene("StartMenuScene");
    }
}
