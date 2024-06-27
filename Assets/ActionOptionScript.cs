using System.Collections;
using UnityEngine;

public class ActionOptionScript : MonoBehaviour {
    [HideInInspector] public CivilAction civilAction;

    public void SetAction(CivilAction action) {
        civilAction = action;
    }

    private void OnMouseDown() {
        CivilActionGod.CallCivilAction(civilAction.FunctionName);
    }
}