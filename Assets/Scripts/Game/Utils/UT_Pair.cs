using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// @author Rivenort
/// </summary>
public struct UT_Pair<T1, T2>
{
    public T1 first;
    public T2 second;

    public UT_Pair(T1 first, T2 second)
    {
        this.first = first;
        this.second = second;
    }
}
