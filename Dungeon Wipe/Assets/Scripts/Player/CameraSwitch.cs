using UnityEngine;

public class CameraSwitch : MonoBehaviour
{
    [SerializeField] private Camera firstPersonCamera;
    [SerializeField] private Camera thirdPersonCamera;

    [SerializeField] private Camera deathCamera;

    [SerializeField] private Stats playerStats;

    [SerializeField] private GameObject head;
    [SerializeField] private GameObject body;

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
            if (playerStats.Dead)
            {
                head.gameObject.SetActive(true);
                body.gameObject.SetActive(true);
                firstPersonCamera.gameObject.SetActive(false);
                thirdPersonCamera.gameObject.SetActive(false);
                deathCamera.gameObject.SetActive(true);
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
        head.gameObject.SetActive(true);
        body.gameObject.SetActive(true);
        firstPersonCamera.gameObject.SetActive(false);
        thirdPersonCamera.gameObject.SetActive(true);
    }

    private void ToggleToFirstPerson()
    {
        head.gameObject.SetActive(false);
        body.gameObject.SetActive(false);
        firstPersonCamera.gameObject.SetActive(true);
        thirdPersonCamera.gameObject.SetActive(false);
    }
}
