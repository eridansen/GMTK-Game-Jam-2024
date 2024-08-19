using Helpers;
using UnityEngine;

namespace Core
{
    public class SaveModule : Singleton<SaveModule>
    {
        private const string MusicVolumeKey = "MusicVolume";
        private const string SfxVolumeKey = "SfxVolume";
        
        public void SaveMusicVolume(float volume)
        {
            PlayerPrefs.SetFloat(MusicVolumeKey, volume);
            PlayerPrefs.Save();
        }
        
        public void SaveSfxVolume(float volume)
        {
            PlayerPrefs.SetFloat(SfxVolumeKey, volume);
            PlayerPrefs.Save();
        }
        
        public float LoadMusicVolume()
        {
            return PlayerPrefs.HasKey(MusicVolumeKey) ? PlayerPrefs.GetFloat(MusicVolumeKey) : 1f;
        }
        
        public float LoadSfxVolume()
        {
            return PlayerPrefs.HasKey(SfxVolumeKey) ? PlayerPrefs.GetFloat(SfxVolumeKey) : 1f;
        }
    }

}