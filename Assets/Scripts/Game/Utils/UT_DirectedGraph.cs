using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

/// <summary>
/// @author Rivenort
/// </summary>
public class UT_DirectedGraph<T>
{

    private List<UT_Node<T>> m_nodes;

    public UT_DirectedGraph()
    {
        m_nodes = new List<UT_Node<T>>();
    }

    public UT_Node<T> AddNode(T data)
    {
        UT_Node<T> node = new UT_Node<T>(data);
        m_nodes.Add(node);
        return node;
    }

    public bool RemoveNode(UT_Node<T> node)
    {
        if (node == null)
            throw new ArgumentNullException("Given node instance cannot be a null!");
        return m_nodes.Remove(node);
    } 

    public void AddConnection(UT_Node<T> nodeOut, UT_Node<T> nodeIn) 
    {
        if (nodeOut == null || nodeIn == null)
            throw new ArgumentNullException("Given node instance cannot be a null!");
        if (!m_nodes.Contains(nodeOut))
            m_nodes.Add(nodeOut);
        if (!m_nodes.Contains(nodeIn))
            m_nodes.Add(nodeIn);

        if (nodeIn.HasAdjacent(nodeOut))
            return;

        nodeOut.AddAdjacent(nodeIn);
    }

    public void Foreach(Action<UT_Node<T>> Func)
    {
        foreach (UT_Node<T> node in m_nodes)
        {
            Func(node);
        }
    }

    public List<UT_Node<T>> GetNodes()
    {
        return m_nodes;
    }

    public UT_Node<T> GetNodeWithData(T data, bool identity)
    {
        foreach (UT_Node<T> node in m_nodes)
        {
            if ((identity && node.GetData().GetHashCode() == data.GetHashCode())
                || !identity && node.GetData().Equals(data))
                return node;
        }
        return null;
    }

    public List<UT_Node<T>> BFS(UT_Node<T> startNode)
    {
        List<UT_Node<T>> res = new List<UT_Node<T>>();
        bool[] visited = new bool[m_nodes.Count];

        Queue<UT_Node<T>> queue = new Queue<UT_Node<T>>();

        visited[m_nodes.IndexOf(startNode)] = true;
        queue.Enqueue(startNode);

        // startNode is now used as a temporary node
        while (queue.Count > 0)
        {
            startNode = queue.Dequeue();
            res.Add(startNode);

            List<UT_Node<T>> adj = startNode.GetAdjacent();
            foreach (UT_Node<T> node in adj)
            {
                int index = m_nodes.IndexOf(node);
                if (!visited[index])
                {
                    visited[index] = true;
                    queue.Enqueue(node);
                }
            }
        }

        return res;
    }

    public List<UT_Pair<UT_Node<T>, int>> GetPathLengths(UT_Node<T> startNode) 
    {
        int level = 0;
        List<UT_Pair<UT_Node<T>, int>> res = new List<UT_Pair<UT_Node<T>, int>>();
        bool[] visited = new bool[m_nodes.Count];

        Queue<UT_Pair<UT_Node<T>, int>> queue = new Queue<UT_Pair<UT_Node<T>, int>>();

        visited[m_nodes.IndexOf(startNode)] = true;
        queue.Enqueue(new UT_Pair<UT_Node<T>, int>(startNode, level));

        while (queue.Count > 0)
        {
            UT_Pair<UT_Node<T>, int> temp = queue.Dequeue();
            res.Add(temp);
            if (temp.second > level)
                level++;

            List<UT_Node<T>> adj = temp.first.GetAdjacent();
            foreach (UT_Node<T> node in adj)
            {
                int index = m_nodes.IndexOf(node);
                if (!visited[index])
                {
                    visited[index] = true;
                    queue.Enqueue(new UT_Pair<UT_Node<T>, int>(node, level + 1));
                }
            }
        }

        return res;
    }


    /// <summary>
    /// Func done by each node traversed.
    /// </summary>
    public void BFS(UT_Node<T> startNode, Action<UT_Node<T>> Func)
    {
        bool[] visited = new bool[m_nodes.Count];

        Queue<UT_Node<T>> queue = new Queue<UT_Node<T>>();

        visited[m_nodes.IndexOf(startNode)] = true;
        queue.Enqueue(startNode);

        // startNode is now used as a temporary node
        while (queue.Count > 0)
        {
            startNode = queue.Dequeue();
            Func(startNode);

            List<UT_Node<T>> adj = startNode.GetAdjacent();
            foreach (UT_Node<T> node in adj)
            {
                int index = m_nodes.IndexOf(node);
                if (!visited[index])
                {
                    visited[index] = true;
                    queue.Enqueue(node);
                }
            }
        }
    }


    public void Print()
    {
        StringBuilder builder = new StringBuilder();
        builder.AppendLine(string.Format("[Undirected Graph]: {0}", GetHashCode() ));
        for (int i = 0; i < m_nodes.Count; i++)
        {
            builder.AppendLine(string.Format("Node[{0}]: {1} | adjacent: {2}", i, m_nodes[i].GetData().ToString(), m_nodes[i].GetAdjacent().Count));
        }
        Debug.Log(builder.ToString());
    }

    public void PrintBFS(UT_Node<T> startNode)
    {
        List<UT_Node<T>> traversed = BFS(startNode);
        StringBuilder builder = new StringBuilder();

        builder.AppendLine("BFS(): ");
        for (int i = 0; i < traversed.Count; i++)
        {
            builder.AppendLine(string.Format("{0} node: {1}", i, traversed[i].GetData().ToString()));
        }
        Debug.Log(builder.ToString());
    }
}
