using Helpers;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : Singleton<AudioManager>
{
    [SerializeField] 
    private AudioSource musicSource, sfxSource;
    public Sound[] musicSounds, sfxSounds;
    

    public void PlayMusic(string name)
    {
        Sound s = Array.Find(musicSounds, s => s.name == name);
        if (s == null)
        {
            Debug.Log("Sound does not exist");
        }
        else
        {
            musicSource.clip = s.clip;
            musicSource.Play();
        }
    }

    public void PlaySfx(string name)
    {
        Sound s = Array.Find(sfxSounds, s => s.name == name);
        if (s == null)
        {
            Debug.Log("Sound does not exist");
        }
        else
        {
            sfxSource.PlayOneShot(s.clip);
        }
    }
}
