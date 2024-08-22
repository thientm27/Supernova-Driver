using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Audio
{
	[System.Serializable]
	public struct Sound
	{
		public string Name;
		public List<AudioClip> AudioClips;
		[Range(0f, 1f)]
		public float Volume;
		public AudioClip AudioClip()
        {
			if (AudioClips.Count <= 1) return AudioClips[0];
			int rd = UnityEngine.Random.Range(0, AudioClips.Count);
			return AudioClips[rd];
        }
	}
}
