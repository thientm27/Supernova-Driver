using System.Collections.Generic;
using Elympics;
using Imba.Audio;
using UnityEngine;

namespace SupernovaDriver.Scripts.SceneController.Game.Entity
{
    public class CarScript : ElympicsMonoBehaviour, IInitializable, IUpdatable
    {
        [SerializeField] private ParticleSystem ghostParticles;
        [SerializeField] private Transform      thisTf;
        [SerializeField] private Transform      myMesh;
        [SerializeField] private Renderer       myRender;
        [SerializeField] private Rigidbody      myRigidbody;
        [SerializeField] private AudioSource    soundDrive;

        [SerializeField] private float configSpeed;
        [SerializeField] private float configRotator;
        [SerializeField] private float configRotatingSpeed;

        public ElympicsFloat speed;
        public ElympicsFloat rotator;
        public ElympicsFloat rotatingSpeed;
        public ElympicsFloat forceRotate;
        public ElympicsBool  isMove;
        public ElympicsBool  isInit;
        public ElympicsBool  isPause;

        private GameObject _finishLineObj;
        private Transform  _finishLineTf;
        private Vector3    _enterPos;
        private Vector3    _exitPos;

        [Header("Effect")]
        [SerializeField] private List<Transform> explosionFx;

        public void SetControl(bool value)
        {
            isMove.Value = value;
        }

        public void ElympicsUpdate()
        {
            if (!isInit)
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

            // if (isPause)
            // {
            //     forceRotate.Value = Mathf.Min(1f, forceRotate.Value + Elympics.TickDuration);
            // }
            // else
            // {
            //     forceRotate.Value = Mathf.Max(0f, forceRotate.Value - Elympics.TickDuration);
            // }

            thisTf.RotateAround(thisTf.position, Vector3.up,
                rotator.Value * Elympics.TickDuration * 10f * rotatingSpeed.Value * forceRotate.Value);
            myRigidbody.rotation = thisTf.rotation;


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

            myRigidbody.AddForce(thisTf.forward * (speed.Value * Elympics.TickDuration * forceRotate.Value));
            myRigidbody.angularVelocity = Vector3.zero;
        }

        private void OnCollisionEnter(Collision col)
        {
            if (col.transform.CompareTag(Constants.DeadZoneTag))
            {
                OnEndGame();
                col.gameObject.SetActive(false);
            }
        }

        private void OnTriggerEnter(Collider col)
        {
            if (col.CompareTag("FinishLine"))
            {
                _enterPos = thisTf.position;
            }
        }

        private void OnTriggerExit(Collider col)
        {
            if (col.CompareTag("FinishLine"))
            {
            }
        }

        public void Init()
        {
            soundDrive.loop    = true;
            soundDrive.enabled = true;

            isInit.Value = true;
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
            GameController.Instance.EndGame();
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

            isMove  = new(false);
            isInit  = new(false);
            isPause = new(false);

            soundDrive.enabled = false;
            ghostParticles.gameObject.SetActive(false);
            thisTf = transform;
        }

        #endregion
    }
}