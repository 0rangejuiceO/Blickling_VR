using UnityEngine;
using UnityEngine.InputSystem;

public class QuitGame : MonoBehaviour
{
    [Header("Input")]
    [SerializeField] private InputActionReference quitGameAction;

    [SerializeField] private ConfirmationMenu confirmationMenu;

    private void OnEnable()
    {
        quitGameAction.action.performed += OnQuitGame;
        quitGameAction.action.Enable();
    }

    private void OnDisable()
    {
        quitGameAction.action.performed -= OnQuitGame;
        quitGameAction.action.Disable();
    }

    private void OnQuitGame(InputAction.CallbackContext context)
    {
        Quit();
    }

    private void Quit()
    {
        confirmationMenu.Show(
            confirmAction: () =>
            {
                Debug.Log("Quitting Game...");
#if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false; // Stops play mode in Editor
#else
                Application.Quit(); // Quits the built game
#endif
            },
            cancelAction: () =>
            {
                Debug.Log("Quit Canceled");
            },
            text: "Quit Application"
        );
    }
}
