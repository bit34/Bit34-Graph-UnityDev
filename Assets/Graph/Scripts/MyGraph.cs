using Com.Bit34Games.Graphs;


public class MyGraph : Graph<MyGraphConfig, MyNode, MyEdge>
{
    //  CONSTRUCTORS
    public MyGraph():base(new MyGraphConfig(), new MyGraphAllocator()){}

    //  METHODS
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