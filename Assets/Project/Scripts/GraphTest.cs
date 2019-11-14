using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Bit34.Unity.Graph.Utilities;
using Bit34.Unity.Graph.Base;


public class GraphTest : GraphTestBase
{
    //  MEMBERS
    //      For Editor
#pragma warning disable 0649
    [SerializeField] private Button     _ChangeModeButton;
    [SerializeField] private Text       _ActiveModeLabel;
    [SerializeField] private GameObject _NodeContainer;
    [SerializeField] private GameObject _NodePrefab;
    [SerializeField] private Material   _StaticEdgeMaterial;
    [SerializeField] private Material   _DynamicEdgeMaterial;
    [SerializeField] private Material   _PathEdgeMaterial;
#pragma warning restore 0649
    //      Internal
    private GraphTestModes _Mode;
    private TestGraph      _Graph;

    private int _PathStartNodeId;
    private int _PathTargetNodeId;
    private GraphPath _Path;
    private GraphPathConfig _PathConfig;

    //  METHODS
    //      Unity callbacks
    void Start()
    {
        _ChangeModeButton.onClick.AddListener(()=>
            {
                int nextModeValue = (1+(int)_Mode)%((int)GraphTestModes.Max_Modes);
                GraphTestModes nextMode = (GraphTestModes)nextModeValue;
                SetMode(nextMode);
            });
        
        _Graph = new TestGraph();
        
        EditModeInit();
        PathFindModeInit();

        SetMode(GraphTestModes.Edit);
    }

    private void Update()
    {
        switch(_Mode)
        {
            case GraphTestModes.Edit:     EditModeUpdate();     break;
            case GraphTestModes.PathFind: PathFindModeUpdate(); break;
        }
    }

    private void OnPostRender()
    {
        GraphUtilities.DrawStaticEdges(_Graph, _StaticEdgeMaterial);
        GraphUtilities.DrawDynamicEdges(_Graph, _DynamicEdgeMaterial);
        if(_Path!=null)
        {
            GraphUtilities.DrawPath(_Graph, _Path, _PathEdgeMaterial);
        }
    }

    private void SetMode(GraphTestModes newMode)
    {
        switch(_Mode)
        {
            case GraphTestModes.Edit:     EditModeUninit();     break;
            case GraphTestModes.PathFind: PathFindModeUninit(); break;
        }

        _Mode = newMode;
        _ActiveModeLabel.text = _Mode.ToString() + " mode";
    }

#region Edit

    private void EditModeInit()
    {
        for (int i = 0; i < _NodeContainer.transform.childCount; i++)
        {                              
            GameObject          nodeObject = _NodeContainer.transform.GetChild(i).gameObject;
            GraphTestNodeComponent nodeScript = nodeObject.GetComponent<GraphTestNodeComponent>();
            TestNode       node       = _Graph.CreateNode(0);
            node.GetData().Value = node.Id;
            node.GetData().SceneObject = nodeObject;
            node.Position = nodeScript.transform.position;
            nodeScript.Init(node);

            ConnectNodeToOthers(node);
        }
    }

