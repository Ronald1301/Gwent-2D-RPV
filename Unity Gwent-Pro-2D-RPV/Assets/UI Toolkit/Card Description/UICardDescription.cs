using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using System;

public class UICardDescription : MonoBehaviour
{
    UIDocument CardDescription;
    //private Card card;

    VisualElement cardImage;
    Label cardName;
    Label cardType;
    Label cardFaction;
    Label cardDescription;
    Label Power;
    Label SubType;
    Label TypeField;
    Label Symbol1;
    Label Symbol2;


    private void OnEnable()
    {
        CardDescription = GetComponent<UIDocument>();
        //card=GetComponent<Card>();
        VisualElement root = CardDescription.rootVisualElement;

        cardImage = root.Q<VisualElement>("CardBody");
        cardName = root.Q<Label>("Name");
        cardType = root.Q<Label>("Type");
        cardFaction = root.Q<Label>("Faction");
        cardDescription = root.Q<Label>("EffectCard");
        Power = root.Q<Label>("Power");
        SubType = root.Q<Label>("SubType");
        TypeField = root.Q<Label>("TypeField");
        Symbol1 = root.Q<Label>("LINQ1");
        Symbol2 = root.Q<Label>("LINQ2");

        /*
        cardImage.style.backgroundImage = card.CardImageForehead.texture;
        cardName.text = card.CardName;
        cardType.text = card.Type.ToString();
        cardFaction.text = card.Faction.ToString();
        cardDescription.text = card.Description;
        Power.text = "";

        if (card.Type == Card.CardType.Unit)
        {
            Power.text = card.Power.ToString();
            Symbol1.style.display = DisplayStyle.Flex;
            SubType.text = card.TypeUnitCard.ToString();
            Symbol2.style.display = DisplayStyle.Flex;
            TypeField.text = card.TypeField.ToString();
        }
        else if (card.Type == Card.CardType.Special)
        {
            if (card.TypeSpecialCard == Card.SubTypeSpecialCard.Lure)
            {
               Power.text= card.Power.ToString();
            }
            Symbol1.style.display = DisplayStyle.Flex;
            SubType.text = card.TypeSpecialCard.ToString();
            TypeField.text = "";
        }
        */
    }

    public void UIUpdateCardDescription(Card newcard)
    {
        //if(cardImage.style.backgroundImage == newcard.CardImageBack.texture) return;
        cardImage.style.backgroundImage = newcard.CardImageForehead.texture;
        cardName.text = newcard.CardName;
        cardType.text = newcard.Type.ToString();
        cardFaction.text = newcard.Faction.ToString();
        cardDescription.text = newcard.Description;
        //Power.text = "";

        if (newcard.Type == Card.CardType.Unit)
        {
            Power.text = newcard.Power.ToString();
            Symbol1.style.display = DisplayStyle.Flex;
            SubType.text = newcard.TypeUnitCard.ToString();
            Symbol2.style.display = DisplayStyle.Flex;
            TypeField.text = newcard.TypeField.ToString();
        }
        else if (newcard.Type == Card.CardType.Special)
        {
            if (newcard.TypeSpecialCard == Card.SubTypeSpecialCard.Lure)
            {
               Power.text= newcard.Power.ToString();
            }
            Symbol1.style.display = DisplayStyle.Flex;
            SubType.text = newcard.TypeSpecialCard.ToString();
            TypeField.text = "";
        }
    }

    /*
    private static UICardDescription instance;
    
    public static UICardDescription Instance 
    { 
        get
        {
            if (instance==null) instance = FindObjectOfType<UICardDescription>();
            return instance;
        }
    } 
   */

}
