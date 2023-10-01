using Com.Bit34Games.Graphs;
using UnityEngine;

public class MyRectGraphNode : RectGraphNode, IGraphNodeForUnity
{
    //  MEMBERS
    public Vector3                position;
    public GraphTestNodeComponent component;
    public bool                   isAccesible;

    //  METHODS
    public Vector3 GetPosition() { return position; }
}