using System.IO;
using System;
using UnityEngine;

public class SaveLoadSystem : MonoBehaviour
{
    private const string NameFile = "infoCode.json";

   // public event Action<string> OnCodeChanged;
    public event Action DataLoaded;
    [SerializeField] private InfoCode infoCode;
    public InfoCode InfoCode => infoCode;

    [ContextMenu("SaveCode")]
    public void SaveCode()
    {
        string json = JsonUtility.ToJson(infoCode);
        string path = Path.Combine(Application.persistentDataPath, NameFile);
        File.WriteAllText(path, json);

        Debug.Log("SaveCode: " + path);
    }

    [ContextMenu("LoadCode")]
    public void LoadCode()
    {
        string path = Path.Combine(Application.persistentDataPath, NameFile);
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            InfoCode newCode = JsonUtility.FromJson<InfoCode>(json);

            infoCode.code = newCode.code;
            Debug.Log("LoadCode: " + path);

            //OnCodeChanged?.Invoke(infoCode.code);
            DataLoaded?.Invoke();
        }
        else
        {
            Debug.Log("LoadCode: File not found");
        }
    }
}
