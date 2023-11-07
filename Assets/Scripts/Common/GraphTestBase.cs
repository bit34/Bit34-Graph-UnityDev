using System.Collections.Generic;
using Com.Bit34Games.Graphs;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public abstract class GraphTestBase : MonoBehaviour
{
    //  MEMBERS
    protected GameObject NodeContainer       { get { return _nodeContainer;       } }
    protected GameObject NodePrefab          { get { return _nodePrefab;          } }
    protected Material   StaticEdgeMaterial  { get { return _staticEdgeMaterial;  } }
    protected Material   DynamicEdgeMaterial { get { return _dynamicEdgeMaterial; } }
    protected Material   PathEdgeMaterial    { get { return _pathEdgeMaterial;    } }
    //      For Editor
#pragma warning disable 0649
    [SerializeField] private GameObject   _nodeContainer;
    [SerializeField] private GameObject   _nodePrefab;
    [SerializeField] private Transform    _connectionContainer;
    [SerializeField] private GameObject   _connectionPrefab;
    [SerializeField] private Transform    _pathContainer;
    [SerializeField] private Material     _staticEdgeMaterial;
    [SerializeField] private Material     _dynamicEdgeMaterial;
    [SerializeField] private Material     _pathEdgeMaterial;
    [SerializeField] private GameObject   _agentPrefab;
    [Header("UI")]
    [SerializeField] private Button       _changeModeButton;
    [SerializeField] private Text         _activeModeLabel;
#pragma warning restore 0649
    protected Path      Path{ get; private set; }
    //      Internal
    private   GraphTestModes _mode;
    private   AgentComponent _agentComponent;
    protected int            _pathStartNodeId;
    protected int            _pathTargetNodeId;


    //  METHODS
    //      Unity callbacks
    private void Start()
    {
        _changeModeButton.onClick.AddListener(()=>
            {
                int            nextModeValue = (1+(int)_mode)%((int)GraphTestModes.Max_Modes);
                GraphTestModes nextMode      = (GraphTestModes)nextModeValue;
                SetMode(nextMode);
            });
        
        EditModeInit();
        SetMode(GraphTestModes.Edit);
    }

    private void Update()
    {
        switch(_mode)
        {
            case GraphTestModes.Edit:     EditModeUpdate();     break;
            case GraphTestModes.PathFind: PathFindModeUpdate(); break;
        }
    }

    private void SetMode(GraphTestModes newMode)
    {
        switch(_mode)
        {
            case GraphTestModes.Edit:     EditModeUninit();     break;
            case GraphTestModes.PathFind: PathFindModeUninit(); ClearPathAndSelection(); break;
        }

        _mode = newMode;
        _activeModeLabel.text = _mode.ToString() + " mode";

        switch(_mode)
        {
            case GraphTestModes.Edit:     EditModeInit();     break;
            case GraphTestModes.PathFind: PathFindModeInit(); break;
        }
    }

    protected void SetPath(Path path)
    {
        Path = path;

        ClearPathObjects();

        if (Path!= null)
        {
            if (_agentComponent == null)
            {
                GameObject agentObject = Instantiate(_agentPrefab, _pathContainer.transform);
                _agentComponent        = agentObject.GetComponent<AgentComponent>();
                _agentComponent.SetTest(this);
            }
            
            _agentComponent.SetPath(Path);

            CreatePathObjects();
        }
        else
        {
            if (_agentComponent != null)
            {
                Destroy(_agentComponent.gameObject);
            }
        }
    }

    abstract protected void EditModeInit();
    abstract protected void EditModeUpdate();
    abstract protected void EditModeUninit();

    abstract protected void PathFindModeInit();
    abstract protected void PathFindUpdatePath();
    abstract protected void PathFindClearPath();
    abstract protected void PathFindModeUninit();
    
    abstract public NodeComponent GetNodeComponent(int nodeId);

    private void PathFindModeUpdate()
    {
        //  Clicked on scene
        if (Input.GetMouseButtonDown(0) && EventSystem.current.IsPointerOverGameObject() == false)
        {
            Ray        ray          = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            bool       hitSomething = Physics.Raycast(ray, out hit);

            //  input intersects an object
            if(hitSomething)
            {
                //  Object is a node
                NodeComponent nodeComponent = hit.collider.GetComponent<NodeComponent>();
                if(nodeComponent!=null)
                {
                    //  Nothing selected before
                    if(_pathStartNodeId==-1 && _pathTargetNodeId==-1)
                    {
                        SelectPathStart(nodeComponent);
                    }
                    else
                    //  Start selected
                    if(_pathStartNodeId!=-1 && _pathTargetNodeId==-1)
                    {
                        //  New selection is same as start
                        if(_pathStartNodeId == nodeComponent.NodeId)
                        {
                            ClearPathAndSelection();
                        }
                        else
                        {
                            SelectPathTarget(nodeComponent);
                            PathFindUpdatePath();
                        }
                    }
                    else
                    //  Start and target selected
                    if(_pathStartNodeId!=-1 && _pathTargetNodeId!=-1)
                    {
                        SelectPathStart(nodeComponent);
                    }
                }
            }
            //  Input is not intersecting an object
            else
            {
                ClearPathAndSelection();
            }
        }
    }

    private void ClearPathAndSelection()
    {
        if(_pathStartNodeId!=-1)
        {
            GetNodeComponent(_pathStartNodeId).SetAsRegular();
            _pathStartNodeId = -1;
        }

        if(_pathTargetNodeId!=-1)
        {
            GetNodeComponent(_pathTargetNodeId).SetAsRegular();
            _pathTargetNodeId = -1;
        }

        PathFindClearPath();
    }

    private void SelectPathStart(NodeComponent nodeComponent)
    {
        ClearPathAndSelection();

        _pathStartNodeId = nodeComponent.NodeId;
        nodeComponent.SetAsStart();
    }

    private void SelectPathTarget(NodeComponent nodeComponent)
    {
        _pathTargetNodeId = nodeComponent.NodeId;
        nodeComponent.SetAsTarget();
    }

    private void ClearPathObjects()
    {
        while (_pathContainer.childCount>0)
        {
            DestroyImmediate(_pathContainer.GetChild(0).gameObject);
        }
    }

    private void CreatePathObjects()
    {
        for (int c = 0; c < Path.ConnectionCount; c++)
        {
            GraphConnection connection = Path.Getconnection(c);
            CreateConnectionObject(connection, _pathContainer, _pathEdgeMaterial, true);
        }
    }

    protected void ClearConnectionObjects()
    {
        while (_connectionContainer.childCount>0)
        {
            DestroyImmediate(_connectionContainer.GetChild(0).gameObject);
        }
    }

    protected void CreateConnectionObjects<TConfig, TNode, TConnection>(Graph<TConfig, TNode, TConnection> graph)
        where TConfig : GraphConfig<TNode>
        where TNode : GraphNode, IGraphNode
        where TConnection : GraphConnection
    {

        IEnumerator<TNode> nodes = graph.GetNodeEnumerator();
        while (nodes.MoveNext() == true)
        {
            TNode node = nodes.Current;

            IEnumerator<GraphConnection>connections = node.GetDynamicConnectionEnumerator();
            while(connections.MoveNext())
            {
                GraphConnection connection = connections.Current;
                if (connection.SourceNodeId < connection.TargetNodeId)
                {
                    CreateConnectionObject(connection, _connectionContainer, _dynamicEdgeMaterial, false);
                }
            }

            for (int i = 0; i < node.StaticConnectionCount; i++)
            {
                GraphConnection connection = node.GetStaticConnection(i);
                if (connection !=null && connection.SourceNodeId < connection.TargetNodeId)
                {
                    CreateConnectionObject(connection, _connectionContainer, _staticEdgeMaterial, false);
                }
            }
        }
    }

    private void CreateConnectionObject(GraphConnection connection, Transform container, Material material, bool isThick)
    {
        GameObject             connectionObject    = Instantiate(_connectionPrefab, container);
        ConnectionComponent    connectionComponent = connectionObject.GetComponent<ConnectionComponent>();
        NodeComponent          startNodeComponent  = GetNodeComponent(connection.SourceNodeId);
        NodeComponent          targetNodeComponent = GetNodeComponent(connection.TargetNodeId);
        connectionComponent.Setup(startNodeComponent.transform.position, targetNodeComponent.transform.position, material, isThick);
    }
}
