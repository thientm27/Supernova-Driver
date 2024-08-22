using System;
using System.Collections.Generic;
using Audio;
using Imba.Utils;
using UnityEngine;

namespace DuckSurvivor.Scripts.Services
{
	public enum MusicName
	{
		BossTheme,
		Forest,
		MainMenu
	}

	public enum UIAudioName
	{
		ChestOpenEnd,
		ChestScale,
		ChestShowSingle,
		Click,
		PopupLose,
		PopupLevelUp,
		PopupWin,
	}

	public enum IngameAudio
	{
		Attack,
		Hit,
		FootStep,
		Test,
		DeathHit,
		Jump,
		Jump2,
	}

	public class AudioService : ManualSingletonMono<AudioService>
	{
		[SerializeField] private List<Sound> sounds;
		[SerializeField] private List<Sound> worldSounds;
		[SerializeField] private Music       music;
		[SerializeField] private GameObject  worldSoundPrefab;

		public event Action<bool> OnSoundChanged;
		public event Action<float> OnSoundVolumeChanged;
		public event Action<bool> OnMusicChanged;
		public event Action<float> OnMusicVolumeChanged;
		private bool                            soundOn;
		private bool                            musicOn;
		private bool                            vibrateOn;
		private float                           soundVolume;
		private float                           musicVolume;
		private Dictionary<string, AudioSource> soundAudioSources;
		private Dictionary<string, Sound>       worldSoundAudio = new();

		// Cache
		private Dictionary<string, float> soundVolumes      = new();
		private Dictionary<string, float> worldSoundVolumes = new();
		private Dictionary<string, AudioSource> worldSoundAudioSource = new();

		public override void Awake()
		{
			base.Awake();
			music.Initialized(this);
			soundAudioSources = new Dictionary<string, AudioSource>();
			foreach (var sound in sounds)
			{
				AudioSource soundSource = gameObject.AddComponent<AudioSource>();
				soundSource.clip        = sound.AudioClip();
				soundSource.volume      = sound.Volume;
				soundSource.playOnAwake = false;
				soundAudioSources.Add(sound.Name, soundSource);
			}

			foreach (var audioSource in soundAudioSources)
			{
				soundVolumes.Add(audioSource.Key, audioSource.Value.volume);
			}

			foreach (var sound in worldSounds)
			{
				worldSoundAudio.Add(sound.Name, sound);
				worldSoundVolumes.Add(sound.Name, sound.Volume);

				AudioSource soundSource = gameObject.AddComponent<AudioSource>();
				soundSource.clip        = sound.AudioClip();
				soundSource.volume      = sound.Volume;
				soundSource.playOnAwake = false;
				worldSoundAudioSource.Add(sound.Name, soundSource);
			}
		}


		public void PlaySound(UIAudioName name)
		{
			if (!soundOn)
			{
				return;
			}

			try
			{
				soundAudioSources[name.ToString()].Play();
			}
			catch
			{
			}
		}
		public void PlayIngameAudio(IngameAudio audio)
		{
			if (!soundOn)
			{
				return;
			}

			try
			{
				worldSoundAudioSource[audio.ToString()].clip = worldSoundAudio[audio.ToString()].AudioClip();

				worldSoundAudioSource[audio.ToString()].Play();
			}
			catch
			{

			}
		}
		public void StopASound(IngameAudio nameGUISound)
		{
			soundAudioSources[nameGUISound.ToString()].loop = false;
			soundAudioSources[nameGUISound.ToString()].Stop();
		}
		public void PlaySound(IngameAudio nameSound, bool isLoop = false)
		{
			if (!soundOn && soundVolume > 0.0f)
			{
				return;
			}

			AudioSource audioSource = soundAudioSources[nameSound.ToString()];
			if (audioSource.isPlaying)
			{
				// if (isOverLap)
				// {
				// 	// Tạo một AudioSource mới
				// 	AudioSource newAudioSource = gameObject.AddComponent<AudioSource>();
				//
				// 	newAudioSource.clip = audioSource.clip;
				// 	newAudioSource.volume = audioSource.volume;
				// 	newAudioSource.pitch = audioSource.pitch;
				// 	newAudioSource.Play();
				//
				// 	return;
				// }

				return;
			}

			audioSource.Play();
			audioSource.loop = isLoop;
		}

