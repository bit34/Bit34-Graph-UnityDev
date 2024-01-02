using Com.Bit34Games.Graphs;
using UnityEngine;

public class MyRectGraph : RectGraph<MyRectNode, MyRectEdge, MyRectAgent>
{
    //  MEMBERS
    public readonly Vector3 xAxis;
    public readonly Vector3 yAxis;

    //  CONSTRUCTORS
    public MyRectGraph(Vector3 xAxis,
                       Vector3 yAxis,
                       bool    isYAxisUp,
                       bool    hasStraightEdges,
                       bool    hasDiagonalEdges,
                       int     columnCount, 
                       int     rowCount) :
     base(isYAxisUp,
          hasStraightEdges,
          hasDiagonalEdges,
          columnCount, 
          rowCount)
    {
        this.xAxis = xAxis;
        this.yAxis = yAxis;

        CreateNodes();
        CreateEdges();

        IsFixed = true;
    }

    //  METHODS
    override protected MyRectNode AllocateNode()            { return new MyRectNode(); }
    override protected void       FreeNode(MyRectNode node) { }
    override protected MyRectEdge AllocateEdge()            { return new MyRectEdge(); }
    override protected void       FreeEdge(MyRectEdge edge) { }

    override protected float CalculateEdgeWeight(MyRectNode sourceNode, MyRectNode targetNode)
    {
        return (targetNode.position - sourceNode.position).magnitude;
    }

    override protected void InitializeNode(RectNode<MyRectEdge> node, int column, int row)
    {
        MyRectNode castedNode = (MyRectNode)node;
        castedNode.position = GetNodePosition(column, row);
    }

    public Vector3 GetNodePosition(int column, int row)
    {
        return (xAxis * column) + (yAxis * row);
    }
}