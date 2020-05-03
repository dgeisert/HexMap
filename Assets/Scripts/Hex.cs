using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hex : MonoBehaviour
{
    public Vector2 loc;
    public MeshRenderer meshRenderer;
    public List<Reward> rewards = new List<Reward>();
    public float dialogChance = 0.2f;
    public List<Dialog> dialog;
    public GameObject Ring;

    bool FirstFocus = true;
    public Vector2Int GetIndex()
    {
        return new Vector2Int(Mathf.FloorToInt(loc.x), Mathf.FloorToInt(loc.y));
    }

    public void Init(Vector2 loc)
    {
        this.loc = loc;
    }

    public void Focus()
    {
        if (FirstFocus)
        {
            FirstFocus = false;
            Game.Instance.GrantRewards(rewards);
        }
        if (Random.value < dialogChance && dialog != null && dialog.Count > 0)
        {
            Game.Instance.inGameUI.Dialog(dialog[Mathf.FloorToInt(Random.value * dialog.Count)]);
        }
    }

    public void Hover()
    {
        Ring.SetActive(true);
    }
    public void Unhover()
    {
        Ring.SetActive(false);
    }
}