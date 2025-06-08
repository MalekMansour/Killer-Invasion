using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 10f;
    public float sprintSpeed = 16f;
    private float currentSpeed;
    private bool isMovementLocked = false;

    void Update()
    {
        if (!isMovementLocked)
        {
            // Toggle sprinting with Shift
            if (Input.GetKey(KeyCode.LeftShift))
            {
                currentSpeed = sprintSpeed;
            }
            else
            {
                currentSpeed = moveSpeed;
            }

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
