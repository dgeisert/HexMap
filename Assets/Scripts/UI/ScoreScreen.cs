using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreScreen : MonoBehaviour
{
    public TextMeshProUGUI scoreText, victoryText;
    public GameObject victoryDisplay;
    public GameObject defeatDisplay;
    public void EndGame(bool victory = false, string text = "")
    {
        Game.Instance.inGameUI.Close();
        Game.InDialog = true;
        gameObject.SetActive(true);
        victoryDisplay.gameObject.SetActive(victory);
        victoryText.text = text;
        defeatDisplay.gameObject.SetActive(!victory);
        Game.Score += victory ? 20000 : -20000;
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