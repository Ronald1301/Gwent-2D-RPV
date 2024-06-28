using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Card", menuName = "Card")]
public class Card : ScriptableObject
{
    public Card(){}
    [SerializeField] private string cardName;
    [SerializeField] private TypeFaction faction;
    [SerializeField] private CardType type;
    [SerializeField] private string description;
    [SerializeField] private Sprite cardImageForehead;
    [SerializeField] private Sprite cardImageback;

    [SerializeField] private int startPower;
    [SerializeField] private int power;
    [SerializeField] private char typeField;
    [SerializeField] private SubTypeUnitCard typeUnitCard;

    [SerializeField] private SubTypeSpecialCard typeSpecialCard;

    [SerializeField] private TypeEffects effects;

    public bool stayintheField = false;
    public bool inTheField = false;
    public bool affectedByClimate = false;
    public bool affectedByIncrease = false;


    //Card
    public string CardName => cardName;
    public TypeFaction Faction { get => faction; }
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
    public SubTypeUnitCard TypeUnitCard { get => typeUnitCard; }

    //SpecialCards
    public SubTypeSpecialCard TypeSpecialCard { get => typeSpecialCard; }

    public TypeEffects Effect { get => effects; }


    public enum CardType
    { Unit, Special, Boss }
    public enum TypeFaction
    { Pirates, Resistance, Neutral }
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
        StayintheField,

    }
}
