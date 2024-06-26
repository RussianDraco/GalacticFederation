using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Milit : Entity
{
    public int MaxAttackPoints { get; set; }
    public int AttackPoints { get; set; }

    public Civil(string name, string description, int entityid, string iconpath, int health, float maxMovePoints, int maxAttackPoints) : base(name, description, entityid, iconpath, health, maxMovePoints)
    {
        MaxAttackPoints = maxAttackPoints;
        AttackPoints = maxAttackPoints;
    }
}