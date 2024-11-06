using System.IO;
using Newtonsoft.Json;
using UnityEngine;

public static class SaveSystem
{
    public static readonly string SAVE_FOLDER = Application.persistentDataPath + "/saves/";
    public static readonly string FILE_EXT = ".json";

    public static void Save(string filename, string dataToSave)
    {
        if (!Directory.Exists(SAVE_FOLDER)) {
            Directory.CreateDirectory(SAVE_FOLDER);
        }

        File.WriteAllText(SAVE_FOLDER + filename + FILE_EXT, dataToSave);

    }

    public static Data Load(string filename)
    {
        if (File.Exists(SAVE_FOLDER + filename + FILE_EXT))
        {
            var json = File.ReadAllText(SAVE_FOLDER + filename + FILE_EXT);
            var serializedData = JsonConvert.DeserializeObject<Data>(json);

            return serializedData;
        }
        else {
            return null;
        }

    }

}
