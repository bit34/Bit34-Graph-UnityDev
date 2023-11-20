using System;
using Com.Bit34Games.Graphs;

public class MyHexPathConfig : PathConfig<MyHexNode, MyHexEdge>
{
    public MyHexPathConfig(Func<Edge, IPathOwner, bool> isEdgeAccessible) : base(true, true, isEdgeAccessible){}
}