using UnityEngine;
using UnityEngine.UI;

public class Stamina : MonoBehaviour
{
    public Slider staminaSlider;
    public float maxStamina = 1f;
    public float currentStamina;
    public float staminaDecreaseRate = 0.1f;
    public float staminaIncreaseRate = 0.05f;

    void Start()
    {
        currentStamina = maxStamina;
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.LeftShift) && currentStamina > 0)
        {
            currentStamina -= staminaDecreaseRate * Time.deltaTime;
            UpdateStaminaBar();
        }
        else if (currentStamina < maxStamina)
        {
            currentStamina += staminaIncreaseRate * Time.deltaTime;
            UpdateStaminaBar();
        }
    }

    void UpdateStaminaBar()
    {
        currentStamina = Mathf.Clamp(currentStamina, 0f, maxStamina);
        staminaSlider.value = currentStamina;
    }
}
