using UnityEngine;

/// <summary>
/// Manages switching between first-person, third-person, and death cameras.
/// </summary>
public class CameraSwitch : MonoBehaviour
{
    [SerializeField] private Camera firstPersonCamera; // Camera for first-person view
    [SerializeField] private Camera thirdPersonCamera; // Camera for third-person view
    [SerializeField] private Camera deathCamera; // Camera used when the player dies

    [SerializeField] private Stats playerStats; // Player statistics

    // Player's body and head parts for enabling/disabling based on camera view
    [SerializeField] private GameObject playerBody;
    [SerializeField] private GameObject playerScarf;
    [SerializeField] private GameObject playerHead;

    private Animator animator;

    /// <summary>
    /// Initializes the Animator component.
    /// </summary>
    private void Start()
    {
        animator = gameObject.GetComponent<Animator>();
    }

    /// <summary>
    /// Handles camera switching based on player status and input.
    /// </summary>
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

                // Show the player's body and head in death camera view
                playerBody.SetActive(true);
                playerScarf.SetActive(true);
                playerHead.SetActive(true);
            }
            else
            {
                // Check if the 'C' key is held down to switch to third-person
                if (Input.GetKey(KeyCode.C))
                {
                    ActivateThirdPersonCamera();
                }
                // Check if the 'C' key is released to switch back to first-person
                else if (Input.GetKeyUp(KeyCode.C))
                {
                    ToggleToFirstPerson();
                }
            }
        }
    }

    /// <summary>
    /// Activates the third-person camera and makes the player's body and head visible.
    /// </summary>
    private void ActivateThirdPersonCamera()
    {
        firstPersonCamera.gameObject.SetActive(false);
        thirdPersonCamera.gameObject.SetActive(true);

        playerBody.SetActive(true);
        playerScarf.SetActive(true);
        playerHead.SetActive(true);
    }

    /// <summary>
    /// Activates the first-person camera and hides the player's body and head.
    /// </summary>
    private void ToggleToFirstPerson()
    {
        firstPersonCamera.gameObject.SetActive(true);
        thirdPersonCamera.gameObject.SetActive(false);

        playerBody.SetActive(false);
        playerScarf.SetActive(false);
        playerHead.SetActive(false);
    }
}
