using System.Collections;
using CartoonFX;
using DG.Tweening;
using Imba.Audio;
using Imba.UI;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace SupernovaDriver.Scripts.UI.View
{
    public class GameView : UIView
    {
        [SerializeField] private TextMeshProUGUI scoreText;
        [SerializeField] private Transform       countDownThree;
        [SerializeField] private Transform       countDownTwo;
        [SerializeField] private Transform       countDownOne;
        [SerializeField] private Transform       countDownStart;

        public IEnumerator StartGameCountDown(UnityAction callBack1, UnityAction callBack2)
        {
            AudioManager.Instance.PlaySFX(AudioName.CountDown);
            DoCountDownAnimation(countDownThree);
            yield return new WaitForSeconds(1);
            AudioManager.Instance.PlaySFX(AudioName.CountDown);
            DoCountDownAnimation(countDownTwo);
            yield return new WaitForSeconds(1);
            AudioManager.Instance.PlaySFX(AudioName.CountDown);
            AudioManager.Instance.PlaySFX(Random.value > 0.5f ? AudioName.SoundCarStart1 : AudioName.SoundCarStart2);
            callBack1?.Invoke();
            DoCountDownAnimation(countDownOne);
            yield return new WaitForSeconds(1);
            AudioManager.Instance.PlaySFX(AudioName.StartGame);
            DoCountDownAnimation(countDownStart);
            yield return new WaitForSeconds(1);
            callBack2?.Invoke();
        }

        public void DoCountDownAnimation(Transform target)
        {
            target.SetActive(true);
        }

        public void SetDisplayScore(int currentScore)
        {
            scoreText.text = "Score: " + currentScore;
        }

        public void SetRandomText(string text, CFXR_ParticleText dynamicParticleText, ParticleSystem particles)
        {
            float size        = Mathf.Lerp(0.8f, 1.3f, 1f);
            Color randomColor = new Color(Random.value, Random.value, Random.value);
            dynamicParticleText.UpdateText(text, size, randomColor);
            particles.Play(true);
        }
    }
}