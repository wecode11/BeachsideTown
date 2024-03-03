using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    public GameObject backgroundMusic;
    public GameObject musicSlider;
    public Slider cameraSlider;
    public Camera[] cameras;
    public Material daySkybox;
    public Material nightSkybox;
    public Material[] lightMaterial;

    void Start()
    {
        SetDay();
        SetMusic();
    }

    public void SetDay()
    {
        RenderSettings.skybox = daySkybox;

        GameObject timeOfDay = GameObject.FindGameObjectWithTag("TimeOfDay");
        if (timeOfDay != null)
        {
            timeOfDay.GetComponent<TMP_Text>().text = "Day";
        }

        SetLights(false);

        GameObject[] vfx = GameObject.FindGameObjectsWithTag("FireVFX");
        for (int vfxIdx = 0; vfxIdx < vfx.Length; vfxIdx++)
        {
            vfx[vfxIdx].GetComponent<ParticleSystem>().Stop();
            vfx[vfxIdx].GetComponent<ParticleSystem>().Clear();
        }

        SetSounds("SoundNight", false);
        SetSounds("SoundDay", true);
    }

    public void SetNight()
    {
        RenderSettings.skybox = nightSkybox;

        GameObject timeOfDay = GameObject.FindGameObjectWithTag("TimeOfDay");
        if (timeOfDay != null)
        {
            timeOfDay.GetComponent<TMP_Text>().text = "Night";
        }

        SetLights(true);

        GameObject[] vfx = GameObject.FindGameObjectsWithTag("FireVFX");
        for (int vfxIdx = 0; vfxIdx < vfx.Length; vfxIdx++)
        {
            vfx[vfxIdx].GetComponent<ParticleSystem>().Play();
        }

        SetSounds("SoundDay", false);
        SetSounds("SoundNight", true);
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

    private void SetMusic()
    {
        if ((backgroundMusic != null) && (musicSlider != null))
        {
            float volume = backgroundMusic.GetComponent<AudioSource>().volume;
            musicSlider.GetComponent<Slider>().value = volume;
        }
    }

    private void SetSounds(string tag, bool enable)
    {
        GameObject[] sounds = GameObject.FindGameObjectsWithTag(tag);
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

        if (enabled)
        {
            sun.GetComponent<Light>().intensity = 0.1f;
        }
        else
        {
            sun.GetComponent<Light>().intensity = 2.8f;
        }

        GameObject[] characters = GameObject.FindGameObjectsWithTag("Character");
        for (int chrIdx = 0; chrIdx < characters.Length; chrIdx++)
        {
            characters[chrIdx].GetComponent<Animator>().SetBool("isDay", !enabled);
        }

        GameObject[] lights = GameObject.FindGameObjectsWithTag("Light");
        for (int lightIdx = 0; lightIdx < lights.Length; lightIdx++)
        {
            lights[lightIdx].GetComponent<Light>().enabled = enabled;
        }

        for (int matIdx = 0; matIdx < lightMaterial.Length; matIdx++)
        {
            Material material = lightMaterial[matIdx];

            if (enabled)
            {
                material.EnableKeyword("_EMISSION");
            }
            else
            {
                material.DisableKeyword("_EMISSION");
            }

            material.globalIlluminationFlags = MaterialGlobalIlluminationFlags.RealtimeEmissive;
        }

        GameObject[] lightObjects = GameObject.FindGameObjectsWithTag("UpdateMaterial");
        for (int litIdx = 0; litIdx < lightObjects.Length; litIdx++)
        {
            RendererExtensions.UpdateGIMaterials(lightObjects[litIdx].GetComponent<Renderer>());
        }

        DynamicGI.UpdateEnvironment();
    }
}
