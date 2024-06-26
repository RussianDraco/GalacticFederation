using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Entity
{
    public string Name { get; set; }
    public string Description { get; set; }
    public int EntityId { get; set; }
    public string IconPath { get; set; }
    public int Health { get; set; }
    public float MaxMovePoints { get; set; }
    public float MovePoints { get; set; }
    public Vector2Int Position { get; set; }
    public GameObject GameObject { get; set; }

    public Entity(string name, string description, int entityid, string iconpath, int health, float maxMovePoints)
    {
        Name = name;
        Description = description;
        EntityId = entityid;
        IconPath = iconpath;
        Health = health;
        MaxMovePoints = maxMovePoints;
        MovePoints = maxMovePoints;
    }
}

