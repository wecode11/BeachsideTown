using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    public Slider cameraSlider;
    public Camera[] cameras;
    public Material daySkybox;
    public Material nightSkybox;
    public Material[] lightMaterial;
    public GameObject fire;

    void Start()
    {
        SetDay();
    }

    void Play()
    {
    }

    void SetDay()
    {
        RenderSettings.skybox = daySkybox;

        GameObject sun = GameObject.FindGameObjectWithTag("Sun");
        sun.GetComponent<Light>().enabled = true;

        fire.SetActive(false);

        GameObject[] lights = GameObject.FindGameObjectsWithTag("Light");

        for (int lightIdx = 0; lightIdx < lights.Length; lightIdx++)
        {
            lights[lightIdx].GetComponent<Light>().enabled = false;
        }

        for (int matIdx = 0; matIdx < lightMaterial.Length; matIdx++)
        {
            lightMaterial[matIdx].DisableKeyword("_EMISSION");
        }

        DynamicGI.UpdateEnvironment();
    }

    void SetNight()
    {
        RenderSettings.skybox = nightSkybox;

        GameObject sun = GameObject.FindGameObjectWithTag("Sun");
        sun.GetComponent<Light>().enabled = false;

        fire.SetActive(true);

        GameObject[] lights = GameObject.FindGameObjectsWithTag("Light");

        for (int lightIdx = 0; lightIdx < lights.Length; lightIdx++)
        {
            lights[lightIdx].GetComponent<Light>().enabled = true;
        }

        for (int matIdx = 0; matIdx < lightMaterial.Length; matIdx++)
        {
            lightMaterial[matIdx].EnableKeyword("_EMISSION");
        }

        DynamicGI.UpdateEnvironment();
    }

    void SelectCamera()
    {
        for (int camIdx = 0; camIdx < cameras.Length; camIdx++)
        {
            cameras[camIdx].enabled = false;
        }

        int camVal = (int)cameraSlider.value;

        if (camVal < cameras.Length)
        {
            cameras[camVal].enabled = true;
        }
    }
}
