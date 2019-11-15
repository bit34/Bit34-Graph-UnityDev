using Bit34.Unity.Graph.Base;


public class GraphTestNodeComponent : BaseTestNodeComponent
{
    //  MEMBERS
    override public int NodeId { get{ return Node.Id; } }
    public GraphNode Node { get; private set; }


    //  METHODS
    public void Init(GraphNode node)
    {
        Node = node;
        gameObject.name = "Node" + node.Id;

        SetInfoText( Node.Id.ToString() );
    }

}
