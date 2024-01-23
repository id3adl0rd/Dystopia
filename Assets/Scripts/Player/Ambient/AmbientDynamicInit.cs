using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmbientDynamicInit : MonoBehaviour
{
    [SerializeField] private float _dynamicAmbientDuration;

    private AmbientDynamicController _ambientDynamicController;

    private void Awake()
    {
        _ambientDynamicController = GetComponent<AmbientDynamicController>();    
    }

    public void StartDynamicAmbient()
    {
        _ambientDynamicController.StartDynamicAmbient(_dynamicAmbientDuration);
    }
}
