using Com.Bit34Games.Graphs;

public class MyRectPathFinder : PathFinder<MyRectAgent, MyRectNode, MyRectEdge>
{

    public override bool CanAgentAccessEdge(MyRectAgent agent, MyRectEdge edge)
    {
        return GetNode(agent, edge.TargetNodeId).isAccesible;
    }

}