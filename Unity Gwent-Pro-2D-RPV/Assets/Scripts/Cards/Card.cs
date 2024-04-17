using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Card", menuName = "Card")]
public class Card : ScriptableObject
{
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


    //Card
    public string CardName => cardName;
    public TypeFaction Faction { get => faction;}
    public CardType Type { get => type;}
    public string Description { get => description;}
    public Sprite CardImageForehead { get => cardImageForehead;}
    public Sprite CardImageBack { get => cardImageback;}

    //UnitCards

    public int StartPower { get => startPower;}
    public int Power { get => power; }

    //public readonly char[] TypeField = { 'M', 'R', 'S' };
   // public readonly bool[] MRS = new bool[3];
   public char TypeField { get => typeField;}
    public SubTypeUnitCard TypeUnitCard { get => typeUnitCard;}

    //SpecialCards
    public SubTypeSpecialCard TypeSpecialCard { get => typeSpecialCard;}

    
    public enum CardType
    { Unit,Special,Boss }
    public enum TypeFaction
    { Pirates,Resistance,Neutral }
    public enum SubTypeUnitCard
    { None, Gold, Silver }
    public enum SubTypeSpecialCard
    { None, Climate, Increase, Clearance, Lure }
}
