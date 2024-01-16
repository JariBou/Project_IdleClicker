using System.IO;
using UnityEngine;

namespace ProjectClicker.Saves
{
    public class SaveManager
    {
        public static void SaveToJson(JsonSaveData data)
        {
            string jsonData = JsonUtility.ToJson(data);
            string filepath = Application.persistentDataPath + "/GameSave.json";
            Debug.LogWarning(jsonData);
            File.WriteAllText(filepath, jsonData);
        }
        
        public static void SaveToJson(JsonSaveData data, string filename)
        {
            string jsonData = JsonUtility.ToJson(data);
            string filepath = Application.persistentDataPath + "/" + filename + (filename.EndsWith(".json") ? ""
                : ".json");
            Debug.LogWarning(jsonData);
            File.WriteAllText(filepath, jsonData);
        }
        
        public static JsonSaveData LoadFromJson()
        {
            string filepath = Application.persistentDataPath + "/GameSave.json";
            string jsonData = File.ReadAllText(filepath);
            Debug.LogWarning(jsonData);

            JsonSaveData data = JsonUtility.FromJson<JsonSaveData>(jsonData);
            return data;
        }
        
        public static JsonSaveData LoadFromJson(string filename)
        {
            string filepath = Application.persistentDataPath + "/" + filename + (filename.EndsWith(".json") ? ""
                : ".json");
            string jsonData = File.ReadAllText(filepath);


            JsonSaveData data = JsonUtility.FromJson<JsonSaveData>(jsonData);
            return data;
        }
    }
}