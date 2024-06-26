using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

// Entities are split into civils and milits: civilian non-combatants and military combatants

public class EntityManager : MonoBehaviour
{
    private EntityManager instance;
    public EntityManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<EntityManager>();
                if (instance == null)
                {
                    GameObject obj = new GameObject();
                    obj.name = "EntityManager";
                    instance = obj.AddComponent<EntityManager>();
                }
            }
            return instance;
        }
    }

    public List<Entity> activeEntities { get; private set; }
    public List<Civil> civils { get; private set; }
    public List<Milit> milits { get; private set; }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        activeEntities = new List<Entity>();
        civils = JsonUtility.FromJson<CivilWrapper>(File.ReadAllText("Assets/Jsons/civils.json")).civils;
        milits = JsonUtility.FromJson<MilitWrapper>(File.ReadAllText("Assets/Jsons/milits.json")).milits;
    }

    public void InitEntity(Entity entity, Vector2Int position) // make an entity somewhere on the map 
    {
        entity.Position = position;
        activeEntities.Add(entity);
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