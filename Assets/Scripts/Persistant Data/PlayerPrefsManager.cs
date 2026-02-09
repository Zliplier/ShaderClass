using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Persistant_Data
{
    public class PlayerPrefsManager : MonoBehaviour
    {
        [SerializeField] private TMP_InputField _inputFieldPlayerName, _inputFieldScore;
        [SerializeField] private Button _saveButton, _loadButton, _clearButton;
        [SerializeField] private Slider _sliderMusicVolume, _sliderSFXVolume;
        
        void Start()
        {
            _saveButton.onClick.AddListener(OnSaveClick);
            _loadButton.onClick.AddListener(OnLoadClick);
            _clearButton.onClick.AddListener(OnClearClick);
        }
        
        void OnSaveClick()
        {
            PlayerPrefs.SetString("PlayerName", _inputFieldPlayerName.text);
            PlayerPrefs.SetInt("Score", int.Parse(_inputFieldScore.text));
            PlayerPrefs.SetFloat("MusicVolume", _sliderMusicVolume.value);
            PlayerPrefs.SetFloat("SFXVolume", _sliderSFXVolume.value);
            
            PlayerPrefs.Save();
        }
        
        void OnLoadClick()
        {
            if (PlayerPrefs.HasKey("PlayerName")){
                string s = PlayerPrefs.GetString("PlayerName");
                _inputFieldPlayerName.text = s;
            }
            if (PlayerPrefs.HasKey("Score")){
                _inputFieldScore.text = PlayerPrefs.GetInt("Score").ToString();
            }
            if (PlayerPrefs.HasKey("MusicVolume")){
                _sliderMusicVolume.value = PlayerPrefs.GetFloat("MusicVolume");
            }
            if (PlayerPrefs.HasKey("SFXVolume")){
                _sliderSFXVolume.value = PlayerPrefs.GetFloat("SFXVolume"); 
            }
        }
        void OnClearClick() => PlayerPrefs.DeleteAll();
    }
}