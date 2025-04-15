using UnityEngine;
using DroneScripts;

namespace DroneController
{
    public class DamageManager : MonoBehaviour
    {
        private static DamageManager _instance;
        public static DamageManager Instance { get { return _instance; } }

        [SerializeField] private float _health = 100f;
        [SerializeField] private float _damageMultiplier = 1f;
        [SerializeField] private float _minSpeedForDamage = 2f;

        public float Health { get { return _health; } }
        public bool Broken { get; private set; }
        public int CollisionCount { get; private set; }

        [SerializeField] private Animator droneAnimator;
        [SerializeField] private GameObject droneSound;

        private void Awake()
        {
            if (_instance == null)
            {
                _instance = this;
            }
            else if (_instance != this)
            {
                Destroy(gameObject);
            }
        }

        private void Start()
        {
            DroneEventManager.onDroneDestroy.AddListener(DestroyDrone);
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (Broken || collision.gameObject.CompareTag("Invisible Wall")) return;

            CollisionCount++;
            float impactSpeed = collision.relativeVelocity.magnitude;

            if (impactSpeed > _minSpeedForDamage)
            {
                float damage = impactSpeed * _damageMultiplier;

                ApplyDamage(damage);
                DroneEventManager.DroneHit();
            }
        }

        void ApplyDamage(float damage)
        {
            _health -= damage;

            if (_health <= 0)
            {
                _health = 0;
                DroneEventManager.LevelFailed();
                if (!ObjectiveManager.Instance.levelComplete)
                    DroneEventManager.GameOver();
            }
        }

        void DestroyDrone()
        {
            droneSound = GameObject.Find("drone_sound");

            if (droneAnimator != null)
                droneAnimator.enabled = false;

            droneSound.SetActive(false);


            Broken = true;
        }
    }
}
