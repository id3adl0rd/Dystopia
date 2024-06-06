using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    private GameObject _mainMenu;

    [SerializeField] private GameObject _settings;
    [SerializeField] private GameObject _subMenu;
    [SerializeField] private GameObject _classMenu;
    
    private void Awake()
    {
        _mainMenu = gameObject;
        _subMenu.SetActive(false);
        _settings.SetActive(false);
        _classMenu.SetActive(false);
    }

    public void StartGame()
    {
        _settings.SetActive(false);
        _subMenu.SetActive(true);
        _classMenu.SetActive(true);
    }

    public void ContinueGame()
    {
        
    }

    public void Settings()
    {
        _classMenu.SetActive(false);
        _subMenu.SetActive(true);
        _settings.SetActive(true);
    }
    
    public void QuitButton()
    {
        Application.Quit();
    }
}
