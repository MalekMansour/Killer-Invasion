using UnityEngine;
using UnityEngine.UI;

public class Stamina : MonoBehaviour
{
    public Slider staminaBar; // Reference to the Stamina Bar UI Slider
    public float maxStamina = 1f; // Maximum stamina value
    public float sprintStaminaCost = 0.1f; // Stamina cost while sprinting
    public float staminaRegenRate = 0.2f; // Stamina regeneration rate per second

    [HideInInspector]
    public float currentStamina; // Current stamina value

    private void Start()
    {
        currentStamina = maxStamina;
        UpdateStaminaBar();
    }

    private void Update()
    {
        if (currentStamina <= 0)
        {
            // Disable sprinting and lock movement
            GetComponent<PlayerMovement>().SetMovementLocked(true);
        }
        else
        {
            // Enable sprinting and unlock movement
            GetComponent<PlayerMovement>().SetMovementLocked(false);

            // Decrease stamina while sprinting
            if (Input.GetKey(KeyCode.LeftShift))
            {
                if (currentStamina > 0.01f)
{
    currentStamina -= sprintStaminaCost * Time.deltaTime;
    UpdateStaminaBar();
}
else
{
    currentStamina = 0.01f; // Minimum stamina value
    UpdateStaminaBar();
}

            }
            else
            {
                // Regenerate stamina over time
                if (currentStamina < maxStamina)
                {
                    currentStamina += staminaRegenRate * Time.deltaTime;
                    UpdateStaminaBar();
                }
            }
        }
    }

    private void UpdateStaminaBar()
    {
        staminaBar.value = currentStamina / maxStamina;
    }
}
