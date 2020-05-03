using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InGameUI : MonoBehaviour
{
    public TextMeshProUGUI scoreText, moneyText, healthText, dialogText;
    public Transform buttonHolder;
    public BasicButton button1, button2, button3, button4;
    public BasicButton BasicButton;
    public GameObject dialogBG;
    public DialogResult startDialog;

    public void UpdateScore(float val)
    {
        scoreText.text = (Game.Instance.night ? "Night " : "Day ") + Game.Instance.day + "\n" +
            Game.Instance.people + " People, " + Game.Instance.camels + " Camels\n" +
            ((Game.Instance.camels >= Game.Instance.people) ? "Speed 5, Everyone on Camels" : "Speed 3, Some on Foot") +
            "\nScore: " + val.ToString("#,#");
    }
    public void UpdateHealth(float val)
    {
        healthText.text = "Water: " + val.ToString("#,#");
    }
    public void UpdateMoney(float val)
    {
        moneyText.text = "Money: " + val.ToString("#,#");
    }
    public void EndGame(bool victory)
    {
        Game.InDialog = true;
        gameObject.SetActive(false);
    }

    public void Dialog(Dialog dialog)
    {
        Game.InDialog = true;
        dialogBG.SetActive(true);
        dialogText.text = dialog.text;
        SetButton(button1, dialog.option1Results, dialog.option1);
        SetButton(button2, dialog.option2Results, dialog.option2);
        SetButton(button3, dialog.option3Results, dialog.option3);
        SetButton(button4, dialog.option4Results, dialog.option4);
    }

    public void InfoDialog(DialogResult dr)
    {
        Game.InDialog = true;
        dialogBG.SetActive(true);
        button1.gameObject.SetActive(true);
        button2.gameObject.SetActive(false);
        button3.gameObject.SetActive(false);
        button4.gameObject.SetActive(false);
        if (dr.text == "distance")
        {
            dialogText.text = Game.Instance.GetDistance();
        }
        else if (dr.text == "direction")
        {
            dialogText.text = Game.Instance.GetDirection();
        }
        else
        {
            dialogText.text = dr.text;
        }
        button1.Init(dr.button, dr, true);
    }

    public void Close()
    {
        dialogBG.SetActive(false);
        Game.InDialog = false;
        button1.gameObject.SetActive(false);
        button2.gameObject.SetActive(false);
        button3.gameObject.SetActive(false);
        button4.gameObject.SetActive(false);
    }

    public void SetButton(BasicButton bb, List<DialogResult> drs, string str)
    {
        if (drs == null || drs.Count == 0)
        {
            bb.gameObject.SetActive(false);
            return;
        }
        bb.gameObject.SetActive(true);
        DialogResult dr = drs[Mathf.FloorToInt(drs.Count * Random.value)];
        bb.Init(str, dr);
    }
}