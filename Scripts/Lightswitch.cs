using UnityEngine;
public class Lightswitch : MonoBehaviour
{
    public Light lights; 
    public float activationRange = 3f; 
    private bool isLightsOn = false; 

    void Start()
    {
        lights.enabled = false;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit) && hit.collider.gameObject == gameObject)
            {
                if (Vector3.Distance(transform.position, hit.point) <= activationRange)
                {
                    ToggleLights();
                }
            }
        }
    }

    void ToggleLights()
    {
        isLightsOn = !isLightsOn; 

        lights.enabled = isLightsOn;

        if (isLightsOn)
        {
            Debug.Log("Lights turned on.");
        }
        else
        {
            Debug.Log("Lights turned off.");
        }
    }
}
