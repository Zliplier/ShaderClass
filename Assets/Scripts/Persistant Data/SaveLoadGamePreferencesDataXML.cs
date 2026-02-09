using System.IO;
using System.Xml.Serialization;

namespace Persistant_Data
{
    public class SaveLoadGamePreferencesDataXML : ISaveLoadGamePreferencesData
    {
        public GamePreferencesData LoadGamePreferencesData(string location)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(GamePreferencesData));
            StreamReader sr = new StreamReader(location);
            GamePreferencesData gpd = (GamePreferencesData)serializer.Deserialize(sr.BaseStream);
            sr.Close();
            return gpd;
        }
        public void SaveGamePreferencesData(GamePreferencesData gpd, string location)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(GamePreferencesData));
            StreamWriter sw = new StreamWriter(location);
            serializer.Serialize(sw, gpd);
            sw.Close();
        }
    }
}