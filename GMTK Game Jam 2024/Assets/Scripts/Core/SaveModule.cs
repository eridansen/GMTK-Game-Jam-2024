using Helpers;
using UnityEngine;

namespace Core
{
    public class SaveModule : Singleton<SaveModule>
    {
        private const string MusicVolumeKey = "MusicVolume";
        private const string SfxVolumeKey = "SfxVolume";
        private const string BackgroundPositionXKey = "BackgroundPositionX";
        private const string BackgroundPositionYKey = "BackgroundPositionY";
        private const string IntroCutsceneKey = "IntroCutsceneKey";
        
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
        
        public void SaveBackgroundPositionX(float volume)
        {
            PlayerPrefs.SetFloat(BackgroundPositionXKey, volume);
            PlayerPrefs.Save();
        }
        
        public void SaveBackgroundPositionY(float volume)
        {
            PlayerPrefs.SetFloat(BackgroundPositionYKey, volume);
            PlayerPrefs.Save();
        }
        
        public void SaveIntroCutscene(int value)
        {
            PlayerPrefs.SetInt(IntroCutsceneKey, value);
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
        
        public float LoadBackgroundPositionX()
        {
            return PlayerPrefs.HasKey(BackgroundPositionXKey) ? PlayerPrefs.GetFloat(BackgroundPositionXKey) : 0f;
        }
        
        public float LoadBackgroundPositionY()
        {
            return PlayerPrefs.HasKey(BackgroundPositionYKey) ? PlayerPrefs.GetFloat(BackgroundPositionYKey) : 0f;
        }
        
        public float LoadIntroCutscene()
        {
            return PlayerPrefs.HasKey(IntroCutsceneKey) ? PlayerPrefs.GetInt(IntroCutsceneKey) : 0f;
        }
    }

}