using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Flytext : MonoBehaviour
{
    public TextMeshPro text;
    public void Init(int amount, Color color)
    {
        Destroy(gameObject, 2);
        text.text = amount.ToString();
        text.color = color;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += Vector3.up * 0.005f;
    }
}