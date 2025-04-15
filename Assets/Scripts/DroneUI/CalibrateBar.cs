using UnityEngine;
using DroneController;
using UnityEngine.UI;

namespace DroneScripts
{
    public class CalibrateBar : MonoBehaviour
    {
        public Slider calibrateSlider;

        private ObjectiveManager _frameManager;
        private DroneMovement _droneMovement;
        private float _calibrationProgress;

        void Start()
        {
            _frameManager = FindObjectOfType<ObjectiveManager>();
            _droneMovement = FindObjectOfType<DroneMovement>();
            //_calibrationProgress = _droneMovement.GetCalibrationProgress();
        }

        void Update()
        {
            //_calibrationProgress = _droneMovement.GetCalibrationProgress();
            calibrateSlider.value = _calibrationProgress;
        }
    }
}

