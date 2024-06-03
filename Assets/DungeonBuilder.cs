using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonBuilder : MonoBehaviour
{
    [SerializeField] private GameObject _generator;
    
    private void Awake()
    {
        _generator.GetComponent<RoomFirstDungeonGenerator>().GenerateDungeon();
    }
}
