using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

//Entities are split into civils and milits: civilian non-combatants and military combatants

public static class EntityManager
{
    //public static List<Entity> entities { get; set; }
    public static List<Civil> civils { get; set; }
    public static List<Milit> milits { get; set; }

    static EntityManager()
    {
        string json = File.ReadAllText(filePath);
        entities = JsonUtility.FromJson<EntityWrapper>(json).entities;

        civils = JsonUtility.FromJson
    }
}

[System.Serializable]
public class EntityWrapper {
    public List<Entity> entities;
}

[System.Serializable]
public class CivilWrapper {
    public List<Civil> civils;
}

[System.Serializable]
public class MilitWrapper {
    public List<Milit> milits;
}