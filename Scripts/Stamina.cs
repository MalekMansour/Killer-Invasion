using UnityEngine;
using UnityEngine.UI;

public class Stamina : MonoBehaviour
{
    public Slider staminaSlider;
    public float maxStamina = 1f;
    public float sprintStaminaCost = 0.1f;
    public float staminaRegenRate = 0.2f;
    public float CurrentStamina { get; private set; } // Make currentStamina accessible

    private bool isSprinting = false;

    void Start()
    {
        CurrentStamina = maxStamina; // Initialize CurrentStamina
        UpdateStaminaUI();
    }

    void Update()
    {
        if (isSprinting)
        {
            CurrentStamina -= sprintStaminaCost * Time.deltaTime;
            UpdateStaminaUI();
        }
        else if (CurrentStamina < maxStamina)
        {
            CurrentStamina += staminaRegenRate * Time.deltaTime;
            CurrentStamina = Mathf.Clamp(CurrentStamina, 0f, maxStamina);
            UpdateStaminaUI();
        }

        if (CurrentStamina <= 0f)
        {
            isSprinting = false;
        }
    }

    public void StartSprinting()
    {
        isSprinting = true;
    }

    public void StopSprinting()
    {
        isSprinting = false;
    }

    void UpdateStaminaUI()
    {
        staminaSlider.value = CurrentStamina / maxStamina;
        staminaSlider.gameObject.SetActive(isSprinting || CurrentStamina < maxStamina);
    }
}
