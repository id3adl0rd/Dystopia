using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
    [SerializeField] private AudioMixer _mixer;
    [SerializeField] private Slider _musicSlider;
    [SerializeField] private Slider _soundSlider;
    [SerializeField] private GameObject _subMenu;

    public static Settings instance;
    
    private void Awake()
    {
        instance = this;
    }
    
    public void ShowMenu()
    {
        //gameObject.SetActive(true);
        _subMenu.SetActive(true);
    }

    [System.Serializable]
    public class SettingsValue
    {
        public string name;
        public float value;
    }
    
    [System.Serializable]
    public class SettingsList
    {
        public SettingsValue music;
        public SettingsValue sfx;
    }
    
    private void Start()
    {
        string filePath = Application.persistentDataPath + "/settings.json";

        LoadData();
        _subMenu.SetActive(false);
    }

    private void SaveData()
    {
        string filePath = Application.persistentDataPath + "/settings.json";

        _mixer.SetFloat("music", GetMusicLevel());
        _mixer.SetFloat("sound", GetSoundLevel());
        
        SettingsValue value1 = new SettingsValue();
        value1.name = "music";
        value1.value = _musicSlider.value;
        
        SettingsValue value2 = new SettingsValue();
        value2.name = "sound";
        value2.value = _soundSlider.value;

        SettingsList setlist = new SettingsList();
        setlist.music = value1;
        setlist.sfx = value2;
        
        string res = JsonUtility.ToJson(setlist);
        
        System.IO.File.WriteAllText(filePath, res);
    }

    public void LoadData()
    {
        string filePath = Application.persistentDataPath + "/settings.json";

        string data = System.IO.File.ReadAllText(filePath);
        SettingsList d = JsonUtility.FromJson<SettingsList>(data);
        _soundSlider.value = d.sfx.value;
        _musicSlider.value = d.music.value;
        
        float volume = d.music.value;
        _mixer.SetFloat("music", Mathf.Log10(volume) * 20);
        
        volume = _soundSlider.value;
        _mixer.SetFloat("sound", Mathf.Log10(volume) * 20);
    }

    public float GetMasterLevel(){
        float value;
        bool result =  _mixer.GetFloat("master", out value);
        if(result) return value;
        
        return 0f;
    }
    
    public float GetMusicLevel(){
        float value;
        bool result =  _mixer.GetFloat("music", out value);
        if(result) return value;
        
        return 0f;
    }
    
    public float GetSoundLevel(){
        float value;
        bool result =  _mixer.GetFloat("sound", out value);
        if(result) return value;
        
        return 0f;
    }

    public void SetMusicVolume()
    {
        float volume = _musicSlider.value;
        _mixer.SetFloat("music", Mathf.Log10(volume) * 20);
        SaveData();
    }
    
    public void SetSFXVolume()
    {
        float volume = _soundSlider.value;
        _mixer.SetFloat("sound", Mathf.Log10(volume) * 20);
        SaveData();
    }

    public void CloseButton()
    {
        //gameObject.SetActive(false);
        _subMenu.SetActive(false);
    }
}
