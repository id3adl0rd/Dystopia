using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmbientDynamicController : MonoBehaviour
{
    private AmbientController _ambientController;

    private void Awake()
    {
        _ambientController = GetComponent<AmbientController>();
    }

    public void StartDynamicAmbient(float dynamicAmbientDuration)
    {
        StartCoroutine(DynamicAmbientCoroutine(dynamicAmbientDuration));
    }

    private IEnumerator DynamicAmbientCoroutine(float dynamicAmbientDuration)
    {
        _ambientController.InFight = true;
        _ambientController.StartDynamic(null);
        Debug.Log(dynamicAmbientDuration);
        yield return new WaitForSeconds(dynamicAmbientDuration);
        _ambientController.InFight = false;
        _ambientController.PlayPassiveAmbient();
    }
}
