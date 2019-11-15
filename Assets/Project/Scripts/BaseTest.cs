using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public abstract class BaseTest : MonoBehaviour
{
    //  MEMBERS
    protected Dictionary<int, BaseTestNodeComponent>  Nodes         { get; private set; }
    protected GameObject                              NodeContainer { get { return _NodeContainer; } }
    protected GameObject                              NodePrefab    { get { return _NodePrefab;    } }
    protected Material     StaticEdgeMaterial  { get { return _StaticEdgeMaterial;  } }
    protected Material     DynamicEdgeMaterial { get { return _DynamicEdgeMaterial; } }
    protected Material     PathEdgeMaterial    { get { return _PathEdgeMaterial;    } }
    //      For Editor
#pragma warning disable 0649
    [SerializeField] private CameraScript _CameraScript;
    [SerializeField] private GameObject   _NodeContainer;
    [SerializeField] private GameObject   _NodePrefab;
    [SerializeField] private Material     _StaticEdgeMaterial;
    [SerializeField] private Material     _DynamicEdgeMaterial;
    [SerializeField] private Material     _PathEdgeMaterial;
    [Header("UI")]
    [SerializeField] private Button       _ChangeModeButton;
    [SerializeField] private Text         _ActiveModeLabel;
#pragma warning restore 0649
    //      Internal
    private BaseTestModes _Mode;
    protected int           _PathStartNodeId;
    protected int           _PathTargetNodeId;


    //  METHODS
    //      Unity callbacks
    private void Start()
    {
        Nodes = new Dictionary<int, BaseTestNodeComponent>();
        _CameraScript.PostRenderAction = TestPostRender;
        
        _ChangeModeButton.onClick.AddListener(()=>
            {
                int nextModeValue = (1+(int)_Mode)%((int)BaseTestModes.Max_Modes);
                BaseTestModes nextMode = (BaseTestModes)nextModeValue;
                SetMode(nextMode);
            });
        
        EditModeInit();
        SetMode(BaseTestModes.Edit);
    }

    private void Update()
    {
        switch(_Mode)
        {
            case BaseTestModes.Edit:     EditModeUpdate();     break;
            case BaseTestModes.PathFind: PathFindModeUpdate(); break;
        }
    }

    private void TestPostRender()
    {
        switch(_Mode)
        {
            case BaseTestModes.Edit:     EditModePostRender();     break;
            case BaseTestModes.PathFind: PathFindModePostRender(); break;
        }
    }

    private void SetMode(BaseTestModes newMode)
    {
        switch(_Mode)
        {
            case BaseTestModes.Edit:     EditModeUninit();     break;
            case BaseTestModes.PathFind: PathFindModeUninit(); ClearPathAndSelection(); break;
        }

        _Mode = newMode;
        _ActiveModeLabel.text = _Mode.ToString() + " mode";

        switch(_Mode)
        {
            case BaseTestModes.Edit:     EditModeInit();     break;
            case BaseTestModes.PathFind: PathFindModeInit(); break;
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
                //  Object is a node
                BaseTestNodeComponent nodeComponent = hit.collider.GetComponent<BaseTestNodeComponent>();
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
                        if(_PathStartNodeId == nodeComponent.NodeId)
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

    private void ClearPathAndSelection()
    {
        if(_PathStartNodeId!=-1)
        {
            Nodes[_PathStartNodeId].SetAsRegular();
            _PathStartNodeId = -1;
        }

        if(_PathTargetNodeId!=-1)
        {
            Nodes[_PathTargetNodeId].SetAsRegular();
            _PathTargetNodeId = -1;
        }

        PathFindClearPath();
    }

    private void SelectPathStart(BaseTestNodeComponent nodeComponent)
    {
        ClearPathAndSelection();

        _PathStartNodeId = nodeComponent.NodeId;
        nodeComponent.SetAsStart();
    }

    private void SelectPathTarget(BaseTestNodeComponent nodeComponent)
    {
        _PathTargetNodeId = nodeComponent.NodeId;
        nodeComponent.SetAsTarget();
    }

}
