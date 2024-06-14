using System;
using System.Collections;
using Cinemachine;
using UnityEngine;

public class ShakeCameraController : MonoBehaviour
{
    public CinemachineVirtualCamera _vcam;
    private CinemachineBasicMultiChannelPerlin _noisePerlin;
    [SerializeField] private float _shakeTime = 0.1f;
    private IEnumerator _coroutine;
    
    private void Awake()
    {
        _noisePerlin = _vcam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        StopShake();
    }

    public void StartShake()
    {
        StartShakeCoroutine();
        if (_noisePerlin.m_FrequencyGain == 0)
        {
            _noisePerlin.m_AmplitudeGain = 1;
            _noisePerlin.m_FrequencyGain = 1;

            Debug.Log("res " + _noisePerlin.m_FrequencyGain);
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
        yield return new WaitForSeconds(1f);
        StopShake();
    }
}
