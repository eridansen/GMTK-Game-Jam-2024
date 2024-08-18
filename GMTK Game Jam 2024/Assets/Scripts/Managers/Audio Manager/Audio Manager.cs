using Helpers;
using System;
using UnityEngine;

public class AudioManager : Singleton<AudioManager>
{
    [SerializeField] private AudioSource _musicSource;
    [SerializeField] private AudioSource _sfxSource;
    [SerializeField] private Sound[] _musicSounds;
    [SerializeField] private Sound[] _sfxSounds;
    
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

    public void PlayMusic(string name)
    {
        Sound s = Array.Find(_musicSounds, s => s.name == name);
        if (s == null)
        {
            Debug.Log("Sound does not exist");
        }
        else
        {
            _musicSource.clip = s.clip;
            _musicSource.Play();
        }
    }

    public void PlaySfx(string name)
    {
        Sound s = Array.Find(_sfxSounds, s => s.name == name);
        if (s == null)
        {
            Debug.Log("Sound does not exist");
        }
        else
        {
            _sfxSource.PlayOneShot(s.clip);
        }
    }
}
