using UnityEngine;
using UnityEngine.Events;

namespace SupernovaDriver.Scripts.SceneController.Game.Entity
{
    public class TriggerZoneCallback : MonoBehaviour
    {
        [SerializeField] private UnityEvent callBack;
        private void OnTriggerEnter(Collider other)
        {
            if (other.transform.CompareTag(Constants.CarTag))
            {
                callBack?.Invoke();
            }
        }   
    }
}
