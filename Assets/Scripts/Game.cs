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
    public static bool started = false;
    public Material Red, Green;
    public List<Hex> HexPrefabs;
    public Hex WaterHex;
    public Camera mainCamera;
    public Vector3 cameraOffset = new Vector3(0, 20, -20);
    public int turn;
    public int day;
    public bool night;
    public int speed;
    public float luck = 0;
    public Light light;
    public Color dayColor, nightColor, targetLightColor;
    public Quaternion sunrise, sunset, targetLightRotation;
    public Wanderers wanderers;
    public Flytext flytext;
    public Vector2Int EndingPosition;

    float HexVerticalOffset = 0.75f;
    float HexHorizontalOffset = 0.866f;
    float lightMoveStart;
    Hex hoverHex;
    Hex selectedHex;
    Dictionary<Vector2Int, Hex> Hexes;

    public static bool InDialog = false;
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
                Instance.UpdateScore();
                Instance.inGameUI.UpdateHealth(value);
                if (value <= 0)
                {
                    Instance.inGameUI.EndGame(false);
                    Instance.scoreScreen.EndGame(false, "As you run out of water you realize, this is the end.  You make your peace and pass away quietly.");
                    Instance.pauseMenu.gameObject.SetActive(false);
                }
            }
        }
    }
    public static int People
    {
        get
        {
            if (Instance)
            {
                return Instance.people;
            }
            return -1;
        }
        set
        {
            if (Instance)
            {
                Instance.people = value;
                Instance.UpdateScore();
                Instance.wanderers.SetPeople(value);
                Instance.speed = value < Camels ? 4 : 2;
                if (value <= 0)
                {
                    Instance.inGameUI.EndGame(false);
                    Instance.scoreScreen.EndGame(false, "Your last person dies and the family line has ended.");
                    Instance.pauseMenu.gameObject.SetActive(false);
                }
            }
        }
    }
    public static int Camels
    {
        get
        {
            if (Instance)
            {
                return Instance.camels;
            }
            return -1;
        }
        set
        {
            if (Instance)
            {
                Instance.camels = Mathf.Max(value, 0);
                Instance.UpdateScore();
                Instance.wanderers.SetCamels(Instance.camels);
                Instance.speed = Instance.camels >= People ? 4 : 2;
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
                if (value < 0)
                {
                    Instance.inGameUI.EndGame(false);
                    Instance.scoreScreen.EndGame(false, "The merchant isn't pleased when you can't afford the merchandise.  His guards throw you out with nothing and you soon die.");
                    Instance.pauseMenu.gameObject.SetActive(false);
                }
            }
        }
    }

    public float score, money, health;
    public int people, camels;

    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
        pauseMenu.gameObject.SetActive(false);
        scoreScreen.gameObject.SetActive(false);
        inGameUI.gameObject.SetActive(true);
        Hexes = new Dictionary<Vector2Int, Hex>();

        Vector3 pos = new Vector3(((0 % 2 == 0 ? 0.5f : 0)) * HexHorizontalOffset, 0, 0 * HexVerticalOffset);
        Vector2 loc = new Vector2((0 + (0 % 2 == 0 ? 0.5f : 0)), 0);
        Vector2Int index = new Vector2Int(0, 0);
        Hexes.Add(index, Instantiate(GetHex(0), pos, Quaternion.identity));
        Hexes[index].Init(loc);
        AddNeighbors(Hexes[index]);
        selectedHex = Hexes[index];
        wanderers.Init();

        Vector2 endDir = new Vector2(Random.value - 0.5f, Random.value - 0.5f).normalized * 30;
        EndingPosition = new Vector2Int(Mathf.FloorToInt(endDir.x), Mathf.FloorToInt(endDir.y));

        inGameUI.InfoDialog(inGameUI.startDialog);

        People = 2;
        Camels = 3;
        Health = 15;
        Money = 10;
    }

    public void UpdateScore()
    {
        float newScore = 30000;
        newScore -= 2000 * day;
        newScore -= night ? 1000 : 0;
        newScore += money * 1000;
        newScore += camels * 2000;
        newScore += people * 3000;
        newScore += health * 500;
        Score = newScore;
    }

    public Hex GetHex(int i = -1)
    {
        if (i == -1)
        {
            float rand = (Random.value * HexPrefabs.Count); //(((Random.value + 0.5f) * (Random.value + 0.5f)) - 0.25f) / 2 * HexPrefabs.Count + luck;
            i = Mathf.FloorToInt(Mathf.Clamp(rand, 0, HexPrefabs.Count - 1));
        }
        return HexPrefabs[i];
    }

    public void GrantRewards(List<Reward> rewards)
    {
        foreach (Reward r in rewards)
        {
            Color c = Color.white;
            float offset = 0;
            switch (r.type)
            {
                case "water":
                    Health += r.amount;
                    c = Color.cyan;
                    offset = -0.2f;
                    break;
                case "camel":
                    Camels += (int) r.amount;
                    c = Color.yellow;
                    offset = -0.4f;
                    break;
                case "person":
                    People += (int) r.amount;
                    break;
                case "money":
                    Money += r.amount;
                    c = Color.green;
                    offset = 0.2f;
                    break;
            }
            Instantiate(flytext, wanderers.transform.position + Vector3.up * 0.1f + Vector3.right * offset, Quaternion.identity).Init((int) r.amount, c);
        }
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
                if (EndingPosition == index)
                {
                    int i = index.x;
                    int j = index.y;
                    Vector3 pos = new Vector3((i + (j % 2 == 0 ? 0.5f : 0)) * HexHorizontalOffset, 0, j * HexVerticalOffset);
                    Vector2 loc = new Vector2((i + (j % 2 == 0 ? 0.5f : 0)), j);
                    Hexes.Add(index, Instantiate(WaterHex, pos, Quaternion.identity));
                    Hexes[index].Init(loc);
                    AddNeighbors(Hexes[index]);
                    Instance.inGameUI.EndGame(true);
                    Instance.scoreScreen.EndGame(true, "You made it to the Grand Oasis. Now you can settle in and make a new home.");
                    Instance.pauseMenu.gameObject.SetActive(false);
                }
                else
                {
                    int i = index.x;
                    int j = index.y;
                    Vector3 pos = new Vector3((i + (j % 2 == 0 ? 0.5f : 0)) * HexHorizontalOffset, 0, j * HexVerticalOffset);
                    Vector2 loc = new Vector2((i + (j % 2 == 0 ? 0.5f : 0)), j);
                    Hexes.Add(index, Instantiate(GetHex(), pos, Quaternion.identity));
                    Hexes[index].Init(loc);
                }
            }
        }
    }

    public bool CheckNeighbors(Hex h1, Hex h2)
    {
        foreach (Vector2Int index in NeighborIndexes(h1))
        {
            if (Hexes[index] == h2)
            {
                return true;
            }
        }
        return false;
    }

    public void Select(Hex h)
    {
        if (InDialog || !CheckNeighbors(selectedHex, h))
        {
            return;
        }
        selectedHex = h;
        h.Focus();
        Health -= ((float)Camels / 2 + (float)People) / (float)speed * 2;
        NextTurn();
        AddNeighbors(h);
    }

    public void Hover(Hex h)
    {
        if (InDialog || hoverHex == h)
        {
            return;
        }
        if (hoverHex != null)
        {
            hoverHex.Unhover();
        }
        if (CheckNeighbors(selectedHex, h))
        {
            h.Hover();
        }
        hoverHex = h;
    }

    public void NextTurn()
    {
        turn++;
        if (turn > speed)
        {
            UpdateScore();
            turn = 0;
            if (!night)
            {
                Night();
                night = true;
            }
            else
            {
                Day();
                night = false;
                day++;
            }
        }
        MoveLight();
    }

    public void Day()
    {
        targetLightColor = dayColor;
    }
    public void Night()
    {
        targetLightColor = nightColor;
    }
    public void MoveLight()
    {
        targetLightRotation = Quaternion.Lerp(sunrise, sunset, (float) turn / (float) speed);
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
        light.color = Color.Lerp(light.color, targetLightColor, 0.05f);
        light.transform.rotation = Quaternion.Lerp(light.transform.rotation, targetLightRotation, 0.05f);
        mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, selectedHex.transform.position + cameraOffset, 0.05f);
    }

    public void Pause()
    {
        pauseMenu.gameObject.SetActive(!pauseMenu.gameObject.activeSelf);
    }

    public string GetDirection()
    {
        string str = "The Grand Oasis is ";
        Vector2Int dir = EndingPosition - selectedHex.GetIndex();
        if ((Random.value > 0.5f || dir.x == 0) && dir.y != 0)
        {
            if (dir.y > 0)
            {
                str += "Somewhere North ";
            }
            else if (dir.y < 0)
            {
                str += "Somewhere South ";
            }
        }
        else
        {
            if (dir.x > 0)
            {
                str += "Somewhere East ";
            }
            else if (dir.x < 0)
            {
                str += "Somewhere West ";
            }
        }
        return str;
    }

    public string GetDistance()
    {
        string str = "The Grand Oasis is ";
        Vector2Int dir = EndingPosition - selectedHex.GetIndex();
        str += Mathf.FloorToInt(dir.magnitude).ToString() + " hexes away";
        return str;
    }
}