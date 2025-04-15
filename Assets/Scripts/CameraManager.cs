using UnityEngine;
using UnityEngine.InputSystem;

public class CameraManager : MonoBehaviour
{
    public GameObject TPVCamera;
    public GameObject FPVCamera;

    [SerializeField] public GameObject _activeCamera;
    [SerializeField] private InputActionReference _changeCameraInput;

    private void Start()
    {
        SetActiveCamera(_activeCamera);
    }

    private void OnEnable()
    {
        _changeCameraInput.action.performed += OnChangeCamera;
    }

    private void OnDisable()
    {
        _changeCameraInput.action.performed -= OnChangeCamera;
    }

    private void OnChangeCamera(InputAction.CallbackContext context)
    {
        ToggleCamera();
    }

    private void ToggleCamera()
    {
        if (_activeCamera == FPVCamera)
        {
            SetActiveCamera(TPVCamera);
        }
        else
        {
            SetActiveCamera(FPVCamera);
        }
    }

    private void SetActiveCamera(GameObject cameraToActivate)
    {
        FPVCamera.SetActive(false);
        TPVCamera.SetActive(false);

        cameraToActivate.SetActive(true);
        _activeCamera = cameraToActivate;
    }
}
