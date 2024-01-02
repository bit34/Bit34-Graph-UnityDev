using Com.Bit34Games.Graphs;
using UnityEngine;

public class MyHexNode : HexNode<MyHexEdge>, IGraphNode
{
    //  MEMBERS
    public Vector3       position;
    public NodeComponent component;
    public bool          isAccesible;

    //  METHODS
    public Vector3 GetPosition() { return position; }
}