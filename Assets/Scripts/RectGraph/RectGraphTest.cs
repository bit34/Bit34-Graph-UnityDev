using UnityEngine;
using UnityEngine.UI;
using Com.Bit34Games.Graphs;
using Com.Bit34Games.Graphs.Unity;

public class MyRectGraphNode : RectGraphNodeForUnity
{
    public GraphTestNodeComponent component;
    public bool                      isAccesible;
}

public class MyRectGraphEdge : RectGraphEdgeForUnity {}

public class MyRectGraph : RectGraph<RectGraphConfigForUnity, MyRectGraphNode, MyRectGraphEdge>
{
    public MyRectGraph(int columnCount, int rowCount, RectGraphConfigForUnity config) :
     base(config, new GraphAllocator<MyRectGraphNode, MyRectGraphEdge>(), columnCount, rowCount)
    {}
}

public class RectGraphTest : GraphTestBase
{
    //  MEMBERS
    //      For Editor
#pragma warning disable 0649
    [Header("Rectangle")]
    [SerializeField] private GameObject _editGraphPanel;
    [SerializeField] private Text       _columnCountLabel;
    [SerializeField] private Slider     _columnCountSlider;
    [SerializeField] private Text       _rowCountLabel;
    [SerializeField] private Slider     _rowCountSlider;
    [SerializeField] private Toggle     _hasStraghtEdgesToggle;
    [SerializeField] private Toggle     _hasDiagonalEdgesToggle;
#pragma warning restore 0649
    //      Internal
    private bool        _uiInitialized;
    private MyRectGraph _graph;


    //  METHODS

#region Edit Mode

    override protected void EditModeInit()
    {
        _editGraphPanel.SetActive(true);

        if (_uiInitialized==false)
        {
            _uiInitialized = true;
            
            _columnCountSlider.onValueChanged.AddListener((float value)=>{ CreateRectangleGraph(); });
            _rowCountSlider.onValueChanged.AddListener((float value)=>{ CreateRectangleGraph(); });
            _hasStraghtEdgesToggle.onValueChanged.AddListener((bool value)=>{ CreateRectangleGraph(); });
            _hasDiagonalEdgesToggle.onValueChanged.AddListener((bool value)=>{ CreateRectangleGraph(); });
        }

        CreateRectangleGraph();
    }

    override protected void EditModeUpdate() { }
    
    override protected void EditModePostRender()
    {
        GraphTestUtilities.DrawStaticEdges( _graph, StaticEdgeMaterial,  NodeContainer.transform.localToWorldMatrix);
    }

    override protected void EditModeUninit() { }

    private void CreateRectangleGraph()
    {
        DestroyNodeObjects();

        int columnCount = (int)_columnCountSlider.value;
        int rowCount    = (int)_rowCountSlider.value;
        RectGraphConfigForUnity config = new RectGraphConfigForUnity(Vector3.right, 
                                                                     Vector3.up, 
                                                                     false, 
                                                                     _hasStraghtEdgesToggle.isOn, 
                                                                     _hasDiagonalEdgesToggle.isOn);
        _columnCountLabel.text = "Columns : " + columnCount;
        _rowCountLabel.text    = "Rows : "    + rowCount;
        _graph = new MyRectGraph(columnCount, rowCount, config);

        CreateNodeObjects();
    }

    private void CreateNodeObjects()
    {
        NodeContainer.transform.position = -0.5f * _graph.Config.GetNodePosition(_graph.columnCount-1, _graph.rowCount-1);

        for (int c = 0; c < _graph.columnCount; c++)
        {
            for (int r = 0; r < _graph.rowCount; r++)
            {
                MyRectGraphNode        node          = _graph.GetNodeByLocation(c,r);
                GameObject             nodeObject    = Instantiate(NodePrefab, NodeContainer.transform);
                GraphTestNodeComponent nodeComponent = nodeObject.GetComponent<GraphTestNodeComponent>();
                
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

    private void DestroyNodeObjects()
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
        _editGraphPanel.SetActive(false);
        _pathConfig = new GraphPathConfig(true, true, IsEdgeAccesible);
        _path       = null;
    }

    override protected void PathFindUpdatePath()
    {
        _path = new GraphPath();
        if (_graph.FindPath(_pathStartNodeId, _pathTargetNodeId, _pathConfig, _path)==false)
        {
            _path = null;
        }
    }
    
    override protected void PathFindClearPath()
    {
        _path = null;
    }

    override protected void PathFindModePostRender()
    {
        GraphTestUtilities.DrawStaticEdges(_graph,  StaticEdgeMaterial,  NodeContainer.transform.localToWorldMatrix);
        if (_path!=null)
        {
            GraphTestUtilities.DrawPath(_graph, _path, PathEdgeMaterial, NodeContainer.transform.localToWorldMatrix);
        }
    }

    override protected void PathFindModeUninit( ) { }

    override protected GraphTestNodeComponent GetNodeComponent(int nodeId)
    {
        return _graph.GetNode(nodeId).component;
    }
    
#endregion

    private bool IsEdgeAccesible(GraphEdge edge, GraphAgent agent)
    {
        return _graph.GetNode(edge.TargetNodeId).isAccesible;
    }

}
