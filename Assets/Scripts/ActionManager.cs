using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionManager : MonoBehaviour {
    private GameManager gameManager;
    private EntityManager entityManager;
    [HideInInspector] public object selectedEntity;
    public SelectEntityScript selectEntityScript;
    private bool isMoving = false;
    public LineScript lineObj;

    private List<Vector2> RealWorldPositions(TileMap tileMap, List<Vector2Int> positions) {
        List<Vector2> realWorldPositions = new List<Vector2>();
        foreach (Vector2Int position in positions) {
            realWorldPositions.Add(tileMap.GetWorldPosition(position));
        }
        return realWorldPositions;
    }

    void Start()
    {
        gameManager = GetComponent<GameManager>();
        entityManager = GetComponent<EntityManager>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(1) && !isMoving) {
            selectedEntity = null;
            selectEntityScript.Deselect();
        }

        if (isMoving && selectedEntity != null && selectedEntity is Civil) {
            lineObj.gameObject.SetActive(true);

            TileMap tileMap = gameManager.tileMap;
            List<Vector2Int> path = Pathfinding.FindPath(tileMap, (((Civil)selectedEntity).Position), tileMap.GetGridPosition(Camera.main.ScreenToWorldPoint(Input.mousePosition)));
            if (((Civil)selectedEntity).MovePoints > 0) {
                lineObj.DrawLine(RealWorldPositions(tileMap, path), Color.green);
            } else {
                lineObj.DrawLine(RealWorldPositions(tileMap, path), Color.red);
            }
            if (Input.GetMouseButtonDown(0) && ((Civil)selectedEntity).MovePoints > 0) {
                int moveQuant = ((Civil)selectedEntity).MovePoints;

                if (path.Count - 1 < ((Civil)selectedEntity).MovePoints) {
                    moveQuant = path.Count - 1;
                }

                Vector2Int dest = path[moveQuant];
                entityManager.MoveEntity(((Civil)selectedEntity), dest);
                ((Civil)selectedEntity).MovePoints -= moveQuant;
                selectEntityScript.CivilReload(((Civil)selectedEntity).MovePoints, ((Civil)selectedEntity).ActionPoints);
            }
            if (Input.GetMouseButtonDown(1)) {
                isMoving = false;
            }
        }
        if (isMoving && selectedEntity != null && selectedEntity is Milit) {
            lineObj.gameObject.SetActive(true);

            TileMap tileMap = gameManager.tileMap;
            List<Vector2Int> path = Pathfinding.FindPath(tileMap, (((Milit)selectedEntity).Position), tileMap.GetGridPosition(Camera.main.ScreenToWorldPoint(Input.mousePosition)));
            if (((Milit)selectedEntity).MovePoints > 0) {
                lineObj.DrawLine(RealWorldPositions(tileMap, path), Color.green);
            } else {
                lineObj.DrawLine(RealWorldPositions(tileMap, path), Color.red);
            }
            if (Input.GetMouseButtonDown(0) && ((Milit)selectedEntity).MovePoints > 0) {
                int moveQuant = ((Milit)selectedEntity).MovePoints;

                if (path.Count - 1 < ((Milit)selectedEntity).MovePoints) {
                    moveQuant = path.Count - 1;
                }

                Vector2Int dest = path[moveQuant];
                entityManager.MoveEntity(((Milit)selectedEntity), dest);
                ((Milit)selectedEntity).MovePoints -= moveQuant;
                selectEntityScript.MilitReload(((Milit)selectedEntity).MovePoints);
            }
            if (Input.GetMouseButtonDown(1)) {
                isMoving = false;
            }
        }


        if (!isMoving) {
            lineObj.gameObject.SetActive(false);
        }
    }

    public void SelectCivil(Civil civil) { //milit needs to be integrated next from this
        selectedEntity = civil;
        selectEntityScript.SetCivil(civil.Name, civil.Description, civil.MovePoints, civil.ActionPoints, entityManager.GrabIcon(civil.IconPath), civil.Actions);
        isMoving = false;
    }
    public void SelectMilit(Milit milit) {
        selectedEntity = milit;
        selectEntityScript.SetMilit(milit.Name, milit.Description, milit.MovePoints, entityManager.GrabIcon(milit.IconPath));
        isMoving = false;
    }

    public void RequestCivilAction(CivilAction civilAction) {
        if (selectedEntity != null || !(selectedEntity is Civil)) {
            Civil civil = (Civil)selectedEntity;
            if (civilAction.ActionPoints <= civil.ActionPoints) {
                if (CivilActionGod.CallCivilAction(civilAction.FunctionName.Replace(" ", "_"), new object[] { civil })) {
                    civil.ActionPoints -= civilAction.ActionPoints;
                    selectEntityScript.CivilReload(((Civil)selectedEntity).MovePoints, ((Civil)selectedEntity).ActionPoints);
                } else {
                    Debug.Log("Action failed!");
                }
            } else {
                Debug.Log("Not enough action points!");
            }
        } else {
            Debug.LogError("RequestCivilAction called without a selected entity OR selected entity is not a Civil!");
        }
    }

    public void MoveButton() {
        isMoving = true;
    }
}