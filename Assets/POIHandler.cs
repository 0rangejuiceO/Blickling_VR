using UnityEngine;

public class POIHandler : MonoBehaviour
{
    private GameObject currentPOI = null;

    public bool preLoadPOIs = false;
    public bool inUse = false;

    public void SetCurrentPOI(GameObject poi = null, bool justRemove = false)
    {
        if (currentPOI != null)
        {
            currentPOI.SetActive(false);
        }
        if (justRemove)
        {
            inUse = false;
            return;
        }
        Debug.Log("Setting current POI to: " + poi.name);
        currentPOI = poi;
        poi.SetActive(true);
        inUse = true;
    }


}
