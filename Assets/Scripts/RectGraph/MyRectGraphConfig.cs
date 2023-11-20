using Com.Bit34Games.Graphs;
using UnityEngine;

public class MyRectGraphConfig : RectGraphConfig<MyRectNode, MyRectEdge>
{
    //  MEMBERS
    public readonly Vector3 xAxis;
    public readonly Vector3 yAxis;


    //  CONSTRUCTORS
    public MyRectGraphConfig(Vector3 xAxis,
                             Vector3 yAxis,
                             bool    isYAxisUp = false,
                             bool    hasStraightEdges = true,
                             bool    hasDiagonalEdges = false) : 
        base(isYAxisUp,
                hasStraightEdges,
                hasDiagonalEdges)
    {
        this.xAxis = xAxis;
        this.yAxis = yAxis;
    }

    override public float CalculateEdgeWeight(MyRectNode sourceNode, MyRectNode targetNode)
    {
        return (targetNode.position - sourceNode.position).magnitude;
    }

    override public void InitializeNode(RectNode<MyRectEdge> node, int column, int row)
    {
        MyRectNode castedNode = (MyRectNode)node;
        castedNode.position = GetNodePosition(column, row);
    }

    public Vector3 GetNodePosition(int column, int row)
    {
        return (xAxis * column) + (yAxis * row);
    }
}