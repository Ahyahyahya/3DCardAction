using System.Collections.Generic;
using UnityEngine;

public enum NodeType
{
    Enemy,
    Elite,
    Rest,
    Tresure,
    Shop,
    Event,
    Boss,
    NodeTypeCount
}

public class Node
{
    public GameObject gameObject;
    public NodeType type;
    public Vector2 index;
    public Vector2 pos;
    public List<Node> previousNodes = new();
    public List<Node> nextNodes = new();
}
