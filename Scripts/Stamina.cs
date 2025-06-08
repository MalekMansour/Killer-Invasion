using UnityEngine;

public class Stamina : MonoBehaviour
{
    public float normalMoveSpeed = 10f;  // Speed when not sprinting
    public float sprintMoveSpeed = 16f;  // Speed when sprinting

    private float currentMoveSpeed;
    private PlayerMovement playerMovement;

    private void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
        currentMoveSpeed = normalMoveSpeed;
    }

    void Update()
    {
        // If Left Shift is held, use sprint speed
        if (Input.GetKey(KeyCode.LeftShift))
        {
            currentMoveSpeed = sprintMoveSpeed;
        }
        else
        {
            currentMoveSpeed = normalMoveSpeed;
        }

        playerMovement.moveSpeed = currentMoveSpeed;
    }
}
