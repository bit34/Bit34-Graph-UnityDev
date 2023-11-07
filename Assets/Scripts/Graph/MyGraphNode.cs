using Com.Bit34Games.Graphs;
using UnityEngine;

public class MyGraphNode : GraphNode, IGraphNode
{
    //  MEMBERS
    public Vector3       position;
    public NodeComponent component;

    //  METHODS
    public Vector3 GetPosition() { return position; }
}