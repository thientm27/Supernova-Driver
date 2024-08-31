using UnityEngine;

namespace SupernovaDriver.Scripts.SceneController.Game.Entity
{
    public class ScoreTrigger : MonoBehaviour
    {
        private bool isScored = false;

        public void Init()
        {
            isScored = false;
        }

        public void OnTriggerDoubleScore()
        {
            if (isScored)
            {
                return;
            }

            isScored = true;
        }

        public void OnTriggerNormalScore()
        {
            if (isScored)
            {
                return;
            }

            isScored = true;
        }
    }
}