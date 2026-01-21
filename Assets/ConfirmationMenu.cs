using UnityEngine;
using TMPro;

public class ConfirmationMenu : MonoBehaviour
{

    [Header("Confirmation Settings")]
    [SerializeField] private Transform XROrigin;
    [SerializeField] private float distanceFromPlayer = 1.5f;
    [SerializeField] private TMP_Text confirmationText;

    private System.Action onConfirm;
    private System.Action onCancel;

    void LateUpdate()
    {
        // Always face the player (comfort-friendly)
        Vector3 lookDir = transform.position - XROrigin.position;
        lookDir.y = 0;
        transform.rotation = Quaternion.LookRotation(lookDir);
    }

    public void Show(System.Action confirmAction, System.Action cancelAction,string text)
    {
        onConfirm = confirmAction;
        onCancel = cancelAction;
        confirmationText.text = text;
        transform.position =
            XROrigin.position + XROrigin.forward * distanceFromPlayer;

        gameObject.SetActive(true);
    }

    public void Confirm()
    {
        onConfirm?.Invoke();
        Hide();
    }

    public void Cancel()
    {
        onCancel?.Invoke();
        Hide();
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

}
