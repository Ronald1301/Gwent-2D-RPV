using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Timeline;

public class GameManager : MonoBehaviour
{
    public Player player1;
    public Player player2;
    public int currentRound=1;

    public bool activeboss1;
    public bool activeboss2;

    public bool Changecards1;
    public bool Changecards2;

    public bool endgamebool;
    private int clickcount;
    [SerializeField] ScriptUIRuntime UIRuntime;

    public GameObject MainBoard;


    void Start()
    {
        StartGame();
        //UIRuntime.GetComponent<ScriptUIRuntime>().UIUpdate();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            clickcount++;
            if (clickcount <= 2)
            {
                if (player1.isPlaying && !Changecards1)
                {
                    player1.hand.ChangeCard();
                    if (clickcount == 2) Changecards1 = true;
                }
                else if (player2.isPlaying && !Changecards2)
                {
                    player2.hand.ChangeCard();
                    if (clickcount == 2) Changecards2 = true;
                }
            }
        }
        //ChangeImageCardsinHand();

        UIRuntime.GetComponent<ScriptUIRuntime>().UIUpdate();
        UpdatePoints();
        CheckEndRound();
        UIRuntime.GetComponent<ScriptUIRuntime>().UIUpdate();
        LateUpdate();
    }

    public void LateUpdate()
    {
        if (player1.passTurn && player2.passTurn)
        {
            EndRound();
        }
        if (currentRound == 4 || player1.RoundsWon == 2 || player2.RoundsWon == 2)
        {
            EndGame();
        }
    }

    /*
        public void ChangeImageCardsinHand()
        {
            if (player1.isPlaying)
            {
                for (int i = 0; i < player1.hand.CardsInHand.Count; i++)
                {
                    player1.hand.CardsInHand[i].GetComponent<CardDisplay>().GetComponent<SpriteRenderer>().sprite = player1.hand.CardsInHand[i].GetComponent<CardDisplay>().card.CardImageForehead;
                }
                for (int i = 0; i < player2.hand.CardsInHand.Count; i++)
                {
                    player2.hand.CardsInHand[i].GetComponent<CardDisplay>().GetComponent<SpriteRenderer>().sprite = player2.hand.CardsInHand[i].GetComponent<CardDisplay>().card.CardImageBack;
                }
            }
            else
            {
                for (int i = 0; i < player1.hand.CardsInHand.Count; i++)
                {
                    player1.hand.CardsInHand[i].GetComponent<CardDisplay>().GetComponent<SpriteRenderer>().sprite = player1.hand.CardsInHand[i].GetComponent<CardDisplay>().card.CardImageBack;
                }
                for (int i = 0; i < player2.hand.CardsInHand.Count; i++)
                {
                    player2.hand.CardsInHand[i].GetComponent<CardDisplay>().GetComponent<SpriteRenderer>().sprite = player2.hand.CardsInHand[i].GetComponent<CardDisplay>().card.CardImageForehead;
                }
            }
        }
        */

    public void StartGame()
    {
        currentRound = 1;
        player1.isPlaying = true;
        activeboss1 = false;
        activeboss2 = false;
        Changecards1 = false;
        Changecards2 = false;
        endgamebool = false;
        clickcount = 0;
    }
    public void ChangeTurn()
    {
        clickcount = 0;
        if (player1.isPlaying && !player2.passTurn)
        {
            Changecards1 = true;
            player1.isPlaying = false;
            player2.isPlaying = true;
            MainBoard.transform.Rotate(0, 0, 180);
        }
        else if (player2.isPlaying && !player1.passTurn)
        {
            Changecards2 = true;
            player2.isPlaying = false;
            player1.isPlaying = true;
            MainBoard.transform.Rotate(0, 0, 180);
        }
        // MainBoard.transform.Rotate(0, 0, 180);
    }
    public void UpdatePoints()
    {
        player1.Points_for_round = player1.board.UpdatePoints();
        player2.Points_for_round = player2.board.UpdatePoints();
    }
    public void UpdatePoints(Card card)
    {
        if (player1.isPlaying)
        {
            player1.Points_for_round += card.Power;
        }
        else
        {
            player2.Points_for_round += card.Power;
        }
    }
    public void CheckEndRound()
    {
        if (player1.hand.CheckHand())
        {
            player1.passTurn = true;
            //StartRound();
        }
        if (player2.hand.CheckHand())
        {
            player2.passTurn = true;
            //StartRound();
        }
    }
    public void EndRound()
    {
        if (player1.Points_for_round > player2.Points_for_round)
        {
            // player1.RoundsWon_for_game[currentRound - 1] = true;
            player1.RoundsWon++;
            currentRound++;

            player1.Points_for_game += player1.Points_for_round;
            player2.Points_for_game += player2.Points_for_round;

            player1.Points_for_round = 0;
            player2.Points_for_round = 0;

            player1.isPlaying = true;
            player1.passTurn = false;

            player1.passTurn = false;
            player2.passTurn = false;
        }
        else if (player1.Points_for_round < player2.Points_for_round)
        {
            // player2.RoundsWon_for_game[currentRound - 1] = true;
            player2.RoundsWon++;
            currentRound++;

            player1.Points_for_game += player1.Points_for_round;
            player2.Points_for_game += player2.Points_for_round;

            player1.Points_for_round = 0;
            player2.Points_for_round = 0;

            player2.isPlaying = true;
            player2.passTurn = false;

            player1.passTurn = false;
            player2.passTurn = false;
        }
        else
        {
            // player1.RoundsWon_for_game[currentRound - 1] = true;
            player1.RoundsWon++;
            //player2.RoundsWon_for_game[currentRound - 1] = true;
            player2.RoundsWon++;
            currentRound++;

            player1.Points_for_game += player1.Points_for_round;
            player2.Points_for_game += player2.Points_for_round;

            player1.Points_for_round = 0;
            player2.Points_for_round = 0;

            int index = Random.Range(1, 2);
            if (index == 1) player1.isPlaying = true;
            else player2.isPlaying = true;

            player1.passTurn = false;
            player2.passTurn = false;

        }

        if (currentRound == 4) EndGame();

        ClearField();
        StartRound();

    }
    private void ClearField()
    {
        if (player1.deck.tag == "Deck Pirates")
        {

            for (int i = 0; i < player1.board.GetComponent<SubBoard>().M.GetComponent<MeleeZone>().melee.Count; i++)
            {
                if (activeboss1)
                {
                    if (!player1.board.GetComponent<SubBoard>().M.GetComponent<MeleeZone>().melee[i].GetComponent<CardDisplay>().card.stayintheField)
                    {
                        player1.board.GetComponent<SubBoard>().M.GetComponent<MeleeZone>().melee[i].GetComponent<MoveCard>().MoveToCemetery();
                    }
                }
                else { player1.board.GetComponent<SubBoard>().M.GetComponent<MeleeZone>().melee.Clear(); }
            }
            for (int i = 0; i < player1.board.GetComponent<SubBoard>().R.GetComponent<RangedZone>().ranged.Count; i++)
            {
                if (activeboss1)
                {
                    if (!player1.board.GetComponent<SubBoard>().R.GetComponent<RangedZone>().ranged[i].GetComponent<CardDisplay>().card.stayintheField)
                    {
                        player1.board.GetComponent<SubBoard>().R.GetComponent<RangedZone>().ranged[i].GetComponent<MoveCard>().MoveToCemetery();
                    }
                }
                else { player1.board.GetComponent<SubBoard>().R.GetComponent<RangedZone>().ranged.Clear(); }
            }
            for (int i = 0; i < player1.board.GetComponent<SubBoard>().S.GetComponent<SiegeZone>().siege.Count; i++)
            {
                if (activeboss1)
                {
                    if (!player1.board.GetComponent<SubBoard>().S.GetComponent<SiegeZone>().siege[i].GetComponent<CardDisplay>().card.stayintheField)
                    {
                        player1.board.GetComponent<SubBoard>().S.GetComponent<SiegeZone>().siege[i].GetComponent<MoveCard>().MoveToCemetery();
                    }
                }
                else { player1.board.GetComponent<SubBoard>().S.GetComponent<SiegeZone>().siege.Clear(); }
            }
            player2.board.GetComponent<SubBoard>().M.GetComponent<MeleeZone>().melee.Clear();
            player2.board.GetComponent<SubBoard>().R.GetComponent<RangedZone>().ranged.Clear();
            player2.board.GetComponent<SubBoard>().S.GetComponent<SiegeZone>().siege.Clear();
        }
        else
        {
            for (int i = 0; i < player2.board.GetComponent<SubBoard>().M.GetComponent<MeleeZone>().melee.Count; i++)
            {
                if (activeboss2)
                {
                    if (!player2.board.GetComponent<SubBoard>().M.GetComponent<MeleeZone>().melee[i].GetComponent<CardDisplay>().card.stayintheField)
                    {
                        player2.board.GetComponent<SubBoard>().M.GetComponent<MeleeZone>().melee[i].GetComponent<MoveCard>().MoveToCemetery();
                    }
                }
                else { player2.board.GetComponent<SubBoard>().M.GetComponent<MeleeZone>().melee.Clear(); }
            }
            for (int i = 0; i < player2.board.GetComponent<SubBoard>().R.GetComponent<RangedZone>().ranged.Count; i++)
            {
                if (activeboss2)
                {
                    if (!player2.board.GetComponent<SubBoard>().R.GetComponent<RangedZone>().ranged[i].GetComponent<CardDisplay>().card.stayintheField)
                    {
                        player2.board.GetComponent<SubBoard>().R.GetComponent<RangedZone>().ranged[i].GetComponent<MoveCard>().MoveToCemetery();
                    }
                }
                else
                {
                    player2.board.GetComponent<SubBoard>().R.GetComponent<RangedZone>().ranged.Clear();
                }

            }
            for (int i = 0; i < player2.board.GetComponent<SubBoard>().S.GetComponent<SiegeZone>().siege.Count; i++)
            {
                if (activeboss2)
                {
                    if (!player2.board.GetComponent<SubBoard>().S.GetComponent<SiegeZone>().siege[i].GetComponent<CardDisplay>().card.stayintheField)
                    {
                        player2.board.GetComponent<SubBoard>().S.GetComponent<SiegeZone>().siege[i].GetComponent<MoveCard>().MoveToCemetery();
                    }
                }
                else { player2.board.GetComponent<SubBoard>().S.GetComponent<SiegeZone>().siege.Clear(); }
            }
            player1.board.GetComponent<SubBoard>().M.GetComponent<MeleeZone>().melee.Clear();
            player1.board.GetComponent<SubBoard>().R.GetComponent<RangedZone>().ranged.Clear();
            player1.board.GetComponent<SubBoard>().S.GetComponent<SiegeZone>().siege.Clear();

        }
    }
    public void StartRound()
    {
        player1.passTurn = false;
        player2.passTurn = false;

        player1.hand.DrawCard(2);
        player2.hand.DrawCard(2);
        if (player1.deck.tag == "Deck Resistance")
        {
            if (activeboss1)
            {
                player1.hand.DrawCard(3);
                player2.hand.DrawCard(2);
            }
            else
            {
                player1.hand.DrawCard(2);
                player2.hand.DrawCard(2);
            }

        }
        else
        {
            if (activeboss2)
            {
                player1.hand.DrawCard(2);
                player2.hand.DrawCard(3);
            }
            else
            {
                player1.hand.DrawCard(2);
                player2.hand.DrawCard(2);
            }
        }



    }
    public void EndGame()
    {
        if (player1.RoundsWon >= 2 && player2.RoundsWon < 2)
        {
            Debug.Log("Player 1 Wins");
            player1.youWin = true;
        }
        else if (player2.RoundsWon >= 2 && player1.RoundsWon < 2)
        {
            Debug.Log("Player 2 Wins");
            player2.youWin = true;
        }
        else
        {
            Debug.Log("Tied Game");
        }
        endgamebool = true;
        UIRuntime.GetComponent<ScriptUIRuntime>().UIUpdate();
        //if (Input.GetKeyDown(KeyCode.R) || Input.GetMouseButtonDown(1)) ResetGame();
        if (Input.GetKeyDown(KeyCode.Escape)) Application.Quit();
        if (Input.GetKeyDown(KeyCode.Space)) SceneManager.LoadScene("StartMenuScene");

    }


    /*/
        public void ResetGame()
        {
            player1 = new Player();
            player2 = new Player();
            currentRound = 1;
            player1.isPlaying = true;
        }
        */


}

