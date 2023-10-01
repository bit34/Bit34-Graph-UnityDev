using Com.Bit34Games.Graphs;


public class MyGraph : Graph<MyGraphConfig, MyGraphNode, MyGraphConnection>
{
    //  CONSTRUCTORS
    public MyGraph():base(new MyGraphConfig(), new GraphAllocator<MyGraphNode, MyGraphConnection>()){}

    //  METHODS
    public MyGraphNode CreateNode()
    {
        return AddNode();
    }

    public void DeleteNode(int nodeId)
    {
        RemoveNode(nodeId);
    }

    public void ConnectNodes(int sourceNodeId, int targetNodeId)
    {
        AddConnection(sourceNodeId, targetNodeId);
    }
}