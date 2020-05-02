using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public void OnEnable()
    {
        Time.timeScale = 0;
    }
    public void OnDisable()
    {
        Time.timeScale = 1;
    }
    public void Resume()
    {
        Game.Instance.Pause();
    }
    public void Restart()
    {
        SceneChanger.LoadScene(Scenes.Game);
    }
    public void Menu()
    {
        SceneChanger.LoadScene(Scenes.MainMenu);
    }
}