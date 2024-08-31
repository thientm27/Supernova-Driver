using System.Collections.Generic;
using Elympics;
using Imba.Audio;
using UnityEngine;

namespace SupernovaDriver.Scripts.SceneController.Game.Entity
{
    public class CarScript : ElympicsMonoBehaviour, IInitializable, IUpdatable
    {
        [SerializeField] private ParticleSystem ghostParticles;
        [SerializeField] private Transform      myMesh;
        [SerializeField] private Rigidbody      myRigidbody;
        [SerializeField] private AudioSource    soundDrive;

        [SerializeField] private float configSpeed;
        [SerializeField] private float configRotator;
        [SerializeField] private float configRotatingSpeed;

        public  ElympicsFloat speed;
        public  ElympicsFloat rotator;
        public  ElympicsFloat rotatingSpeed;
        public  ElympicsFloat forceRotate;
        public  ElympicsBool  isMove;
        public  ElympicsBool  isInit;
        public  ElympicsBool  isPause;
        public  ElympicsBool  isEndGame;
        private GameObject    _finishLineObj;
        private Transform     _finishLineTf;

        [Header("Effect")]
        [SerializeField] private List<Transform> explosionFx;

        public void SetControl(bool value)
        {
            isMove.Value = value;
        }

        public void Init()
        {
            soundDrive.loop    = true;
            soundDrive.enabled = true;
            isInit.Value       = true;
        }

        public void Pause()
        {
            soundDrive.enabled = false;
            isPause.Value      = false;
        }

        public void Resume()
        {
            soundDrive.enabled = true;
            isPause.Value      = false;
        }

        private void OnEndGame()
        {
            PlayExplosion();
            soundDrive.enabled = false;
            myMesh.SetActive(false);
            isInit.Value = false;
        }

        private void PlayExplosion()
        {
            if (explosionFx.Count == 0)
            {
                return;
            }

            var x = Random.Range(0, explosionFx.Count);
            explosionFx[x].SetActive(true);
            var rd = Random.Range(35, 38);
            AudioManager.Instance.PlaySFX((AudioName)rd);
        }

        #region NEW ElympicsMonoBehaviour

        public void Initialize()
        {
            speed         = new(configSpeed);
            rotator       = new(configRotator);
            rotatingSpeed = new(configRotatingSpeed);
            forceRotate   = new(0);

            isMove    = new(false);
            isInit    = new(false);
            isPause   = new(false);
            isEndGame = new(false);

            soundDrive.enabled = false;
            ghostParticles.gameObject.SetActive(false);
        }

        public void ElympicsUpdate()
        {
            if (!isInit || isEndGame)
            {
                return;
            }

            if (!isPause)
            {
                forceRotate.Value = Mathf.Min(1f, forceRotate.Value + Elympics.TickDuration);
            }
            else
            {
                forceRotate.Value = Mathf.Max(0f, forceRotate.Value - Elympics.TickDuration);
            }

            // thisTf.RotateAround(thisTf.position, Vector3.up,
            //     rotator.Value * Elympics.TickDuration * 10f * rotatingSpeed.Value * forceRotate.Value);
            Quaternion rotationDelta = Quaternion.AngleAxis(
                rotator.Value * Elympics.TickDuration * 10f * rotatingSpeed.Value * forceRotate.Value,
                Vector3.up
            );

            myRigidbody.rotation *= rotationDelta;
            //myRigidbody.rotation =  thisTf.rotation;


            if (!isMove)
            {
                rotator.Value = (Mathf.Max(-5f, rotator - Elympics.TickDuration * 30f));
            }
            else
            {
                if (!isPause)
                {
                    rotator.Value = (Mathf.Min(5f, rotator + Elympics.TickDuration * 30f));
                }
            }

            var x = speed.Value;
            var y = Elympics.TickDuration;
            var z = forceRotate.Value;

            myRigidbody.AddForce(myRigidbody.transform.forward *
                                 (speed.Value * Elympics.TickDuration * forceRotate.Value));
            myRigidbody.angularVelocity = Vector3.zero;
        }

        private void OnCollisionEnter(Collision col)
        {
            if (col.transform.CompareTag(Constants.DeadZoneTag))
            {
                if (Elympics.IsServer)
                {
                    soundDrive.enabled = false;
                    isInit.Value       = false;
                    isEndGame.Value    = true;

                    Elympics.EndGame();
                    GameController.Instance.EndGame(false);
                }
                else
                {
                    OnEndGame();
                    GameController.Instance.EndGame(true);
                }

                col.gameObject.SetActive(false);
            }
        }

        #endregion
    }
}