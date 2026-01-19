using UnityEngine;
using UnityEngine.InputSystem;

public class LimitedObjectRotation : MonoBehaviour
{
    [Header("Rotation Settings")]
    public float rotationSpeed = 60f;
    public float maxAngle = 30f;

    [Header("Input Action (Vector2)")]
    public InputActionProperty rotateInput;

    private float currentX;
    private float currentY;

    void OnEnable()
    {
        rotateInput.action.Enable();
    }

    void OnDisable()
    {
        rotateInput.action.Disable();
    }

    void Update()
    {
        if (!enabled) return;

        Vector2 input = rotateInput.action.ReadValue<Vector2>();

        currentY += input.x * rotationSpeed * Time.deltaTime;
        currentX -= input.y * rotationSpeed * Time.deltaTime;

        currentX = Mathf.Clamp(currentX, -maxAngle, maxAngle);
        currentY = Mathf.Clamp(currentY, -maxAngle, maxAngle);

        transform.localRotation = Quaternion.Euler(currentX, currentY, 0f);
    }

}
