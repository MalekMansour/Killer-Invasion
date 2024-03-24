using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject crosshair;
    public LayerMask interactableLayer;
    private bool isCursorLocked = true;
    private float lastEscapeTime;
    private float escapeWindow = 0.5f; 

    private void Start()
    {
        LockCursor();
    }

    private void Update()
    {
        UpdateCrosshairPosition();

        if (Input.GetMouseButtonDown(0) && isCursorLocked)
        {
            LockCursor(); 
            InteractWithObject();
        }

        if (Input.GetMouseButtonDown(1) && isCursorLocked)
        {
            LockCursor(); // Lock cursor on right-click
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            float currentTime = Time.time;

            if (currentTime - lastEscapeTime < escapeWindow)
            {
                ToggleCursorLock(); // Double press Esc to toggle cursor lock
            }

            lastEscapeTime = currentTime;
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
            UnlockCursor();
        }
        else
        {
            LockCursor();
        }
    }
}
