using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class ActionOptionScript : MonoBehaviour, IPointerClickHandler
{
    [HideInInspector] public CivilAction civilAction;
    public TMP_Text actionText;

    public void SetAction(CivilAction action)
    {
        civilAction = action;
        actionText.text = action.FunctionName + " - " + action.Description + " - " + action.ActionPoints + " AP";
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        GameObject.Find("MANAGER").GetComponent<ActionManager>().RequestCivilAction(civilAction);
    }
}