using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public static class CivilActionGod
{
    public static void CallCivilAction(string methodName, List<object> parameters = null)
    {
        // Get the type of the current class
        Type type = typeof(CivilActionGod);

        // Find the method by name
        MethodInfo method = type.GetMethod(methodName, BindingFlags.Static | BindingFlags.Public);

        if (method != null)
        {
            if (parameters == null)
            {
                parameters = new List<object>();
            }

            // Convert the List<object> to an array
            object[] parametersArray = parameters.ToArray();

            // Invoke the method with the parameters
            method.Invoke(null, parametersArray);
        }
        else
        {
            Debug.Log("Method not found: " + methodName);
        }
    }

    public static void Stick()
    {
        Debug.Log("Wow, it's a stick. Pretty neat!");
    }
}