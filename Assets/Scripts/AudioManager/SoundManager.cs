using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField] private AudioSource _sfxSource;
    [SerializeField] private AudioSource _musicSource;

    public static SoundManager instance = null;

    private float _lowPitchRange = .95f;
    private float _highPitchRange = 1.05f;

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

    public void RandomizeSfx(params AudioClip[] clips)
    {
        int randomIndex = Random.Range(0, clips.Length);
        float randomPitch = Random.Range(_lowPitchRange, _highPitchRange);

        _sfxSource.pitch = randomPitch;
        _sfxSource.clip = clips[randomIndex];
        _sfxSource.Play();
    }

    private AudioClip LoadClip(string name)
    {
        string path = "Sounds/" + name;
        AudioClip clip = Resources.Load<AudioClip>("Sounds/step_01");
        Debug.Log(path);
        return clip;
    }

    public void PlaySoundByPath(string path)
    {
        _sfxSource.clip = LoadClip(path);
        _sfxSource.Play();
    }

    public static void PlaySound(AudioClip _clip)
    {
        AudioSource audioSource = instance.gameObject.AddComponent<AudioSource>();//.AddComponent<AudioSource>();
        audioSource.PlayOneShot(_clip);
        instance.StartCoroutine(instance.atFinish(audioSource));
    }

    private IEnumerator atFinish(AudioSource _clip)
    {
        do yield return null; while( _clip.isPlaying );
        Destroy(_clip);
    }
}