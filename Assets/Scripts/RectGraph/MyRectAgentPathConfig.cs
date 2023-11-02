using System;
using Com.Bit34Games.Graphs;

public class MyRectAgentPathConfig : AgentPathConfig<MyRectGraphNode, MyRectGraphConnection>
{
    public MyRectAgentPathConfig(Func<GraphConnection, IAgentPathOwner, bool> isConnectionAccessible) : base(true, true, isConnectionAccessible)
    {}
}