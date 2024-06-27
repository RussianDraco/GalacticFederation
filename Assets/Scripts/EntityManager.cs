using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

// Entities are split into civils and milits: civilian non-combatants and military combatants

public class EntityManager : MonoBehaviour
{
    private GameManager gameManager;
    private TileMapManager tileMapManager;

    public float movementSpeed = 5f;
    private TileMap tileMap;

    [HideInInspector] public List<Civil> civils = new List<Civil>();
    [HideInInspector] public List<Milit> milits = new List<Milit>();

    [HideInInspector] public List<Civil> activeCivils = new List<Civil>();
    [HideInInspector] public List<Milit> activeMilits = new List<Milit>();
    
    public GameObject civilPrefab;
    public GameObject militPrefab;

    private void Awake()
    {
        activeCivils = new List<Civil>();
        activeMilits = new List<Milit>();

        string jsonPath = Application.dataPath + "/Jsons/civils.json";
        if (File.Exists(jsonPath))
        {
            string json = File.ReadAllText(jsonPath);
            CivilWrapper civilWrapper = JsonUtility.FromJson<CivilWrapper>(json);
            civils = civilWrapper.civils;
        }
        else
        {
            Debug.LogError("JSON file not found: " + jsonPath);
        }
        jsonPath = Application.dataPath + "/Jsons/milits.json";
        if (File.Exists(jsonPath))
        {
            string json = File.ReadAllText(jsonPath);
            MilitWrapper militWrapper = JsonUtility.FromJson<MilitWrapper>(json);
            milits = militWrapper.milits;
        }
        else
        {
            Debug.LogError("JSON file not found: " + jsonPath);
        }

        gameManager = GetComponent<GameManager>();
        tileMapManager = GetComponent<TileMapManager>();
        tileMap = gameManager.tileMap;
    }

    public Sprite GrabIcon(string iconPath) {
        if (Resources.Load<Sprite>(iconPath) == null) {
            Debug.LogError("Icon not found at " + iconPath);
        }

        return Resources.Load<Sprite>(iconPath);
    }

    public void SpawnCivil(Civil civil, Vector2Int position) {
        var newCivil = new Civil(civil.Name, civil.Description, civil.IconPath, civil.Health, civil.MaxMovePoints, civil.MaxActionPoints, civil.Actions);
        newCivil.Position = position;
        newCivil.GameObject = Instantiate(civilPrefab, new Vector3(position.x, position.y, 0), Quaternion.identity);
        newCivil.GameObject.GetComponent<CivilScript>().SetCivil(newCivil, GrabIcon(newCivil.IconPath));
        activeCivils.Add(newCivil);
    }
    public void SpawnMilit(Milit milit, Vector2Int position) {
        var newMilit = new Milit(milit.Name, milit.Description, milit.EntityId, milit.IconPath, milit.Health, milit.MaxMovePoints, milit.MaxAttackPoints);
        newMilit.Position = position;
        newMilit.GameObject = Instantiate(militPrefab, new Vector3(position.x, position.y, 0), Quaternion.identity);
        newMilit.GameObject.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>(newMilit.IconPath);
        activeMilits.Add(newMilit);
    }

    private void UpdateEntityPosition(Civil civil) {
        if (civil.Position != new Vector2Int((int)civil.GameObject.transform.position.x, (int)civil.GameObject.transform.position.y))
        {
            Vector3 targetPosition = new Vector3(civil.Position.x, civil.Position.y, 0);
            civil.GameObject.transform.position = Vector3.MoveTowards(civil.GameObject.transform.position, targetPosition, movementSpeed * Time.deltaTime);
        }
    }
    private void UpdateEntityPosition(Milit milit) {
        if (milit.Position != new Vector2Int((int)milit.GameObject.transform.position.x, (int)milit.GameObject.transform.position.y)) {
            Vector3 targetPosition = new Vector3(milit.Position.x, milit.Position.y, 0);
            milit.GameObject.transform.position = Vector3.MoveTowards(milit.GameObject.transform.position, targetPosition, movementSpeed * Time.deltaTime);
        }
    }

    public void MoveEntity(Civil civil, Vector2Int targetPosition) {
        civil.Position = targetPosition;
        civil.GameObject.transform.position = new Vector3(targetPosition.x, targetPosition.y, 0);
    }
    public void MoveEntity(Milit milit, Vector2Int targetPosition) {
        milit.Position = targetPosition;
        milit.GameObject.transform.position = new Vector3(targetPosition.x, targetPosition.y, 0);
    }

    public void UpdateEntities()
    {
        foreach (var entity in activeCivils)
        {
            UpdateEntityPosition(entity);
        }
        foreach (var entity in activeMilits)
        {
            UpdateEntityPosition(entity);
        }
    }
}


[System.Serializable]
public class CivilWrapper
{
    public List<Civil> civils;
}

[System.Serializable]
public class MilitWrapper
{
    public List<Milit> milits;
}


[System.Serializable]
public class Civil
{
    public string Name;
    public string Description;
    public string IconPath;
    public float Health;
    public float MaxMovePoints;
    public float MovePoints;
    public Vector2Int Position;
    public GameObject GameObject;
    public int MaxActionPoints;
    public int ActionPoints;
    public List<CivilAction> Actions = new List<CivilAction>();

    public Civil(string Name, string Description, string IconPath, float Health, float MaxMovePoints, int MaxActionPoints, List<CivilAction> Actions)
    {
        this.Name = Name;
        this.Description = Description;
        this.IconPath = IconPath;
        this.Health = Health;
        this.MaxMovePoints = MaxMovePoints;
        this.MovePoints = MaxMovePoints;
        this.MaxActionPoints = MaxActionPoints;
        this.ActionPoints = MaxActionPoints;
        this.Actions = Actions;
    }
}
[System.Serializable]
public class CivilActionWrapper
{
    public List<CivilAction> civilActions = new List<CivilAction>();
}
[System.Serializable]
public class CivilAction
{
    public string FunctionName;
    public string Description;
    public int ActionPoints;

    public CivilAction(string FunctionName, string Description, int ActionPoints)
    {
        this.FunctionName = FunctionName;
        this.Description = Description;
        this.ActionPoints = ActionPoints;
    }
}

[System.Serializable]
public class Milit
{
    public string Name;
    public string Description;
    public int EntityId;
    public string IconPath;
    public float Health;
    public float MaxMovePoints;
    public float MovePoints;
    public Vector2Int Position;
    public GameObject GameObject;
    public int MaxAttackPoints;
    public int AttackPoints;

    public Milit(string Name, string Description, int EntityId, string IconPath, float Health, float MaxMovePoints, int MaxAttackPoints)
    {
        this.Name = Name;
        this.Description = Description;
        this.EntityId = EntityId;
        this.IconPath = IconPath;
        this.Health = Health;
        this.MaxMovePoints = MaxMovePoints;
        this.MovePoints = MaxMovePoints;
        this.MaxAttackPoints = MaxAttackPoints;
        this.AttackPoints = MaxAttackPoints;
    }

    public void Attack(Milit target)
    {
        target.Health -= 10;
    }
}