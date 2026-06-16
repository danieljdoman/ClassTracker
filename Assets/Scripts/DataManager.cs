using System.IO;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    static string filePath = Path.Combine(Application.persistentDataPath, "gameData.json");

    public static void SaveData(ChildManager childManager)
    {
        string json = JsonUtility.ToJson(childManager, true);  // true for pretty print
        File.WriteAllText(filePath, json);
        Debug.Log("Data saved to: " + filePath);
    }

    public static void LoadData(ChildManager childManager)
    {
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            JsonUtility.FromJsonOverwrite(json, childManager);  // Overwrite the existing data with the loaded data
            Debug.Log("Data loaded from: " + filePath);
        }
        else
        {
            Debug.LogWarning("No saved data found.");
        }
    }
}
