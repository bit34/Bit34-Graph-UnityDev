using System.Collections.Generic;
using UnityEngine;
using Com.Bit34Games.Graphs;

public static class GraphTestUtilities
{
    static public void DrawStaticEdges<TConfig, TNode, TConnection>(Graph<TConfig, TNode, TConnection> graph, Material edgeMaterial, Matrix4x4 matrix)
        where TConfig : GraphConfig<TNode>
        where TNode : GraphNode, IGraphNodeForUnity
        where TConnection : GraphConnection
    {
        edgeMaterial.SetPass(0);
        GL.Begin(GL.LINES);

        IEnumerator<TNode> nodes = graph.GetNodeEnumerator();
        while (nodes.MoveNext() == true)
        {
            TNode node = nodes.Current;

            for(int i=0; i<node.StaticConnectionCount; i++)
            {
                GraphConnection connection = node.GetStaticConnection(i);
                
                if(connection!=null)
                {
                    DrawEdge(graph, connection, matrix);
                }
            }
        }

        GL.End();
    }

    static public void DrawDynamicEdges<TConfig, TNode, TConnection>(Graph<TConfig, TNode, TConnection> graph, Material edgeMaterial, Matrix4x4 matrix)
        where TConfig : GraphConfig<TNode>
        where TNode : GraphNode, IGraphNodeForUnity
        where TConnection : GraphConnection
    {
        edgeMaterial.SetPass(0);
        GL.Begin(GL.LINES);

        IEnumerator<TNode> nodes = graph.GetNodeEnumerator();
        while (nodes.MoveNext() == true)
        {
            TNode node = nodes.Current;

            IEnumerator<GraphConnection>connections = node.GetDynamicConnectionEnumerator();
            while(connections.MoveNext())
            {
                DrawEdge(graph, connections.Current, matrix);
            }
        }

        GL.End();
    }

    static public void DrawPath<TConfig, TNode, TConnection>(Graph<TConfig, TNode, TConnection> graph, AgentPath path, Material edgeMaterial, Matrix4x4 matrix)
        where TConfig : GraphConfig<TNode>
        where TNode : GraphNode, IGraphNodeForUnity
        where TConnection : GraphConnection
    {
        edgeMaterial.SetPass(0);
        GL.Begin(GL.LINES);

        IEnumerator<GraphConnection> connections = path.Connections.GetEnumerator();
        while (connections.MoveNext() == true)
        {
            DrawEdge(graph, connections.Current, matrix);
        }

        GL.End();
    }

    static private void DrawEdge<TConfig, TNode, TConnection>(Graph<TConfig, TNode, TConnection> graph, GraphConnection connection, Matrix4x4 matrix)
        where TConfig : GraphConfig<TNode>
        where TNode : GraphNode, IGraphNodeForUnity
        where TConnection : GraphConnection
    {
        GL.Vertex( matrix.MultiplyPoint( graph.GetNode(connection.SourceNodeId).GetPosition() ) );
        GL.Vertex( matrix.MultiplyPoint( graph.GetNode(connection.TargetNodeId).GetPosition() ) );
    }
}