    private void EditModeUpdate()
    {
        if (Input.GetMouseButtonDown(0) && EventSystem.current.IsPointerOverGameObject() == false)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            bool hitSomething = Physics.Raycast(ray, out hit);

            if(hitSomething)
            {
                GraphTestNodeComponent nodeScript = hit.collider.GetComponent<GraphTestNodeComponent>();
                if(nodeScript!=null)
                {
                    _Graph.RemoveNode(nodeScript.Node.Id);
                    Destroy(nodeScript.gameObject);
                }
            }
            else
            {
                GameObject          nodeObject = Instantiate(_NodePrefab, _NodeContainer.transform);
                GraphTestNodeComponent nodeScript = nodeObject.GetComponent<GraphTestNodeComponent>();
                Vector3             nodePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition+new Vector3(0,0,-Camera.main.transform.position.z));
                TestNode            node       = _Graph.CreateNode(0);
                node.GetData().Value = node.Id;
                node.GetData().SceneObject = nodeObject;
                node.Position = nodeScript.transform.position = nodePosition;
                nodeScript.Init(node);

                ConnectNodeToOthers(node);
            }
        }
    }

    private void EditModeUninit()
    {}

    private void ConnectNodeToOthers(TestNode node)
    {
        IEnumerator<TestNode> nodes =_Graph.GetNodeEnumerator();

        while(nodes.MoveNext())
        {
            TestNode node2 = nodes.Current;

            //  Skip self
            if(node2 == node)
            {
                continue;
            }

            //  skip if already connected
            if(node2.HasDynamicEdgeTo(node.Id))
            {
                continue;
            }

            Vector3 direction = node2.Position-node.Position;
            float maxDistance = direction.magnitude;
            direction.Normalize();
            Ray ray = new Ray(node.Position, direction);
            RaycastHit hitInfo;
            bool hitSomething = Physics.Raycast(ray, out hitInfo, maxDistance, LayerMasks.Obstacles);
            if(hitSomething==false)
            {
                _Graph.CreateEdge(node, node2);
            }
        }
    }

#endregion

#region Path finding

    private void PathFindModeInit()
    {
        _PathConfig = new GraphPathConfig(true, true);
        _PathStartNodeId = -1;
        _PathTargetNodeId = -1;
    }

    private void PathFindModeUpdate()
    {
        //  Clicked on scene
        if (Input.GetMouseButtonDown(0) && EventSystem.current.IsPointerOverGameObject() == false)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            bool hitSomething = Physics.Raycast(ray, out hit);

            //  input intersects an object
            if(hitSomething)
            {
                //  Objcet is a node
                GraphTestNodeComponent nodeComponent = hit.collider.GetComponent<GraphTestNodeComponent>();
                if(nodeComponent!=null)
                {
                    //  Nothing selected before
                    if(_PathStartNodeId==-1 && _PathTargetNodeId==-1)
                    {
                        SelectPathStart(nodeComponent);
                    }
                    else
                    //  Start selected
                    if(_PathStartNodeId!=-1 && _PathTargetNodeId==-1)
                    {
                        //  New selection is same as start
                        if(_PathStartNodeId == nodeComponent.Node.Id)
                        {
                            ClearPathAndSelection();
                        }
                        else
                        {
                            SelectPathTarget(nodeComponent);
                            FindPath();
                        }
                    }
                    else
                    //  Start and target selected
                    if(_PathStartNodeId!=-1 && _PathTargetNodeId!=-1)
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

    private void PathFindModeUninit()
    {
        ClearPathAndSelection();
    }

    private void ClearPathAndSelection()
    {
        if(_PathStartNodeId!=-1)
        {
            _Graph.GetNodeById(_PathStartNodeId).GetData().SceneObject.GetComponent<GraphTestNodeComponent>().SetAsRegular();
            _PathStartNodeId = -1;
        }

        if(_PathTargetNodeId!=-1)
        {
            _Graph.GetNodeById(_PathTargetNodeId).GetData().SceneObject.GetComponent<GraphTestNodeComponent>().SetAsRegular();
            _PathTargetNodeId = -1;
        }

        _Path = null;
    }

    private void SelectPathStart(GraphTestNodeComponent nodeComponent)
    {
        ClearPathAndSelection();

        _PathStartNodeId = nodeComponent.Node.Id;
        nodeComponent.SetAsStart();
    }

    private void SelectPathTarget(GraphTestNodeComponent nodeComponent)
    {
        _PathTargetNodeId = nodeComponent.Node.Id;
        nodeComponent.SetAsTarget();
    }

    private void FindPath()
    {
        _Path = new GraphPath();
        if(_Graph.FindPath(_PathStartNodeId, _PathTargetNodeId, _PathConfig, _Path)==false)
        {
            _Path = null;
        }
    }

#endregion

}
