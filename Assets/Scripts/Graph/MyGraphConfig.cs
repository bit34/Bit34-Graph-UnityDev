using Com.Bit34Games.Graphs;

public class MyGraphConfig : GraphConfig<MyNode, MyEdge>
{
    //  CONSTRUCTOR
    public MyGraphConfig() : 
        base(0)
    {}

    //  METHODS
    override public float CalculateEdgeWeight(MyNode sourceNode, MyNode targetNode)
    {
        return (targetNode.position - sourceNode.position).magnitude;
    }
}