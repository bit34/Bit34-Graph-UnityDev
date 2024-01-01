using System.Collections.Generic;
using Com.Bit34Games.Graphs;
using UnityEngine;
using UnityEngine.EventSystems;

public class GraphTest : GraphTestBase
{
    //  MEMBERS
    //      Private
    private MyGraph                             _graph;
    private MyAgent                             _agent;
    private PathFinder<MyAgent, MyNode, MyEdge> _pathFinder;


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
                MyNode        node          = _graph.CreateNode();

                node.component = nodeComponent;
                node.position  = nodeComponent.transform.position;

                nodeComponent.Init(node.Id, node.Id.ToString());

                ConnectNodeToOthers(node);
            }

            ClearEdgeObjects();
            CreateEdgeObjects<MyNode, MyEdge>(_graph);

            _agent = new MyAgent();
            _graph.AddAgent(_agent);

            _pathFinder = new PathFinder<MyAgent, MyNode, MyEdge>(true, true);
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
                    _graph.DeleteNode(nodeComponent.NodeId);
                    Destroy(nodeComponent.gameObject);

                    ClearEdgeObjects();
                    CreateEdgeObjects<MyNode, MyEdge>(_graph);
                }
            }
            else
            {
                Vector3       nodePosition  = Camera.main.ScreenToWorldPoint(Input.mousePosition+new Vector3(0,0,-Camera.main.transform.position.z));
                GameObject    nodeObject    = Instantiate(NodePrefab, NodeContainer.transform);
                NodeComponent nodeComponent = nodeObject.GetComponent<NodeComponent>();
                MyNode        node          = _graph.CreateNode();

                nodeObject.transform.position = nodePosition;

                node.component = nodeComponent;
                node.position  = nodePosition;

                nodeComponent.Init(node.Id, node.Id.ToString());

                ConnectNodeToOthers(node);

                ClearEdgeObjects();
                CreateEdgeObjects(_graph);
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
        SetPath(_pathFinder.FindPath(_agent, _pathStartNodeId, _pathTargetNodeId));
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

    private void ConnectNodeToOthers(MyNode sourceNode)
    {
        IEnumerator<MyNode> nodes =_graph.GetNodeEnumerator();

        while (nodes.MoveNext())
        {
            MyNode node = nodes.Current;

            //  Skip self
            if (node == sourceNode)
            {
                continue;
            }

            //  skip if already connected
            if (node.GetDynamicEdgeTo(sourceNode.Id)!=null)
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
