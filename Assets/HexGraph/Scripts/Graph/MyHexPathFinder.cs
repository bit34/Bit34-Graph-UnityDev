using Com.Bit34Games.Graphs;

public class MyHexPathFinder : PathFinder<MyHexAgent, MyHexNode, MyHexEdge>
{
    public override bool CanAgentAccessEdge(MyHexAgent agent, MyHexEdge edge)
    {
        return agent.owner.GetNode(edge.TargetNodeId).isAccesible;
    }
}