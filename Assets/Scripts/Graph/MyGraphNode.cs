using Com.Bit34Games.Graphs;
using UnityEngine;

public class MyGraphNode : GraphNode, IGraphNodeForUnity
{
    //  MEMBERS
    public Vector3                position;
    public GraphTestNodeComponent component;

    //  METHODS
    public Vector3 GetPosition() { return position; }
}