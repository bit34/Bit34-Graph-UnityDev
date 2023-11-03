using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MyGraphTest : GraphTestBase
{
    //  MEMBERS
    //      Internal
    private MyGraph           _graph;
    private MyAgent           _agent;
    private MyAgentPathConfig _pathConfig;


    //  METHODS

#region Edit Mode

    override protected void EditModeInit()
    {
        if (_graph==null)
        {
            _graph = new MyGraph();

            for (int i = 0; i < NodeContainer.transform.childCount; i++)
            {
                GameObject             nodeObject    = NodeContainer.transform.GetChild(i).gameObject;
                GraphTestNodeComponent nodeComponent = nodeObject.GetComponent<GraphTestNodeComponent>();
                MyGraphNode            node          = _graph.CreateNode();

                node.component = nodeComponent;
                node.position  = nodeComponent.transform.position;

                nodeComponent.Init(node, node.Id.ToString());

                ConnectNodeToOthers(node);
            }

            _agent = new MyAgent();
            _graph.AddAgent(_agent);

            _pathConfig = new MyAgentPathConfig();
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
                GraphTestNodeComponent nodeComponent = hit.collider.GetComponent<GraphTestNodeComponent>();
                if (nodeComponent!=null)
                {
                    _graph.DeleteNode(nodeComponent.Node.Id);
                    Destroy(nodeComponent.gameObject);
                }
            }
            else
            {
                Vector3                nodePosition  = Camera.main.ScreenToWorldPoint(Input.mousePosition+new Vector3(0,0,-Camera.main.transform.position.z));
                GameObject             nodeObject    = Instantiate(NodePrefab, NodeContainer.transform);
                GraphTestNodeComponent nodeComponent = nodeObject.GetComponent<GraphTestNodeComponent>();
                MyGraphNode            node          = _graph.CreateNode();

                nodeObject.transform.position = nodePosition;

                node.component = nodeComponent;
                node.position  = nodePosition;

                nodeComponent.Init(node, node.Id.ToString());

                ConnectNodeToOthers(node);
            }
        }
    }

    override protected void EditModePostRender()
    {
        GraphTestUtilities.DrawDynamicEdges(_graph, DynamicEdgeMaterial, NodeContainer.transform.localToWorldMatrix);
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

    override protected void PathFindModePostRender()
    {
        GraphTestUtilities.DrawDynamicEdges(_graph, DynamicEdgeMaterial, NodeContainer.transform.localToWorldMatrix);
        if (Path!=null)
        {
            GraphTestUtilities.DrawPath(_graph, Path, PathEdgeMaterial, NodeContainer.transform.localToWorldMatrix);
        }
    }

    override protected void PathFindModeUninit() { }

    override public GraphTestNodeComponent GetNodeComponent(int nodeId)
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
