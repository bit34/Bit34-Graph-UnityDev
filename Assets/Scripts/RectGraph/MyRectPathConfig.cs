using System;
using Com.Bit34Games.Graphs;

public class MyRectPathConfig : PathConfig<MyRectGraphNode, MyRectGraphConnection>
{
    public MyRectPathConfig(Func<GraphConnection, IAgentPathOwner, bool> isConnectionAccessible) : base(true, true, isConnectionAccessible)
    {}
}