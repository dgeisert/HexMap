using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hex : MonoBehaviour
{
    public Vector2 loc;
    public MeshRenderer meshRenderer;
    public Vector2Int GetIndex()
    {
        return new Vector2Int(Mathf.FloorToInt(loc.x), Mathf.FloorToInt(loc.y));
    }

    public void Init(Vector2 loc)
    {
        this.loc = loc;
    }
}