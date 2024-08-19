using Helpers;
using System;
using System.Collections;
using UnityEngine;

public class AudioManager : Singleton<AudioManager>
{
    [SerializeField] private AudioSource _musicSource;
    [SerializeField] private AudioSource _sfxSource;

    [SerializeField] private float _fadeTime = 0f;

    public void PlayMusicClip(Sound sound)
    {
        if (sound.clip == _musicSource.clip)
            return;

        StartCoroutine(ChangeMusic(sound.clip));
        
    }
    
    public IEnumerator ChangeMusic(AudioClip newMusic)
    {
        float fadeTime = _fadeTime; // Adjust fade time as needed
        float startVolume = _musicSource.volume;

        while (_musicSource.volume > 0)
        {
            _musicSource.volume -= startVolume * Time.deltaTime / fadeTime;
            yield return null;
        }

        _musicSource.clip
 = newMusic;
        _musicSource.Play();

        while (_musicSource.volume < 1)
        {
            _musicSource.volume += startVolume * Time.deltaTime / fadeTime;
            yield return null;
        }
    }

    public void PlaySoundFXClip(AudioClip audioClip, Transform spawntransform, float volume)
    {
        AudioSource audioSource = Instantiate(_sfxSource, spawntransform.position, Quaternion.identity);

        audioSource.clip = audioClip;

        audioSource.volume = volume;

        audioSource.Play();

        float clipLength = audioSource.clip.length;

        Destroy(audioSource.gameObject, clipLength);
    }
            
    public void PlayRandomSoundFXClip(AudioClip[] audioClip, Transform spawntransform, float volume)
    {
        int randomIndex = UnityEngine.Random.Range(0, audioClip.Length);
        
        PlaySoundFXClip(audioClip[randomIndex], spawntransform, volume);
        
    }

    public void SetMusicVolume(float volume)
    {
        _musicSource.volume = Mathf.Clamp(volume, 0f, 1f);
    }

    public void SetSfxVolume(float volume)
    {
        _sfxSource.volume = Mathf.Clamp(volume, 0f, 1f);
    }

    public float GetMusicVolume()
    {
        return _musicSource.volume;
    }

    public float GetSfxVolume()
    {
        return _sfxSource.volume;
    }

    public void StopMusic()
    {
        _musicSource.Stop();
    }
}
