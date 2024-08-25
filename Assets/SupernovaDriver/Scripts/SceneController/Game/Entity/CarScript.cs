using System.Collections.Generic;
using Imba.Audio;
using UnityEngine;

namespace SupernovaDriver.Scripts.SceneController.Game.Entity
{
    public class CarScript : MonoBehaviour
    {
        [SerializeField] private ParticleSystem ghostParticles;
        [SerializeField] private Transform      thisTf;
        [SerializeField] private Transform      myMesh;
        [SerializeField] private Renderer       myRender;
        [SerializeField] private Rigidbody      myRigidbody;
        [SerializeField] private AudioSource    soundDrive;
        [SerializeField] private float          speed;
        [SerializeField] private float          rotator;
        [SerializeField] private float          rotatingSpeed = 1f;
        [SerializeField] private KeyCode        key;

        public TextMesh lapDisplay;

        public Material dayCar;
        public Material nightCar;
        public Material customCar;

        public Transform target;
        public Transform myDriver;
        public Transform explosion;

        private GameObject _finishLineObj;
        private Transform  _finishLineTf;
        private Vector3    _enterPos;
        private Vector3    _exitPos;
        private float      _forceRotate;
        private float      _idAppear;
        private float      _keyAppear;
        private float      _shake;
        private bool       _isInit;
        private bool       _isPause;
        private int        _lap;

        [Header("Effect")]
        [SerializeField] private List<Transform> explosionFx;

        private void Start()
        {
            ghostParticles.gameObject.SetActive(false);
            thisTf = transform;
        }

        private void Update()
        {
            if (!_isInit)
            {
                return;
            }

            if (_shake > 0f)
            {
                _shake               = Mathf.Max(0f, _shake - Time.deltaTime / 2f);
                myMesh.localPosition = Random.insideUnitSphere * (_shake * 0.08f);
            }

            if (key != 0)
            {
                _forceRotate = Mathf.Min(1f, _forceRotate + Time.deltaTime);
            }
            else
            {
                if (!_isPause)
                {
                    _forceRotate = Mathf.Max(0f, _forceRotate - Time.deltaTime);
                }
            }

            thisTf.RotateAround(
                thisTf.position, Vector3.up,
                rotator * Time.deltaTime * 10f * rotatingSpeed * _forceRotate);
            myRigidbody.rotation = thisTf.rotation;

            if (!Input.GetKey(key))
            {
                rotator = Mathf.Max(-5f, rotator - Time.deltaTime * 30f);
            }
            else
            {
                if (!_isPause)
                {
                    rotator = Mathf.Min(5f, rotator + Time.deltaTime * 30f);
                }
            }

            myRigidbody.AddForce(thisTf.forward * (speed * Time.deltaTime * _forceRotate));
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
                _exitPos = transform.position;
                if (Vector3.Angle(_finishLineObj.transform.forward, _exitPos - _finishLineObj.transform.position) <=
                    90f &&
                    Vector3.Angle(_finishLineObj.transform.forward, _enterPos - _finishLineObj.transform.position) >
                    90f)
                {
                    _lap++;
                }

                if (Vector3.Angle(_finishLineObj.transform.forward, _exitPos - _finishLineObj.transform.position) >=
                    90f &&
                    Vector3.Angle(_finishLineObj.transform.forward, _enterPos - _finishLineObj.transform.position) <
                    90f)
                {
                    _lap--;
                }
            }
        }

        public void Login(KeyCode keynew)
        {
            key    = keynew;
            _shake = 0.5f;
        }

        public void Init()
        {
            soundDrive.loop = true;
            soundDrive.Play(22000);
            _isInit = true;
        }

        public void Pause()
        {
            soundDrive.Pause();
            _isPause = true;
        }

        public void Resume()
        {
            soundDrive.UnPause();
            _isPause = false;
        }

        private void OnEndGame()
        {
            PlayExplosion();
            soundDrive.Pause();
            myMesh.SetActive(false);
            _isInit = false;
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
    }
}