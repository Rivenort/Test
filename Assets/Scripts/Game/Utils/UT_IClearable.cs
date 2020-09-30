using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Classes that implement the following interface are supposed to 
/// prepare themselfs for a new session (clear all heavy session-related data)
/// @author Dominik
/// </summary>
public interface UT_IClearable
{
    void Clear();
}
