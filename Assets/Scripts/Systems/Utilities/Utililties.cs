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
			objectsOfType[i].enabled = false;
		}
	}
	public static void DisableAllObjectsOfType<T>(T[] objectsOfType) where T : MonoBehaviour
	{
		for (int i = 0; i < objectsOfType.Length; i++)
		{
			objectsOfType[i].gameObject.SetActive(false);
		}
	}

	public static void EnableComponents<T>(T[] components, bool isEnable) where T : Behaviour
	{
		for (int i = 0; i < components.Length; i++)
		{
			components[i].enabled = isEnable;
		}
	}

	public static void EnableIsTriggerOnColliders(Collider2D[] colliders)
	{
		for (int i = 0; i < colliders.Length; i++)
		{
			colliders[i].isTrigger = true;
		}
	}
}
