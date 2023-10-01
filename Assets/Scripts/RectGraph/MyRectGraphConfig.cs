using Com.Bit34Games.Graphs;
using UnityEngine;

public class MyRectGraphConfig : RectGraphConfig<MyRectGraphNode>
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

    override public float CalculateConnectionWeight(MyRectGraphNode sourceNode, MyRectGraphNode targetNode)
    {
        return (targetNode.position - sourceNode.position).magnitude;
    }

    override public void InitializeNode(RectGraphNode node, int column, int row)
    {
        MyRectGraphNode castedNode = (MyRectGraphNode)node;
        castedNode.position = GetNodePosition(column, row);
    }

    public Vector3 GetNodePosition(int column, int row)
    {
        return (xAxis * column) + (yAxis * row);
    }
}