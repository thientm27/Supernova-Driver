using Elympics;
using SupernovaDriver.Scripts.SceneController.Game.Entity;
using UnityEngine;

namespace SupernovaDriver.Scripts.SceneController.Game
{
    public class InputManager : ElympicsMonoBehaviour, IInputHandler, IUpdatable
    {
        [SerializeField] private CarScript carScript;

        public void ElympicsUpdate()
        {
            if (ElympicsBehaviour.TryGetInput(PredictableFor, out var inputReader))
            {
                inputReader.Read(out bool isMoving);
                carScript.SetControl(isMoving);
            }
        }

        public void OnInputForClient(IInputWriter inputWriter)
        {
            if (Elympics.Player != PredictableFor)
                return;

            var isMoving = Input.GetKey(KeyCode.Space);
            inputWriter.Write(isMoving);
        }

        public void OnInputForBot(IInputWriter inputSerializer)
        {
        }
    }
}