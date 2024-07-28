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
}

public class Ai {
    public Civilization civilization;
}