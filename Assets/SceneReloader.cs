using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class SceneReloader : MonoBehaviour
{
    [Header("Input")]
    [SerializeField] private InputActionReference restartSceneAction;

    [SerializeField] private ConfirmationMenu confirmationMenu;

    private void OnEnable()
    {
        restartSceneAction.action.performed += OnRestart;
        restartSceneAction.action.Enable();
    }

    private void OnDisable()
    {
        restartSceneAction.action.performed -= OnRestart;
        restartSceneAction.action.Disable();
    }

    private void OnRestart(InputAction.CallbackContext context)
    {
        ReloadScene();
    }

    private void ReloadScene()
    {
        confirmationMenu.Show(
            confirmAction: () =>
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            },
            cancelAction: () =>
            {
                Debug.Log("Scene Restart Canceled");
            },
            text:"Reload Scene?"
        );
        
    }
}
