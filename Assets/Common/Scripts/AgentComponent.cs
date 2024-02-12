using Com.Bit34Games.Graphs;
using UnityEngine;

public class AgentComponent : MonoBehaviour
{
    //  MEMBERS
    private IGraphTest _test;
    private Path       _path;
    private int        _pathCurrentEdge;
    private float      _pathCurrentEdgeProgress;
    private float      _speed;

    //  METHODS
    public void SetTest(IGraphTest test)
    {
        _test = test;
    }

    public void SetPath(Path path, float speed)
    {
        _path                    = path;
        _pathCurrentEdge         = 0;
        _pathCurrentEdgeProgress = 0;
        _speed                   = speed;

        transform.position = _test.GetNodeComponent(_path.startNodeId).transform.position;
    }

    private void Update()
    {
        if (_path != null)
        {
            Edge    edge              = _path.GetEdge(_pathCurrentEdge);
            Vector3 startNodePosition = _test.GetNodeComponent(edge.SourceNodeId).transform.position;
            Vector3 endNodePosition   = _test.GetNodeComponent(edge.TargetNodeId).transform.position;
            float   edgeLength        = (endNodePosition - startNodePosition).magnitude;

            transform.position = Vector3.Lerp(startNodePosition, endNodePosition, _pathCurrentEdgeProgress);

            _pathCurrentEdgeProgress += (_speed*Time.deltaTime)/edgeLength;

            if (_pathCurrentEdgeProgress >= 1)
            {
                _pathCurrentEdgeProgress = 0;

                _pathCurrentEdge++;

                if (_pathCurrentEdge == _path.EdgeCount)
                {
                    _pathCurrentEdge = 0;
                }
            }
        }

    }
}
