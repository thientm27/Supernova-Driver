using DuckSurvivor.Scripts.Configs;
using UnityEngine;

namespace SupernovaDriver.Scripts.SceneController.Entry
{
    public class EntryController : MonoBehaviour
    {
        [SerializeField] private GameObject canvas;
        [SerializeField] private GameObject services;

        private void Awake()
        {
            Application.targetFrameRate = 60;
        }

        private void Start()
        {
            //DontDestroyOnLoad(canvas);
            DontDestroyOnLoad(services);
            // LOAD ALL CONFIG
            // ConfigManager.Instance.LoadAllConfigLocal();
            UnityEngine.SceneManagement.SceneManager.LoadScene(Constants.MainScene);
        }
    }
}