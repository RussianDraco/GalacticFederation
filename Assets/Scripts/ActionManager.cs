using System.Collections;
using UnityEngine;

public class ActionManager : MonoBehaviour {
    private GameManager gameManager;
    private EntityManager entityManager;
    [HideInInspector] public dynamic selectedEntity;
    public SelectEntityScript selectEntityScript;

    void Start()
    {
        gameManager = GetComponent<GameManager>();
        entityManager = GetComponent<EntityManager>();
    }

    public void SelectCivilian(Civil civil) { //milit needs to be integrated next from this
        selectedEntity = civil;
        selectEntityScript.SetCivil(civil.Name, civil.Description, entityManager.GrabIcon(civil.IconPath), civil.Actions);
        Debug.Log("Selected civilian: " + civil.Name);
    }
}