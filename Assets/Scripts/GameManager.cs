using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public Slider cameraSlider;
    public Camera[] cameras;
    public Material daySkybox;
    public Material nightSkybox;
    public Material[] lightMaterial;
    public GameObject[] daySounds;
    public GameObject[] nightSounds;
    public GameObject backgroundMusic;
    public GameObject musicSlider;

    void Start()
    {
        SetDay();
        float volume = backgroundMusic.GetComponent<AudioSource>().volume;
        musicSlider.GetComponent<Slider>().value = volume;
    }

    public void Play()
    {
    }

    private void SetSounds(GameObject[] sounds, bool enable)
    {
        for (int sndIdx = 0; sndIdx < sounds.Length; sndIdx++)
        {
            if (enable)
            {
                sounds[sndIdx].GetComponent<AudioSource>().Play();
            }
            else
            {
                sounds[sndIdx].GetComponent<AudioSource>().Stop();
            }
        }
    }

    public void SetDay()
    {
        RenderSettings.skybox = daySkybox;

        GameObject sun = GameObject.FindGameObjectWithTag("Sun");
        sun.GetComponent<Light>().enabled = true;

        SetSounds(nightSounds, false);
        SetSounds(daySounds, true);

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

    public void SetNight()
    {
        RenderSettings.skybox = nightSkybox;

        GameObject sun = GameObject.FindGameObjectWithTag("Sun");
        sun.GetComponent<Light>().enabled = false;

        SetSounds(daySounds, false);
        SetSounds(nightSounds, true);

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

    public void SelectCamera()
    {
        for (int camIdx = 0; camIdx < cameras.Length; camIdx++)
        {
            cameras[camIdx].GetComponent<AudioListener>().enabled = false;
            cameras[camIdx].enabled = false;
        }

        int camVal = (int)cameraSlider.value;

        if (camVal < cameras.Length)
        {
            cameras[camVal].enabled = true;
            cameras[camVal].GetComponent<AudioListener>().enabled = true;
        }
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        SetDay();
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
