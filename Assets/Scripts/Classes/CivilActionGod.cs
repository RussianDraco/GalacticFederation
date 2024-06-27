using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public static class CivilActionGod
{
    public static void CallCivilAction(string methodName, object[] parameters = null) //first parameter is casting Civil
    {
        // Get the type of the current class
        Type type = typeof(CivilActionGod);

        // Find the method by name
        MethodInfo method = type.GetMethod(methodName, BindingFlags.Static | BindingFlags.Public);

        if (method != null)
        {
            if (parameters == null)
            {
                Debug.LogWarning("No parameters provided(first parameter needs to be casting Civil object) for method: " + methodName);
            }

            // Invoke the method with the parameters
            method.Invoke(null, parameters);
        }
        else
        {
            Debug.Log("Method not found: " + methodName);
        }
    }

    //all civil actions require parameter 1: Civil caster
    public static void Stick(Civil caster)
    {
        Debug.Log(caster.Name + " used Stick!");
        Debug.Log("Wow, it's a stick. Pretty neat!");
    }
}