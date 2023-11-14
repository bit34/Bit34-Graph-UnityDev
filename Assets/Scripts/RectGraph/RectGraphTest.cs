using UnityEngine;
using UnityEngine.UI;
using Com.Bit34Games.Graphs;


public class RectGraphTest : GraphTestBase
{
    //  MEMBERS
    //      For Editor
#pragma warning disable 0649
    [Header("Rectangle Test")]
    [SerializeField] private RectGraphEditPanel _editPanel;
#pragma warning restore 0649
    //      Private
    private MyRectGraph      _graph;
    private MyRectAgent      _agent;
    private MyRectPathConfig _pathConfig;


    //  METHODS

#region Edit Mode

    override protected void EditModeInit()
    {
        _editPanel.Initialize(CreateRectangleGraph);
        _editPanel.Show(true);

        CreateRectangleGraph();
    }

    override protected void EditModeUpdate() { }

    override protected void EditModeUninit()
    {
        _editPanel.Show(false);
    }

    private void CreateRectangleGraph()
    {
        ClearNodeObjects();

        int               columnCount = (int)_editPanel.ColumnCount;
        int               rowCount    = (int)_editPanel.RowCount;
        MyRectGraphConfig config      = new MyRectGraphConfig(Vector3.right, 
                                                              Vector3.up, 
                                                              false, 
                                                              _editPanel.StraightConnections, 
                                                              _editPanel.DiagonalConnections);
        _graph = new MyRectGraph(columnCount, rowCount, config);

        CreateNodeObjects();
        ClearConnectionObjects();
        CreateConnectionObjects<MyRectGraphConfig, MyRectGraphNode, MyRectGraphConnection>(_graph);

        _agent = new MyRectAgent();
        _graph.AddAgent(_agent);

        _pathConfig = new MyRectPathConfig(IsConnectionAccesible);
    }

    private void CreateNodeObjects()
    {
        NodeContainer.transform.position = -0.5f * _graph.Config.GetNodePosition(_graph.columnCount-1, _graph.rowCount-1);

        for (int c = 0; c < _graph.columnCount; c++)
        {
            for (int r = 0; r < _graph.rowCount; r++)
            {
                MyRectGraphNode node          = _graph.GetNodeByLocation(c,r);
                GameObject      nodeObject    = Instantiate(NodePrefab, NodeContainer.transform);
                NodeComponent   nodeComponent = nodeObject.GetComponent<NodeComponent>();
                
                nodeObject.transform.localPosition = node.position;

                node.component = nodeComponent;

                nodeComponent.Init(node, "["+node.Column+","+node.Row+"]");

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

    private bool IsConnectionAccesible(GraphConnection connection, IAgentPathOwner pathOwner)
    {
        return _graph.GetNode(connection.TargetNodeId).isAccesible;
    }

}
