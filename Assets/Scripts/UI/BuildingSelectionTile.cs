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
        if(Input.GetKeyDown(hotKey.ToString()))
        {
            OnClick();
        }
    }
    public void Start()
    {
        cost.text = "$" + newBuilding.cost.ToString();
        hotKeyTexts.text = hotKey.ToString();
        if(newBuilding.type == BuildingType.MONEY)
        {
            OnClick();
        }
    }

    public void OnClick()
    {
        Game.Instance.clickBuilding = newBuilding;
        foreach (BuildingSelectionTile bst in otherButtons)
        {
            bst.Deselect();
        }
        backing.color = selectColor;
        outline.enabled = true;
    }

    public void Deselect()
    {
        outline.enabled = false;
        backing.color = Color.white;
    }
}