using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiManager : MonoBehaviour {
    public List<Ai> ais = new List<Ai>();

    public Ai CreateAi(Civilization civilization) {
        Ai ai = new Ai();
        ai.civilization = civilization;
        ais.Add(ai);
        return ai;
    }

    public void NextTurn() {
        foreach (Ai ai in ais) {
            ai.NextTurn();
        }
    }
}

public class Ai {
    public Civilization civilization;

    List<Vector2Int> MilitMovePath(Vector2Int position, Vector2Int targetPosition) {
        return Pathfinding.FindPath(GameObject.Find("MANAGER").GetComponent<GameManager>().tileMap, position, targetPosition);
    }
    Vector2Int TargetPosition(Vector2Int position) {
        List<Vector2Int> positions = Pathfinding.GetNeighorsList(position);
        return positions[Random.Range(0, positions.Count)];
    }
    void EntityMovement() {
        foreach (Civil civil in civilization.entityIdentity.civils) {
            GameObject.Find("MANAGER").GetComponent<EntityManager>().MoveEntity(civil, TargetPosition(civil.Position));
        }
        foreach (Milit milit in civilization.entityIdentity.milits) {
            GameObject.Find("MANAGER").GetComponent<EntityManager>().MoveEntity(milit, MilitMovePath(milit.Position, TargetPosition(milit.Position)));
        }
    }
    void CityManagement() {
        foreach (City city in civilization.cityIdentity.cities) {
            List<string> options = GameObject.Find("MANAGER").GetComponent<CityManager>().CityOptions(city, civilization.ownerId);
            if (city.Production == null) {
                GameObject.Find("MANAGER").GetComponent<CityManager>().CityOptionFunction(civilization.ownerId, city, 
                options[Random.Range(0, options.Count)]);
            }
        }
    }

    public void NextTurn() {
        EntityMovement();    
        CityManagement();
    }
}