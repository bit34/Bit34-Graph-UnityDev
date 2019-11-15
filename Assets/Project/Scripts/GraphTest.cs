using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Bit34.Unity.Graph.Utilities;
using Bit34.Unity.Graph.Base;


public class GraphTest : BaseTest
{
    //  MEMBERS
    //      Internal
    private GraphTestGraph  _Graph;
    private GraphPath       _Path;
    private GraphPathConfig _PathConfig;


    //  METHODS

#region Edit Mode

    override protected void EditModeInit()
    {
        if(_Graph==null)
        {
            _Graph = new GraphTestGraph();

            for (int i = 0; i < NodeContainer.transform.childCount; i++)
            {
                GameObject             nodeObject    = NodeContainer.transform.GetChild(i).gameObject;
                GraphTestNodeComponent nodeComponent = nodeObject.GetComponent<GraphTestNodeComponent>();
                GraphTestNode          node          = _Graph.CreateNode(0);

                node.GetData().SceneObject = nodeObject;
                node.Position = nodeComponent.transform.position;
                nodeComponent.Init(node);

                ConnectNodeToOthers(node);

                Nodes.Add(node.Id, nodeComponent);
            }
        }
    }

    override protected void EditModeUpdate()
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
                    Nodes.Remove(nodeScript.Node.Id);
                    _Graph.RemoveNode(nodeScript.Node.Id);
                    Destroy(nodeScript.gameObject);
                }
            }
            else
            {
                GameObject             nodeObject    = Instantiate(NodePrefab, NodeContainer.transform);
                GraphTestNodeComponent nodeComponent = nodeObject.GetComponent<GraphTestNodeComponent>();
                Vector3                nodePosition  = Camera.main.ScreenToWorldPoint(Input.mousePosition+new Vector3(0,0,-Camera.main.transform.position.z));
                GraphTestNode          node          = _Graph.CreateNode(0);

                node.GetData().SceneObject = nodeObject;
                node.Position = nodePosition;
                nodeObject.transform.position = nodePosition;
                nodeComponent.Init(node);

                ConnectNodeToOthers(node);

                Nodes.Add(node.Id, nodeComponent);
            }
        }
    }

    override protected void EditModePostRender()
    {
        GraphUtilities.DrawDynamicEdges(_Graph, DynamicEdgeMaterial, NodeContainer.transform.localToWorldMatrix);
    }

    override protected void EditModeUninit() { }

    private void ConnectNodeToOthers(GraphTestNode node)
    {
        IEnumerator<GraphTestNode> nodes =_Graph.GetNodeEnumerator();

        while(nodes.MoveNext())
        {
            GraphTestNode node2 = nodes.Current;

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

    override protected void PathFindModeInit()
    {
        _PathConfig = new GraphPathConfig(true, true);
        _Path = null;
    }

    override protected void PathFindUpdatePath()
    {
        _Path = new GraphPath();
        if(_Graph.FindPath(_PathStartNodeId, _PathTargetNodeId, _PathConfig, _Path)==false)
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
        GraphUtilities.DrawDynamicEdges(_Graph, DynamicEdgeMaterial, NodeContainer.transform.localToWorldMatrix);
        if(_Path!=null)
        {
            GraphUtilities.DrawPath(_Graph, _Path, PathEdgeMaterial, NodeContainer.transform.localToWorldMatrix);
        }
    }

    override protected void PathFindModeUninit() { }

#endregion

}
