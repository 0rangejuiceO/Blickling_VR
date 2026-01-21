using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class VRJoystickHandle : MonoBehaviour
{
    [SerializeField] private Vector2 xLimits = new Vector2(-0.05f, 0.05f);
    [SerializeField] private Vector2 yLimits = new Vector2(-0.05f, 0.05f);
    [SerializeField] private float returnSpeed = 12f;

    private XRGrabInteractable grab;
    private Vector3 startLocalPos;
    private bool isGrabbed;


    void Awake()
    {
        grab = GetComponent<XRGrabInteractable>();
        startLocalPos = transform.position;

        grab.selectEntered.AddListener(_ => isGrabbed = true);
        grab.selectExited.AddListener(_ => isGrabbed = false);
    }

    void LateUpdate()
    {


        Vector3 local = transform.position;

        if (!isGrabbed)
        {
            local = Vector3.Lerp(local, startLocalPos, Time.deltaTime * returnSpeed);
        }

        local.z = Mathf.Clamp(local.z, startLocalPos.z- xLimits.x, startLocalPos.z+ xLimits.y);
        local.y = Mathf.Clamp(local.y, startLocalPos.y- yLimits.x, startLocalPos.y + yLimits.y);
        local.x = startLocalPos.x;

        transform.position = local;
    }

    public Vector2 GetNormalized()
    {
        float z = Mathf.InverseLerp(startLocalPos.z - xLimits.x, startLocalPos.z + xLimits.y, transform.position.z) * 2f - 1f;
        float y = Mathf.InverseLerp(startLocalPos.y - yLimits.x, startLocalPos.y + yLimits.y, transform.position.y) * 2f - 1f;
        return new Vector2(z, y);
    }
}
