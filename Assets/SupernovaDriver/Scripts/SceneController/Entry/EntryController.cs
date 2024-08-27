using DuckSurvivor.Scripts.Configs;
using Services;
using UnityEngine;

namespace SupernovaDriver.Scripts.SceneController.Entry
{
    public class EntryController : MonoBehaviour
    {
        [SerializeField] private GameObject canvas;
        [SerializeField] private GameObject services;

		private GameServices gameServices = null;
		private void Awake()
        {
            Application.targetFrameRate = 60;

			if (GameObject.FindGameObjectWithTag(Constants.ServicesTag) == null)
			{
				GameObject gameServiceObject = new(nameof(GameServices))
				{
					tag = Constants.ServicesTag
				};
				gameServices = gameServiceObject.AddComponent<GameServices>();

				// Add Services
				gameServices.AddService(new ElympicsService());
			}
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