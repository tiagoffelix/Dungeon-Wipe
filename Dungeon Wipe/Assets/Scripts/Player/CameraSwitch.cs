using UnityEngine;

public class CameraSwitch : MonoBehaviour
{
    [SerializeField] private Camera firstPersonCamera;
    [SerializeField] private Camera thirdPersonCamera;
    [SerializeField] private Camera deathCamera;

    [SerializeField] private Stats playerStats;

    // New serialized fields for player body and head
    [SerializeField] private GameObject playerBody;
    [SerializeField] private GameObject playerScarf;
    [SerializeField] private GameObject playerHead;

    private Animator animator;

    private void Start()
    {
        animator = gameObject.GetComponent<Animator>();
    }

    private void Update()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Spawn"))
        {
            if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.9f && animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f)
            {
                animator.Play("Idle");
                deathCamera.gameObject.SetActive(false);
                ToggleToFirstPerson();
            }
        }
        else
        {
            if (playerStats.Health <= 0)
            {
                firstPersonCamera.gameObject.SetActive(false);
                thirdPersonCamera.gameObject.SetActive(false);
                deathCamera.gameObject.SetActive(true);

                // Ensure player body and head are visible upon death
                playerBody.SetActive(true);
                playerScarf.SetActive(true);
                playerHead.SetActive(true);
            }
            else
            {
                // Check if the 'C' key is being held down
                if (Input.GetKey(KeyCode.C))
                {
                    ActivateThirdPersonCamera();
                }
                // Check if the 'C' key has been released
                else if (Input.GetKeyUp(KeyCode.C))
                {
                    ToggleToFirstPerson();
                }
            }
        }
    }

    private void ActivateThirdPersonCamera()
    {
        firstPersonCamera.gameObject.SetActive(false);
        thirdPersonCamera.gameObject.SetActive(true);

        // Ensure player body and head are visible in third-person
        playerBody.SetActive(true);
        playerScarf.SetActive(true);
        playerHead.SetActive(true);
    }

    private void ToggleToFirstPerson()
    {
        firstPersonCamera.gameObject.SetActive(true);
        thirdPersonCamera.gameObject.SetActive(false);

        // Hide player body and head in first-person
        playerBody.SetActive(false);
        playerScarf.SetActive(false);
        playerHead.SetActive(false);
    }
}
