using System.Collections;
using UnityEngine;

public class ActionManager : MonoBehaviour {
    private GameManager gameManager;
    private EntityManager entityManager;
    [HideInInspector] public object selectedEntity;
    public SelectEntityScript selectEntityScript;

    void Start()
    {
        gameManager = GetComponent<GameManager>();
        entityManager = GetComponent<EntityManager>();
    }

    public void SelectCivilian(Civil civil) { //milit needs to be integrated next from this
        selectedEntity = civil;
        selectEntityScript.SetCivil(civil.Name, civil.Description, entityManager.GrabIcon(civil.IconPath), civil.Actions);
    }

    public void RequestCivilAction(CivilAction civilAction) {
        if (selectedEntity != null || !(selectedEntity is Civil)) {
            Civil civil = (Civil)selectedEntity;
            if (civilAction.ActionPoints <= civil.ActionPoints) {
                civil.ActionPoints -= civilAction.ActionPoints;
                CivilActionGod.CallCivilAction(civilAction.FunctionName, new object[] { civil });
            }
        } else {
            Debug.LogError("RequestCivilAction called without a selected entity OR selected entity is not a Civil!");
        }
    }
}