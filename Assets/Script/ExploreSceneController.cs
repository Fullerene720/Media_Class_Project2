using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class ExploreSceneController : MonoBehaviour
{
    private void Update()
    {
        if (Keyboard.current != null &&
            Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            ReturnToMainMenu();
        }
    }

    private void ReturnToMainMenu()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        SceneManager.LoadScene("MainMenu");
    }
}