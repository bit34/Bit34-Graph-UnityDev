using Com.Bit34Games.Graphs;
using UnityEngine;

public class GraphTestAgentComponent : MonoBehaviour
{
    //  MEMBERS
    private GraphTestBase _test;
    private AgentPath     _path;
    private int           _currentConnection;
    private float         _currentProgress;

    //  METHODS
    public void SetTest(GraphTestBase test)
    {
        _test = test;
    }

    public void SetPath(AgentPath path)
    {
        _path              = path;
        _currentConnection = 0;
        _currentProgress   = 0;

        transform.position = _test.GetNodeComponent(_path.startNodeId).transform.position;
    }

    private void Update()
    {
        if (_path != null)
        {
            _currentProgress += Time.deltaTime;

            if (_currentProgress >= 1)
            {
                _currentProgress = 0;

                _currentConnection++;

                if (_currentConnection == _path.ConnectionCount)
                {
                    _currentConnection = 0;
                }
            }

            GraphConnection connection = _path.Getconnection(_currentConnection);

            Vector3 startNodePosition = _test.GetNodeComponent(connection.SourceNodeId).transform.position;
            Vector3 endNodePosition   = _test.GetNodeComponent(connection.TargetNodeId).transform.position;

            transform.position = Vector3.Lerp(startNodePosition, endNodePosition, _currentProgress);
        }


    }
}
