using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

// Entities are split into civils and milits: civilian non-combatants and military combatants

public class EntityManager : MonoBehaviour
{
    public GameManager gameManager;
    public TileMapManager tileMapManager;

    public float movementSpeed = 5f;
    private TileMap tileMap;
    public List<Civil> civils { get; private set; }
    public List<Milit> milits { get; private set; }

    public List<Civil> activeCivils { get; private set; }
    public List<Milit> activeMilits { get; private set; }
    public GameObject entityPrefab;

    private void Start()
    {
        activeCivils = new List<Civil>();
        activeMilits = new List<Milit>();

        civils = JsonUtility.FromJson<CivilWrapper>(File.ReadAllText("Assets/Jsons/civils.json")).civils;
        milits = JsonUtility.FromJson<MilitWrapper>(File.ReadAllText("Assets/Jsons/milits.json")).milits;

        tileMap = gameManager.tileMap;
    }

    private Sprite GrabIcon(string iconPath) {
        if (Resources.Load<Sprite>(iconPath) == null) {
            Debug.LogError("Icon not found at " + iconPath);
        }

        return Resources.Load<Sprite>(iconPath);
    }

    public void SpawnCivil(Civil civil, Vector2Int position) {
        var newCivil = new Civil(civil.Name, civil.Description, civil.EntityId, civil.IconPath, civil.Health, civil.MaxMovePoints, civil.MaxActionPoints);
        newCivil.Position = position;
        newCivil.GameObject = Instantiate(entityPrefab, new Vector3(position.x, position.y, 0), Quaternion.identity);
        newCivil.GameObject.GetComponent<SpriteRenderer>().sprite = GrabIcon(newCivil.IconPath);
        activeCivils.Add(newCivil);
    }
    public void SpawnMilit(Milit milit, Vector2Int position) {
        var newMilit = new Milit(milit.Name, milit.Description, milit.EntityId, milit.IconPath, milit.Health, milit.MaxMovePoints, milit.MaxAttackPoints);
        newMilit.Position = position;
        newMilit.GameObject = Instantiate(entityPrefab, new Vector3(position.x, position.y, 0), Quaternion.identity);
        newMilit.GameObject.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>(newMilit.IconPath);
        activeMilits.Add(newMilit);
    }

    private void UpdateEntityPosition(Entity entity)
    {
        if (entity.Position != new Vector2Int((int)entity.GameObject.transform.position.x, (int)entity.GameObject.transform.position.y))
        {
            Vector3 targetPosition = new Vector3(entity.Position.x, entity.Position.y, 0);
            entity.GameObject.transform.position = Vector3.MoveTowards(entity.GameObject.transform.position, targetPosition, movementSpeed * Time.deltaTime);
        }
    }

    public void MoveEntity(Entity entity, Vector2Int targetPosition)
    {
        entity.Position = targetPosition;
        entity.GameObject.transform.position = new Vector3(targetPosition.x, targetPosition.y, 0);
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