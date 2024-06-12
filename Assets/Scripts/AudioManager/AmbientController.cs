using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AmbientController : MonoBehaviour
{
    private static AudioSource _dynamic;
    private static AudioSource _ambient;

    [SerializeField] private AudioMixerGroup _mixer;
    
    [SerializeField] private AudioClip[] _ambientClips;
    [SerializeField] private AudioClip[] _dynamicAmbientClips;

    private int _currentAmbient;
    public bool InCombat { get; set; }
    
    public static AmbientController instance = null;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != null)
        {
            Destroy(gameObject);
        }
        
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        _dynamic = instance.gameObject.AddComponent<AudioSource>();
        _dynamic.outputAudioMixerGroup = _mixer;
        _ambient = instance.gameObject.AddComponent<AudioSource>();
        _ambient.outputAudioMixerGroup = _mixer;


        ChangeAmbient(_ambientClips[Random.Range(0, _ambientClips.Length)]);
    }

    public void ChangeAmbient(AudioClip audio)
    {
        if (audio == null)
        {
            _currentAmbient = Random.Range(0, _ambientClips.Length);
            audio = _ambientClips[_currentAmbient];
        }

        if (_dynamic.volume > 0)
        {
            _dynamic.clip = _dynamicAmbientClips[_currentAmbient];
            _dynamic.Play();
        }
        
        _ambient.clip = audio;
        _ambient.Play();
        StartAmbient(_ambient);
        instance.StopAllCoroutines();
        FadeOutCaller(_dynamic, 5);
        FadeInCaller(_ambient, 0.25f, 0.75f);
    }
    
    public void StartAmbient(AudioSource _source)
    {
        StartCoroutine(AmbientCoroutine(_source));
    }

    private IEnumerator AmbientCoroutine(AudioSource _source)
    {
        do yield return null; while( _source.isPlaying );
        ChangeAmbient(null);
    }
    
    public void StartDynamic(AudioClip audio)
    {
        if (audio == null)
        {
            audio = _dynamicAmbientClips[_currentAmbient];
        }
        
        if (InCombat)
        {
            if (!_dynamic.isPlaying || _dynamic.volume <= 0)
            {
                _dynamic.clip = audio;
                _dynamic.Play();
            }
            
            instance.StopAllCoroutines();
            FadeOutCaller(_ambient, 0.015f);
            FadeInCaller(_dynamic, 0.01f, 0.75f);
        }
    }
    
    public void PlayPassiveAmbient()
    {
        if (_dynamic.isPlaying)
        {
            instance.StopAllCoroutines();
            FadeInCaller(_ambient, 0.015f, 1f);
            FadeOutCaller(_dynamic, 0.02f);
        }
    }
    
    public void StopAmbient()
    {
        if (_dynamic.isPlaying)
        {
            instance.StopAllCoroutines();
            _ambient.volume = 0f;
            _dynamic.volume = 0f;
        }
    }


    public static void FadeInCaller(AudioSource _source, float speed, float maxVolume)
    {
        instance.StartCoroutine(FadeIn(_source, speed, maxVolume));
    }
    
    public static void FadeOutCaller(AudioSource _source, float speed)
    {
        instance.StartCoroutine(FadeOut(_source, speed));
    }

    static IEnumerator FadeIn(AudioSource _source, float speed, float maxVolume)
    {
        float audioVolume = _source.volume;

        while (_source.volume < maxVolume)
        {
            audioVolume += speed;
            _source.volume = audioVolume;
            yield return new WaitForSeconds(0.1f);
        }
    }
    
    static IEnumerator FadeOut(AudioSource _source, float speed)
    {
        float audioVolume = _source.volume;

        while (_source.volume > 0)
        {
            audioVolume -= speed;
            _source.volume = audioVolume;
            yield return new WaitForSeconds(0.1f);
        }
    }
}
