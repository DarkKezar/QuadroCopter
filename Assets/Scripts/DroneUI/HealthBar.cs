using UnityEngine;
using TMPro;
using DroneController;
using UnityEngine.UI;
using DroneScripts;

public class HealthBar : MonoBehaviour
{
    private Slider healthSlider = default;

    private float _maxHealth;

    void Start()
    {
        DroneEventManager.onDroneGetHit.AddListener(UpdateBar);

        healthSlider = GetComponent<Slider>();
        _maxHealth = DamageManager.Instance.Health;
    }

    void UpdateBar()
    {
        healthSlider.value = Mathf.InverseLerp(0f, _maxHealth, DamageManager.Instance.Health);
    }

    string FormatHeathText(float health)
    {
        return string.Format("{0}/{1}", health, _maxHealth);
    }
}
