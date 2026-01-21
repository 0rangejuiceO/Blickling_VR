using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Repositioner : MonoBehaviour
{
    [Header("Input")]
    [SerializeField] private InputActionReference repositionAction;

    [Header("Transforms")]
    [SerializeField] private Transform xrOriginTransform;
    [SerializeField] private Transform targetTransform;
    [SerializeField] private Transform lookTransform;

    private void OnEnable()
    {
        repositionAction.action.performed += OnReposition;
        repositionAction.action.Enable();
    }

    private void OnDisable()
    {
        repositionAction.action.performed -= OnReposition;
        repositionAction.action.Disable();
    }

    private void OnReposition(InputAction.CallbackContext context)
    {
        Reposition();
    }

    private void Reposition()
    {
        xrOriginTransform.position = targetTransform.position;
        Vector3 lookDirection = lookTransform.position - targetTransform.position;
        lookDirection.y = 0;

        xrOriginTransform.rotation = Quaternion.LookRotation(lookDirection.normalized);
    }

}
