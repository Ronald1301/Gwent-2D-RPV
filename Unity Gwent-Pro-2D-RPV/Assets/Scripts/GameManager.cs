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
    public int currentRound;

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
        //UIRuntime.gameObject.SetActive(true);
        UIRuntime.GetComponent<ScriptUIRuntime>().UIUpdate();
        //StartGame();
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
        ChangeImageCardsinHand();

        UIRuntime.GetComponent<ScriptUIRuntime>().UIUpdate();
        UpdatePoints();
        EndRound2();
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
    /*
    public void StartGame()
    {
        
        player1 = gameObject.AddComponent<Player>();
        player2 = gameObject.AddComponent<Player>();
        
        
        player1 = new Player(GameObject.FindGameObjectWithTag("Deck Pirates").GetComponent<Decks>(),
            GameObject.FindGameObjectWithTag("SubBoard1").GetComponent<SubBoard>(),
            GameObject.FindGameObjectWithTag("Hand1").GetComponent<Hand>());
        player2 = new Player(GameObject.FindGameObjectWithTag("Deck Resistance").GetComponent<Decks>(),
            GameObject.FindGameObjectWithTag("SubBoard2").GetComponent<SubBoard>(),
            GameObject.FindGameObjectWithTag("Hand2").GetComponent<Hand>());

        player1 = new Player();
        player2 = new Player();

        player1.deck = GameObject.FindGameObjectWithTag("Deck Pirates").GetComponent<Decks>();
        player1.board = GameObject.FindGameObjectWithTag("SubBoard1").GetComponent<SubBoard>();
        player1.hand = GameObject.FindGameObjectWithTag("Hand1").GetComponent<Hand>();
        player2.deck = GameObject.FindGameObjectWithTag("Deck Resistance").GetComponent<Decks>();
        player2.board = GameObject.FindGameObjectWithTag("SubBoard2").GetComponent<SubBoard>();
        player2.hand = GameObject.FindGameObjectWithTag("Hand2").GetComponent<Hand>();

        currentRound = 1;
        player1.isPlaying = true;

    }
    */


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

    public void UpdatePoints()
    {
        //player1.Points_for_round[currentRound - 1] = player1.board.UpdatePoints();
        //player2.Points_for_round[currentRound - 1] = player2.board.UpdatePoints();
    }


    public void ChangeTurn()
    {
        clickcount = 0;
        if (player1.isPlaying)
        {
            Changecards1 = true;
            player1.isPlaying = false;
            player2.isPlaying = true;
        }
        else
        {
            Changecards2 = true;
            player2.isPlaying = false;
            player1.isPlaying = true;
        }
        MainBoard.transform.Rotate(0, 0, 180);
    }

    public void EndRound()
    {
        if (player1.Points_for_round > player2.Points_for_round)
        {
            player1.RoundsWon_for_game[currentRound - 1] = true;
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
            player2.RoundsWon_for_game[currentRound - 1] = true;
            player2.RoundsWon++;
            currentRound++;

            player1.Points_for_game += player1.Points_for_round;
            player2.Points_for_game += player2.Points_for_round;

            player2.isPlaying = true;
            player2.passTurn = false;

            player1.passTurn = false;
            player2.passTurn = false;
        }
        else
        {
            player1.RoundsWon_for_game[currentRound - 1] = true;
            player1.RoundsWon++;
            player2.RoundsWon_for_game[currentRound - 1] = true;
            player2.RoundsWon++;
            currentRound++;

            player1.Points_for_game += player1.Points_for_round;
            player2.Points_for_game += player2.Points_for_round;

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
    public void EndRound2()
    {
        if (player1.hand.CheckHand())
        {
            player1.passTurn = true;
            StartRound();
        }
        if (player2.hand.CheckHand())
        {
            player2.passTurn = true;
            StartRound();
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
        if (player1.RoundsWon == 2)
        {
            Debug.Log("Player 1 Wins");
        }
        else if (player2.RoundsWon == 2)
        {
            Debug.Log("Player 2 Wins");
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

