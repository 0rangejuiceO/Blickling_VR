using UnityEngine;

public class JoystickRotationDriver : MonoBehaviour
{
    [SerializeField] private GameObject handleObject;
    [SerializeField] private float maxRotation = 20f;

    private VRJoystickHandle handle;
    [SerializeField] private POIHandler poiHandler;
    private Quaternion startRotation;

    private void Awake()
    {
        handle = handleObject.GetComponent<VRJoystickHandle>();
    }

    void Start()
    {
        startRotation = transform.localRotation;
    }

    void Update()
    {
        if (!poiHandler.inUse) {
            if (handleObject.activeSelf)
            {
                handleObject.SetActive(false);
            }
            return; 
        }
        if (!handleObject.activeSelf)
        {
            handleObject.SetActive(true);
        }
        Vector2 input = handle.GetNormalized();

        float yRotation = input.x * maxRotation;
        float xRotation = input.y * maxRotation * -1;

        transform.localRotation =
            startRotation * Quaternion.Euler(xRotation, yRotation, 0f);
    }
}
