using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Civil : Entity
{
    public int MaxActionPoints { get; set; }
    public int ActionPoints { get; set; }

    public Civil(string name, string description, int entityid, string iconpath, int health, float maxMovePoints, int maxActionPoints) : base(name, description, entityid, iconpath, health, maxMovePoints)
    {
        MaxActionPoints = maxActionPoints;
        ActionPoints = maxActionPoints;
    }
}