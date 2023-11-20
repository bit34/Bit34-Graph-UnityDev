using Com.Bit34Games.Graphs;
using UnityEngine;

public class MyHexGraphConfig : HexGraphConfig<MyHexNode, MyHexEdge>
{
    //  MEMBERS
    public readonly Vector3 xAxis;
    public readonly Vector3 yAxis;
    //      Private
    private readonly Vector3 _indent;


    //  CONSTRUCTORS
    public MyHexGraphConfig(Vector3 xAxis,
                            Vector3 yAxis,
                            bool    isYAxisUp = false) : 
        base(isYAxisUp)
    {
        this.xAxis = xAxis;
        this.yAxis = yAxis;
        _indent = xAxis * Mathf.Cos(Mathf.Deg2Rad*60);
    }

    override public float CalculateEdgeWeight(MyHexNode sourceNode, MyHexNode targetNode)
    {
        return (targetNode.position - sourceNode.position).magnitude;
    }

    override public void InitializeNode(HexNode<MyHexEdge> node, int column, int row)
    {
        MyHexNode castedNode = (MyHexNode)node;
        castedNode.position = GetNodePosition(column, row);
    }

    public Vector3 GetNodePosition(int column, int row)
    {
        Vector3 position = (xAxis * column) + (yAxis * row);
        if( (row%2)>0/* != indentFirstRow*/)
        {
            position += _indent;
        }
        return position;
    }
}