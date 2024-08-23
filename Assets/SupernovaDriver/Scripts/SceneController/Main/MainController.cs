using UnityEngine;
using UnityEngine.SceneManagement;

namespace SupernovaDriver.Scripts.SceneController.Main
{
    public class MainController : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
            SceneManager.LoadScene(Constants.GameScene);
        }

        // Update is called once per frame
        void Update()
        {
        }
    }
}