using System.Collections;
using CartoonFX;
using Imba.Audio;
using Imba.UI;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace SupernovaDriver.Scripts.UI.View
{
    public class GameView : UIView
    {
        [SerializeField] private TextMeshProUGUI   scoreText;
        [SerializeField] private CFXR_ParticleText countDownThree;
        [SerializeField] private CFXR_ParticleText countDownTwo;
        [SerializeField] private CFXR_ParticleText countDownOne;
        [SerializeField] private CFXR_ParticleText countDownStart;

        [SerializeField] private ParticleSystem fxThree;
        [SerializeField] private ParticleSystem fxTwo;
        [SerializeField] private ParticleSystem fxOne;
        [SerializeField] private ParticleSystem fxStart;

        public IEnumerator StartGameCountDown(UnityAction callBack1, UnityAction callBack2)
        {
            AudioManager.Instance.PlaySFX(AudioName.CountDown);
            SetRandomText("3", countDownThree, fxThree);
            yield return new WaitForSeconds(1);
            AudioManager.Instance.PlaySFX(AudioName.CountDown);
            SetRandomText("2", countDownTwo, fxTwo);
            yield return new WaitForSeconds(1);
            AudioManager.Instance.PlaySFX(AudioName.CountDown);
            AudioManager.Instance.PlaySFX(Random.value > 0.5f ? AudioName.SoundCarStart1 : AudioName.SoundCarStart2);
            callBack1?.Invoke();
            SetRandomText("1", countDownOne, fxOne);
            yield return new WaitForSeconds(1);
            AudioManager.Instance.PlaySFX(AudioName.StartGame);
            SetRandomText("Go", countDownStart, fxStart);
            yield return new WaitForSeconds(1);
            callBack2?.Invoke();
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