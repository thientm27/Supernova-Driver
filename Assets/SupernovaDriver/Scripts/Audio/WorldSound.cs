using System.Collections;
using UnityEngine;

namespace Audio
{
    public class WorldSound : MonoBehaviour
    {
        [SerializeField] private AudioSource audioSource;
        [SerializeField] private Transform soundTf;
        public void Initialized(Sound sound, Vector3 position)
        {
            var audioClip = sound.AudioClip();
            audioSource.clip = audioClip;
            audioSource.volume = sound.Volume;
            soundTf.position = position;
            audioSource.Play();
            StartCoroutine(DelayDespawn(audioClip.length));
        }
        private IEnumerator DelayDespawn(float time)
        {
            yield return new WaitForSeconds(time + 0.3f);
            SimplePool.Despawn(gameObject);
        }
    }
}