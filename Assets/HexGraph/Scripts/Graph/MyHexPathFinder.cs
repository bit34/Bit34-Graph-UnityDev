using Com.Bit34Games.Graphs;

public class MyHexPathFinder : PathFinder<MyHexAgent, MyHexNode, MyHexEdge>
{
    public override bool IsEdgeAccessible(MyHexAgent agent, MyHexEdge edge)
    {
        return GetNode(agent, edge.TargetNodeId).isAccesible;
    }
    
    public override float CalculateHeuristic(MyHexNode node, MyHexNode targetNode)
    {
        return (node.position - targetNode.position).magnitude;
    }

}