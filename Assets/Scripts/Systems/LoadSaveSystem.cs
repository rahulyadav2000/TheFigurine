using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class LoadSaveSystem : MonoBehaviour
{
    public static void SaveGameData(Player player, ArrowSystem arrowSystem)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string filePath = Application.persistentDataPath + "/GameData.txt";
        FileStream fileStream = new FileStream(filePath, FileMode.Create);

        GameData data = new GameData(player, arrowSystem);

        formatter.Serialize(fileStream, data);
        fileStream.Close();
    }

    public static GameData LoadGameData()
    {
        string filePath = Application.persistentDataPath + "/GameData.txt";
        if(File.Exists(filePath))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream fileStream = new FileStream(filePath, FileMode.Open);
            GameData data = formatter.Deserialize(fileStream) as GameData;
            fileStream.Close();

            return data;
        }
        else
        {
            Debug.LogError("Save File Not Found!! : " + filePath);
            return null;
        }
    }
}
