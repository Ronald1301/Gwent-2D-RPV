using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ScriptCardCreated : MonoBehaviour
{
    UIDocument CardCreated;

    Label Name;

    VisualElement Image;
    Label Faction;
    Label Power;
    Label Type;
    Label effect;


    private void OnEnable()
    {
        CardCreated = GetComponent<UIDocument>();
        VisualElement root = CardCreated.rootVisualElement;

        Name = root.Q<Label>("TextName");
        Image = root.Q<VisualElement>("ImageCard");
        Faction = root.Q<Label>("TextFaction");
        Power = root.Q<Label>("TextPower");
        Type = root.Q<Label>("TextType");
        effect = root.Q<Label>("TextEffect");

        SetCard(this.gameObject.GetComponent<CardDisplay>().cardData);

        root.transform.scale = new Vector3(0.5f, 0.5f, 0.5f);
    }

    public void SetCard(CardData card)
    {
        Name.text = card.CardName;
        Faction.text = card.Faction.ToString();
        Power.text = card.Power.ToString();
        AssignmentType(card);
        effect.text = card.Description.ToString();
        SetImage(card.CardImageForehead);
    }

    public void SetImage(Sprite sprite)
    {
        Image.style.backgroundImage = new StyleBackground(sprite);
    }

    void AssignmentType(CardData card)
    {
        if (card.Type == CardData.CardType.Unit)
        {
            Type.text = card.TypeUnitCard.ToString();
        }
        else if(card.Type == CardData.CardType.Special)
        {
            Type.text = card.TypeSpecialCard.ToString();
        }
        else
        {
            Type.text = card.Type.ToString();
        }
    }
}
