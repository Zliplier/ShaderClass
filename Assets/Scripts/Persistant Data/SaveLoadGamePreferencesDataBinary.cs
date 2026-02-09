using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;
using UnityEngine;

namespace Persistant_Data
{
    public class SaveLoadGamePreferencesDataBinary : ISaveLoadGamePreferencesData
    {
        public GamePreferencesData LoadGamePreferencesData(string location)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream file = File.Open(location, FileMode.Open);
            
            GamePreferencesData gpd = new GamePreferencesData();
            gpd= (GamePreferencesData)formatter.Deserialize(file);
            
            file.Close();
            
            return gpd;
        }
        public void SaveGamePreferencesData(GamePreferencesData gpd, string location)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream file = File.Create(location);
            formatter.Serialize(file, gpd);
            file.Close();
        }
    }
}