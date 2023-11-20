using System;
using Com.Bit34Games.Graphs;

public class MyRectPathConfig : PathConfig<MyRectNode, MyRectEdge>
{
    public MyRectPathConfig(Func<Edge, IPathOwner, bool> isEdgeAccessible) : base(true, true, isEdgeAccessible){}
}