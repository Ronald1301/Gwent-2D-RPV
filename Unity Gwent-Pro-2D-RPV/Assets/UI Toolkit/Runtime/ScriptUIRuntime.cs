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
    public GameObject SelectDecks;
    public GameObject UICardDescription;

    private Label currentRound;
    private Label playerTurn;

    private Label powerPlayer1;
    private ProgressBar roundsWonPlayer1;
    private Label powerPlayer2;
    private ProgressBar roundsWonPlayer2;

    private Label TextWinner;
    private VisualElement Winner;
    private VisualElement Right;
    private VisualElement Round;

    private VisualElement BoxMessage;
    private Label Message;

    private Button changeTurn;
    private Button reset;
    private Button exit;

    private void OnEnable()
    {
        //if (Input.GetKeyDown(KeyCode.R)) SceneManager.LoadScene("GameScene");
        // if (Input.GetKeyDown(KeyCode.Escape)) Application.Quit();
        //if (Input.GetKeyDown(KeyCode.Space)) SceneManager.LoadScene("StartMenuScene");

        UIRuntime = GetComponent<UIDocument>();
        VisualElement root = UIRuntime.rootVisualElement;

        //References
        currentRound = root.Q<Label>("NumCurrentRound");
        playerTurn = root.Q<Label>("PlayerTurn");

        powerPlayer1 = root.Q<Label>("Power1");
        roundsWonPlayer1 = root.Q<ProgressBar>("RoundsWon1");
        powerPlayer2 = root.Q<Label>("Power2");
        roundsWonPlayer2 = root.Q<ProgressBar>("RoundsWon2");

        BoxMessage = root.Q<VisualElement>("BoxMessage");
        Message = root.Q<Label>("Message");

        TextWinner = root.Q<Label>("Text2");
        Winner = root.Q<VisualElement>("Winners");
        Right = root.Q<VisualElement>("Right");
        Round = root.Q<VisualElement>("Round");

        changeTurn = root.Q<Button>("ChangeTurn");
        reset = root.Q<Button>("Reset");
        exit = root.Q<Button>("Exit");

        //Callbacks
        changeTurn.RegisterCallback<ClickEvent>(ChangeTurnButton);
        reset.RegisterCallback<ClickEvent>(ResetGame);
        exit.RegisterCallback<ClickEvent>(ExitToStartMenu);
    }
    private void ResetGame(ClickEvent evt)
    {
        SceneManager.LoadScene("GameScene");
    }
    public void UIUpdate()
    {
        if (GameManager.GetComponent<GameManager>().startGame)
        {
            if (GameManager.GetComponent<GameManager>().currentRound == 1)
            {
                Round.style.visibility = Visibility.Visible;
                Right.style.visibility = Visibility.Visible;
                Winner.style.display = DisplayStyle.None;
                //Winner.style.visibility = Visibility.Hidden;
                reset.style.visibility = Visibility.Hidden;
                GameManager.GetComponent<GameManager>().MainBoard.SetActive(true);
            }

            currentRound.text = GameManager.GetComponent<GameManager>().currentRound.ToString();

            if (GameManager.GetComponent<GameManager>().player1.isPlaying) playerTurn.text = "Player 1";
            else playerTurn.text = "Player 2";

            powerPlayer1.text = GameManager.GetComponent<GameManager>().player1.Points_for_round.ToString();
            roundsWonPlayer1.value = GameManager.GetComponent<GameManager>().player1.RoundsWon;
            roundsWonPlayer1.title = GameManager.GetComponent<GameManager>().player1.RoundsWon.ToString();

            powerPlayer2.text = GameManager.GetComponent<GameManager>().player2.Points_for_round.ToString();
            roundsWonPlayer2.value = GameManager.GetComponent<GameManager>().player2.RoundsWon;
            roundsWonPlayer2.title = GameManager.GetComponent<GameManager>().player2.RoundsWon.ToString();

            if (GameManager.GetComponent<GameManager>().endgamebool) StartCoroutine(WaitForSeconds(2.0f));
        }
    }
    public void ShowMessage(string message)
    {
        BoxMessage.style.display = DisplayStyle.Flex;
        //BoxMessage.style.visibility = Visibility.Visible;
        Message.text = message;
        StartCoroutine(WaitAndPrint(1.0f));

        //BoxMessage.style.visibility = Visibility.Hidden;
        //BoxMessage.style.display = DisplayStyle.None;
    }
    private void ShowWinner()
    {
        Round.style.visibility = Visibility.Hidden;
        Right.style.visibility = Visibility.Hidden;
        reset.style.visibility = Visibility.Visible;
        Winner.style.display = DisplayStyle.Flex;

        GameManager.GetComponent<GameManager>().MainBoard.SetActive(false);
        UICardDescription.SetActive(false);

        if (GameManager.GetComponent<GameManager>().player1.youWin)
        {
            TextWinner.text = "Player 1";
        }
        else if (GameManager.GetComponent<GameManager>().player2.youWin)
        {
            TextWinner.text = "Player 2";
        }
        else
        {
            TextWinner.text = "It's a tie!";
        }
    }
    private void ChangeTurnButton(ClickEvent evt)
    {
        if (GameManager.GetComponent<GameManager>().player1.isPlaying)
        {
            GameManager.GetComponent<GameManager>().player1.passTurn = true;
            //Debug.Log("Player 1 pass turn");
            GameManager.GetComponent<GameManager>().ChangeTurn();
            //Debug.Log("Change Turn");
        }
        else
        {
            GameManager.GetComponent<GameManager>().player2.passTurn = true;
            //Debug.Log("Player 2 pass turn");
            GameManager.GetComponent<GameManager>().ChangeTurn();
        }
        //GameManager.GetComponent<GameManager>().ChangeTurn();
    }
    private void ExitToStartMenu(ClickEvent evt)
    {
        SceneManager.LoadScene("StartMenuScene");
    }
    public IEnumerator WaitAndPrint(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        BoxMessage.style.display = DisplayStyle.None;
    }
    public IEnumerator WaitForSeconds(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        BoxMessage.style.display = DisplayStyle.None;
        //BoxMessage.style.visibility = Visibility.Hidden;
        ShowWinner();
        //Winner.style.visibility = Visibility.Visible;
    }
}
