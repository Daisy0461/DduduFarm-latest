using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TextFormat
{
    public static string IntToFraction(in int val1, in int val2)
    {
        return $"{val1}/{val2}";
    }

    public static string IntToPrice(this int val)
    {
        return $"{val:#,##0}";
    }
}
