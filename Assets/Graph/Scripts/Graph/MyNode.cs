using Com.Bit34Games.Graphs;
using UnityEngine;

public class MyNode : Node<MyEdge>, IGraphNode
{
    //  MEMBERS
    public Vector3       position;
    public NodeComponent component;

    //  METHODS
    public Vector3 GetPosition() { return position; }
}