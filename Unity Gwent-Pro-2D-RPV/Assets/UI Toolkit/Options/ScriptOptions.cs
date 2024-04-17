using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class ScriptOptions : MonoBehaviour
{
    UIDocument MenuOptions;
    public GameObject StartMenu;

    private DropdownField resolution;
    private DropdownField quality;
    private SliderInt sounds;
    private Toggle fullscreen;
    private Button back;

    private void OnEnable()
    {
        MenuOptions = GetComponent<UIDocument>();
        VisualElement root = MenuOptions.rootVisualElement;

        //References to the buttons
        resolution = root.Q<DropdownField>("Resolution");
        quality = root.Q<DropdownField>("Quality");
        sounds = root.Q<SliderInt>("Sounds");
        fullscreen = root.Q<Toggle>("FullScreen");
        back = root.Q<Button>("Back");


        //Set the values of the buttons

        //resolution.choices = Screen.resolutions.Select(res => $"{res.width}x{res.height}").ToList();
        resolution.value = Screen.width.ToString() + " x " + Screen.height.ToString();

        //quality.choices = QualitySettings.names.ToList();
        quality.index = QualitySettings.GetQualityLevel(); // Convert the integer value to a string


        sounds.value = (int)(AudioListener.volume * 100);
        fullscreen.value = Screen.fullScreen;



        //Callbacks
        //resolution.RegisterValueChangedCallback(evt => Screen.SetResolution(Screen.resolutions[Convert.ToInt32(resolution.value)].width, Screen.resolutions[Convert.ToInt32(resolution.value)].height, Screen.fullScreen));
        quality.RegisterValueChangedCallback(evt => QualitySettings.SetQualityLevel(quality.index));
        sounds.RegisterValueChangedCallback(evt => AudioListener.volume = sounds.value / 100f);
        fullscreen.RegisterValueChangedCallback(evt => Screen.fullScreen = evt.newValue);
        back.RegisterCallback<ClickEvent>(BackToStartMenu);
    }
    private void BackToStartMenu(ClickEvent evt)
    {
        StartMenu.SetActive(true);
        gameObject.SetActive(false);
    }


}
