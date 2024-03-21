using UnityEngine;
public class FlashlightToggle : MonoBehaviour
{
    public Light flashlight; 
    private bool isFlashlightOn = false; 

    void Start()
    {
        flashlight.enabled = false;
    }

    void Update()
    {
        if (Input.GetMouseButton(1)) 
        {
            ToggleFlashlight();
        }
    }

    void ToggleFlashlight()
    {
        isFlashlightOn = !isFlashlightOn; 
        flashlight.enabled = isFlashlightOn;
    }
}
