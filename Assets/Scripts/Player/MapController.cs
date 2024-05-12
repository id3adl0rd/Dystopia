using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapController : MonoBehaviour
{
    [SerializeField] private GameObject _map;
    [SerializeField] private Transform _target;
    private Player _player;
    public void OnMapPressed()
    {
        _map.SetActive(!_map.activeInHierarchy); }

    private void Update()
    {
        Vector2 pos = _map.transform.position;
        pos.x = _target.position.x;
        pos.y = _target.position.y;

        /*Debug.Log(pos.x);
        Debug.Log(pos.y);
        */
        
        _map.transform.position = pos;
        
        /*
        Debug.Log(_map.transform.position.x);
        Debug.Log(_map.transform.position.y);
        Debug.Log(_map.transform.position.z);*/
    }
}
