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
    [SerializeField] private CameraScript _cameraScript;
    [SerializeField] private GameObject   _nodeContainer;
    [SerializeField] private GameObject   _nodePrefab;
    [SerializeField] private Material     _staticEdgeMaterial;
    [SerializeField] private Material     _dynamicEdgeMaterial;
    [SerializeField] private Material     _pathEdgeMaterial;
    [Header("UI")]
    [SerializeField] private Button       _changeModeButton;
    [SerializeField] private Text         _activeModeLabel;
#pragma warning restore 0649
    //      Internal
    private   GraphTestModes _mode;
    protected int               _pathStartNodeId;
    protected int               _pathTargetNodeId;
    protected AgentPath         _path;


    //  METHODS
    //      Unity callbacks
    private void Start()
    {
        _cameraScript.PostRenderAction = PostRenderMethod;
        
        _changeModeButton.onClick.AddListener(()=>
            {
                int nextModeValue = (1+(int)_mode)%((int)GraphTestModes.Max_Modes);
                GraphTestModes nextMode = (GraphTestModes)nextModeValue;
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

    private void PostRenderMethod()
    {
        switch(_mode)
        {
            case GraphTestModes.Edit:     EditModePostRender();     break;
            case GraphTestModes.PathFind: PathFindModePostRender(); break;
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

    abstract protected void EditModeInit();
    abstract protected void EditModeUpdate();
    abstract protected void EditModePostRender();
    abstract protected void EditModeUninit();

    abstract protected void PathFindModeInit();
    abstract protected void PathFindUpdatePath();
    abstract protected void PathFindClearPath();
    abstract protected void PathFindModePostRender();
    abstract protected void PathFindModeUninit();
    
    abstract protected GraphTestNodeComponent GetNodeComponent(int nodeId);

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
                GraphTestNodeComponent nodeComponent = hit.collider.GetComponent<GraphTestNodeComponent>();
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

    private void SelectPathStart(GraphTestNodeComponent nodeComponent)
    {
        ClearPathAndSelection();

        _pathStartNodeId = nodeComponent.NodeId;
        nodeComponent.SetAsStart();
    }

    private void SelectPathTarget(GraphTestNodeComponent nodeComponent)
    {
        _pathTargetNodeId = nodeComponent.NodeId;
        nodeComponent.SetAsTarget();
    }

}
