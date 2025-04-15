using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameUIClose : MonoBehaviour
{
    GameObject GameUI;
    GameObject GlobalVolume;
    EventFunctions EventF;
    [SerializeField] private InputActionReference _toggleUI;
    [SerializeField] private InputActionReference _togglePostProcessing;

    private void Start()
    {
        GameUI = GameObject.Find("GameUI");
        GlobalVolume = GameObject.Find("Global Volume");
        EventF = FindObjectOfType<EventFunctions>();

        //DroneScripts.DroneEventManager.onDroneDestroy.AddListener(ToggleUI);
    }

    private void OnEnable()
    {
        _toggleUI.action.performed += ToggleUI;
        _togglePostProcessing.action.performed += TogglePostProcessing;
    }

    private void OnDisable()
    {
        _toggleUI.action.performed -= ToggleUI;
        _togglePostProcessing.action.performed -= TogglePostProcessing;
    }

    private void ToggleUI(InputAction.CallbackContext context)
    {
        EventF.toggleGameObject(GameUI);
    }

    private void TogglePostProcessing(InputAction.CallbackContext context)
    {
        EventF.toggleGameObject(GlobalVolume);
    }
}
