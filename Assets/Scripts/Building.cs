using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum BuildingType
{
    MOUNTAIN,
    HEAL,
    SLOW,
    WATER,
    MONEY,
    EMPTY,
    CLOUD
}

public class Building : MonoBehaviour
{
    public float cost, water, waterConsumptionRate, maxWater, growTime, minViableSize, startSize;
    public float rate, amount, lastActivation;
    public Transform growObject;
    public Vector2Int pos;
    public Sprite icon;
    public Building defaultBuilding;
    public Building cloudBuilding;
    public Building waterBuilding;
    public Building mountainBuilding;
    public Building moneyBuilding;
    public BuildingType type;
    public Building targetBuilding;
    public Enemy targetEnemy;
    public float size
    {
        get
        {
            return growObject.localScale.x;
        }
        set
        {
            growObject.localScale = Vector3.one * value;
        }
    }

    public void Init(Vector2Int pos)
    {
        lastActivation = Time.time;
        size = startSize;
        this.pos = pos;
    }
    public void Init(Building old)
    {
        water += old.water;
        water = Mathf.Min(water, maxWater);
        Init(old.pos);
    }

    void Update()
    {
        switch (type)
        {
            case BuildingType.WATER:
                size = water / startSize;
                if (Activate())
                {
                    Water();
                    lastActivation = Time.time - Random.value * rate / 5;
                }
                break;
            case BuildingType.EMPTY:
                if (Activate())
                {
                    Empty();
                    lastActivation = Time.time - Random.value * rate / 5;
                }
                break;
            case BuildingType.CLOUD:
                if (Activate())
                {
                    Cloud();
                    lastActivation = Time.time - Random.value * rate / 5;
                }
                break;
            case BuildingType.MONEY:
                Grow();
                if (Activate())
                {
                    Money();
                    lastActivation = Time.time - Random.value * rate / 5;
                }
                break;
            case BuildingType.MOUNTAIN:
                if (Activate())
                {
                    Mountain();
                    lastActivation = Time.time - Random.value * rate / 5;
                }
                break;
        }
    }

    bool Activate()
    {
        return size >= minViableSize && lastActivation + rate < Time.time;
    }

    void Grow()
    {
        if (size < 1 && waterConsumptionRate * Time.deltaTime < water)
        {
            water -= waterConsumptionRate * Time.deltaTime;
            size += Time.deltaTime / growTime * (1 - startSize) * 3;
        }
        else if (waterConsumptionRate * Time.deltaTime < water)
        {
            water -= waterConsumptionRate * Time.deltaTime;
        }
        else if (waterConsumptionRate * Time.deltaTime > water && size > 0)
        {
            size -= Time.deltaTime / growTime * (1 - startSize) * 3;
        }
        if (size <= 0.01)
        {
            Game.Instance.ChangeBuilding(this, defaultBuilding);
        }
    }

    void Cloud()
    {
        if (Random.value < 0.5f)
        {
            Game.Instance.ChangeBuilding(this, defaultBuilding);
            return;
        }
        int clouds = 0;
        int mountains = 0;
        foreach (Building b in Game.Instance.GetNeighbors(this))
        {
            clouds += b.type == BuildingType.CLOUD ? 1 : 0;
            mountains += b.type == BuildingType.MOUNTAIN ? 1 : 0;
        }
        if (Random.value < 0.7f && clouds > 0)
        {
            Game.Instance.ChangeBuilding(this, waterBuilding);
        }
        if (mountains > 0)
        {
            water += 2;
        }
        water++;
    }
    void Water()
    {
        if (water >= 5)
        {
            foreach (Building b in Game.Instance.GetNeighbors(this))
            {
                if (b.type != BuildingType.EMPTY)
                {
                    if (b.water + amount < b.maxWater)
                    {
                        b.water += amount;
                        water -= amount;
                    }
                    else
                    {
                        water -= amount / 5;
                    }
                }
            }
            size = water / startSize;
        }
        else
        {
            Game.Instance.ChangeBuilding(this, defaultBuilding);
        }
        lastActivation = Time.time;
    }
    void Money()
    {
        Game.Score += amount * 100;
        int monies = 0;
        foreach (Building b in Game.Instance.GetNeighbors(this))
        {
            monies += b.type == BuildingType.MONEY ? 1 : 0;
        }
        if (monies > 1)
        {
            Game.Money += 10;
            Game.Instance.ChangeBuilding(this, mountainBuilding);
        }
    }

    void Mountain()
    {
        //Game.Money += amount;
    }

    void Empty()
    {
        int clouds = 0;
        int trees = 0;
        int waters = 0;
        int mountains = 0;
        foreach (Building b in Game.Instance.GetNeighbors(this))
        {
            if (b.type == BuildingType.CLOUD && Random.value < 0.15f)
            {
                Game.Instance.ChangeBuilding(this, cloudBuilding);
                return;
            }
            trees += b.type == BuildingType.MONEY ? 1 : 0;
            waters += b.type == BuildingType.WATER ? 1 : 0;
            mountains += b.type == BuildingType.MOUNTAIN ? 1 : 0;
        }
        if (trees > 0 && waters > 0 && Random.value < 0.6f)
        {
            Game.Instance.ChangeBuilding(this, moneyBuilding);
            return;
        }
        if (mountains > 0 && trees > 0 && Random.value < 0.9f)
        {
            Game.Instance.ChangeBuilding(this, cloudBuilding);
            return;
        }
    }

    void OnClick()
    {

    }

    void OnEnable()
    {
        if (type == BuildingType.MONEY)
        {
            Game.Health++;
        }
    }
    void OnDisable()
    {
        if (type == BuildingType.MONEY)
        {
            Game.Health--;
        }
    }

}