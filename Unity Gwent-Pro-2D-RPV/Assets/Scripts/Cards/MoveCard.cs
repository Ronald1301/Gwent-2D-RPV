using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class MoveCard : MonoBehaviour
{
    GameObject subBoard;
    GameObject subBoard2;
    public GameObject GameManager;

    private GameManager componentGameManager;
    public ScriptUIRuntime UIRuntime;
    public GameObject UISelectField;
    private GameObject climateCard;

    private bool isClicked = false;
    private bool activeLure = false;

    private bool IsMRS = false;

    void Start()
    {
        //if (GameManager == null) GameManager = GameObject.FindGameObjectWithTag("GameManager");
        if (UISelectField == null) UISelectField = GameObject.FindGameObjectWithTag("UI Select Field");
        if (UIRuntime == null) UIRuntime = GameObject.FindGameObjectWithTag("UI Runtime").GetComponent<ScriptUIRuntime>();
        componentGameManager = GameManager.GetComponent<GameManager>();
        climateCard = GameObject.FindGameObjectWithTag("ClimateCard");


        StartCoroutine(WaitForClick());
        //GameManager = GameObject.Find("GameManager");
        //UIRuntime = GameObject.FindGameObjectWithTag("UI Runtime").GetComponent<ScriptUIRuntime>();

        if (componentGameManager.player1.hand.CardsInHand.Contains(this.gameObject))
        {
            // subBoard = GameObject.FindGameObjectWithTag("SubBoard1");
            subBoard = componentGameManager.player1.subBoard.gameObject;
            subBoard2 = componentGameManager.player2.subBoard.gameObject;
            //Debug.Log("SubBoard1");
        }
        else if (componentGameManager.player2.hand.CardsInHand.Contains(this.gameObject))
        {
            //subBoard = GameObject.FindGameObjectWithTag("SubBoard2");
            subBoard = componentGameManager.player2.subBoard.gameObject;
            subBoard2 = componentGameManager.player1.subBoard.gameObject;
            //Debug.Log("SubBoard2");
        }
        // subBoard = GameObject.FindGameObjectWithTag("SubBoard1");
    }
    void Update()
    {
        if (GameManager == null) GameManager = GameObject.FindGameObjectWithTag("GameManager");
        if (Input.GetMouseButtonDown(1) && activeLure)
        {
            isClicked = true;
        }
    }
    private void OnMouseDown()
    {
        if ((componentGameManager.player1.isPlaying &&
        componentGameManager.player1.subBoard.GetComponent<SubBoard>().Hand.GetComponent<Hand>().CardsInHand.Contains(this.gameObject))
            || (componentGameManager.player2.isPlaying &&
            componentGameManager.player2.subBoard.GetComponent<SubBoard>().Hand.GetComponent<Hand>().CardsInHand.Contains(this.gameObject)))
        {
            Move();
            //this.gameObject.GetComponent<CardDisplay>().card.inTheField = true;

            if (!IsMRS)
            {
                BeforeMoveCard();
            }
        }
    }
    public void Move()
    {
        gameObject.transform.localScale = new Vector3(1, 1, 1);

        //Move card to the corresponding zone
        if (GetComponent<CardDisplay>().cardData.Type == CardData.CardType.Unit)
        {
            switch (GetComponent<CardDisplay>().cardData.Range)
            {
                case CardData.EnumRange.M:
                    MoveToM();
                    break;
                case CardData.EnumRange.R:
                    MoveToR();
                    break;
                case CardData.EnumRange.S:
                    MoveToS();
                    break;
                case CardData.EnumRange.MR:
                case CardData.EnumRange.MS:
                case CardData.EnumRange.RS:
                case CardData.EnumRange.MRS:
                    IsMRS = true;
                    StartCoroutine(WaitForButtonClick());
                    UISelectField.SetActive(true);
                    UISelectField.GetComponent<ScriptSelectField>().LoadSelectField(GetComponent<CardDisplay>().cardData.Range);
                    break;
            }
            /*
                        if (GetComponent<CardDisplay>().cardData.TypeField == 'M')
                        {
                            MoveToM();
                            Debug.Log("Move to M");
                        }
                        else if (GetComponent<CardDisplay>().cardData.TypeField == 'R')
                        {
                            MoveToR();
                            Debug.Log("Move to R");
                        }
                        else if (GetComponent<CardDisplay>().cardData.TypeField == 'S')
                        {
                            MoveToS();
                            Debug.Log("Move to S");
                        }
            */
        }
        else
        {
            if (gameObject.GetComponent<CardDisplay>().cardData.TypeSpecialCard == CardData.SubTypeSpecialCard.Climate)
            {
                if (subBoard.GetComponent<SubBoard>().Climate.GetComponent<ClimateZone>().climate != climateCard)
                {
                    Effects.DisableEffectClimate(subBoard.GetComponent<SubBoard>().Climate.GetComponent<ClimateZone>().climate);
                    subBoard.GetComponent<SubBoard>().Climate.GetComponent<ClimateZone>().climate.GetComponent<MoveCard>().MoveToCemetery(subBoard.GetComponent<SubBoard>().Climate.GetComponent<ClimateZone>().climate, subBoard.GetComponent<SubBoard>());
                    //this.gameObject.transform.position = componentFameManager.player1.board.Climate.GetComponent<ClimateZone>().climate.transform.position;
                }
                MoveToClimate();
                Debug.Log("Move to Climate");
            }
            else if (gameObject.GetComponent<CardDisplay>().cardData.TypeSpecialCard == CardData.SubTypeSpecialCard.Increase)
            {
                if (GetComponent<CardDisplay>().cardData.Range == CardData.EnumRange.M || GetComponent<CardDisplay>().cardData.TypeField == 'M')
                {
                    MoveToIncrease(0);
                    Debug.Log("Move to Increase en 0");
                }
                else if (GetComponent<CardDisplay>().cardData.Range == CardData.EnumRange.R || GetComponent<CardDisplay>().cardData.TypeField == 'R')
                {
                    MoveToIncrease(1);
                    Debug.Log("Move to Increase en 1");
                }
                else if (GetComponent<CardDisplay>().cardData.Range == CardData.EnumRange.S || GetComponent<CardDisplay>().cardData.TypeField == 'S')
                {
                    MoveToIncrease(2);
                    Debug.Log("Move to Increase en 2");
                }
            }
            else if (gameObject.GetComponent<CardDisplay>().cardData.TypeSpecialCard == CardData.SubTypeSpecialCard.Clearance)
            {
                if (subBoard == componentGameManager.player1.subBoard.gameObject)
                {
                    if (componentGameManager.player2.subBoard.Climate.GetComponent<ClimateZone>().climate != climateCard)
                    {
                        Effects.DisableEffectClimate(componentGameManager.player2.subBoard.Climate.GetComponent<ClimateZone>().climate);
                        componentGameManager.player2.subBoard.Climate.GetComponent<ClimateZone>().climate.GetComponent<MoveCard>().MoveToCemetery(componentGameManager.player2.subBoard.Climate.GetComponent<ClimateZone>().climate, subBoard2.GetComponent<SubBoard>());
                        Debug.Log("Move to Cemetery card climate");
                    }
                }
                else /*if (subBoard == componentFameManager.player2.board.gameObject)*/
                {
                    if (componentGameManager.player1.subBoard.Climate.GetComponent<ClimateZone>().climate != climateCard)
                    {
                        Effects.DisableEffectClimate(componentGameManager.player1.subBoard.Climate.GetComponent<ClimateZone>().climate);
                        componentGameManager.player1.subBoard.Climate.GetComponent<ClimateZone>().climate.GetComponent<MoveCard>().MoveToCemetery(componentGameManager.player1.subBoard.Climate.GetComponent<ClimateZone>().climate, subBoard2.GetComponent<SubBoard>());
                        Debug.Log("Move to Cemetery card climate");
                    }
                }
                MoveToClimate();
                Debug.Log("Move to Climate card clearance");
            }
            else if (gameObject.GetComponent<CardDisplay>().cardData.TypeSpecialCard == CardData.SubTypeSpecialCard.Lure)
            {
                //if (Effects.IsRowEMpty(1) && Effects.IsRowEMpty(2) && Effects.IsRowEMpty(3))
                if (subBoard.GetComponent<SubBoard>().M.GetComponent<MeleeZone>().melee.Count == 0 &&
                subBoard.GetComponent<SubBoard>().R.GetComponent<RangedZone>().ranged.Count == 0 &&
                subBoard.GetComponent<SubBoard>().S.GetComponent<SiegeZone>().siege.Count == 0)
                {
                    int indextype = UnityEngine.Random.Range(1, 3);
                    if (indextype == 1)
                    {
                        MoveToM();
                        Debug.Log("Move to M card lure");
                    }
                    else if (indextype == 2)
                    {
                        MoveToR();
                        Debug.Log("Move to R card lure");
                    }
                    else if (indextype == 3)
                    {
                        MoveToS();
                        Debug.Log("Move to S card lure");
                    }
                    UIRuntime.UIUpdate();
                    componentGameManager.ChangeTurn();
                }
                else
                {
                    activeLure = true;
                    StartCoroutine(WaitForClick());
                }
            }
        }

        subBoard.GetComponent<SubBoard>().Hand.GetComponent<Hand>().CardsInHand.Remove(this.gameObject);

        if (componentGameManager.player1.isPlaying && this.gameObject.GetComponent<CardDisplay>().cardData.TypeSpecialCard != CardData.SubTypeSpecialCard.Lure)
        {
            componentGameManager.player1.PlayedACard = true;
        }
        else if (componentGameManager.player2.isPlaying && this.gameObject.GetComponent<CardDisplay>().cardData.TypeSpecialCard != CardData.SubTypeSpecialCard.Lure)
        {
            componentGameManager.player2.PlayedACard = true;
        }

        /*
                if (subBoard.GetComponent<SubBoard>().Hand.GetComponent<Hand>().CardsInHand.Count == 0)
                {
                    componentFameManager.UpdatePoints();
                    UIRuntime.UIUpdate();
                    componentFameManager.ChangeTurn();
                }
                else
                {
                    componentFameManager.UpdatePoints();
                    UIRuntime.UIUpdate();
                }
                */
    }

    private void BeforeMoveCard()
    {

        if (this.gameObject.GetComponent<CardDisplay>().cardData.TypeSpecialCard != CardData.SubTypeSpecialCard.Lure)
        {
            UIRuntime.UIUpdate();
            Effects.ActivateEffect(gameObject);
            UIRuntime.UIUpdate();
            CheckHandEmpty();
            componentGameManager.ChangeTurn();
        }
        else
        {
            componentGameManager.UpdatePoints();
            UIRuntime.UIUpdate();
        }
    }

    void CheckHandEmpty()
    {
        if (componentGameManager.player1.isPlaying && componentGameManager.player1.subBoard.GetComponent<SubBoard>().Hand.GetComponent<Hand>().CardsInHand.Count == 0)
        {
            componentGameManager.player1.passTurn = true;
        }
        if (componentGameManager.player2.isPlaying && componentGameManager.player2.subBoard.GetComponent<SubBoard>().Hand.GetComponent<Hand>().CardsInHand.Count == 0)
        {
            componentGameManager.player2.passTurn = true;
        }
    }

    public void MoveToM()
    {
        subBoard.GetComponent<SubBoard>().M.GetComponent<MeleeZone>().melee.Add(this.gameObject);
        subBoard.GetComponent<SubBoard>().Hand.GetComponent<Hand>().CardsInHand.Remove(this.gameObject);
        this.gameObject.transform.SetParent(subBoard.GetComponent<SubBoard>().M.transform, false);
    }
    public void MoveToR()
    {
        subBoard.GetComponent<SubBoard>().R.GetComponent<RangedZone>().ranged.Add(this.gameObject);
        subBoard.GetComponent<SubBoard>().Hand.GetComponent<Hand>().CardsInHand.Remove(this.gameObject);
        this.gameObject.transform.SetParent(subBoard.GetComponent<SubBoard>().R.transform, false);
    }
    public void MoveToS()
    {
        subBoard.GetComponent<SubBoard>().S.GetComponent<SiegeZone>().siege.Add(this.gameObject);
        subBoard.GetComponent<SubBoard>().Hand.GetComponent<Hand>().CardsInHand.Remove(this.gameObject);
        this.gameObject.transform.SetParent(subBoard.GetComponent<SubBoard>().S.transform, false);
    }
    public void MoveToIncrease(int i)
    {
        subBoard.GetComponent<SubBoard>().Increase.GetComponent<IncreaseZone>().increase[i] = this.gameObject;
        subBoard.GetComponent<SubBoard>().Hand.GetComponent<Hand>().CardsInHand.Remove(this.gameObject);
        //subBoard.GetComponent<SubBoard>().Increase.GetComponent<IncreaseZone>().increase[i].transform.SetParent(subBoard.GetComponent<SubBoard>().Increase.transform, false);
        this.gameObject.transform.SetParent(subBoard.GetComponent<SubBoard>().Increase.transform, false);
        switch (i)
        {
            case 0:
                this.gameObject.transform.position = subBoard.GetComponent<SubBoard>().Increase.transform.GetChild(0).transform.position;
                break;
            case 1:
                this.gameObject.transform.position = subBoard.GetComponent<SubBoard>().Increase.transform.GetChild(1).transform.position;
                break;
            default:
                this.gameObject.transform.position = subBoard.GetComponent<SubBoard>().Increase.transform.GetChild(2).transform.position;
                break;

        }
        //this.gameObject.transform.position = new Vector3(transform.position.x, transform.position.y - 4, transform.position.z);
    }
    public void MoveToClimate()
    {
        subBoard.GetComponent<SubBoard>().Climate.GetComponent<ClimateZone>().climate = this.gameObject;
        subBoard.GetComponent<SubBoard>().Hand.GetComponent<Hand>().CardsInHand.Remove(this.gameObject);
        this.gameObject.transform.SetParent(subBoard.GetComponent<SubBoard>().Climate.transform, false);
        this.gameObject.transform.position = new Vector3(transform.position.x - 7, transform.position.y, transform.position.z - 0.1f);
    }
    public void MoveToCemetery()
    {
        //subBoard.GetComponent<SubBoard>().Cemetery.GetComponent<CemeteryZone>().GetComponent<SpriteRenderer>().sprite = this.gameObject.GetComponent<SpriteRenderer>().sprite;
        subBoard.GetComponent<SubBoard>().Cemetery.GetComponent<CemeteryZone>().Cemetery.Add(this.gameObject);

        if (subBoard.GetComponent<SubBoard>().Hand.GetComponent<Hand>().CardsInHand.Contains(this.gameObject))
        {
            subBoard.GetComponent<SubBoard>().Hand.GetComponent<Hand>().CardsInHand.Remove(this.gameObject);
        }
        else if (subBoard.GetComponent<SubBoard>().M.GetComponent<MeleeZone>().melee.Contains(this.gameObject))
        {
            subBoard.GetComponent<SubBoard>().M.GetComponent<MeleeZone>().melee.Remove(this.gameObject);
        }
        else if (subBoard.GetComponent<SubBoard>().R.GetComponent<RangedZone>().ranged.Contains(this.gameObject))
        {
            subBoard.GetComponent<SubBoard>().R.GetComponent<RangedZone>().ranged.Remove(this.gameObject);
        }
        else if (subBoard.GetComponent<SubBoard>().S.GetComponent<SiegeZone>().siege.Contains(this.gameObject))
        {
            subBoard.GetComponent<SubBoard>().S.GetComponent<SiegeZone>().siege.Remove(this.gameObject);
        }
        else if (subBoard.GetComponent<SubBoard>().Increase.GetComponent<IncreaseZone>().increase[0] == this.gameObject)
        {
            subBoard.GetComponent<SubBoard>().Increase.GetComponent<IncreaseZone>().increase[0] = null;
        }
        else if (subBoard.GetComponent<SubBoard>().Increase.GetComponent<IncreaseZone>().increase[1] == this.gameObject)
        {
            subBoard.GetComponent<SubBoard>().Increase.GetComponent<IncreaseZone>().increase[1] = null;
        }
        else if (subBoard.GetComponent<SubBoard>().Increase.GetComponent<IncreaseZone>().increase[2] == this.gameObject)
        {
            subBoard.GetComponent<SubBoard>().Increase.GetComponent<IncreaseZone>().increase[2] = null;
        }
        else if (subBoard.GetComponent<SubBoard>().Climate.GetComponent<ClimateZone>().climate == this.gameObject)
        {
            subBoard.GetComponent<SubBoard>().Climate.GetComponent<ClimateZone>().climate = GameObject.FindGameObjectWithTag("ClimateCard");
        }
        //Destroy(this.gameObject);
        this.gameObject.transform.SetParent(subBoard.GetComponent<SubBoard>().Cemetery.transform, false);
    }
    public void MoveToCemetery(GameObject card, SubBoard board)
    {
        //board.Cemetery.GetComponent<CemeteryZone>().GetComponent<SpriteRenderer>().sprite = card.GetComponent<SpriteRenderer>().sprite;
        board.Cemetery.GetComponent<CemeteryZone>().Cemetery.Add(card);

        if (board.Hand.GetComponent<Hand>().CardsInHand.Contains(card))
        {
            board.Hand.GetComponent<Hand>().CardsInHand.Remove(card);
        }
        else if (board.M.GetComponent<MeleeZone>().melee.Contains(card))
        {
            board.M.GetComponent<MeleeZone>().melee.Remove(card);
        }
        else if (board.R.GetComponent<RangedZone>().ranged.Contains(card))
        {
            board.R.GetComponent<RangedZone>().ranged.Remove(card);
        }
        else if (board.S.GetComponent<SiegeZone>().siege.Contains(card))
        {
            board.S.GetComponent<SiegeZone>().siege.Remove(card);
        }
        else if (board.Increase.GetComponent<IncreaseZone>().increase[0] == card)
        {
            board.Increase.GetComponent<IncreaseZone>().increase[0] = null;
        }
        else if (board.Increase.GetComponent<IncreaseZone>().increase[1] == card)
        {
            board.Increase.GetComponent<IncreaseZone>().increase[1] = null;
        }
        else if (board.Increase.GetComponent<IncreaseZone>().increase[2] == card)
        {
            board.Increase.GetComponent<IncreaseZone>().increase[2] = null;
        }
        else if (board.Climate.GetComponent<ClimateZone>().climate == card)
        {
            board.Climate.GetComponent<ClimateZone>().climate = GameObject.FindGameObjectWithTag("ClimateCard");
        }
        //Destroy(card);
        card.transform.SetParent(board.GetComponent<SubBoard>().Cemetery.transform);
        // card.transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z - 0.1f);
    }
    public void MoveToHand()
    {
        subBoard.GetComponent<SubBoard>().Hand.GetComponent<Hand>().CardsInHand.Add(this.gameObject);

        if (subBoard.GetComponent<SubBoard>().M.GetComponent<MeleeZone>().melee.Contains(this.gameObject))
        {
            subBoard.GetComponent<SubBoard>().M.GetComponent<MeleeZone>().melee.Remove(this.gameObject);
        }
        else if (subBoard.GetComponent<SubBoard>().R.GetComponent<RangedZone>().ranged.Contains(this.gameObject))
        {
            subBoard.GetComponent<SubBoard>().R.GetComponent<RangedZone>().ranged.Remove(this.gameObject);
        }
        else if (subBoard.GetComponent<SubBoard>().S.GetComponent<SiegeZone>().siege.Contains(this.gameObject))
        {
            subBoard.GetComponent<SubBoard>().S.GetComponent<SiegeZone>().siege.Remove(this.gameObject);
        }
        /*
        else if (subBoard.GetComponent<SubBoard>().Increase.GetComponent<IncreaseZone>().increase[0] == this.gameObject)
        {
            subBoard.GetComponent<SubBoard>().Increase.GetComponent<IncreaseZone>().increase[0] = null;
        }
        else if (subBoard.GetComponent<SubBoard>().Increase.GetComponent<IncreaseZone>().increase[1] == this.gameObject)
        {
            subBoard.GetComponent<SubBoard>().Increase.GetComponent<IncreaseZone>().increase[1] = null;
        }
        else if (subBoard.GetComponent<SubBoard>().Increase.GetComponent<IncreaseZone>().increase[2] == this.gameObject)
        {
            subBoard.GetComponent<SubBoard>().Increase.GetComponent<IncreaseZone>().increase[2] = null;
        }
        else if (subBoard.GetComponent<SubBoard>().Climate.GetComponent<ClimateZone>().climate == this.gameObject)
        {
            subBoard.GetComponent<SubBoard>().Climate.GetComponent<ClimateZone>().climate = GameObject.FindGameObjectWithTag("ClimateCard");
        }
        */
        this.gameObject.transform.SetParent(subBoard.GetComponent<SubBoard>().Hand.transform, false);
        this.gameObject.transform.localScale = new Vector3(0.4f, 0.6f, 1);
        this.gameObject.GetComponent<CardDisplay>().cardData.inTheField = false;
        this.gameObject.GetComponent<CardDisplay>().cardData.stayintheField = false;
        this.gameObject.GetComponent<CardDisplay>().cardData.affectedByClimate = false;
        this.gameObject.GetComponent<CardDisplay>().cardData.affectedByIncrease = false;
        this.gameObject.GetComponent<CardDisplay>().cardData.Power = this.gameObject.GetComponent<CardDisplay>().cardData.StartPower;
    }
    public void MoveToDeck()
    {
        subBoard.GetComponent<SubBoard>().Hand.GetComponent<Hand>().CardsPriority.Add(this.gameObject);

        if (subBoard.GetComponent<SubBoard>().Hand.GetComponent<Hand>().CardsInHand.Contains(this.gameObject))
        {
            subBoard.GetComponent<SubBoard>().Hand.GetComponent<Hand>().CardsInHand.Remove(this.gameObject);
        }
        else if (subBoard.GetComponent<SubBoard>().M.GetComponent<MeleeZone>().melee.Contains(this.gameObject))
        {
            subBoard.GetComponent<SubBoard>().M.GetComponent<MeleeZone>().melee.Remove(this.gameObject);
        }
        else if (subBoard.GetComponent<SubBoard>().R.GetComponent<RangedZone>().ranged.Contains(this.gameObject))
        {
            subBoard.GetComponent<SubBoard>().R.GetComponent<RangedZone>().ranged.Remove(this.gameObject);
        }
        else if (subBoard.GetComponent<SubBoard>().S.GetComponent<SiegeZone>().siege.Contains(this.gameObject))
        {
            subBoard.GetComponent<SubBoard>().S.GetComponent<SiegeZone>().siege.Remove(this.gameObject);
        }
        else if (subBoard.GetComponent<SubBoard>().Increase.GetComponent<IncreaseZone>().increase[0] == this.gameObject)
        {
            subBoard.GetComponent<SubBoard>().Increase.GetComponent<IncreaseZone>().increase[0] = null;
        }
        else if (subBoard.GetComponent<SubBoard>().Increase.GetComponent<IncreaseZone>().increase[1] == this.gameObject)
        {
            subBoard.GetComponent<SubBoard>().Increase.GetComponent<IncreaseZone>().increase[1] = null;
        }
        else if (subBoard.GetComponent<SubBoard>().Increase.GetComponent<IncreaseZone>().increase[2] == this.gameObject)
        {
            subBoard.GetComponent<SubBoard>().Increase.GetComponent<IncreaseZone>().increase[2] = null;
        }
        else if (subBoard.GetComponent<SubBoard>().Climate.GetComponent<ClimateZone>().climate == this.gameObject)
        {
            subBoard.GetComponent<SubBoard>().Climate.GetComponent<ClimateZone>().climate = GameObject.FindGameObjectWithTag("ClimateCard");
        }
        this.gameObject.transform.SetParent(subBoard.GetComponent<SubBoard>().Hand.GetComponent<Hand>().CardsInDeck[0].transform, false);
    }
    //Card Lure
    public IEnumerator WaitForClick()
    {
        while (!isClicked)
        {
            yield return null;
        }
        // if (Input.GetMouseButtonDown(1))
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);
            if (hit.collider != null)
            {
                GameObject card = hit.collider.gameObject;
                if (CheckCardInBoard(card))
                {
                    if (card.GetComponent<CardDisplay>().cardData.Type == CardData.CardType.Unit)
                    {
                        if (card.GetComponent<CardDisplay>().cardData.TypeField == 'M')
                        {
                            card.GetComponent<MoveCard>().MoveToHand();
                            MoveToM();
                        }
                        else if (card.GetComponent<CardDisplay>().cardData.TypeField == 'R')
                        {
                            card.GetComponent<MoveCard>().MoveToHand();
                            MoveToR();
                        }
                        else if (card.GetComponent<CardDisplay>().cardData.TypeField == 'S')
                        {
                            card.GetComponent<MoveCard>().MoveToHand();
                            MoveToS();
                        }

                        isClicked = false;
                        activeLure = false;

                        CheckHandEmpty();

                        if (componentGameManager.player1.isPlaying)
                        {
                            componentGameManager.player1.PlayedACard = true;
                        }
                        else if (componentGameManager.player2.isPlaying)
                        {
                            componentGameManager.player2.PlayedACard = true;
                        }

                        componentGameManager.UpdatePoints();
                        UIRuntime.UIUpdate();
                        componentGameManager.ChangeTurn();

                    }
                }
            }
            isClicked = false;
        }
    }
    bool CheckCardInBoard(GameObject card)
    {
        if (subBoard.GetComponent<SubBoard>().M.GetComponent<MeleeZone>().melee.Contains(card))
            return true;
        else if (subBoard.GetComponent<SubBoard>().R.GetComponent<RangedZone>().ranged.Contains(card))
            return true;
        else if (subBoard.GetComponent<SubBoard>().S.GetComponent<SiegeZone>().siege.Contains(card))
            return true;
        else return false;
    }
    IEnumerator WaitForButtonClick()
    {
        yield return new WaitUntil(() => UISelectField.GetComponent<ScriptSelectField>().selected != '\0');
        // Continue with the rest of the code here

        if (UISelectField.GetComponent<ScriptSelectField>().selected == 'M')
        {
            MoveToM();
        }
        else if (UISelectField.GetComponent<ScriptSelectField>().selected == 'R')
        {
            MoveToR();
        }
        else if (UISelectField.GetComponent<ScriptSelectField>().selected == 'S')
        {
            MoveToS();
        }

        UISelectField.GetComponent<ScriptSelectField>().selected = '\0';
        BeforeMoveCard();
        /*
        UIRuntime.UIUpdate();
        Effects.ActivateEffect(gameObject);
        UIRuntime.UIUpdate();
        componentFameManager.ChangeTurn();
        */
    }

}
