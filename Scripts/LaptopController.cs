using UnityEngine;

public class LaptopController : MonoBehaviour
{
    public Canvas laptopCanvas; 
    public Transform playerTransform; 
    public float interactionRadius = 3f; 

    private bool isLaptopOn = false; 

    private void Start()
    {
        laptopCanvas.gameObject.SetActive(false);

        if (playerTransform == null)
        {
            playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && isLaptopOn)
        {
            CloseLaptop();
        }

        if (Vector3.Distance(playerTransform.position, transform.position) <= interactionRadius && Input.GetMouseButtonDown(0) && !isLaptopOn)
        {
            OpenLaptop();
        }
    }

    private void OpenLaptop()
    {
        playerTransform.GetComponent<PlayerMovement>().enabled = false;

        laptopCanvas.gameObject.SetActive(true);

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        isLaptopOn = true;
    }

    private void CloseLaptop()
    {
        playerTransform.GetComponent<PlayerMovement>().enabled = true;

        laptopCanvas.gameObject.SetActive(false);

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        isLaptopOn = false;
    }
}

