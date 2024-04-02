using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float sprintSpeed = 8f;
    private float currentSpeed;
    private bool isMovementLocked = false;
    private Stamina stamina; 

    void Start()
    {
        stamina = GetComponent<Stamina>(); 
    }

    void Update()
    {
        if (!isMovementLocked)
        {
            currentSpeed = Input.GetKey(KeyCode.LeftShift) && stamina.GetCurrentStamina() > 0 ? sprintSpeed : moveSpeed;

            float horizontalInput = Input.GetAxis("Horizontal");
            float verticalInput = Input.GetAxis("Vertical");
            Vector3 movementDirection = new Vector3(horizontalInput, 0f, verticalInput).normalized;

            transform.Translate(movementDirection * currentSpeed * Time.deltaTime);
        }
    }

    public void SetMovementLocked(bool shouldLock)
    {
        isMovementLocked = shouldLock;

        if (shouldLock)
        {
            Rigidbody rb = GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.velocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
            }
        }
    }
}
