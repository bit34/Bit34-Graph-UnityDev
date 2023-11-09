using Com.Bit34Games.Graphs;
using UnityEngine;

public class AgentComponent : MonoBehaviour
{
    //  MEMBERS
    private IGraphTest _test;
    private Path       _path;
    private int        _pathCurrentConnection;
    private float      _pathCurrentConnectionProgress;

    //  METHODS
    public void SetTest(IGraphTest test)
    {
        _test = test;
    }

    public void SetPath(Path path)
    {
        _path = path;
        _pathCurrentConnection = 0;
        _pathCurrentConnectionProgress = 0;

        transform.position = _test.GetNodeComponent(_path.startNodeId).transform.position;
    }

    private void Update()
    {
        if (_path != null)
        {
            _pathCurrentConnectionProgress += Time.deltaTime;

            if (_pathCurrentConnectionProgress >= 1)
            {
                _pathCurrentConnectionProgress = 0;

                _pathCurrentConnection++;

                if (_pathCurrentConnection == _path.ConnectionCount)
                {
                    _pathCurrentConnection = 0;
                }
            }

            GraphConnection connection        = _path.Getconnection(_pathCurrentConnection);
            Vector3         startNodePosition = _test.GetNodeComponent(connection.SourceNodeId).transform.position;
            Vector3         endNodePosition   = _test.GetNodeComponent(connection.TargetNodeId).transform.position;

            transform.position = Vector3.Lerp(startNodePosition, endNodePosition, _pathCurrentConnectionProgress);
        }

    }
}
