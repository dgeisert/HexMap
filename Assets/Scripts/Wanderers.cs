using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wanderers : MonoBehaviour
{
    List<GameObject> Camels, People;
    public GameObject CamelPrefab, PersonPrefab;

    public void Init()
    {
        Camels = new List<GameObject>();
        People = new List<GameObject>();
    }

    public void SetMenu(int p, int c)
    {
        Init();
        for (int j = 0; j < p; j++)
        {
            Transform t = Instantiate(PersonPrefab, Vector3.zero, Quaternion.identity, transform).transform;
            t.localPosition = Vector3.zero;
            t.localEulerAngles = new Vector3(0, 0, (Random.value - 0.5f) * 360);
            t.localScale = Vector3.one;
            People.Add(t.gameObject);
        }
        for (int j = 0; j <= c; j++)
        {
            Transform t = Instantiate(CamelPrefab, Vector3.zero, Quaternion.identity, transform).transform;
            t.localPosition = Vector3.zero;
            t.localEulerAngles = new Vector3(0, 0, (Random.value - 0.5f) * 360);
            t.localScale = Vector3.one;
            Camels.Add(t.gameObject);
        }
    }

    public void SetPeople(int i)
    {
        if (i < People.Count)
        {
            for (int j = People.Count - 1; j >= i; j--)
            {
                Destroy(People[j]);
                People.Remove(People[j]);
            }
        }
        else if (i > People.Count)
        {
            for (int j = 0; j < i - People.Count; j++)
            {
                Transform t = Instantiate(PersonPrefab, Vector3.zero, Quaternion.identity, transform).transform;
                t.localPosition = Vector3.zero;
                t.localEulerAngles = new Vector3(0, 0, (Random.value - 0.5f) * 360);
                t.localScale = Vector3.one;
                People.Add(t.gameObject);
            }
        }
    }

    public void SetCamels(int i)
    {
        if (i < Camels.Count)
        {
            for (int j = Camels.Count - 1; j >= i; j--)
            {
                Destroy(Camels[j]);
                Camels.Remove(Camels[j]);
            }
        }
        else if (i > Camels.Count)
        {
            for (int j = 0; j <= i - Camels.Count; j++)
            {
                Transform t = Instantiate(CamelPrefab, Vector3.zero, Quaternion.identity, transform).transform;
                t.localPosition = Vector3.zero;
                t.localEulerAngles = new Vector3(0, 0, (Random.value - 0.5f) * 360);
                t.localScale = Vector3.one;
                Camels.Add(t.gameObject);
            }
        }
    }

    void Update()
    {
        foreach (GameObject go in People)
        {
            Wander(go);
        }
        foreach (GameObject go in Camels)
        {
            Wander(go);
        }
    }

    public float speed = 0.01f, turnspeed = 0.01f;

    void Wander(GameObject go)
    {
        if (Vector3.Distance(go.transform.position, transform.position) > 0.25f &&
            Vector3.Distance(go.transform.position - 0.25f * go.transform.up, transform.position) > 0.1f)
        {
            go.transform.localEulerAngles += new Vector3(0, 0, turnspeed);
        }
        else
        {
            go.transform.position -= go.transform.up * speed;
            go.transform.localEulerAngles += new Vector3(0, 0, (Random.value - 0.5f) * turnspeed * 8);
        }
    }
}