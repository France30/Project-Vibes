using UnityEngine;

public static class Utilities
{
    public static void InitializeReferenceOfComponent<T>(GameObject gameObject, ref T component) where T : Component
    {
        component = (gameObject.GetComponent<T>()) ? gameObject.GetComponent<T>() : gameObject.AddComponent<T>();
    }

    public static void RemoveReferenceOfDisabledComponent<T>(ref T reference) where T : MonoBehaviour
    {
        if (reference == null) return;

        if (!reference.enabled)
            reference = null;
    }

    public static void DisableAllInstancesOfType<T>(T[] objectsOfType) where T : MonoBehaviour
    {
        for (int i = 0; i < objectsOfType.Length; i++)
        {
            if (!objectsOfType[i].enabled) continue;

            objectsOfType[i].enabled = false;
        }
    }
}
