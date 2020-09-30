using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class M_SaveManager
{
    private static M_SaveManager s_instance = null;
    private static readonly object s_lock = new object();

    public const string DIRECTORY = "/Saves";

    private M_SaveManager() { }

    public static M_SaveManager Instance
    {
        get
        {
            lock (s_lock)
            {
                if (s_instance == null)
                    s_instance = new M_SaveManager();
                return s_instance;
            }
        }
    }

    public BinaryFormatter GetBinaryFormatter()
    {
        BinaryFormatter formatter = new BinaryFormatter();
        SurrogateSelector selector = new SurrogateSelector();

        UT_Vector3SerializationSurrogate vector3Surr = new UT_Vector3SerializationSurrogate();
        selector.AddSurrogate(typeof(Vector3), new StreamingContext(StreamingContextStates.All), vector3Surr);
        formatter.SurrogateSelector = selector;

        return formatter;
    }

    public void Save(string name)
    {
        string filename = Application.persistentDataPath + DIRECTORY;

        if (!Directory.Exists(filename)) 
            Directory.CreateDirectory(filename);
        filename += ("/" + name);

        Debug.Log("Saving game... " + filename);
        BinaryFormatter formatter = GetBinaryFormatter();
        FileStream stream = new FileStream(filename, FileMode.Create);

        SAV_Game gameSav = new SAV_Game(M_BuildingManager.SGetBuildings(),
                                        M_SettlersManager.SGetSettlers(),
                                        new SAV_InGameResources(M_InGameResourcesManager.SGetData()));

        formatter.Serialize(stream, gameSav);

        stream.Close();
    }

    public bool Load(string name)
    {
        string filename = Application.persistentDataPath + DIRECTORY;
        if (!Directory.Exists(filename))
        {
            Directory.CreateDirectory(filename);
            return false;
        }
        filename += ("/" + name);

        if (!File.Exists(filename))
        {
            Debug.LogWarning("File doesn't exsist!");
            return false;
        }

        BinaryFormatter formatter = GetBinaryFormatter();
        FileStream stream = new FileStream(filename, FileMode.Open);
        SAV_Game gameSav = formatter.Deserialize(stream) as SAV_Game;
        // Apply data
        M_BuildingManager.SAddBuildings(gameSav.GetBuildings());
        M_SettlersManager.SAddSettlers(gameSav.settlers);
        M_InGameResourcesManager.SApplySavedData(gameSav.resources);
        
        

        return true;
    }
}
