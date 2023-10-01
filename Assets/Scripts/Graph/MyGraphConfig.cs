using Com.Bit34Games.Graphs;

public class MyGraphConfig : GraphConfig<MyGraphNode>
{
    //  CONSTRUCTOR
    public MyGraphConfig() : 
        base(0)
    {}

    //  METHODS
    override public float CalculateConnectionWeight(MyGraphNode sourceNode, MyGraphNode targetNode)
    {
        return (targetNode.position - sourceNode.position).magnitude;
    }
}