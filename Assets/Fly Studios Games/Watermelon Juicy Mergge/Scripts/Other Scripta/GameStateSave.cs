using UnityEngine;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.IO;

public class GameStateSave : MonoBehaviour
{
    public Transform rootParentObject;
    public Transform instantiatedObject;

    [Button]
    public void SaveAllGameState()
    {
        // Creează o listă pentru a stoca datele subobiectelor
        List<ObjectSaveData> objectSaveDataList = new List<ObjectSaveData>();

        // Adaugă datele fiecărui subobiect la listă
        foreach (Transform child in rootParentObject)
        {
            ObjectSaveData objectSaveData = new ObjectSaveData
            {
                Position = child.position,
                // Alte informații legate de subobiect pe care dorești să le salvezi
            };
            objectSaveDataList.Add(objectSaveData);
        }

        // Converteste lista în format JSON și salvează într-un fișier pe dispozitiv
        string jsonData = JsonUtility.ToJson(objectSaveDataList);
        string filePath = Path.Combine(Application.persistentDataPath, "ObjectSaveData.json");
        File.WriteAllText(filePath, jsonData);

        Debug.Log("Game state saved!");
    }

    [Button]
    public void LoadAllGameState()
    {
        // Încarcă datele din fișierul salvat
        string filePath = Path.Combine(Application.persistentDataPath, "ObjectSaveData.json");
        if (File.Exists(filePath))
        {
            string jsonData = File.ReadAllText(filePath);
            List<ObjectSaveData> loadedDataList = JsonUtility.FromJson<List<ObjectSaveData>>(jsonData);

            // Utilizează datele încărcate pentru a face ceva, de exemplu, instantierea unor obiecte
        }
        else
        {
            Debug.LogError("Save file not found.");
        }

        Debug.Log("Game state loaded!");
    }

    [System.Serializable]
    private class ObjectSaveData
    {
        public Vector3 Position;
        // Alte informații legate de subobiect pe care dorești să le salvezi
    }
}
