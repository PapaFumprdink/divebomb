using UnityEngine;

public static class Util
{
    public static float MaxAbsolute(float a, float b) => MaxAbsolute(new float[] { a, b });
    public static float MaxAbsolute (float[] values)
    {
        if (values.Length > 1)
        {
            float maxValue = values[0];

            for (int i = 0; i < values.Length; i++)
            {
                if (Mathf.Abs(values[i]) > Mathf.Abs(maxValue))
                {
                    maxValue = values[i];
                }
            }

            return maxValue;
        }
        else if (values.Length == 1)
        {
            return values[0];
        }
        else
        {
            return default;
        }
    }
}
