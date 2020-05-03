using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuildingSelectionTile : MonoBehaviour
{
    public Building newBuilding;
    public int hotKey;
    public Image outline, backing;
    public TextMeshProUGUI cost, hotKeyTexts;
    public Color selectColor;
    public List<BuildingSelectionTile> otherButtons;

    void Update()
    {
        if (Input.GetKeyDown(hotKey.ToString()))
        {
            OnClick();
        }
    }
    public void Start()
    {
        hotKeyTexts.text = hotKey.ToString();
    }

    public void OnClick()
    { }

    public void Deselect()
    { }
}