using System;
using System.Collections;
using Cinemachine;
using UnityEngine;

public class ShakeCameraController : MonoBehaviour
{
    [HideInInspector] public CinemachineVirtualCamera _vcam;
    private CinemachineBasicMultiChannelPerlin _noisePerlin;
    [SerializeField] private float _shakeTime = 0.1f;
    private IEnumerator _coroutine;
    
    private void Awake()
    {
        _noisePerlin = _vcam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }

    public void StartShake()
    {
        StartShakeCoroutine();
        
        if (_noisePerlin.m_FrequencyGain == 0)
        {
            _noisePerlin.m_AmplitudeGain = 1;
            _noisePerlin.m_FrequencyGain = 1; 

        }
    }

    private void StopShake()
    {
        _noisePerlin.m_AmplitudeGain = 0;
        _noisePerlin.m_FrequencyGain = 0;
    }

    public void StartShakeCoroutine()
    {
        if (_coroutine != null)
        {
            StopCoroutine(_coroutine);
            _coroutine = null;
        }
        
        _coroutine = ShakeCoroutine(_shakeTime);
        StartCoroutine(_coroutine);
    }

    private IEnumerator ShakeCoroutine(float dynamicAmbientDuration)
    {
        yield return new WaitForSeconds(0.5f);
        StopShake();
    }
}
