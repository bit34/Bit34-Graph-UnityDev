using Com.Bit34Games.Graphs;

public class MyRectPathFinder : PathFinder<MyRectAgent, MyRectNode, MyRectEdge>
{
    //  CONSTRUCTORS
    public MyRectPathFinder() : base(true, true, true) { }

    //  METHODS
    public override bool IsEdgeAccessible(MyRectAgent agent, MyRectEdge edge)
    {
        return GetNode(agent, edge.TargetNodeId).isAccesible;
    }
    
    public override float CalculateHeuristic(MyRectNode node, MyRectNode targetNode)
    {
        return (node.position - targetNode.position).magnitude;
    }

}