using Com.Bit34Games.Graphs;
using UnityEngine;

public class MyRectNode : RectNode<MyRectEdge>, IGraphNode
{
    //  MEMBERS
    public Vector3       position;
    public NodeComponent component;
    public bool          isAccesible;

    //  METHODS
    public Vector3 GetPosition() { return position; }
}