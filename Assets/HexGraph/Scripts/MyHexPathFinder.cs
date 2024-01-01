using Com.Bit34Games.Graphs;

public class MyHexPathFinder : PathFinder<MyHexAgent, MyHexNode, MyHexEdge>
{

    public override bool CanAgentAccessEdge(MyHexAgent agent, MyHexEdge edge)
    {
        return GetNode(agent, edge.TargetNodeId).isAccesible;
    }

}