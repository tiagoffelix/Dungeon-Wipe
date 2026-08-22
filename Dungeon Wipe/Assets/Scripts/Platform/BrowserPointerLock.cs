using UnityEngine;

/// <summary>
/// Browsers only grant pointer lock in response to a user gesture, so the
/// Cursor.lockState the gameplay scene requests on Start is refused on WebGL and
/// mouse look stays dead until the player clicks. This re-requests the lock on
/// the first click after it is lost, and stays out of the way while a menu is
/// open or the game is paused. Added automatically on WebGL only.
/// </summary>
public class BrowserPointerLock : MonoBehaviour
{
    private void Update()
    {
        if (!Input.GetMouseButtonDown(0))
        {
            return;
        }

        // A visible cursor or a stopped clock means a menu owns the input.
        if (Cursor.visible || Time.timeScale == 0f)
        {
            return;
        }

        if (Cursor.lockState != CursorLockMode.Locked)
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
    }
}
