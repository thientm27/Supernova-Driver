using System;
using Imba.Utils;
using UnityEngine;

namespace DuckSurvivor.Scripts.Services
{
    public class AudioSettingService : Singleton<AudioSettingService>
    {
        // Key Audio
        private const string MusicVolumeKey = "mvl";
        private const string SoundVolumeKey = "svl";
        private const string VibrateKey = "vbr";
        #region Audio
        
        public Action<float> OnMusicVolumeChange;
        public Action<float> OnSoundVolumeChange;

        public Action<bool> OnVibrateChange;
        
        public float GetMusicVolume()
        {
            return PlayerPrefs.GetFloat(MusicVolumeKey, 1.0f);
        }
        public void SetMusicVolume(float volume)
        {
            PlayerPrefs.SetFloat(MusicVolumeKey, volume);
            OnMusicVolumeChange?.Invoke(volume);
        }
        public float GetSoundVolume()
        {
            return PlayerPrefs.GetFloat(SoundVolumeKey, 1.0f);
        }
        public void SetSoundVolume(float volume)
        {
            PlayerPrefs.SetFloat(SoundVolumeKey, volume);
            OnSoundVolumeChange?.Invoke(volume);
        }
        public bool GetVibrate()
        {
            return PlayerPrefs.GetInt(VibrateKey, 1) != 0;
        }
        public void SetVibrate(bool isVibrate)
        {
            OnVibrateChange?.Invoke(isVibrate);
            if (isVibrate == true)
            {
                PlayerPrefs.SetInt(VibrateKey, 1);
            }
            else
            {
                PlayerPrefs.SetInt(VibrateKey, 0);
            }
        }
        #endregion
    }
}