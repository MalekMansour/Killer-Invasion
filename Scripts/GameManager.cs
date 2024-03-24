using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject crosshair; 
    public LayerMask interactableLayer; 
    public Canvas laptopCanvas; 
    private bool isCursorLocked = true; 

    private void Start()
    {
        LockCursor();
    }

    private void Update()
    {
        UpdateCrosshairPosition();

        if (Input.GetMouseButtonDown(0) && isCursorLocked)
        {
            bool isLaptopUIActive = laptopCanvas.gameObject.activeSelf;

            if (!isLaptopUIActive)
            {
                InteractWithObject();
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ToggleCursorLock();
        }
    }

    void UpdateCrosshairPosition()
    {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = 10f; 
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(mousePosition);

        crosshair.transform.position = worldPosition;
    }

    void InteractWithObject()
    {
        Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2f, Screen.height / 2f, 0f));
        RaycastHit hitInfo;

        if (Physics.Raycast(ray, out hitInfo, Mathf.Infinity, interactableLayer))
        {
            GameObject hitObject = hitInfo.collider.gameObject;
            Debug.Log("Interacting with: " + hitObject.name);
        }
    }

    void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        isCursorLocked = true;
    }

    void UnlockCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        isCursorLocked = false;
    }

    void ToggleCursorLock()
    {
        if (isCursorLocked)
        {
            bool isLaptopUIActive = laptopCanvas.gameObject.activeSelf;

            if (!isLaptopUIActive)
            {
                UnlockCursor();
            }
        }
        else
        {
            LockCursor();
        }
    }
}
