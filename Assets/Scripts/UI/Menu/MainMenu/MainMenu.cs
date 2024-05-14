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
    
    private void Awake()
    {
        _mainMenu = gameObject;
        _subMenu.SetActive(false);
    }

    public void StartGame()
    {
        SceneManager.LoadSceneAsync("Scenes/Dungeon");
    }

    public void ContinueGame()
    {
        
    }

    public void Settings()
    {
        _subMenu.SetActive(true);
        _settings.SetActive(true);
    }
    
    public void QuitButton()
    {
        Application.Quit();
    }
}
