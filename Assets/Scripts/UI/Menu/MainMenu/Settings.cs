using System;
using System.Collections;
using System.Collections.Generic;
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
        gameObject.SetActive(false);
    }

    public void ShowMenu()
    {
        gameObject.SetActive(true);
        _subMenu.SetActive(true);
    }

    private void Start()
    {
        _mixer.SetFloat("music", instance.GetMusicLevel());
        _mixer.SetFloat("sound", instance.GetSoundLevel());
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
    }
    
    public void SetSFXVolume()
    {
        float volume = _soundSlider.value;
        _mixer.SetFloat("sound", Mathf.Log10(volume) * 20);
    }

    public void CloseButton()
    {
        gameObject.SetActive(false);
        _subMenu.SetActive(false);
    }
}
