using Helpers;
using System;
using UnityEngine;

public class AudioManager : Singleton<AudioManager>
{
    [SerializeField] private AudioSource _musicSource;
    [SerializeField] private AudioSource _sfxSource;
    [SerializeField] private Sound[] _musicSounds;

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
    public void PlaySoundFXClip(AudioClip audioClip, Transform spawntransform, float volume)
    {
        AudioSource audioSource = Instantiate(_sfxSource, spawntransform.position, Quaternion.identity);

        audioSource.clip = audioClip;

        audioSource.volume = volume;

        audioSource.Play();

        float clipLength = audioSource.clip.length;

        Destroy(audioSource.gameObject, clipLength);
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
}
