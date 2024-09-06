using System.IO;
using System;
using UnityEngine;
using Gwent;
using UnityEditor;

[Serializable]

public class SaveLoadSystem : MonoBehaviour
{
    private const string NameJson = "infoCode.json";

    // public event Action<string> OnCodeChanged;
   // public event Action DataLoaded;
    [SerializeField] public InfoCode infoCode;
    //public InfoCode InfoCode => infoCode;

    public void SaveCode()
    {
        string path = EditorUtility.SaveFilePanel("Save file Gwent", "", "code.gwent", "Gwent");
        string content = infoCode.Code;
        if (!string.IsNullOrEmpty(path))
        {
            if (File.Exists(path))
            {
                if (EditorUtility.DisplayDialog("Existing file", "The file already exists. Do you want to overwrite it?", "Yes", "No"))
                {
                    File.WriteAllText(path, content);
                    Debug.Log("File overwritten in: " + path);
                }
                else
                {
                    Debug.Log("Operation cancelled by user");
                }
            }
            else
            {
                File.WriteAllText(path, content);
                Debug.Log("File saved in: " + path);
            }
        }
        
    }
    public void LoadCode()
    {
        string path = EditorUtility.OpenFilePanel("Open file Gwent", "", "Gwent");
        if (!string.IsNullOrEmpty(path))
        {
            string content = File.ReadAllText(path);
            infoCode.Code = content;
            Debug.Log("File loaded from: " + path);
        }
        else Debug.Log("Operation cancelled by user");
    }

    [ContextMenu("SaveCode")]
    public void SaveJson(string aux)
    {
        string json = JsonUtility.ToJson(infoCode);
        string path = Path.Combine(Application.dataPath, NameJson);
        File.WriteAllText(path, aux + "\n\n\n" + json);

        Debug.Log("SaveCode: " + path);
    }

    [ContextMenu("LoadCode")]
    public string LoadJson()
    {
        string path = Path.Combine(Application.dataPath, NameJson);
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            Debug.Log("LoadCode: " + path);
            return JsonUtility.FromJson<string>(json);
        }
        else
        {
            Debug.Log("LoadCode: File not found");
            return "";
        }
    }
}
