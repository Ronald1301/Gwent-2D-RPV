using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ScriptShowCard : MonoBehaviour
{
    UIDocument ShowCardCreated;
    // [SerializeField] GameObject SceneCardCreator;
    Button OK;
    VisualElement Box;
    [SerializeField] UICardDescription UI;

    private void OnEnable()
    {
        ShowCardCreated = GetComponent<UIDocument>();
        VisualElement root = ShowCardCreated.rootVisualElement;

        OK = root.Q<Button>("OK");
        Box = root.Q<VisualElement>("Box");

        OK.RegisterCallback<ClickEvent>(CloseCardCreator);

    }

    private void CloseCardCreator(ClickEvent evt)
    {
        // SceneCardCreator.SetActive(true);
        this.gameObject.SetActive(false);
    }
    private void CreateVisualElement(CardData card)
    {
        VisualElement visual = new VisualElement();
        visual.style.width = 200;
        visual.style.height = 300;
        visual.RegisterCallback<MouseDownEvent>(evt => OnMouseEnter(evt, card));
        visual.RegisterCallback<MouseLeaveEvent>(evt => OnMouseExit());
        visual.style.backgroundImage = new StyleBackground(card.CardImageForehead);
        Box.Add(visual);
    }

    private void OnMouseEnter(MouseDownEvent evt, CardData card)
    {
        UI.gameObject.SetActive(true);
        UI.UIUpdateCardDescription(card);
    }
    public void OnMouseExit()
    {
        UI.gameObject.SetActive(false);
    }
    public void Show()
    {
        //this.gameObject.SetActive(true);
        Bridge.cardsDataCreated.ForEach(CreateVisualElement);
    }

}
