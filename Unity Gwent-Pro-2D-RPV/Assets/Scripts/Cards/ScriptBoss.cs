using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptBoss : MonoBehaviour
{
    public GameObject GameManager;
    void OnMouseDown()
    {
        if (GameManager.GetComponent<GameManager>().player1.isPlaying && this.gameObject == GameManager.GetComponent<GameManager>().player2.board.Boss.GetComponent<BossZone>().Boss)
        {
            GameObject.Find("UI Runtime").GetComponent<ScriptUIRuntime>().ShowMessage("You cannot activate the head of the opponent");
            return;
        }
        else if (GameManager.GetComponent<GameManager>().player2.isPlaying && this.gameObject == GameManager.GetComponent<GameManager>().player1.board.Boss.GetComponent<BossZone>().Boss)
        {
            GameObject.Find("UI Runtime").GetComponent<ScriptUIRuntime>().ShowMessage("You cannot activate the head of the opponent");
            return;
        }
        else if (GameManager.GetComponent<GameManager>().activeboss1 && GameManager.GetComponent<GameManager>().player1.isPlaying)
        {
            GameObject.Find("UI Runtime").GetComponent<ScriptUIRuntime>().ShowMessage("It is already activated");
            return;
        }
        else if (GameManager.GetComponent<GameManager>().activeboss2 && GameManager.GetComponent<GameManager>().player2.isPlaying)
        {
            GameObject.Find("UI Runtime").GetComponent<ScriptUIRuntime>().ShowMessage("It is already activated");
            return;
        }
        else
        {
            GameObject.Find("UI Runtime").GetComponent<ScriptUIRuntime>().ShowMessage("Boss Effect Activated!");
            Effects.ActivateEffect(this.gameObject);
            if (GameManager.GetComponent<GameManager>().player1.isPlaying)
            {
                GameManager.GetComponent<GameManager>().activeboss1 = true;
                Debug.Log("Boss 1");
            }
            else
            {
                GameManager.GetComponent<GameManager>().activeboss2 = true;
                Debug.Log("Boss 2");
            }
            GameManager.GetComponent<GameManager>().ChangeTurn();
        }
    }
}
