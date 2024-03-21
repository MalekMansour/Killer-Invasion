using UnityEngine;
public class FlashlightToggle : MonoBehaviour
{
    public Light flashlight;
    private bool isRightClicking = false;

    void Update()
    {
        if (Input.GetMouseButtonDown(1)) 
        {
            isRightClicking = true; 
            flashlight.enabled = true; 
        }

        if (Input.GetMouseButtonUp(1)) 
        {
            isRightClicking = false; 
            flashlight.enabled = false; 
        }
    }
}
