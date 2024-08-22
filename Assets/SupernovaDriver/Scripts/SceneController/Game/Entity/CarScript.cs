using UnityEngine;

namespace SupernovaDriver.Scripts.SceneController.Game.Entity
{
    public class CarScript : MonoBehaviour
    {
        [SerializeField] private ParticleSystem ghostParticles;
        [SerializeField] private Transform      myMesh;
        [SerializeField] private Renderer       myRender;
        [SerializeField] private Rigidbody      myRigidbody;

        public  float speed;
        public  float rotator;
        public  float rotatingSpeed = 1f;
        private float _forceRotate;
        private float _idAppear;
        private float _keyAppear;
        private float _shake;

        public  KeyCode    key;
     

        public  TextMesh  lapDisplay;
        private Transform _finishLine;
        public  int       idNumber = -1;

        public Material dayCar;
        public Material nightCar;
        public Material customCar;

        public Transform target;
        public Transform myDriver;
        public Transform explosion;

        private GameObject _finishline;
        private Material   _originalCar;
        private int        _lap;
        private Vector3    _enterPos;
        private Vector3    _exitPos;
        private void Start()
        {
            _originalCar = myRender.material;
        }

        private void Update()
        {
            if (_shake > 0f)
            {
                _shake                = Mathf.Max(0f, _shake - Time.deltaTime / 2f);
                myMesh.localPosition = Random.insideUnitSphere * (_shake * 0.08f);
            }

            if (key != 0)
            {
                _forceRotate = Mathf.Min(1f, _forceRotate + Time.deltaTime);
            }
            else
            {
                _forceRotate = Mathf.Max(0f, _forceRotate - Time.deltaTime);
            }

            transform.RotateAround(
                transform.position, Vector3.up,
                rotator * Time.deltaTime * 10f * rotatingSpeed * _forceRotate);
            myRigidbody.rotation = transform.rotation;

            if (!Input.GetKey(key))
            {
                rotator = Mathf.Max(-5f, rotator - Time.deltaTime * 30f);
            }
            else
            {
                rotator = Mathf.Min(5f, rotator + Time.deltaTime * 30f);
            }

            myRigidbody.AddForce(transform.forward * (speed * Time.deltaTime * _forceRotate));
            myRigidbody.angularVelocity = Vector3.zero;
        }

        private void OnCollisionEnter(Collision col)
        {
            if (!(GetComponent<Rigidbody>().velocity.magnitude > 1.1f))
            {
                return;
            }
        }

        private void OnTriggerEnter(Collider col)
        {
            if (col.tag == "FinishLine")
            {
                _enterPos = base.transform.position;
            }
        }

        private void OnTriggerExit(Collider col)
        {
            if (col.tag == "FinishLine")
            {
                _exitPos = base.transform.position;
                if (Vector3.Angle(_finishline.transform.forward, _exitPos - _finishline.transform.position) <= 90f &&
                    Vector3.Angle(_finishline.transform.forward, _enterPos - _finishline.transform.position) > 90f)
                {
                    _lap++;
                }

                if (Vector3.Angle(_finishline.transform.forward, _exitPos - _finishline.transform.position) >= 90f &&
                    Vector3.Angle(_finishline.transform.forward, _enterPos - _finishline.transform.position) < 90f)
                {
                    _lap--;
                }
            }
        }

        public void Login(int number, KeyCode keynew)
        {
            key      = keynew;
            idNumber = number;
            _shake    = 0.5f;
        }
    }
}