using UnityEngine;
using UnityEngine.UI;

public class Stamina : MonoBehaviour
{
    public Slider staminaBar; 
    public float maxStamina = 1f; 
    public float sprintStaminaCost = 0.1f;
    public float staminaRegenRate = 0.2f; 

    [HideInInspector]
    public float currentStamina; 

    private void Start()
    {
        currentStamina = maxStamina;
        UpdateStaminaBar();
    }

    private void Update()
    {
        if (currentStamina <= 0)
        {
            GetComponent<PlayerMovement>().SetMovementLocked(true);
        }
        else
        {
            GetComponent<PlayerMovement>().SetMovementLocked(false);

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
