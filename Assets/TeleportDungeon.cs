using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TeleportDungeon : MonoBehaviour
{
    private void Awake()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.collider.tag == "Player")
            SceneManager.LoadSceneAsync("Scenes/Dungeon");
    }
}
