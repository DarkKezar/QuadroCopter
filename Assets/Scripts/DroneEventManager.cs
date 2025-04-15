using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace DroneScripts
{
    public static class DroneEventManager
    {
        public static UnityEvent onDroneDestroy = new();
        public static UnityEvent onTimeOut = new();
        public static UnityEvent onLevelPassed = new();
        public static UnityEvent onCollected = new();
        public static UnityEvent onCalibrate = new();
        public static UnityEvent onDroneGetHit = new();
        public static UnityEvent onGamePaused = new();
        public static UnityEvent onGameOver = new();

        public static UnityEvent onRequestsDone = new();

        public static void GameOver()
        {
            onGameOver.Invoke();
        }
        public static void Collected()
        {
            onCollected?.Invoke();
        }

        public static void DroneHit()
        {
            onDroneGetHit?.Invoke();
        }

        public static void RequestsDone()
        {
            onRequestsDone?.Invoke();
        }

        public static void LevelFailed()
        {
            onDroneDestroy?.Invoke();
        }

        public static void LevelPassed()
        {
            onLevelPassed?.Invoke();
        }

        public static void GamePaused()
        {
            if (PauseManager.Instance.IsPaused)
                PauseManager.Instance.SetPaused(false);
            else
                PauseManager.Instance.SetPaused(true);
        }
    }
}