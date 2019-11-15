using UnityEngine;
using UnityEngine.UI;
using Bit34.Unity.Graph.Rectangle;
using Bit34.Unity.Graph.Utilities;
using Bit34.Unity.Graph.Base;
using Bit34.Unity.Graph;

public class GridTest : BaseTest
{
    //  MEMBERS
    //      For Editor
#pragma warning disable 0649
    [Header("Rectangle")]
    [SerializeField] private Button     _RectangleGraphButton;
    [SerializeField] private GameObject _RectangleGraphPanel;
    [SerializeField] private Text       _RectangleGraphColumnCountLabel;
    [SerializeField] private Slider     _RectangleGraphColumnCountSlider;
    [SerializeField] private Text       _RectangleGraphRowCountLabel;
    [SerializeField] private Slider     _RectangleGraphRowCountSlider;
    [SerializeField] private Toggle     _RectangleGraphStraghtEdgesToggle;
    [SerializeField] private Toggle     _RectangleGraphDiagonalEdgesToggle;
#pragma warning restore 0649
    //      Internal
    private bool               _UIInitialized;
    private RectangleTestGraph _RectangleGraph;
    private GraphPath          _Path;
    private GraphPathConfig    _PathConfig;


    //  METHODS

#region Edit Mode

    override protected void EditModeInit()
    {
        _RectangleGraphPanel.SetActive(true);
        _RectangleGraphButton.gameObject.SetActive(true);

        if (_UIInitialized==false)
        {
            _UIInitialized = true;
            
            _RectangleGraphColumnCountSlider.onValueChanged.AddListener((float value)=>{ CreateRectangleGraph(); });
            _RectangleGraphRowCountSlider.onValueChanged.AddListener((float value)=>{ CreateRectangleGraph(); });
            _RectangleGraphStraghtEdgesToggle.onValueChanged.AddListener((bool value)=>{ CreateRectangleGraph(); });
            _RectangleGraphDiagonalEdgesToggle.onValueChanged.AddListener((bool value)=>{ CreateRectangleGraph(); });
        }

        CreateRectangleGraph();
    }

    override protected void EditModeUpdate() { }
    
    override protected void EditModePostRender()
    {
        GraphUtilities.DrawStaticEdges( _RectangleGraph, StaticEdgeMaterial,  NodeContainer.transform.localToWorldMatrix);
    }

    override protected void EditModeUninit() { }

    private void CreateRectangleGraph()
    {
        DestroyNodeObjects();

        int columnCount = (int)_RectangleGraphColumnCountSlider.value;
        int rowCount    = (int)_RectangleGraphRowCountSlider.value;
        RectangleGraphConfig config = new RectangleGraphConfig(
            Vector3.right, Vector3.up, 
            false, 
            _RectangleGraphStraghtEdgesToggle.isOn, _RectangleGraphDiagonalEdgesToggle.isOn);
        _RectangleGraphColumnCountLabel.text = "Columns : " + columnCount;
        _RectangleGraphRowCountLabel.text    = "Rows : "    + rowCount;
        _RectangleGraph = new RectangleTestGraph(columnCount, rowCount, config);

        CreateNodeObjects();
    }

    private void CreateNodeObjects()
    {
        NodeContainer.transform.position = -0.5f * _RectangleGraph.Config.GetNodePosition(_RectangleGraph.ColumnCount-1, _RectangleGraph.RowCount-1);

        for (int c = 0; c < _RectangleGraph.ColumnCount; c++)
        {
            for (int r = 0; r < _RectangleGraph.RowCount; r++)
            {
                RectangleTestNode     node          = _RectangleGraph.GetNodeByLocation(c,r);
                GameObject            nodeObject    = Instantiate(NodePrefab, NodeContainer.transform);
                GridTestNodeComponent nodeComponent = nodeObject.GetComponent<GridTestNodeComponent>();
                
                nodeObject.transform.localPosition = node.Position;
                node.GetData().SceneObject = nodeObject;
                nodeComponent.Init(node);

                Nodes.Add(node.Id, nodeComponent);

                if (nodeComponent.IsIntersectingObstacle())
                {
                    node.GetData().IsAccesible = false;
                    nodeComponent.SetAsNotAccessible();
                }
                else
                {
                    node.GetData().IsAccesible = true;
                    nodeComponent.SetAsRegular();
                }
            }
        }

    }

    private void DestroyNodeObjects()
    {
        while(NodeContainer.transform.childCount>0)
        {
            DestroyImmediate(NodeContainer.transform.GetChild(0).gameObject);
        }
        Nodes.Clear();
    }

#endregion


#region Path finding

    override protected void PathFindModeInit()
    {
        _RectangleGraphPanel.SetActive(false);
        _RectangleGraphButton.gameObject.SetActive(false);
        _PathConfig = new GraphPathConfig(true, true, IsEdgeAccesible);
        _Path = null;
    }

    override protected void PathFindUpdatePath()
    {
        _Path = new GraphPath();
        if(_RectangleGraph.FindPath(_PathStartNodeId, _PathTargetNodeId, _PathConfig, _Path)==false)
        {
            _Path = null;
        }
    }
    
    override protected void PathFindClearPath()
    {
        _Path = null;
    }

    override protected void PathFindModePostRender()
    {
        GraphUtilities.DrawStaticEdges(_RectangleGraph,  StaticEdgeMaterial,  NodeContainer.transform.localToWorldMatrix);
        if(_Path!=null)
        {
            GraphUtilities.DrawPath(_RectangleGraph, _Path, PathEdgeMaterial, NodeContainer.transform.localToWorldMatrix);
        }
    }

    override protected void PathFindModeUninit( ) { }


    private bool IsEdgeAccesible(GraphEdge edge, GraphAgent agent)
    {
        return _RectangleGraph.GetNodeById(edge.TargetNodeId).GetData().IsAccesible;
    }
    
#endregion

}
