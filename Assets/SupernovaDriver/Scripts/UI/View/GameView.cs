using System.Collections;
using Imba.UI;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace SupernovaDriver.Scripts.UI.View
{
    public class GameView : UIView
    {
        [SerializeField] private TextMeshProUGUI scoreText;

        public IEnumerator StartGameCountDown(UnityAction callBack)
        {
            yield return null;
            callBack?.Invoke();
        }

        public void SetDisplayScore(int currentScore)
        {
            scoreText.text = "Score: " + currentScore;
        }
    }
}