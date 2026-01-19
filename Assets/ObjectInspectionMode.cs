using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactors;
using UnityEngine.XR.Interaction.Toolkit.Locomotion.Teleportation;
using UnityEngine.XR.Interaction.Toolkit.Locomotion.Turning;

public class ObjectInspectionMode : MonoBehaviour
{
    [Header("XR References")]
    public TeleportationProvider teleportProvider;
    public NearFarInteractor rightNearFarInteractor;
    public SnapTurnProvider snapTurnProvider;

    [Header("Rotation Script")]
    public LimitedObjectRotation rotationController;

    private bool inspectionActive;

    public void EnterInspectionMode()
    {
        inspectionActive = true;

        // Disable teleport system
        if (teleportProvider != null)
            teleportProvider.enabled = false;

        // Disable right-hand ray / far interaction
        //if (rightNearFarInteractor != null)
            //rightNearFarInteractor.enabled = false;

        // Enable rotation
        if (rotationController != null)
            rotationController.enabled = true;
    }

    public void ExitInspectionMode()
    {
        inspectionActive = false;

        if (teleportProvider != null)
            teleportProvider.enabled = true;

        if (rightNearFarInteractor != null)
            rightNearFarInteractor.enabled = true;

        if (rotationController != null)
            rotationController.enabled = false;
    }
}
