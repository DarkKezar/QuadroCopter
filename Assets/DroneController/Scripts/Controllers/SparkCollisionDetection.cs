using UnityEngine;

namespace DroneController
{
    [RequireComponent(typeof(Rigidbody))]
    public class SparkCollisionDetection : MonoBehaviour
    {
        public delegate void SparkCollisionDetectionEventHandler(ContactPoint contactPoint);
        public static event SparkCollisionDetectionEventHandler CollisionDetected;

        protected virtual void OnCollisionEnter(Collision collision)
        {
            if (!collision.gameObject.CompareTag("Invisible Wall"))
                CollisionDetected?.Invoke(collision.GetContact(0));
        }
    }
}
