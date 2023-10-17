using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public static class SerializationManager
{
    public static readonly string mainSaveDirName = "/Saves";
    public static readonly string saveExtension = ".sav";
    public static string mainSaveDirPath { get { return Application.persistentDataPath + mainSaveDirName; } }

    public static bool Save(string saveName, object saveData)
    {
        CreateDir(mainSaveDirPath);
        BinaryFormatter formatter = GetBinaryFormatter();
        string savePath = mainSaveDirPath + saveName + saveExtension;
        FileStream file = File.Create(savePath);
        formatter.Serialize(file, saveData);
        file.Close();

        return true;
    }

    private static void CreateDir(string path)
    {
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }
    }

    public static T Load<T>(string saveName) where T : Data
    {
        string path = mainSaveDirPath + saveName + saveExtension;
        
        if (!Directory.Exists(path))
        {
            string newDir = mainSaveDirPath;
            string[] splittedName = saveName.Split("/");
            for (int i = 0; i < splittedName.Length - 1; i++)
                newDir += "/" + splittedName[i];
            Directory.CreateDirectory(newDir);
        }

        if (!File.Exists(path))
        {
            return null;
        }

        BinaryFormatter formatter = GetBinaryFormatter();
        FileStream file = File.Open(path, FileMode.Open);

        try
        {
            T saved = (T)formatter.Deserialize(file);
            file.Close();
            return saved;
        }
        catch
        {
            Debug.LogErrorFormat("Failed to load file at {0}", path);
            file.Close();
            return null;
        }
    }

    public static BinaryFormatter GetBinaryFormatter()
    {
        BinaryFormatter formatter = new BinaryFormatter();

        return formatter;
    }
}