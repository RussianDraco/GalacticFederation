using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

// Entities are split into civils and milits: civilian non-combatants and military combatants

public class EntityManager : MonoBehaviour
{
    private GameManager gameManager;
    private TileMapManager tileMapManager;
    private ScienceManager scienceManager;

    public float movementSpeed = 5f;
    private TileMap tileMap;

    [HideInInspector] public List<Civil> civils = new List<Civil>();
    [HideInInspector] public List<Milit> milits = new List<Milit>();

    [HideInInspector] public List<Civil> activeCivils = new List<Civil>();
    [HideInInspector] public List<Milit> activeMilits = new List<Milit>();
    
    public GameObject civilPrefab;
    public GameObject militPrefab;

    public Civil GetCivil(string name) {return civils.Find(x => x.Name == name);}
    public Milit GetMilit(string name) {return milits.Find(x => x.Name == name);}

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
        scienceManager = GetComponent<ScienceManager>();
    }

    public Sprite GrabIcon(string iconPath) {
        if (Resources.Load<Sprite>(iconPath) == null) {
            Debug.LogError("Icon not found at " + iconPath);
        }

        return Resources.Load<Sprite>(iconPath);
    }

    public string UnitToolTip(object entity) {
        if (entity is Civil) {
            Civil civil = (Civil)entity;
            return civil.Name + "\n" + civil.Description + "\nHealth: " + civil.Health + "\nMove Points: " + civil.MovePoints + "\nAction Points: " + civil.ActionPoints;
        } else if (entity is Milit) {
            Milit milit = (Milit)entity;
            return milit.Name + "\n" + milit.Description + "\nHealth: " + milit.Health + "\nMove Points: " + milit.MovePoints + "\nAttack Damage: " + milit.AttackDamage;
        }
        return "No tooltip available";
    }

    Vector3 CoordToPosition(Vector2Int coord) {
        Vector2 worldPosition = tileMap.GetWorldPosition(coord);
        return new Vector3(worldPosition.x, worldPosition.y, 0f);
    }
    Vector2Int PositionToCoord(Vector3 position) {
        return tileMap.GetGridPosition(position);
    }

    public void CitySpawn(City city, object entity) {
        if (entity is Civil) {
            SpawnCivil((Civil)entity, city.Position);
        } else if (entity is Milit) {
            SpawnMilit((Milit)entity, city.Position);
        }
    }

    public void SpawnCivil(Civil civil, Vector2Int position) {
        var newCivil = new Civil(civil.Name, civil.Description, civil.IconPath, civil.Health, civil.MaxMovePoints, civil.MaxActionPoints, civil.Actions, civil.Cost, civil.researchRequirement);
        newCivil.Position = position;
        newCivil.GameObject = Instantiate(civilPrefab, CoordToPosition(position), Quaternion.identity);
        newCivil.GameObject.GetComponent<CivilScript>().SetCivil(newCivil, GrabIcon(newCivil.IconPath));
        activeCivils.Add(newCivil);
    }
    public void SpawnMilit(Milit milit, Vector2Int position) {
        var newMilit = new Milit(milit.Name, milit.Description, milit.EntityId, milit.IconPath, milit.Health, milit.MaxMovePoints, milit.AttackDamage, milit.Cost, milit.researchRequirement);
        newMilit.Position = position;
        newMilit.GameObject = Instantiate(militPrefab, CoordToPosition(position), Quaternion.identity);
        newMilit.GameObject.GetComponent<MilitScript>().SetMilit(newMilit, GrabIcon(newMilit.IconPath));
        activeMilits.Add(newMilit);
    }

    private void UpdateEntityPosition(Civil civil) {
        if (civil.Position != PositionToCoord(civil.GameObject.transform.position)) {
            civil.GameObject.transform.position = Vector3.MoveTowards(civil.GameObject.transform.position, CoordToPosition(civil.Position), movementSpeed * Time.deltaTime);
        }
    }
    private void UpdateEntityPosition(Milit milit) {
        if (milit.Position != PositionToCoord(milit.GameObject.transform.position)) {
            milit.GameObject.transform.position = Vector3.MoveTowards(milit.GameObject.transform.position, CoordToPosition(milit.Position), movementSpeed * Time.deltaTime);
        }
    }

    private bool IsOccupied(Vector2Int position) {
        foreach (var entity in activeCivils) {
            if (entity.Position == position) {
                return true;
            }
        }
        foreach (var entity in activeMilits) {
            if (entity.Position == position) {
                return true;
            }
        }
        return false;
    }
    public object EntityOn(Vector2Int position) {
        foreach (var entity in activeCivils) {
            if (entity.Position == position) {
                return entity;
            }
        }
        foreach (var entity in activeMilits) {
            if (entity.Position == position) {
                return entity;
            }
        }
        return null;
    }
    public bool MoveEntity(Civil civil, Vector2Int targetPosition) {
        if (IsOccupied(targetPosition)) {
            return false;
        }
        civil.Position = targetPosition;
        civil.GameObject.transform.position = CoordToPosition(targetPosition);
        return true;
    }
    public bool MoveEntity(Milit milit, Vector2Int targetPosition) {
        if (IsOccupied(targetPosition)) {
            return false;
        }
        milit.Position = targetPosition;
        milit.GameObject.transform.position = CoordToPosition(targetPosition);
        return true;
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

    public void NextTurn()
    {
        foreach (var entity in activeCivils)
        {
            entity.MovePoints = entity.MaxMovePoints;
        }
        foreach (var entity in activeMilits)
        {
            entity.MovePoints = entity.MaxMovePoints;
            entity.hasAttacked = false;
        }
    }

    public void KillEntity(object entity)
    {
        if (entity is Civil)
        {
            activeCivils.Remove((Civil)entity);
            Destroy(((Civil)entity).GameObject);
        }
        else if (entity is Milit)
        {
            activeMilits.Remove((Milit)entity);
            Destroy(((Milit)entity).GameObject);
        }
    }

    public (List<Civil>, List<Milit>) PossibleUnits(City city)
    {
        List<Civil> possibleCivils = new List<Civil>();
        List<Milit> possibleMilits = new List<Milit>();

        foreach (Civil civil in civils) {
            if (civil.researchRequirement != -1) {
                if (!scienceManager.IsResearched(civil.researchRequirement)) {
                    continue;
                }
            }
            possibleCivils.Add(civil);
        }

        foreach (Milit milit in milits) {
            if (milit.researchRequirement != -1) {
                if (!scienceManager.IsResearched(milit.researchRequirement)) {
                    continue;
                }
            }
            possibleMilits.Add(milit);
        }

        return (possibleCivils, possibleMilits);
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
    public float MaxHealth;
    public int MaxMovePoints;
    public int MovePoints;
    public Vector2Int Position;
    public GameObject GameObject;
    public int MaxActionPoints;
    public int ActionPoints;
    public List<CivilAction> Actions = new List<CivilAction>();
    public int researchRequirement;
    public string Owner;
    public int Cost;

    public Civil(string Name, string Description, string IconPath, float Health, int MaxMovePoints, int MaxActionPoints, List<CivilAction> Actions, int Cost, int researchRequirement)
    {
        this.Name = Name;
        this.Description = Description;
        this.IconPath = IconPath;
        this.Health = Health;
        this.MaxHealth = Health;
        this.MaxMovePoints = MaxMovePoints;
        this.MovePoints = MaxMovePoints;
        this.MaxActionPoints = MaxActionPoints;
        this.ActionPoints = MaxActionPoints;
        this.Actions = Actions;
        this.researchRequirement = researchRequirement; //-1 if no research requirement
        this.Owner = "Player";
        this.Cost = Cost;
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
    public float MaxHealth;
    public int MaxMovePoints;
    public int MovePoints;
    public Vector2Int Position;
    public GameObject GameObject;
    public bool hasAttacked;
    public int AttackDamage;
    public int researchRequirement; //-1 if no research requirement
    public string Owner;
    public int Cost;

    public Milit(string Name, string Description, int EntityId, string IconPath, float Health, int MaxMovePoints, int AttackDamage, int Cost, int researchRequirement)
    {
        this.Name = Name;
        this.Description = Description;
        this.EntityId = EntityId;
        this.IconPath = IconPath;
        this.Health = Health;
        this.MaxHealth = Health;
        this.MaxMovePoints = MaxMovePoints;
        this.MovePoints = MaxMovePoints;
        this.AttackDamage = AttackDamage;
        this.Owner = "Player";
        this.researchRequirement = researchRequirement;
        this.Cost = Cost;
    }

    public void Attack(Milit target)
    {
        target.Health -= 10;
    }
}