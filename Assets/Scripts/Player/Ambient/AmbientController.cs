using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmbientController : MonoBehaviour
{
    [SerializeField] private AudioClip[] _ambientClips;
    [SerializeField] private AudioClip[] _dynamicAmbientClips;
    private AudioSource _ambient;
    private AudioSource _dynamicAmbient;
    private int _currentAmbient;
    
    public bool InFight { get; set; }
    
    private void Start()
    {
        _ambient = gameObject.AddComponent<AudioSource>();
        _dynamicAmbient = gameObject.AddComponent<AudioSource>();
        _currentAmbient = Random.Range(0, _ambientClips.Length);
        InFight = false;
        
        ChangeAmbient(_ambientClips[_currentAmbient]);
    }

    public void UnPause()
    {
        _ambient.UnPause();
    }

    private void ChangeAmbient(AudioClip audio)
    {
        if (audio == null)
        {
            _currentAmbient = Random.Range(0, _ambientClips.Length);
            audio = _ambientClips[_currentAmbient];
        }
        
        _ambient.clip = audio;
        _dynamicAmbient.clip = _dynamicAmbientClips[_currentAmbient];
        StartAmbient(_ambient.clip.length);
        _ambient.Play();
        
        if (InFight)
        {
            if (_ambient.isPlaying)
            {
                _ambient.Pause();
            }
            
            _dynamicAmbient.clip = _dynamicAmbientClips[_currentAmbient];
            _dynamicAmbient.Play();
        }
        else
        {
            if (_dynamicAmbient.isPlaying)
            {
                _dynamicAmbient.Stop();
                _ambient.UnPause();
            }
        }
    }

    public void StartDynamic(AudioClip audio)
    {
        if (_dynamicAmbient.isPlaying)
        {
            return;
        }
        
        if (audio == null)
        {
            audio = _dynamicAmbientClips[_currentAmbient];
        }
        
        if (InFight)
        {
            if (_ambient.isPlaying)
            {
                _ambient.Pause();
            }

            _dynamicAmbient.clip = audio;
            _dynamicAmbient.Play();
        }
    }

    public void PlayPassiveAmbient()
    {
        if (_dynamicAmbient.isPlaying)
        {
            _dynamicAmbient.Stop();
            _ambient.UnPause();
        }
    }
    
    public void StartAmbient(float ambientDuration)
    {
        StartCoroutine(AmbientCoroutine(ambientDuration));
    }

    private IEnumerator AmbientCoroutine(float ambientDuration)
    {
        yield return new WaitForSeconds(ambientDuration);
        ChangeAmbient(null);
    }
}
