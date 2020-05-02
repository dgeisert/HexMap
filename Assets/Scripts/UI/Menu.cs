using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    public GameObject settings;
    public GameObject controls;
    public GameObject menu;
    public UnityEngine.Audio.AudioMixer mixer;
    public Image NoMusic;
    public Image NoSound;
    void Start()
    {
        OpenMenu();
    }
    public void PlayGame()
    {
        SceneChanger.LoadScene(Scenes.Game);
    }
    public void OpenSettings()
    {
        menu.SetActive(false);
        controls.SetActive(false);
        settings.SetActive(true);
    }
    public void OpenControls()
    {
        menu.SetActive(false);
        controls.SetActive(true);
        settings.SetActive(false);
    }
    public void OpenMenu()
    {
        menu.SetActive(true);
        controls.SetActive(false);
        settings.SetActive(false);
    }
    public void MuteMusic()
    {
        float volume;
        mixer.GetFloat("MusicVolume", out volume);
        if (volume < -70)
        {
            NoMusic.gameObject.SetActive(false);
            mixer.SetFloat("MusicVolume", 0);
        }
        else
        {
            NoMusic.gameObject.SetActive(true);
            mixer.SetFloat("MusicVolume", -80);
        }
    }
    public void MuteSound()
    {
        float volume;
        mixer.GetFloat("MasterVolume", out volume);
        if (volume < -70)
        {
            NoSound.gameObject.SetActive(false);
            mixer.SetFloat("MasterVolume", 0);
        }
        else
        {
            NoSound.gameObject.SetActive(true);
            mixer.SetFloat("MasterVolume", -80);
        }
    }
}