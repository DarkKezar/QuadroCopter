using DroneScripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GamePauseButton : MonoBehaviour
{
    public GameObject pause;
    public GameObject window;
    public GameObject inputManager;
    private EventFunctions EventF;
    [SerializeField] private InputActionReference _togglePause;

    private void Start()
    {
        EventF = FindObjectOfType<EventFunctions>();
    }

    private void OnEnable()
    {
        _togglePause.action.performed += TogglePause;
    }

    private void OnDisable()
    {
        _togglePause.action.performed -= TogglePause;
    }

    private void TogglePause(InputAction.CallbackContext context)
    {
        EventF.toggleGameObject(window);
        EventF.toggleGameObject(pause);
        if (window.activeSelf)
        {
            PauseManager.Instance.SetPaused(true);
        }
        else
        {
            PauseManager.Instance.SetPaused(false);
        }

    }
}
