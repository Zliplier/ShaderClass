using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Persistant_Data
{
    public class SaveLoadGamePreferencesDataManager : MonoBehaviour
    {
        [SerializeField] private TMP_InputField _inputFieldPlayerName, _inputFieldScore, _inputFieldFileName;
        [SerializeField] private Button _saveButton, _loadButton;
        [SerializeField] private Slider _sliderMusicVolume, _sliderSFXVolume;
        [SerializeField] TMP_Dropdown _dropdownSaveType;

        private string _saveFolderLocation;

        // Start is called before the first frame update
        void Start()
        {
            _saveButton.onClick.AddListener(OnSaveClick);
            _loadButton.onClick.AddListener(OnLoadClick);

            if (!Directory.Exists("Saves"))
                Directory.CreateDirectory("Saves");
            _saveFolderLocation = "Saves";
        }

        void OnSaveClick()
        {
            GamePreferencesData gdp = new GamePreferencesData
            {
                _playerName = _inputFieldPlayerName.text,
                _score = int.Parse(_inputFieldScore.text),
                _musicVolume = _sliderMusicVolume.value,
                _SFXVolume = _sliderSFXVolume.value
            };

            string filename = _inputFieldFileName.text;
            string location = Path.Combine(_saveFolderLocation, filename);

            ISaveLoadGamePreferencesData islg = null;

            switch (_dropdownSaveType.value)
            {
                case 0: //XML
                    islg = new SaveLoadGamePreferencesDataXML();
                    location = location + ".xml";
                    break;
                case 1: //JSON
                    islg = new SaveLoadGamePreferencesDataJSON();
                    location = location + ".json";
                    break;
                case 2: //Binary
                    islg = new SaveLoadGamePreferencesDataBinary();
                    location = location + ".bin";
                    break;
            }
            
            islg.SaveGamePreferencesData(gdp, location);
        }
        
        void OnLoadClick()
        {
            string filename = _inputFieldFileName.text;
            string location = Path.Combine(_saveFolderLocation , filename);
            
            ISaveLoadGamePreferencesData islg = null;
            
            switch (_dropdownSaveType.value)
            {
                case 0://XML
                islg = new SaveLoadGamePreferencesDataXML();
                location = location + ".xml";
                break;
                case 1://JSON
                islg = new SaveLoadGamePreferencesDataJSON();
                location = location + ".json";
                break;
                case 2://Binary
                islg = new SaveLoadGamePreferencesDataBinary();
                location = location + ".bin";
                break;
            }
            
            GamePreferencesData gdp = new GamePreferencesData();
            gdp = islg.LoadGamePreferencesData(location);
            
            _inputFieldPlayerName.text = gdp._playerName;
            _inputFieldScore.text = gdp._score.ToString();
            _sliderMusicVolume.value = gdp._musicVolume;
            _sliderSFXVolume.value = gdp._SFXVolume;
        }
    }
}