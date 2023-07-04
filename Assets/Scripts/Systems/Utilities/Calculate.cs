using UnityEngine;

public static class Calculate
{
    public static float RoundedAbsoluteValue(float value)
    {
        value = Mathf.Abs(value);
        return Mathf.Round(value);
    }
}
