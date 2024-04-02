using UnityEngine;
using UnityEngine.UI;

public class Stamina : MonoBehaviour
{
    public float maxStamina = 5f;
    public float sprintSpeed = 8f;
    public Slider staminaBar;
    private float currentStamina; // Private field to store current stamina

    private bool isSprinting = false;

    private void Start()
    {
        currentStamina = maxStamina;
        UpdateStaminaBar();
    }

    public float GetCurrentStamina()
    {
        return currentStamina;
    }

    private void Update()
    {
        if (isSprinting)
        {
            currentStamina -= Time.deltaTime;
            UpdateStaminaBar();

            if (currentStamina <= 0f)
            {
                currentStamina = 0f;
                isSprinting = false;
            }
        }
        else if (currentStamina < maxStamina)
        {
            currentStamina += Time.deltaTime * 0.5f; // Stamina regen rate
            UpdateStaminaBar();
        }

        if (Input.GetKey(KeyCode.LeftShift) && !isSprinting && currentStamina >= 0.1f)
        {
            StartSprint();
        }
    }

    private void StartSprint()
    {
        isSprinting = true;
    }

    private void UpdateStaminaBar()
    {
        staminaBar.value = currentStamina / maxStamina;
    }
}
