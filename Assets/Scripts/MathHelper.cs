using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MathHelper
{
    public static Vector3 a;
    public static Vector3 b;

    public static float VectorDistance(Vector3 a, Vector3 b)
    {
        float num1 = a.x - b.x;
        float num2 = a.y - b.y;
        float num3 = a.z - b.z;
        return (float)Mathf.Sqrt(num1 * num1 + num2 * num2 + num3 * num3);
    }

    public static float DotProduct(Vector3 a, Vector3 b)
    {
        return a.x * b.x + a.y * b.y + a.z * b.z;
    }

    public static Vector3 CrossProduct(Vector3 a, Vector3 b)
    {
        return new Vector3(a.y * b.z - a.z * b.y, a.z * b.x - a.x * b.z, a.x * b.y - a.y * b.x);
    }
    
    public static float AngleBetween(Vector3 a, Vector3 b)
    {
        return (float)Math.Acos(DotProduct(a, b) /Math.Sqrt(a.magnitude * b.magnitude)) * Mathf.Rad2Deg;
    }
}
