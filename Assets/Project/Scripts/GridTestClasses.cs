using Bit34.Unity.Graph.Base;
using Bit34.Unity.Graph.Grid;
using Bit34.Unity.Graph.Rectangle;


public class GridTestNodeData : BaseTestNodeData
{
    public bool IsAccesible;
}

public class GridTestEdgeData : BaseTestEdgeData { }


public class GridTestNode : GridGraphNode
{
    public GridTestNodeData GetData()
    {
        return GetData<GridTestNodeData>();
    }
}

public class GridTestEdge : GraphEdge
{
    public GridTestEdgeData GetData()
    {
        return GetData<GridTestEdgeData>();
    }
}




public class RectangleTestNode : GridTestNode
{
    new public GridTestNodeData GetData()
    {
        return GetData<GridTestNodeData>();
    }
}

public class RectangleTestEdge : GridTestEdge
{
    new public GridTestEdgeData GetData()
    {
        return GetData<GridTestEdgeData>();
    }
}

public class RectangleTestGraphAllocator : GraphDefaultAllocator<RectangleTestNode, RectangleTestEdge, GridTestNodeData, GridTestEdgeData>{}

public class RectangleTestGraph : RectangleGraph<RectangleTestNode, RectangleTestEdge>
{
    public RectangleTestGraph(int columnCount, int rowCount, RectangleGraphConfig config) :
     base(columnCount, rowCount, config, new RectangleTestGraphAllocator())
    {}
}

