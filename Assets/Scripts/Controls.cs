using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controls : MonoBehaviour
{
    public static Controls Instance;
    public static bool Next
    {
        get
        {
            return Input.GetKeyDown(KeyCode.Space) ||
                Input.GetKeyDown(KeyCode.W) ||
                Input.GetKeyDown(KeyCode.UpArrow);
        }
    }
    public static bool Pause
    {
        get
        {
            return Input.GetKeyDown(KeyCode.P);
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 5000.0f))
            {
                Hex h = hit.transform.GetComponentInParent<Hex>();
                if (h != null)
                {
                    if (Game.Instance)
                    {
                        Game.Instance.Select(h);
                    }
                }
            }
        }
        else
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 5000.0f))
            {
                Hex h = hit.transform.GetComponentInParent<Hex>();
                if (h != null)
                {
                    if (Game.Instance)
                    {
                        Game.Instance.Hover(h);
                    }
                }
            }
        }
    }
}