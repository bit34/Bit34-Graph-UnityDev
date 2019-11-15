using Bit34.Unity.Graph.Base;


public class GraphTestNodeData : BaseTestNodeData { }

public class GraphTestEdgeData : BaseTestEdgeData { }


public class GraphTestNode : GraphNode
{
    public GraphTestNodeData GetData()
    {
        return GetData<GraphTestNodeData>();
    }
}

public class GraphTestEdge : GraphEdge
{
    public GraphTestEdgeData GetData()
    {
        return GetData<GraphTestEdgeData>();
    }
}

public class GraphTestGraphAllocator : GraphDefaultAllocator<GraphTestNode, GraphTestEdge, GraphTestNodeData, GraphTestEdgeData>{}

public class GraphTestGraph : Graph<GraphTestNode, GraphTestEdge>
{
    public GraphTestGraph() :
     base(new GraphTestGraphAllocator())
    {}
}
