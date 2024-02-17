using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    public TMP_Text timeOfDayLabel;
    public Slider cameraSlider;
    public Camera[] cameras;
    public Material daySkybox;
    public Material nightSkybox;
    public Material[] lightMaterial;
    public GameObject[] daySounds;
    public GameObject[] nightSounds;
    public GameObject backgroundMusic;
    public GameObject musicSlider;
    public GameObject[] lightObjects;
    public GameObject[] characters;

    void Start()
    {
        float volume = backgroundMusic.GetComponent<AudioSource>().volume;
        musicSlider.GetComponent<Slider>().value = volume;

        SetDay();
    }

    public void SetDay()
    {
        timeOfDayLabel.text = "Day";

        RenderSettings.skybox = daySkybox;

        SetLights(false);

        GameObject fire = GameObject.FindGameObjectWithTag("FireVFX");
        fire.GetComponent<ParticleSystem>().Stop();
        fire.GetComponent<ParticleSystem>().Clear();

        SetSounds(nightSounds, false);
        SetSounds(daySounds, true);
    }

    public void SetNight()
    {
        timeOfDayLabel.text = "Night";

        RenderSettings.skybox = nightSkybox;

        SetLights(true);

        GameObject fire = GameObject.FindGameObjectWithTag("FireVFX");
        fire.GetComponent<ParticleSystem>().Play();

        SetSounds(daySounds, false);
        SetSounds(nightSounds, true);
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

    private void SetLights(bool enabled)
    {
        GameObject sun = GameObject.FindGameObjectWithTag("Sun");
        sun.GetComponent<Light>().enabled = !enabled;

        GameObject[] lights = GameObject.FindGameObjectsWithTag("Light");

        for (int lightIdx = 0; lightIdx < lights.Length; lightIdx++)
        {
            lights[lightIdx].GetComponent<Light>().enabled = enabled;
        }

        for (int matIdx = 0; matIdx < lightMaterial.Length; matIdx++)
        {
            if (enabled)
            {
                lightMaterial[matIdx].EnableKeyword("_EMISSION");
            }
            else
            {
                lightMaterial[matIdx].DisableKeyword("_EMISSION");
            }
        }

        for (int litIdx = 0; litIdx < lightObjects.Length; litIdx++)
        {
            RendererExtensions.UpdateGIMaterials(lightObjects[litIdx].GetComponent<Renderer>());
        }

        for (int chrIdx = 0; chrIdx < characters.Length; chrIdx++)
        {
            characters[chrIdx].GetComponent<Animator>().SetBool("isDay", !enabled);
        }

        DynamicGI.UpdateEnvironment();
    }
}
