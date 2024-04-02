using UnityEngine;
using UnityEngine.UI;

public class Stamina : MonoBehaviour
{
    public float maxStamina = 5f; // Maximum sprint time in seconds
    public float staminaRechargeRate = 1f; // Rate at which stamina recharges per second
    public Slider staminaSlider; // Reference to the Stamina Wheel UI slider
    private float currentStamina; // Current stamina value
    private PlayerMovement playerMovement; // Reference to the PlayerMovement script

    private void Start()
    {
        currentStamina = maxStamina; // Initialize current stamina to maximum
        staminaSlider.maxValue = maxStamina; // Set maximum value for the stamina slider
        staminaSlider.value = maxStamina; // Set initial value for the stamina slider
        playerMovement = GetComponent<PlayerMovement>(); // Get the PlayerMovement component
    }

    private void Update()
    {
        if (currentStamina < maxStamina)
        {
            // Recharge stamina over time
            currentStamina += staminaRechargeRate * Time.deltaTime;
            currentStamina = Mathf.Clamp(currentStamina, 0f, maxStamina); // Clamp stamina within limits

            // Update the stamina slider value
            staminaSlider.value = currentStamina;
        }
    }

    public void UseStamina(float amount)
    {
        // Reduce stamina when sprinting or using stamina
        currentStamina -= amount;
        currentStamina = Mathf.Clamp(currentStamina, 0f, maxStamina); // Clamp stamina within limits

        // Update the stamina slider value
        staminaSlider.value = currentStamina;

        // Lock movement if out of stamina
        if (currentStamina <= 0f)
        {
            playerMovement.SetMovementLocked(true);
        }
    }

    public void ReplenishStamina(float amount)
    {
        // Increase stamina (e.g., when resting or after a cooldown period)
        currentStamina += amount;
        currentStamina = Mathf.Clamp(currentStamina, 0f, maxStamina); // Clamp stamina within limits

        // Update the stamina slider value
        staminaSlider.value = currentStamina;

        // Unlock movement when stamina is replenished
        playerMovement.SetMovementLocked(false);
    }
}

