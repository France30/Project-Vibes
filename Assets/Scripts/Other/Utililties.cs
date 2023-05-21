using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utilities
{
    public static void InitializeReferenceOfComponent<T>(GameObject gameObject, ref T component) where T : Component
    {
        component = (gameObject.GetComponent<T>()) ? gameObject.GetComponent<T>() : gameObject.AddComponent<T>();
    }
}
