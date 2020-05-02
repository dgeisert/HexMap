using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
    public static Game Instance;
    public PauseMenu pauseMenu;
    public ScoreScreen scoreScreen;
    public InGameUI inGameUI;
    public bool active = true;
    public Transform buildingPositions;
    public Building[, ] buildings;
    public Vector2Int currentSelection;
    public Building clickBuilding;
    public static bool started = false;
    public Hex Hex;
    public Material Red, Green;
    public Dictionary<Vector2Int, Hex> Hexes;
    public Hex hoverHex;
    float HexVerticalOffset = 0.75f;
    float HexHorizontalOffset = 0.866f;

    public static float Score
    {
        get
        {
            if (Instance)
            {
                return Instance.score;
            }
            return -1f;
        }
        set
        {
            if (Instance)
            {
                Instance.score = value;
                Instance.inGameUI.UpdateScore(value);
            }
        }
    }
    public static float Health
    {
        get
        {
            if (Instance)
            {
                return Instance.health;
            }
            return -1f;
        }
        set
        {
            if (Instance)
            {
                Instance.health = value;
                Instance.inGameUI.UpdateHealth(value);
                if (value > 0)
                {
                    started = true;
                }
                if (value == 0 & started)
                {
                    started = false;
                    Instance.inGameUI.EndGame(true);
                    Instance.scoreScreen.EndGame(true);
                    Instance.pauseMenu.gameObject.SetActive(false);
                }
            }
        }
    }
    public static float Money
    {
        get
        {
            if (Instance)
            {
                return Instance.money;
            }
            return -1f;
        }
        set
        {
            if (Instance)
            {
                Instance.money = value;
                Instance.inGameUI.UpdateMoney(value);
            }
        }
    }
    public float score, money, health;
    int step = 0;
    int grid = 20;
    float buildingSpacing = 5;
    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
        pauseMenu.gameObject.SetActive(false);
        scoreScreen.gameObject.SetActive(false);
        inGameUI.gameObject.SetActive(true);
        Hexes = new Dictionary<Vector2Int, Hex>();
        for (int i = 0; i < 10; i++)
        {
            for (int j = 0; j < 10; j++)
            {
                Vector3 pos = new Vector3((i + (j % 2 == 0 ? 0.5f : 0)) * HexHorizontalOffset, 0, j * HexVerticalOffset);
                Vector2 loc = new Vector2((i + (j % 2 == 0 ? 0.5f : 0)), j);
                Vector2Int index = new Vector2Int(i, j);
                Hexes.Add(index, Instantiate(Hex, pos, Quaternion.identity));
                Hexes[index].Init(loc);
            }
        }
    }

    public List<Building> GetNeighbors(Building b)
    {
        List<Building> neighbors = new List<Building>();
        for (int i = -1; i < 2; i++)
        {
            for (int j = -1; j < 2; j++)
            {
                if (b.pos.x + i >= 0 &&
                    b.pos.x + i < buildings.GetLength(0) &&
                    b.pos.y + j >= 0 &&
                    b.pos.y + j < buildings.GetLength(1) &&
                    !(i == 0 && j == 0))
                {
                    if (buildings[b.pos.x + i, b.pos.y + j] != null)
                    {
                        neighbors.Add(buildings[b.pos.x + i, b.pos.y + j]);
                    }
                }
            }
        }
        return neighbors;
    }

    public Vector2Int[] NeighborIndexes(Hex h)
    {
        return new Vector2Int[]
        {
            new Vector2Int(Mathf.FloorToInt(h.loc.x - 1), Mathf.FloorToInt(h.loc.y)),
                new Vector2Int(Mathf.FloorToInt(h.loc.x + 1), Mathf.FloorToInt(h.loc.y)),
                new Vector2Int(Mathf.FloorToInt(h.loc.x - 0.5f), Mathf.FloorToInt(h.loc.y + 1)),
                new Vector2Int(Mathf.FloorToInt(h.loc.x + 0.5f), Mathf.FloorToInt(h.loc.y + 1)),
                new Vector2Int(Mathf.FloorToInt(h.loc.x - 0.5f), Mathf.FloorToInt(h.loc.y - 1)),
                new Vector2Int(Mathf.FloorToInt(h.loc.x + 0.5f), Mathf.FloorToInt(h.loc.y - 1))
        };
    }
    public List<Hex> GetNeighbors(Hex h)
    {
        List<Hex> neighbors = new List<Hex>();

        foreach (Vector2Int loc in NeighborIndexes(h))
        {
            if (Hexes.ContainsKey(loc))
            {
                neighbors.Add(Hexes[new Vector2Int(loc.x, loc.y)]);
            }
        }
        return neighbors;
    }
    public void AddNeighbors(Hex h)
    {
        foreach (Vector2Int index in NeighborIndexes(h))
        {
            if (!Hexes.ContainsKey(index))
            {
                int i = index.x;
                int j = index.y;
                Vector3 pos = new Vector3((i + (j % 2 == 0 ? 0.5f : 0)) * HexHorizontalOffset, 0, j * HexVerticalOffset);
                Vector2 loc = new Vector2((i + (j % 2 == 0 ? 0.5f : 0)), j);
                Hexes.Add(index, Instantiate(Hex, pos, Quaternion.identity));
                Hexes[index].Init(loc);
            }
        }
    }

    public void Select(Hex h)
    {
        h.meshRenderer.material = Red;
        foreach (Hex h2 in GetNeighbors(h))
        {
            h2.meshRenderer.material = Green;
        }
        AddNeighbors(h);
    }

    public void Hover(Hex h)
    {
        if (hoverHex == h)
        {
            return;
        }
        if (hoverHex != null)
        {
            hoverHex.meshRenderer.material = Red;
        }
        h.meshRenderer.material = Green;
        hoverHex = h;
    }

    public void Select(Building building)
    {
        if (Money >= clickBuilding.cost)
        {
            currentSelection = building.pos;
            ChangeBuilding(building, clickBuilding);
            Money = money - clickBuilding.cost;
        }
    }

    public void ChangeBuilding(Building oldBuilding, Building newBuilding)
    {
        Building newInstance = Instantiate(newBuilding, oldBuilding.transform.position, Quaternion.identity, buildingPositions);
        buildings[oldBuilding.pos.x, oldBuilding.pos.y] = newInstance;
        newInstance.Init(oldBuilding);
        Destroy(oldBuilding.gameObject);
    }

    public void ChangeBuilding(Building newBuilding)
    {
        if (currentSelection != null)
        {
            ChangeBuilding(buildings[currentSelection.x, currentSelection.y], newBuilding);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (active)
        {
            if (Controls.Pause)
            {
                Pause();
            }
        }
    }

    public void Pause()
    {
        pauseMenu.gameObject.SetActive(!pauseMenu.gameObject.activeSelf);
    }
}