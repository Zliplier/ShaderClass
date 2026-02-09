
using Persistant_Data;

public interface ISaveLoadGamePreferencesData
{
    void SaveGamePreferencesData(GamePreferencesData gpd,string location);
    GamePreferencesData LoadGamePreferencesData(string location);
}