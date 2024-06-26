using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Milit : Entity
{
    public int MaxAttackPoints { get; set; }
    public int AttackPoints { get; set; }

    public Milit(string name, string description, int entityid, string iconpath, float health, float maxMovePoints, int maxAttackPoints) : base(name, description, entityid, iconpath, health, maxMovePoints)
    {
        MaxAttackPoints = maxAttackPoints;
        AttackPoints = maxAttackPoints;
    }

    public void Attack(Entity target)
    {
        target.Health -= 10;
    }
}