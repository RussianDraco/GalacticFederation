using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiManager : MonoBehaviour {
    public List<Ai> ais = new List<Ai>();

    public Ai CreateAi(Civilization civilization) {
        Ai ai = new Ai();
        ai.civilization = civilization;
        ai.manager = this.gameObject;
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
    public GameObject manager;

    List<Vector2Int> MilitMovePath(Vector2Int position, Vector2Int targetPosition) {
        return Pathfinding.FindPath(manager.GetComponent<GameManager>().tileMap, position, targetPosition);
    }
    Vector2Int TargetPosition(Vector2Int position) {
        List<Vector2Int> positions = Pathfinding.GetNeighorsList(manager.GetComponent<GameManager>().tileMap, position);
        return positions[Random.Range(0, positions.Count)];
    }
    void EntityMovement() {
        for (int i = 0; i < civilization.entityIdentity.civils.Count; i++) {
            Civil civil = civilization.entityIdentity.civils[i];
            manager.GetComponent<EntityManager>().MoveEntity(civil, TargetPosition(civil.Position));
            if (Random.Range(0, 3) == 0) {
                List<CivilAction> acts = civil.Actions;
                manager.GetComponent<ActionManager>().RequestCivilAction(civil, acts[Random.Range(0, acts.Count)]);
            }
            try {
                if (civil == null) {
                    i--;
                }
            } catch {
                i--;
            }
        }
        foreach (Milit milit in civilization.entityIdentity.milits) {
            manager.GetComponent<EntityManager>().MoveEntity(milit, MilitMovePath(milit.Position, TargetPosition(milit.Position)));
            List<Civil> ctargets = new List<Civil>();
            List<Milit> mtargets = new List<Milit>();
            foreach (Vector2Int t in manager.GetComponent<GameManager>().tileMap.GetNeighors(milit.Position)) {
                Civil c = manager.GetComponent<EntityManager>().CivilOnPosition(t);
                Milit m = manager.GetComponent<EntityManager>().MilitOnPosition(t);
                if (c != null) {
                    ctargets.Add(c);
                }
                if (m != null) {
                    mtargets.Add(m);
                }
            }
            foreach (Vector2Int t in manager.GetComponent<GameManager>().tileMap.GetSecondNeighbours(milit.Position)) {
                Civil c = manager.GetComponent<EntityManager>().CivilOnPosition(t);
                Milit m = manager.GetComponent<EntityManager>().MilitOnPosition(t);
                if (c != null) {
                    ctargets.Add(c);
                }
                if (m != null) {
                    mtargets.Add(m);
                }
            }
            
        }
    }
    void CityManagement() {
        foreach (City city in civilization.cityIdentity.cities) {
            List<string> options = manager.GetComponent<CityManager>().CityOptions(city, civilization.ownerId);
            if (city.Production == null) {
                manager.GetComponent<CityManager>().CityOptionFunction(civilization.ownerId, city, 
                options[Random.Range(0, options.Count)]);
            }
        }
    }

    public void NextTurn() {
        EntityMovement();    
        CityManagement();
    }
}