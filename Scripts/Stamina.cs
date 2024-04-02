using UnityEngine;
using UnityEngine.UI;

public class Stamina : MonoBehaviour
{
    public Slider staminaBar;
    public float maxStamina = 1f;
    public float minStamina = 0.05f; // Minimum stamina value
    public float staminaDecreaseRate = 0.1f;
    public float staminaIncreaseRate = 0.2f;

    private float currentStamina;

    void Start()
    {
        currentStamina = maxStamina;
        UpdateStaminaBar();
    }

    void Update()
    {
        if (currentStamina > 0)
        {
            currentStamina -= staminaDecreaseRate * Time.deltaTime;
            currentStamina = Mathf.Clamp(currentStamina, minStamina, maxStamina);
            UpdateStaminaBar();
        }
    }

    void UpdateStaminaBar()
    {
        staminaBar.value = currentStamina / maxStamina;
    }

    public void IncreaseStamina(float amount)
    {
        currentStamina += amount;
        currentStamina = Mathf.Clamp(currentStamina, minStamina, maxStamina);
        UpdateStaminaBar();
    }

    public void DecreaseStamina(float amount)
    {
        currentStamina -= amount;
        currentStamina = Mathf.Clamp(currentStamina, minStamina, maxStamina);
        UpdateStaminaBar();
    }

    public bool CanSprint()
    {
        return currentStamina > minStamina;
    }

    public void ShowStaminaBar(bool show)
    {
        staminaBar.gameObject.SetActive(show);
    }
}
