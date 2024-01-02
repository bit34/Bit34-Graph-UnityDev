using Com.Bit34Games.Graphs;


public class MyGraph : Graph<MyNode, MyEdge, MyAgent>
{
    //  CONSTRUCTORS
    public MyGraph():base(new MyGraphAllocator(), 0){}

    //  METHODS
    override protected float CalculateEdgeWeight(MyNode sourceNode, MyNode targetNode)
    {
        return (targetNode.position - sourceNode.position).magnitude;
    }

    public MyNode CreateNode()
    {
        return AddNode();
    }

    public void DeleteNode(int nodeId)
    {
        RemoveNode(nodeId);
    }

    public void ConnectNodes(int sourceNodeId, int targetNodeId)
    {
        AddEdge(sourceNodeId, targetNodeId);
    }
}