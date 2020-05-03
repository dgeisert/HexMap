using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BasicButton : MonoBehaviour
{
    public Button button;
    public TextMeshProUGUI text;

    public void Init(string str, DialogResult dr, bool isInfo = false)
    {
        text.text = str;
        button.onClick.RemoveAllListeners();
        if (isInfo)
        {
            button.onClick.AddListener(() =>
            {
                Game.Instance.GrantRewards(dr.rewards);
                Game.Instance.inGameUI.Close();
            });
        }
        else
        {
            button.onClick.AddListener(() =>
            {
                Game.Instance.inGameUI.InfoDialog(dr);
            });
        }
    }
}