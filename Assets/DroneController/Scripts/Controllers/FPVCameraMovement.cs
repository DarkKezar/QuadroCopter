using UnityEngine;

namespace DroneController
{
    public class FPVCameraMovement : MonoBehaviour
    {
        [Header("Project References:")]
        [Header("Scene References:")]
        [SerializeField] private Transform _objecToFollow = default;


        protected virtual void FixedUpdate()
        {
            ApplyCameraRotation();
        }

        private void ApplyCameraRotation()
        {
            transform.rotation = Quaternion.Euler(
                _objecToFollow.rotation.eulerAngles.x,
                _objecToFollow.rotation.eulerAngles.y,
                _objecToFollow.rotation.eulerAngles.z);
        }
    }
}