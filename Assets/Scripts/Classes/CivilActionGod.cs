using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public static class CivilActionGod
{
    public static bool CallCivilAction(string methodName, object[] parameters = null) //first parameter is casting Civil
    {
        // Get the type of the current class
        Type type = typeof(CivilActionGod);

        // Find the method by name
        MethodInfo method = type.GetMethod(methodName, BindingFlags.Static | BindingFlags.Public);

        if (method != null)
        {
            if (parameters == null)
            {
                Debug.LogWarning("No parameters provided (first parameter needs to be casting Civil object) for method: " + methodName);
            }

            // Invoke the method with the parameters
            object result = method.Invoke(null, parameters);

            if (result is bool)
            {
                return (bool)result;
            }
            else
            {
                Debug.LogWarning("Method " + methodName + " does not return a boolean value.");
            }
        }
        else
        {
            Debug.Log("Method not found: " + methodName);
        }

        return false;
    }


    //all civil actions require parameter 1: Civil caster

    public static bool Settle(Civil civil)
    {
        if (GameObject.Find("MANAGER").GetComponent<GameManager>().tileMap.AddTileExtra(civil.Position, "City")) {
            GameObject.Find("MANAGER").GetComponent<EntityManager>().KillEntity(civil);
            GameObject.Find("MANAGER").GetComponent<ActionManager>().Deselection();
            GameObject.Find("MANAGER").GetComponent<CityManager>().AddCity(civil.Position);
            return true;
        } else {
            return false;
        }
    }
}