		// Play Music
		public void PlayMusic(MusicName musicName)
		{
			if (musicOn == true && musicVolume > 0.0f)
			{
				try
				{
					music.PlayMusic(musicName.ToString());
				}
				catch
				{
				}
			}
		}

		// Fade Music
		public void FadeMusic(float time)
		{
			if (musicOn == true && musicVolume > 0.0f)
			{
				music.FadeMusic("music", time);
			}
		}

		// End
		public void StopAllSound()
		{
			foreach (var audioSource in soundAudioSources)
			{
				audioSource.Value.Stop();
			}
		}

		public void StopMusic()
		{
			music.StopMusic("music");
		}

		public bool IsMusicPlaying()
		{
			return music.IsMusicPlaying("music");
		}

		public void SetMusicVolume(float volume)
		{
			MusicVolume = volume;
		}

		public void SetSoundVolume(float volume)
		{
			SoundVolume = volume;
		}

		public void SetVibrate(bool isOn)
		{
			VibrateOn = isOn;
		}

		public void Vibrate()
		{
#if UNITY_ANDROID
			if(vibrateOn == true)
			{
				Handheld.Vibrate();
			}
#endif
		}

		#region Get - Set

		// GET - SET
		public float SoundVolume
		{
			get { return soundVolume; }
			set
			{
				soundVolume = value;
				foreach (var audioSource in soundAudioSources)
				{
					audioSource.Value.volume = soundVolume * soundVolumes[audioSource.Key];
				}

				foreach (var audioSource in worldSoundAudioSource)
				{
					audioSource.Value.volume = soundVolume * worldSoundVolumes[audioSource.Key];
				}

				OnSoundVolumeChanged?.Invoke(soundVolume);
			}
		}

		public float MusicVolume
		{
			get { return musicVolume; }
			set
			{
				musicVolume = value;
				OnMusicVolumeChanged?.Invoke(musicVolume);
			}
		}
		public bool SoundOn
		{
			get { return soundOn; }
			set
			{
				soundOn = value;
				if (SoundOn == false)
				{
					StopAllSound();
				}

				OnSoundChanged?.Invoke(soundOn);
			}
		}

		public bool MusicOn
		{
			get { return musicOn; }
			set
			{
				musicOn = value;
				if (musicOn == true && musicVolume > 0)
				{
					music.PlayMusic("music");
				}
				else
				{
					music.StopMusic("music");
				}

				OnMusicChanged?.Invoke(musicOn);
			}
		}
		public bool VibrateOn
		{
			get { return vibrateOn; }
			set { vibrateOn = value; }
		}

		// AUDIO PLAY
		//
		// public void PlaySound(SoundToPlay nameSound)
		// {
		//     //if (!soundOn)
		//     //{
		//     //    return;
		//     //}
		//
		//     //soundAudioSources[nameSound.ToString()].Play();
		// }

		#endregion

		// public void PlaySound(WorldAudioName audioName, Vector3 position)
		// {
		//     if (!soundOn) return;
		//     try
		//     {
		//         GameObject worldSoundGO = SimplePool.Spawn(worldSoundPrefab, position, Quaternion.identity);
		//         WorldSound worldSound = worldSoundGO.GetComponent<WorldSound>();
		//         worldSound.Initialized(worldSoundAudio[audioName.ToString()], position);
		//     }
		//     catch
		//     {
		//     }
		// }

		//public void PlayRandomSound(List<WorldAudioName> listNameSound, Vector3 position)
		//{
		//    if (!soundOn) return;
		//    try
		//    {
		//        int rd = UnityEngine.Random.Range(0, listNameSound.Count);
		//        WorldAudioName audioName = listNameSound[rd];
		//        PlaySound(audioName, position);
		//    }
		//    catch
		//    {
		//    }
		//}
	}
}