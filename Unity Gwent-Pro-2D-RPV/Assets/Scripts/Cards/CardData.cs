using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[CreateAssetMenu(fileName = "New Card", menuName = "Card")]
public class CardData : ScriptableObject
{

    //    public CardData() { }
    public CardData(string cardName, string faction, CardType type, string description, Sprite cardImageForehead, Sprite cardImageback, int startPower, int power, char typeField, EnumRange range, SubTypeUnitCard typeUnitCard, SubTypeSpecialCard typeSpecialCard, TypeEffects effects)
    {
        this.cardName = cardName;
        this.faction = faction;
        this.type = type;
        this.description = description;
        this.cardImageForehead = cardImageForehead;
        this.cardImageback = cardImageback;
        this.startPower = startPower;
        this.power = power;
        this.typeField = typeField;
        this.range = range;
        this.typeUnitCard = typeUnitCard;
        this.typeSpecialCard = typeSpecialCard;
        this.effects = effects;
    }

    public CardData(string name, string faction, string description, string power, EnumRange range, CardType type, SubTypeUnitCard typeUnitCard, SubTypeSpecialCard typeSpecialCard, Queue<string> ability, Sprite frontImage, Sprite backImage)
    {
        this.cardName = name;
        this.type = type;
        this.typeUnitCard = typeUnitCard;
        this.typeSpecialCard = typeSpecialCard;
        this.faction = faction;
        this.cardImageForehead = frontImage;
        this.cardImageback = backImage;
        this.description = description;
        this.ListEffect = ability;
        this.typeField = ' ';

        switch (type)
        {
            case CardType.Unit:
                this.startPower = int.Parse(power);
                this.power = this.startPower;
                this.range = range;
                if (ability.Count == 0) this.effects = TypeEffects.None;
                else this.effects = TypeEffects.effectCardCompiler;
                break;

            case CardType.Special:
                switch (typeSpecialCard)
                {
                    case SubTypeSpecialCard.Climate:
                        this.range = range;
                        if (range == EnumRange.M || range == EnumRange.R)
                        {
                            this.description = "Reduces the points to 0  of all cards placed  in the " + range.ToString() + " rows  (own or the opponent's)";
                        }
                        else this.description = "Reduce points to 1 of all the cards placed in the " + range.ToString() + " rows  (own or opponent's)";
                        this.effects = TypeEffects.Climate;
                        break;
                    case SubTypeSpecialCard.Increase:
                        this.range = range;
                        this.description = " Increases all cards  in the " + range.ToString() + " row by 2 points";
                        this.effects = TypeEffects.Increase;
                        break;
                    case SubTypeSpecialCard.Clearance:
                        this.description = "Destroy the activated weather card  on the opponent's field.";
                        this.effects = TypeEffects.Clearance;
                        break;
                    case SubTypeSpecialCard.Lure:
                        this.description = " Select a card from the field  to return to the hand and  be replaced by this card";
                        this.effects = TypeEffects.None;
                        break;
                }
                this.Power = 0;
                this.startPower = 0;
                break;
            case CardType.Boss:
                this.Power = 0;
                this.startPower = 0;
                if (ability.Count == 0) this.effects = TypeEffects.None;
                else this.effects = TypeEffects.effectCardCompiler;
                break;
        }
    }

    [SerializeField] private string cardName;
    [SerializeField] private string faction;
    [SerializeField] private CardType type;
    [SerializeField] private string description;
    [SerializeField] private Sprite cardImageForehead;
    [SerializeField] private Sprite cardImageback;

    [SerializeField] private int startPower;
    [SerializeField] private int power;
    [SerializeField] private char typeField;
    [SerializeField] private EnumRange range;
    [SerializeField] private SubTypeUnitCard typeUnitCard;

    [SerializeField] private SubTypeSpecialCard typeSpecialCard;

    [SerializeField] private TypeEffects effects;

    public Queue<string> ListEffect;
    public int owner { get; set; }

    public bool stayintheField = false;
    public bool inTheField = false;
    public bool affectedByClimate = false;
    public bool affectedByIncrease = false;


    //Card
    public string CardName => cardName;
    public string Faction { get => faction; }
    public CardType Type { get => type; }
    public string Description { get => description; }
    public Sprite CardImageForehead { get => cardImageForehead; }
    public Sprite CardImageBack { get => cardImageback; }

    //UnitCards

    public int StartPower { get => startPower; set => startPower = value; }
    public int Power { get => power; set => power = value; }

    //public readonly char[] TypeField = { 'M', 'R', 'S' };
    // public readonly bool[] MRS = new bool[3];
    public char TypeField { get => typeField; }
    public EnumRange Range { get => range; }

    public SubTypeUnitCard TypeUnitCard { get => typeUnitCard; }

    //SpecialCards
    public SubTypeSpecialCard TypeSpecialCard { get => typeSpecialCard; }

    public TypeEffects Effect { get => effects; }


    public enum CardType
    { Unit, Special, Boss }
    // public enum TypeFaction
    //{ Pirates, Resistance, Neutral }

    public enum EnumRange
    { M, R, S, MR, MS, RS, MRS, Boss }
    public enum SubTypeUnitCard
    { None, Gold, Silver }
    public enum SubTypeSpecialCard
    { None, Climate, Increase, Clearance, Lure }

    public enum TypeEffects
    { //Unit
        None,
        Put_Increase,
        Put_Climate,
        Delete_Card_with_Max_Power_on_the_field,
        Delete_Card_with_Min_Power_on_the_field, Draw_Card_from_Deck,
        Clear_file,
        Average_Power_on_the_field,
        Decreases_one_Point,
        Multiply_the_attack_of_the_card_by_the_number_of_identical_cards_on_the_field,

        //Special
        Clearance, Climate, Increase, Lure,

        //Boss
        StayintheField,

        effectCardCompiler

    }
}
