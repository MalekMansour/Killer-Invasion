using UnityEngine;
public class DoorKnock : MonoBehaviour
{
    public AudioClip knockSound; 
    private AudioSource audioSource;
    public float knockRange = 5f; 

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        audioSource.clip = knockSound;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && IsMouseOverDoor() && IsWithinKnockRange())
        {
            audioSource.Play();
        }
    }

    bool IsMouseOverDoor()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitInfo;

        if (Physics.Raycast(ray, out hitInfo))
        {
            if (hitInfo.collider != null && hitInfo.collider.gameObject == gameObject)
            {
                return true;
            }
        }

        return false; 
    }

    bool IsWithinKnockRange()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            float distance = Vector3.Distance(transform.position, player.transform.position);
            return distance <= knockRange; 
        }

        return false; 
    }
}
