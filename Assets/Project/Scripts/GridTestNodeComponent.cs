using Bit34.Unity.Graph.Grid;


public class GridTestNodeComponent : BaseTestNodeComponent
{
    //  MEMBERS
    override public int NodeId { get{ return Node.Id; } }
    public GridGraphNode Node { get; private set; }


    //  METHODS
    public void Init(GridGraphNode node)
    {
        Node = node;
        gameObject.name = "Node" + node.Id;

        SetInfoText("["+Node.Column+","+Node.Row+"]"+Node.Id);
    }

}
