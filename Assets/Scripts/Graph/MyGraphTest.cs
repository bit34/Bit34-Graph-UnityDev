using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MyGraphTest : GraphTest
{
    //  MEMBERS
    //      Internal
    private MyGraph      _graph;
    private MyAgent      _agent;
    private MyPathConfig _pathConfig;


    //  METHODS

#region Edit Mode

    override protected void EditModeInit()
    {
        if (_graph == null)
        {
            _graph = new MyGraph();

            for (int i = 0; i < NodeContainer.transform.childCount; i++)
            {
                GameObject    nodeObject    = NodeContainer.transform.GetChild(i).gameObject;
                NodeComponent nodeComponent = nodeObject.GetComponent<NodeComponent>();
                MyGraphNode   node          = _graph.CreateNode();

                node.component = nodeComponent;
                node.position  = nodeComponent.transform.position;

                nodeComponent.Init(node, node.Id.ToString());

                ConnectNodeToOthers(node);
            }

            ClearConnectionObjects();
            CreateConnectionObjects<MyGraphConfig, MyGraphNode, MyGraphConnection>(_graph);

            _agent = new MyAgent();
            _graph.AddAgent(_agent);

            _pathConfig = new MyPathConfig();
        }
    }

    override protected void EditModeUpdate()
    {
        if (Input.GetMouseButtonDown(0) && EventSystem.current.IsPointerOverGameObject() == false)
        {
            RaycastHit hit;
            Ray        ray          = Camera.main.ScreenPointToRay(Input.mousePosition);
            bool       hitSomething = Physics.Raycast(ray, out hit);

            if (hitSomething)
            {
                NodeComponent nodeComponent = hit.collider.GetComponent<NodeComponent>();
                if (nodeComponent!=null)
                {
                    _graph.DeleteNode(nodeComponent.Node.Id);
                    Destroy(nodeComponent.gameObject);

                    ClearConnectionObjects();
                    CreateConnectionObjects<MyGraphConfig, MyGraphNode, MyGraphConnection>(_graph);
                }
            }
            else
            {
                Vector3                nodePosition  = Camera.main.ScreenToWorldPoint(Input.mousePosition+new Vector3(0,0,-Camera.main.transform.position.z));
                GameObject             nodeObject    = Instantiate(NodePrefab, NodeContainer.transform);
                NodeComponent nodeComponent = nodeObject.GetComponent<NodeComponent>();
                MyGraphNode            node          = _graph.CreateNode();

                nodeObject.transform.position = nodePosition;

                node.component = nodeComponent;
                node.position  = nodePosition;

                nodeComponent.Init(node, node.Id.ToString());

                ConnectNodeToOthers(node);

                ClearConnectionObjects();
                CreateConnectionObjects<MyGraphConfig, MyGraphNode, MyGraphConnection>(_graph);
            }
        }
    }

    override protected void EditModeUninit() {}

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

    override protected void PathFindModeUninit() { }

    override public NodeComponent GetNodeComponent(int nodeId)
    {
        return _graph.GetNode(nodeId).component;
    }
    
#endregion

    private void ConnectNodeToOthers(MyGraphNode sourceNode)
    {
        IEnumerator<MyGraphNode> nodes =_graph.GetNodeEnumerator();

        while (nodes.MoveNext())
        {
            MyGraphNode node = nodes.Current;

            //  Skip self
            if (node == sourceNode)
            {
                continue;
            }

            //  skip if already connected
            if (node.GetDynamicConnectionTo(sourceNode.Id)!=null)
            {
                continue;
            }

            Vector3    direction    = node.position-sourceNode.position;
            float      distance     = direction.magnitude;
            direction.Normalize();
            RaycastHit hit;
            Ray        ray          = new Ray(sourceNode.position, direction);
            bool       hitSomething = Physics.Raycast(ray, out hit, distance, LayerMasks.Obstacles);
            if (hitSomething==false)
            {
                _graph.ConnectNodes(sourceNode.Id, node.Id);
            }
        }
    }

}
