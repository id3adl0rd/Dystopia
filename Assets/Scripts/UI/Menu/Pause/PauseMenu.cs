using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    private GameObject _pauseMenu;
    [SerializeField] private GameObject _submenu;
    [HideInInspector] public bool isPaused;

    public static PauseMenu Instance { get; private set; }

    private void Awake()
    {
        _submenu.SetActive(false);
        _pauseMenu = gameObject;
        Instance = this;
    }

    private void Start()
    {
        _pauseMenu.SetActive(false);
    }

    public void PauseGame()
    {
        _pauseMenu.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
    }

    public void ResumeGame()
    {
        _pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
    }

    public void ExitButton()
    {
        SceneManager.LoadSceneAsync("Scenes/MainMenu");
    }
    
    public void QuitButton()
    {
        Application.Quit();
    }
}
