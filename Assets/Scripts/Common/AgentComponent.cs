using Com.Bit34Games.Graphs;
using UnityEngine;

public class AgentComponent : MonoBehaviour
{
    //  MEMBERS
    private GraphTestBase _test;
    private Path          _path;
    private int           _currentPathConnection;
    private float         _currentPathConnectionProgress;

    //  METHODS
    public void SetTest(GraphTestBase test)
    {
        _test = test;
    }

    public void SetPath(Path path)
    {
        _path = path;
        _currentPathConnection = 0;
        _currentPathConnectionProgress = 0;

        transform.position = _test.GetNodeComponent(_path.startNodeId).transform.position;
    }

    private void Update()
    {
        if (_path != null)
        {
            _currentPathConnectionProgress += Time.deltaTime;

            if (_currentPathConnectionProgress >= 1)
            {
                _currentPathConnectionProgress = 0;

                _currentPathConnection++;

                if (_currentPathConnection == _path.ConnectionCount)
                {
                    _currentPathConnection = 0;
                }
            }

            GraphConnection connection = _path.Getconnection(_currentPathConnection);

            Vector3 startNodePosition = _test.GetNodeComponent(connection.SourceNodeId).transform.position;
            Vector3 endNodePosition   = _test.GetNodeComponent(connection.TargetNodeId).transform.position;

            transform.position = Vector3.Lerp(startNodePosition, endNodePosition, _currentPathConnectionProgress);
        }

    }
}
