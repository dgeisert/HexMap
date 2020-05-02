using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreScreen : MonoBehaviour
{
    public TextMeshProUGUI scoreText, victoryText;
    public GameObject victoryDisplay;
    public GameObject defeatDisplay;
    public void EndGame(bool victory = false)
    {
        gameObject.SetActive(true);
        victoryDisplay.gameObject.SetActive(victory);
        if (Game.Score == 0)
        {
            victoryText.text = "You need to water your trees!";
        }
        else
        {
            victoryText.text = "Game Over";
        }
        defeatDisplay.gameObject.SetActive(!victory);
        scoreText.text = Game.Score.ToString("#,#");
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