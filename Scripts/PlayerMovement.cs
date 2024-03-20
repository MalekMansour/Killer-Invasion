using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f; 
    public float sprintSpeed = 8f; 
    private float currentSpeed; 

    void Update()
    {
        currentSpeed = Input.GetKey(KeyCode.LeftShift) ? sprintSpeed : moveSpeed;

        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        Vector3 movementDirection = new Vector3(horizontalInput, 0f, verticalInput).normalized;

        transform.Translate(movementDirection * currentSpeed * Time.deltaTime);
    }
}
