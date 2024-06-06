using System;
using UnityEngine;

public class NotifyController : MonoBehaviour
{
    public GameObject _gameObject;

    public static NotifyController instance; 
    
    private void Awake()
    {
        instance = this;
    }

    public void SetNotifyObj(GameObject obj)
    {
        Debug.Log(obj);
        _gameObject = obj;
        Debug.Log("1: " + _gameObject);
    }

    public void AddToQueue(string text, float time)
    {
        _gameObject.GetComponent<PopupNotify>().AddToQueue(text);
    }
}