using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using TMPro;

public class TapToPlace : MonoBehaviour
{
    public GameObject spawnObject;
    public TMP_Text debugText;
    public ARRaycastManager raycastManager;
    public ARPlaneManager planeManager;
    public GameObject[] activate;
    private bool spawned = false;
    private Pose location;

    void Update()
    {
        if (!spawned && HaveSpawnLocation())
        {
            PutObject(location.position, location.rotation);
            DisablePlaneDetection();
            ActivateObjects();
            DisplayDebugText("Placed");
        }
    }

    private void ActivateObjects()
    {
        for (int objIdx = 0; objIdx < activate.Length; objIdx++)
        {
            activate[objIdx].SetActive(true);
        }
    }

    private bool HaveSpawnLocation()
    {
        if (Input.touchCount == 0)
        {
            return false;
        }

        Touch touch = Input.GetTouch(0);

        if (touch.phase == TouchPhase.Began)
        {
            DisplayDebugText("Not began");
            return false;
        }

        var hits = new List<ARRaycastHit>();
        raycastManager.Raycast(touch.position, hits, TrackableType.PlaneWithinPolygon);

        if (hits.Count == 0)
        {
            DisplayDebugText("No plane found");
            return false;
        }

        location = hits[0].pose;
        return true;
    }

    private void PutObject(Vector3 position, Quaternion rotation)
    {
        Instantiate(spawnObject, position, rotation);
    }

    private void DisablePlaneDetection()
    {
        spawned = true;
        planeManager.SetTrackablesActive(false);
        planeManager.enabled = false;
    }

    private void DisplayDebugText(string s)
    {
        if (debugText != null)
        {
            debugText.text = s;
        }
    }
}
