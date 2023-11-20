using UnityEngine;
using UnityEngine.UI;
using Com.Bit34Games.Graphs;


public class HexGraphTest : GraphTestBase
{
    //  MEMBERS
    //      For Editor
#pragma warning disable 0649
    [Header("Hexagon Test")]
    [SerializeField] private HexGraphEditPanel _editPanel;
#pragma warning restore 0649
    //      Private
    private MyHexGraph      _graph;
    private MyHexAgent      _agent;
    private MyHexPathConfig _pathConfig;


    //  METHODS

#region Edit Mode

    override protected void EditModeInit()
    {
        _editPanel.Initialize(CreateHexGraph);
        _editPanel.Show(true);

        CreateHexGraph();
    }

    override protected void EditModeUpdate() { }

    override protected void EditModeUninit()
    {
        _editPanel.Show(false);
    }

    private void CreateHexGraph()
    {
        ClearNodeObjects();

        int              columnCount = _editPanel.ColumnCount;
        int              rowCount    = _editPanel.RowCount;
        MyHexGraphConfig config      = new MyHexGraphConfig(Vector3.right, 
                                                              Vector3.up, 
                                                              false);
        _graph = new MyHexGraph(columnCount, rowCount, config);

        CreateNodeObjects();
        ClearEdgeObjects();
        CreateEdgeObjects(_graph);

        _agent = new MyHexAgent();
        _graph.AddAgent(_agent);

        _pathConfig = new MyHexPathConfig(IsEdgeAccesible);
    }

    private void CreateNodeObjects()
    {
        NodeContainer.transform.position = -0.5f * _graph.Config.GetNodePosition(_graph.columnCount-1, _graph.rowCount-1);

        for (int c = 0; c < _graph.columnCount; c++)
        {
            for (int r = 0; r < _graph.rowCount; r++)
            {
                MyHexNode     node          = _graph.GetNodeByLocation(c,r);
                GameObject    nodeObject    = Instantiate(NodePrefab, NodeContainer.transform);
                NodeComponent nodeComponent = nodeObject.GetComponent<NodeComponent>();
                
                nodeObject.transform.localPosition = node.position;

                node.component = nodeComponent;

                nodeComponent.Init(node.Id, "["+node.Column+","+node.Row+"]");

                if (nodeComponent.IsIntersectingObstacle())
                {
                    node.isAccesible = false;
                    nodeComponent.SetAsNotAccessible();
                }
                else
                {
                    node.isAccesible = true;
                    nodeComponent.SetAsRegular();
                }
            }
        }

    }

    private void ClearNodeObjects()
    {
        while (NodeContainer.transform.childCount>0)
        {
            DestroyImmediate(NodeContainer.transform.GetChild(0).gameObject);
        }
    }

#endregion


#region Path finding

    override protected void PathFindModeInit()
    {
        SetPath(null);
    }

    override protected void PathFindUpdatePath()
    {
        SetPath(_agent.FindPath(_pathStartNodeId, _pathTargetNodeId, _pathConfig));
    }
    
    override protected void PathFindClearPath()
    {
        SetPath(null);
    }

    override protected void PathFindModeUninit( ) { }

    override public NodeComponent GetNodeComponent(int nodeId)
    {
        return _graph.GetNode(nodeId).component;
    }
    
#endregion

    private bool IsEdgeAccesible(Edge edge, IPathOwner pathOwner)
    {
        return _graph.GetNode(edge.TargetNodeId).isAccesible;
    }

}
