using Helpers;
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
        _musicSource.clip = sound.clip;
        _musicSource.loop = true;
        _musicSource.Play();    
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
        if (audioClip.Length==0){
            return;
        }
        int randomIndex = UnityEngine.Random.Range(0, audioClip.Length);
        PlaySoundFXClip(audioClip[randomIndex], spawntransform, GetSfxVolume() * volume);
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
