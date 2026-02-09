using System;

namespace Persistant_Data
{
    [Serializable]
    public class GamePreferencesData
    {
        public string _playerName;
        public int _score;
        public float _musicVolume;
        public float _SFXVolume;
    }
}