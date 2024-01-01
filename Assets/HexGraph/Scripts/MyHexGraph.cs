using Com.Bit34Games.Graphs;
using UnityEngine;

public class MyHexGraph : HexGraph<MyHexNode, MyHexEdge>
{
    //  MEMBERS
    public readonly Vector3 xAxis;
    public readonly Vector3 yAxis;
    //      Private
    private readonly Vector3 _indent;

    //  CONSTRUCTORS
    public MyHexGraph(Vector3 xAxis,
                      Vector3 yAxis,
                      bool    isYAxisUp,
                      int     columnCount, 
                      int     rowCount) :
     base(new MyHexGraphAllocator(), 
          isYAxisUp,
          columnCount, 
          rowCount)
    {
        this.xAxis = xAxis;
        this.yAxis = yAxis;
        _indent = xAxis * Mathf.Cos(Mathf.Deg2Rad*60);

        CreateNodes();
        CreateEdges();

        IsFixed = true;
    }

    //  METHODS
    override protected float CalculateEdgeWeight(MyHexNode sourceNode, MyHexNode targetNode)
    {
        return (targetNode.position - sourceNode.position).magnitude;
    }

    override protected void InitializeNode(HexNode<MyHexEdge> node, int column, int row)
    {
        MyHexNode castedNode = (MyHexNode)node;
        castedNode.position = GetNodePosition(column, row);
    }

    public Vector3 GetNodePosition(int column, int row)
    {
        Vector3 position = (xAxis * column) + (yAxis * row);
        if( (row%2)>0)
        {
            position += _indent;
        }
        return position;
    }
}