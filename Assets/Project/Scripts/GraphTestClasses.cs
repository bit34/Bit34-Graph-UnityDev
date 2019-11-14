using Bit34.Unity.Graph.Base;
using UnityEngine;

public class TestNodeData
{
    public int        Value;
    public GameObject SceneObject;
}

public class TestEdgeData{}


public class TestNode : GraphNode
{
    public TestNodeData GetData()
    {
        return GetData<TestNodeData>();
    }
}

public class TestEdge : GraphEdge
{
    public TestEdgeData GetData()
    {
        return GetData<TestEdgeData>();
    }
}

public class TestGraphAllocator : GraphDefaultAllocator<TestNode, TestEdge, TestNodeData, TestEdgeData>{}

public class TestGraph : Graph<TestNode, TestEdge>
{
    public TestGraph() :
     base(new TestGraphAllocator())
    {}
}
