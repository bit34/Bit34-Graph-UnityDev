using Com.Bit34Games.Graphs;

public class MyPathFinder : PathFinder<MyAgent, MyNode, MyEdge>
{
    //  CONSTRUCTORS
    public MyPathFinder() : base(true, true, true) { }

    //  METHODS
    public override float CalculateHeuristic(MyNode node, MyNode targetNode)
    {
        return (node.position - targetNode.position).magnitude;
    }
    
}