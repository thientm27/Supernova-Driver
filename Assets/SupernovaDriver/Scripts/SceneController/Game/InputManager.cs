using Elympics;
using SupernovaDriver.Scripts.SceneController.Game.Entity;
using SupernovaDriver.Scripts.Utilities;
using UnityEngine;

namespace SupernovaDriver.Scripts.SceneController.Game
{
    public class InputManager : ElympicsMonoBehaviour, IInputHandler, IUpdatable
    {
        [SerializeField] private CarScript carScript;

        private bool _run = false;

        private readonly int _inputAbsenceFallbackTicks = 4;

        public void ElympicsUpdate()
        {
            if (ElympicsBehaviour.TryGetInput(ElympicsPlayer.FromIndex(0), out var inputReader,
                    _inputAbsenceFallbackTicks))
            {
                inputReader.Read(out _run);

                carScript.SetControl(_run);
            }
        }

        public void OnInputForClient(IInputWriter inputWriter)
        {
            _run = Input.GetKey(KeyCode.Space);
            inputWriter.Write(_run);
        }

        public void OnInputForBot(IInputWriter inputSerializer)
        {
        }
    }
}