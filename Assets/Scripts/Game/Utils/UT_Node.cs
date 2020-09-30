using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// @author Rivenort
/// </summary>
/// <typeparam name="T"></typeparam>
public class UT_Node<T>
{

    private List<UT_Node<T>> m_adjacent;
    private List<UT_Node<T>> m_inAdjacent;
    private T m_data;
    
    public UT_Node()
    {
        m_adjacent = new List<UT_Node<T>>();
        m_inAdjacent = new List<UT_Node<T>>();
    }

    public UT_Node(T data) : this()
    {
        m_data = data;
    }

    public void AddAdjacent(UT_Node<T> node)
    {
        m_adjacent.Add(node);
        node.m_inAdjacent.Add(this);
    }

    public void RemoveAdjacent(UT_Node<T> node)
    {
        node.m_inAdjacent.Remove(this);
        m_adjacent.Remove(node);
    }

    public List<UT_Node<T>> GetAdjacent()
    {
        return m_adjacent;
    }

    public List<UT_Node<T>> GetInAdjacent()
    {
        return m_inAdjacent;
    }

    public bool HasAdjacent(UT_Node<T> node)
    {
        return m_adjacent.Contains(node);
    }

    public void SetData(T data)
    {
        m_data = data;
    }

    public T GetData()
    {
        return m_data;
    }
}
