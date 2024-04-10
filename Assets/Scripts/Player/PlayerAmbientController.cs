using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAmbientController : MonoBehaviour
{
    private AmbientController _ambientController;
    private int dynamicAmbientDuration = 10;
    private bool _inProcess;
    private IEnumerator _coroutine;

    private void Awake()
    {
        /*
        _ambientController = GameObject.Find("SoundManager").GetComponent<AmbientController>();
        */
        _ambientController = AmbientController.instance;
    }

    public void StartDynamic()
    {
        if (_inProcess)
        {
            _inProcess = false;
            StopCoroutine(_coroutine);
            _coroutine = null;
        }
        
        _coroutine = DynamicAmbientCoroutine(dynamicAmbientDuration);
        StartCoroutine(_coroutine);
    }

    private IEnumerator DynamicAmbientCoroutine(float dynamicAmbientDuration)
    {
        Debug.Log(_ambientController);
        _ambientController.InCombat = true;
        _ambientController.StartDynamic(null);
        _inProcess = true;
        Debug.Log("then here");
        yield return new WaitForSeconds(dynamicAmbientDuration);
        _ambientController.InCombat = false;
        _inProcess = false;
        _coroutine = null;
        _ambientController.PlayPassiveAmbient();
    }
}