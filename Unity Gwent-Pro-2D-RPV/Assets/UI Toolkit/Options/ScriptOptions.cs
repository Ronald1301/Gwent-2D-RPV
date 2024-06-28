using System.Diagnostics;
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
        resolution.value = $"{Screen.currentResolution.width}x{Screen.currentResolution.height}";
        resolution.choices = Screen.resolutions.Select(res => $"{res.width}x{res.height}").ToList();

        quality.index = QualitySettings.GetQualityLevel(); // Convert the integer value to a string
        sounds.value = (int)(AudioListener.volume * 100);
        fullscreen.value = Screen.fullScreen;

        //Callbacks
        resolution.RegisterValueChangedCallback(evt => ChangeResolution(evt.newValue));
        quality.RegisterValueChangedCallback(evt => QualitySettings.SetQualityLevel(quality.index));
        sounds.RegisterValueChangedCallback(evt => AudioListener.volume = sounds.value / 100f);
        fullscreen.RegisterValueChangedCallback(evt => Screen.fullScreen = evt.newValue);
        back.RegisterCallback<ClickEvent>(BackToStartMenu);
    }
    static void ChangeResolution(string newValue)
    {
        UnityEngine.Debug.Log("Change Resolution");
        // Parse the new resolution string into width and height values
        string[] resolutionValues = newValue.Split('x');
        int width = int.Parse(resolutionValues[0]);
        int height = int.Parse(resolutionValues[1]);

        // Set the screen resolution
        Screen.SetResolution(width, height, Screen.fullScreen);
    }
    private void BackToStartMenu(ClickEvent evt)
    {
        StartMenu.SetActive(true);
        gameObject.SetActive(false);
    }
}
