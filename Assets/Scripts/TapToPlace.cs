using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class TapToPlace : MonoBehaviour
{
    public TMP_Text debugText;

    public ARRaycastManager raycastManager;

    public ARPlaneManager planeManager;

    private bool spawned = false;

    void Update()
    {
        if (!spawned)
        {
            GetSpawnLocation();
        }
    }


    void GetSpawnLocation()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            var hits = new List<ARRaycastHit>();
            raycastManager.Raycast(touch.position, hits, TrackableType.PlaneWithinPolygon);

            if (hits.Count > 0)
            {
                if (debugText != null)
                {
                    debugText.text = "Plane";
                }
            }
            else
            {
                if (debugText != null)
                {
                    debugText.text = "Other";
                }
            }
        }
    }
}
