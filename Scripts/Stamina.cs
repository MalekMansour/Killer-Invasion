using UnityEngine;
using UnityEngine.UI;

public class Stamina : MonoBehaviour
{
    public Slider staminaBar; // Reference to the stamina bar UI slider
    public float maxStamina = 1f;
    public float sprintSpeed = 8f;
    public float staminaConsumptionRate = 0.5f; // Adjust as needed

    private float currentStamina;

    void Start()
    {
        currentStamina = maxStamina;
        UpdateStaminaBar();
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.LeftShift) && currentStamina > 0)
        {
            // Sprinting behavior
            MovePlayer(sprintSpeed);
            currentStamina -= staminaConsumptionRate * Time.deltaTime;
            UpdateStaminaBar();
        }
        else
        {
            // Normal movement
            MovePlayer(moveSpeed);
            if (currentStamina < maxStamina)
            {
                currentStamina += Time.deltaTime; // Stamina regeneration
                UpdateStaminaBar();
            }
        }

        // Ensure stamina doesn't go below 0 or above max value
        currentStamina = Mathf.Clamp(currentStamina, 0f, maxStamina);
    }

    void MovePlayer(float speed)
    {
        // Implement your player movement logic here
    }

    void UpdateStaminaBar()
    {
        if (currentStamina <= 0)
        {
            // Hide stamina bar when stamina is depleted
            staminaBar.gameObject.SetActive(false);
        }
        else
        {
            // Show stamina bar when sprinting and stamina > 0
            staminaBar.gameObject.SetActive(Input.GetKey(KeyCode.LeftShift));
            staminaBar.value = currentStamina / maxStamina;
        }
    }
}
