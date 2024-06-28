using System.Collections;
using System.Collections.Generic;
//using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Timeline;

public class GameManager : MonoBehaviour
{
    public bool startGame;
    public Player player1;
    public Player player2;
    public int currentRound = 0;

    public bool activeboss1;
    public bool activeboss2;

    public bool Changecards1;
    public bool Changecards2;

    public bool endgamebool;
    public int clickcount;
    [SerializeField] ScriptUIRuntime UIRuntime;

    public GameObject MainBoard;

    void Start()
    {
        StartGame();
        UIRuntime.UIUpdate();
    }
    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
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

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (player1.isPlaying)
            {
                player1.passTurn = true;
                ChangeTurn();
            }
            else
            {
                player2.passTurn = true;
                ChangeTurn();
            }
        }

        UpdateClimateAndIncrease();
        ChangeImageCardsinHand();
        // UIRuntime.GetComponent<ScriptUIRuntime>().UIUpdate();
        UpdatePoints();
        UIRuntime.GetComponent<ScriptUIRuntime>().UIUpdate();
        //CheckEndRound();
        LateUpdate();
        UIRuntime.GetComponent<ScriptUIRuntime>().UIUpdate();
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
    public void StartGame()
    {
        currentRound = 1;
        /*
        player1.isPlaying = true;
        player2.isPlaying = false;
        */
        activeboss1 = false;
        activeboss2 = false;
        Changecards1 = false;
        Changecards2 = false;
        endgamebool = false;
        clickcount = 0;
        StartUpdateCards();
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
            UIRuntime.ShowMessage("Player 2 Turn");
            //StartCoroutine(UIRuntime.GetComponent<ScriptUIRuntime>().WaitAndPrint(4.0f));
        }
        else if (player2.isPlaying && !player1.passTurn)
        {
            Changecards2 = true;
            player2.isPlaying = false;
            player1.isPlaying = true;
            MainBoard.transform.Rotate(0, 0, 180);
            UIRuntime.ShowMessage("Player 1 Turn");
            //StartCoroutine(UIRuntime.GetComponent<ScriptUIRuntime>().WaitAndPrint(4.0f));
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
            Debug.Log("Puntos Agregados player 1");
        }
        else if (player2.isPlaying)
        {
            player2.Points_for_round += card.Power;
            Debug.Log("Puntos Agregados player 2");
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
        /*
        if (player1.hand.CardsInHand.Count == 0 && player2.hand.CardsInHand.Count == 0)
        {
            UpdatePoints();
            UIRuntime.UIUpdate();
            EndRound();
        }
        */
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
            player2.isPlaying = false;

            player1.passTurn = false;
            player2.passTurn = false;

            MainBoard.transform.rotation = Quaternion.identity;

            UIRuntime.ShowMessage("Player 1 won round " + (currentRound - 1));
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

            player1.isPlaying = false;
            player2.isPlaying = true;

            player1.passTurn = false;
            player2.passTurn = false;

            MainBoard.transform.rotation = Quaternion.identity;
            MainBoard.transform.rotation = Quaternion.Euler(0, 0, 180);

            UIRuntime.ShowMessage("Player 2 won round " + (currentRound - 1));
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
            if (index == 1)
            {
                player1.isPlaying = true;
                MainBoard.transform.rotation = Quaternion.identity;
            }
            else
            {
                player2.isPlaying = true;
                MainBoard.transform.rotation = Quaternion.identity;
                MainBoard.transform.rotation = Quaternion.Euler(0, 0, 180);
                /*
                MainBoard.transform.rotation = Quaternion.identity;
                MainBoard.transform.Rotate(0, 0, 180);
                */
            }

            player1.passTurn = false;
            player2.passTurn = false;

            UIRuntime.ShowMessage("Round " + (currentRound - 1) + " is a tie");
        }

        //if (currentRound == 4) EndGame();

        ClearField();
        StartRound();

    }
    private void ClearField()
    {
        int k = 0;
        while (player1.board.GetComponent<SubBoard>().M.GetComponent<MeleeZone>().melee.Count != 0)
        {
            if (k >= player1.board.GetComponent<SubBoard>().M.GetComponent<MeleeZone>().melee.Count) break;

            if (!player1.board.GetComponent<SubBoard>().M.GetComponent<MeleeZone>().melee[k].GetComponent<CardDisplay>().card.stayintheField)
            {
                player1.board.GetComponent<SubBoard>().M.GetComponent<MeleeZone>().melee[k].GetComponent<MoveCard>().MoveToCemetery();
            }
            else k++;
        }
        k = 0;
        while (player1.board.GetComponent<SubBoard>().R.GetComponent<RangedZone>().ranged.Count != 0)
        {
            if (k >= player1.board.GetComponent<SubBoard>().R.GetComponent<RangedZone>().ranged.Count) break;

            if (!player1.board.GetComponent<SubBoard>().R.GetComponent<RangedZone>().ranged[k].GetComponent<CardDisplay>().card.stayintheField)
            {
                player1.board.GetComponent<SubBoard>().R.GetComponent<RangedZone>().ranged[k].GetComponent<MoveCard>().MoveToCemetery();
            }
            else k++;
        }
        k = 0;
        while (player1.board.GetComponent<SubBoard>().S.GetComponent<SiegeZone>().siege.Count != 0)
        {
            if (k >= player1.board.GetComponent<SubBoard>().S.GetComponent<SiegeZone>().siege.Count) break;

            if (!player1.board.GetComponent<SubBoard>().S.GetComponent<SiegeZone>().siege[k].GetComponent<CardDisplay>().card.stayintheField)
            {
                player1.board.GetComponent<SubBoard>().S.GetComponent<SiegeZone>().siege[k].GetComponent<MoveCard>().MoveToCemetery();
            }
            else k++;
        }
        /*
                for (int i = 0; i < player1.board.GetComponent<SubBoard>().M.GetComponent<MeleeZone>().melee.Count; i++)
                {
                    if (!player1.board.GetComponent<SubBoard>().M.GetComponent<MeleeZone>().melee[i].GetComponent<CardDisplay>().card.stayintheField)
                    {
                        //player1.board.GetComponent<SubBoard>().M.GetComponent<MeleeZone>().melee[i].GetComponent<MoveCard>().MoveToCemetery(player1.board.GetComponent<SubBoard>().M.GetComponent<MeleeZone>().melee[i], player1.board);
                        player1.board.GetComponent<SubBoard>().M.GetComponent<MeleeZone>().melee[i].GetComponent<MoveCard>().MoveToCemetery();
                    }
                }
                for (int i = 0; i < player1.board.GetComponent<SubBoard>().R.GetComponent<RangedZone>().ranged.Count; i++)
                {
                    if (!player1.board.GetComponent<SubBoard>().R.GetComponent<RangedZone>().ranged[i].GetComponent<CardDisplay>().card.stayintheField)
                    {
                        //player1.board.GetComponent<SubBoard>().R.GetComponent<RangedZone>().ranged[i].GetComponent<MoveCard>().MoveToCemetery(player1.board.GetComponent<SubBoard>().R.GetComponent<RangedZone>().ranged[i], player1.board);
                        player1.board.GetComponent<SubBoard>().R.GetComponent<RangedZone>().ranged[i].GetComponent<MoveCard>().MoveToCemetery();
                    }
                }
                for (int i = 0; i < player1.board.GetComponent<SubBoard>().S.GetComponent<SiegeZone>().siege.Count; i++)
                {
                    if (!player1.board.GetComponent<SubBoard>().S.GetComponent<SiegeZone>().siege[i].GetComponent<CardDisplay>().card.stayintheField)
                    {
                        //player1.board.GetComponent<SubBoard>().S.GetComponent<SiegeZone>().siege[i].GetComponent<MoveCard>().MoveToCemetery(player1.board.GetComponent<SubBoard>().S.GetComponent<SiegeZone>().siege[i], player1.board);
                        player1.board.GetComponent<SubBoard>().S.GetComponent<SiegeZone>().siege[i].GetComponent<MoveCard>().MoveToCemetery();
                    }
                }
        */

        for (int i = 0; i < player1.board.GetComponent<SubBoard>().Increase.GetComponent<IncreaseZone>().increase.Length; i++)
        {
            if (player1.board.GetComponent<SubBoard>().Increase.GetComponent<IncreaseZone>().increase[i] != null)
            {
                //player1.board.GetComponent<SubBoard>().Increase.GetComponent<IncreaseZone>().increase[i].GetComponent<MoveCard>().MoveToCemetery(player1.board.GetComponent<SubBoard>().Increase.GetComponent<IncreaseZone>().increase[i], player1.board);
                player1.board.GetComponent<SubBoard>().Increase.GetComponent<IncreaseZone>().increase[i].GetComponent<MoveCard>().MoveToCemetery();
            }
        }
        if (player1.board.GetComponent<SubBoard>().Climate.GetComponent<ClimateZone>().climate != GameObject.FindGameObjectWithTag("ClimateCard"))
        {
            player1.board.GetComponent<SubBoard>().Climate.GetComponent<ClimateZone>().climate.GetComponent<MoveCard>().MoveToCemetery(player1.board.GetComponent<SubBoard>().Climate.GetComponent<ClimateZone>().climate, player1.board);
            //player1.board.GetComponent<SubBoard>().Climate.GetComponent<ClimateZone>().climate.GetComponent<MoveCard>().MoveToCemetery();
        }

        //player1.board.GetComponent<SubBoard>().M.GetComponent<MeleeZone>().melee = new();//.Clear();
        //player1.board.GetComponent<SubBoard>().R.GetComponent<RangedZone>().ranged = new();//.Clear();
        //player1.board.GetComponent<SubBoard>().S.GetComponent<SiegeZone>().siege = new();//.Clear();
        //player1.board.GetComponent<SubBoard>().Increase.GetComponent<IncreaseZone>().increase = new GameObject[3];
        player1.board.GetComponent<SubBoard>().Climate.GetComponent<ClimateZone>().climate = GameObject.FindGameObjectWithTag("ClimateCard");

        k = 0;
        while (player2.board.GetComponent<SubBoard>().M.GetComponent<MeleeZone>().melee.Count != 0)
        {
            if (k >= player2.board.GetComponent<SubBoard>().M.GetComponent<MeleeZone>().melee.Count) break;

            if (!player2.board.GetComponent<SubBoard>().M.GetComponent<MeleeZone>().melee[k].GetComponent<CardDisplay>().card.stayintheField)
            {
                player2.board.GetComponent<SubBoard>().M.GetComponent<MeleeZone>().melee[k].GetComponent<MoveCard>().MoveToCemetery();
            }
            else k++;
        }
        k = 0;
        while (player2.board.GetComponent<SubBoard>().R.GetComponent<RangedZone>().ranged.Count != 0)
        {
            if (k >= player2.board.GetComponent<SubBoard>().R.GetComponent<RangedZone>().ranged.Count) break;

            if (!player2.board.GetComponent<SubBoard>().R.GetComponent<RangedZone>().ranged[k].GetComponent<CardDisplay>().card.stayintheField)
            {
                player2.board.GetComponent<SubBoard>().R.GetComponent<RangedZone>().ranged[k].GetComponent<MoveCard>().MoveToCemetery();
            }
            else k++;
        }
        k = 0;
        while (player2.board.GetComponent<SubBoard>().S.GetComponent<SiegeZone>().siege.Count != 0)
        {
            if (k >= player2.board.GetComponent<SubBoard>().S.GetComponent<SiegeZone>().siege.Count) break;

            if (!player2.board.GetComponent<SubBoard>().S.GetComponent<SiegeZone>().siege[k].GetComponent<CardDisplay>().card.stayintheField)
            {
                player2.board.GetComponent<SubBoard>().S.GetComponent<SiegeZone>().siege[k].GetComponent<MoveCard>().MoveToCemetery();
            }
            else k++;
        }
        /*
        for (int i = 0; i < player2.board.GetComponent<SubBoard>().M.GetComponent<MeleeZone>().melee.Count; i++)
        {
            if (!player2.board.GetComponent<SubBoard>().M.GetComponent<MeleeZone>().melee[i].GetComponent<CardDisplay>().card.stayintheField)
            {
                //player2.board.GetComponent<SubBoard>().M.GetComponent<MeleeZone>().melee[i].GetComponent<MoveCard>().MoveToCemetery(player2.board.GetComponent<SubBoard>().M.GetComponent<MeleeZone>().melee[i], player2.board);
                player2.board.GetComponent<SubBoard>().M.GetComponent<MeleeZone>().melee[i].GetComponent<MoveCard>().MoveToCemetery();
            }
        }
        for (int i = 0; i < player2.board.GetComponent<SubBoard>().R.GetComponent<RangedZone>().ranged.Count; i++)
        {
            if (!player2.board.GetComponent<SubBoard>().R.GetComponent<RangedZone>().ranged[i].GetComponent<CardDisplay>().card.stayintheField)
            {
                //player2.board.GetComponent<SubBoard>().R.GetComponent<RangedZone>().ranged[i].GetComponent<MoveCard>().MoveToCemetery(player2.board.GetComponent<SubBoard>().R.GetComponent<RangedZone>().ranged[i], player2.board);
                player2.board.GetComponent<SubBoard>().R.GetComponent<RangedZone>().ranged[i].GetComponent<MoveCard>().MoveToCemetery();
            }
        }
        for (int i = 0; i < player2.board.GetComponent<SubBoard>().S.GetComponent<SiegeZone>().siege.Count; i++)
        {
            if (!player2.board.GetComponent<SubBoard>().S.GetComponent<SiegeZone>().siege[i].GetComponent<CardDisplay>().card.stayintheField)
            {
                //player2.board.GetComponent<SubBoard>().S.GetComponent<SiegeZone>().siege[i].GetComponent<MoveCard>().MoveToCemetery(player2.board.GetComponent<SubBoard>().S.GetComponent<SiegeZone>().siege[i], player2.board);
                player2.board.GetComponent<SubBoard>().S.GetComponent<SiegeZone>().siege[i].GetComponent<MoveCard>().MoveToCemetery();
            }
        }
        */

        for (int i = 0; i < player2.board.GetComponent<SubBoard>().Increase.GetComponent<IncreaseZone>().increase.Length; i++)
        {
            if (player2.board.GetComponent<SubBoard>().Increase.GetComponent<IncreaseZone>().increase[i] != null)
            {
                //player2.board.GetComponent<SubBoard>().Increase.GetComponent<IncreaseZone>().increase[i].GetComponent<MoveCard>().MoveToCemetery(player2.board.GetComponent<SubBoard>().Increase.GetComponent<IncreaseZone>().increase[i], player2.board);
                player2.board.GetComponent<SubBoard>().Increase.GetComponent<IncreaseZone>().increase[i].GetComponent<MoveCard>().MoveToCemetery();
            }
        }
        if (player2.board.GetComponent<SubBoard>().Climate.GetComponent<ClimateZone>().climate != GameObject.FindGameObjectWithTag("ClimateCard"))
        {
            player2.board.GetComponent<SubBoard>().Climate.GetComponent<ClimateZone>().climate.GetComponent<MoveCard>().MoveToCemetery(player2.board.GetComponent<SubBoard>().Climate.GetComponent<ClimateZone>().climate, player2.board);
            //player2.board.GetComponent<SubBoard>().Climate.GetComponent<ClimateZone>().climate.GetComponent<MoveCard>().MoveToCemetery();
        }

        //player2.board.GetComponent<SubBoard>().M.GetComponent<MeleeZone>().melee = new();//.Clear();
        //player2.board.GetComponent<SubBoard>().R.GetComponent<RangedZone>().ranged = new();//.Clear();
        //player2.board.GetComponent<SubBoard>().S.GetComponent<SiegeZone>().siege = new();//.Clear();
        //player2.board.GetComponent<SubBoard>().Increase.GetComponent<IncreaseZone>().increase = new GameObject[3];
        player2.board.GetComponent<SubBoard>().Climate.GetComponent<ClimateZone>().climate = GameObject.FindGameObjectWithTag("ClimateCard");

    }
    public void StartRound()
    {
        if (currentRound == 4 || (currentRound == 3 && (player1.RoundsWon == 2 || player2.RoundsWon == 2)))
        {
            UIRuntime.ShowMessage("End of the Game");
        }
        else
        {
            UIRuntime.ShowMessage("Start of Round " + currentRound);
        }

        player1.passTurn = false;
        player2.passTurn = false;

        if (player1.deck.CompareTag("Deck Resistance"))
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
        //if (Input.GetKeyDown(KeyCode.Escape)) Application.Quit();
        if (Input.GetKeyDown(KeyCode.R)) SceneManager.LoadScene("GameScene");
        if (Input.GetKeyDown(KeyCode.Space)) SceneManager.LoadScene("StartMenuScene");

    }
    public void ChangeImageCardsinHand()
    {
        if (player1.isPlaying)
        {
            for (int i = 0; i < player1.hand.CardsInHand.Count; i++)
            {
                if (player1.hand.CardsInHand[i] != null)
                {
                    if (!player1.hand.CardsInHand[i].GetComponent<CardDisplay>().card.inTheField)
                    {
                        player1.hand.CardsInHand[i].GetComponent<CardDisplay>().GetComponent<SpriteRenderer>().sprite = player1.hand.CardsInHand[i].GetComponent<CardDisplay>().card.CardImageForehead;
                    }
                }

            }
            for (int i = 0; i < player2.hand.CardsInHand.Count; i++)
            {
                if (player2.hand.CardsInHand[i] != null)
                {
                    if (!player2.hand.CardsInHand[i].GetComponent<CardDisplay>().card.inTheField)
                    {
                        player2.hand.CardsInHand[i].GetComponent<CardDisplay>().GetComponent<SpriteRenderer>().sprite = player2.hand.CardsInHand[i].GetComponent<CardDisplay>().card.CardImageBack;
                    }
                }
            }
        }
        else
        {
            for (int i = 0; i < player1.hand.CardsInHand.Count; i++)
            {
                if (player1.hand.CardsInHand[i] != null)
                {
                    if (!player1.hand.CardsInHand[i].GetComponent<CardDisplay>().card.inTheField)
                    {
                        player1.hand.CardsInHand[i].GetComponent<CardDisplay>().GetComponent<SpriteRenderer>().sprite = player1.hand.CardsInHand[i].GetComponent<CardDisplay>().card.CardImageBack;
                    }
                }
            }
            for (int i = 0; i < player2.hand.CardsInHand.Count; i++)
            {
                if (player2.hand.CardsInHand[i] != null)
                {
                    if (!player2.hand.CardsInHand[i].GetComponent<CardDisplay>().card.inTheField)
                    {
                        player2.hand.CardsInHand[i].GetComponent<CardDisplay>().GetComponent<SpriteRenderer>().sprite = player2.hand.CardsInHand[i].GetComponent<CardDisplay>().card.CardImageForehead;
                    }
                }
            }
        }
    }
    public void StartUpdateCards()
    {
        for (int i = 0; i < GameObject.FindGameObjectWithTag("Deck Pirates").GetComponent<Decks>().deck.Count; i++)
        {
            GameObject.FindGameObjectWithTag("Deck Pirates").GetComponent<Decks>().deck[i].GetComponent<CardDisplay>().card.inTheField = false;
            GameObject.FindGameObjectWithTag("Deck Pirates").GetComponent<Decks>().deck[i].GetComponent<CardDisplay>().card.affectedByClimate = false;
            GameObject.FindGameObjectWithTag("Deck Pirates").GetComponent<Decks>().deck[i].GetComponent<CardDisplay>().card.affectedByIncrease = false;
            GameObject.FindGameObjectWithTag("Deck Pirates").GetComponent<Decks>().deck[i].GetComponent<CardDisplay>().card.stayintheField = false;
            GameObject.FindGameObjectWithTag("Deck Pirates").GetComponent<Decks>().deck[i].GetComponent<CardDisplay>().card.Power = GameObject.FindGameObjectWithTag("Deck Pirates").GetComponent<Decks>().deck[i].GetComponent<CardDisplay>().card.StartPower;
        }
        for (int i = 0; i < GameObject.FindGameObjectWithTag("Deck Resistance").GetComponent<Decks>().deck.Count; i++)
        {
            GameObject.FindGameObjectWithTag("Deck Resistance").GetComponent<Decks>().deck[i].GetComponent<CardDisplay>().card.inTheField = false;
            GameObject.FindGameObjectWithTag("Deck Resistance").GetComponent<Decks>().deck[i].GetComponent<CardDisplay>().card.affectedByClimate = false;
            GameObject.FindGameObjectWithTag("Deck Resistance").GetComponent<Decks>().deck[i].GetComponent<CardDisplay>().card.affectedByIncrease = false;
            GameObject.FindGameObjectWithTag("Deck Resistance").GetComponent<Decks>().deck[i].GetComponent<CardDisplay>().card.stayintheField = false;
            GameObject.FindGameObjectWithTag("Deck Resistance").GetComponent<Decks>().deck[i].GetComponent<CardDisplay>().card.Power = GameObject.FindGameObjectWithTag("Deck Resistance").GetComponent<Decks>().deck[i].GetComponent<CardDisplay>().card.StartPower;
        }

        /*
        for (int i = 0; i < player1.hand.CardsInDeck.Count; i++)
        {
            player1.hand.CardsInHand[i].GetComponent<CardDisplay>().card.inTheField = false;
        }
        for (int i = 0; i < player2.hand.CardsInDeck.Count; i++)
        {
            player2.hand.CardsInHand[i].GetComponent<CardDisplay>().card.inTheField = false;
        }
        */
        /*
        for (int i = 0; i < player1.hand.CardsInHand.Count; i++)
        {
            player1.hand.CardsInHand[i].GetComponent<CardDisplay>().card.inTheField = false;
        }
        for (int i = 0; i < player2.hand.CardsInHand.Count; i++)
        {
            player2.hand.CardsInHand[i].GetComponent<CardDisplay>().card.inTheField = false;
        }
        */

    }
    public void UpdateClimateAndIncrease()
    {
        if (player1.board.GetComponent<SubBoard>().Climate.GetComponent<ClimateZone>().climate != GameObject.FindGameObjectWithTag("ClimateCard"))
        {
            Effects.ActivateEffect(player1.board.GetComponent<SubBoard>().Climate.GetComponent<ClimateZone>().climate);
        }

        if (player2.board.GetComponent<SubBoard>().Climate.GetComponent<ClimateZone>().climate != GameObject.FindGameObjectWithTag("ClimateCard"))
        {
            Effects.ActivateEffect(player2.board.GetComponent<SubBoard>().Climate.GetComponent<ClimateZone>().climate);
        }


        for (int i = 0; i < player1.board.GetComponent<SubBoard>().Increase.GetComponent<IncreaseZone>().increase.Length; i++)
        {
            if (player1.board.GetComponent<SubBoard>().Increase.GetComponent<IncreaseZone>().increase[i] != null)
            {
                Effects.ActivateEffect(player1.board.GetComponent<SubBoard>().Increase.GetComponent<IncreaseZone>().increase[i]);
            }
        }

        for (int i = 0; i < player2.board.GetComponent<SubBoard>().Increase.GetComponent<IncreaseZone>().increase.Length; i++)
        {
            if (player2.board.GetComponent<SubBoard>().Increase.GetComponent<IncreaseZone>().increase[i] != null)
            {
                Effects.ActivateEffect(player2.board.GetComponent<SubBoard>().Increase.GetComponent<IncreaseZone>().increase[i]);
            }

        }


    }
}