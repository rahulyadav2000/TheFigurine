using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class LoadSaveSystem : MonoBehaviour
{
    public static void SaveGameData(Player player, ArrowSystem arrowSystem)
    {
        // create instance of binaryformatter for serialization
        BinaryFormatter formatter = new BinaryFormatter();

        // file path definition 
        string filePath = Application.persistentDataPath + "/GameData.txt";
        
        // create the file stream to write the data
        FileStream fileStream = new FileStream(filePath, FileMode.Create);

        GameData data = new GameData(player, arrowSystem);
        
        // serialize the data and write it to the file
        formatter.Serialize(fileStream, data);

        // closes the file
        fileStream.Close();
    }

    public static GameData LoadGameData()
    {
        string filePath = Application.persistentDataPath + "/GameData.txt";
        if(File.Exists(filePath))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            // create the file stream to open the file
            FileStream fileStream = new FileStream(filePath, FileMode.Open);

            // deserialize the data
            GameData data = formatter.Deserialize(fileStream) as GameData;

            // closes the file
            fileStream.Close();

            return data; // returns the saved data 
        }
        else
        {
            Debug.LogError("Save File Not Found!! : " + filePath);
            return null;
        }
    }
}
