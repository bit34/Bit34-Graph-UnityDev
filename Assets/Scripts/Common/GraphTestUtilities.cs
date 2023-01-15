using System.Collections.Generic;
using UnityEngine;
using Com.Bit34Games.Graphs;
using Com.Bit34Games.Graphs.Unity;

public static class GraphTestUtilities
{
    static public void DrawStaticEdges<TConfig, TNode, TEdge>(Graph<TConfig, TNode, TEdge> graph, Material edgeMaterial, Matrix4x4 matrix)
        where TConfig : GraphConfig
        where TNode : GraphNode, IGraphNodeForUnity
        where TEdge : GraphEdge
    {
        edgeMaterial.SetPass(0);
        GL.Begin(GL.LINES);

        IEnumerator<TNode> nodes = graph.GetNodeEnumerator();
        while (nodes.MoveNext() == true)
        {
            TNode node = nodes.Current;

            for(int i=0; i<node.StaticEdgeCount; i++)
            {
                GraphEdge edge = node.GetStaticEdge(i);
                
                if(edge!=null)
                {
                    DrawEdge(graph, edge, matrix);
                }
            }
        }

        GL.End();
    }

    static public void DrawDynamicEdges<TConfig, TNode, TEdge>(Graph<TConfig, TNode, TEdge> graph, Material edgeMaterial, Matrix4x4 matrix)
        where TConfig : GraphConfig
        where TNode : GraphNode, IGraphNodeForUnity
        where TEdge : GraphEdge
    {
        edgeMaterial.SetPass(0);
        GL.Begin(GL.LINES);

        IEnumerator<TNode> nodes = graph.GetNodeEnumerator();
        while (nodes.MoveNext() == true)
        {
            TNode node = nodes.Current;

            IEnumerator<GraphEdge>edges = node.GetDynamicEdgeEnumerator();
            while(edges.MoveNext())
            {
                DrawEdge(graph, edges.Current, matrix);
            }
        }

        GL.End();
    }

    static public void DrawPath<TConfig, TNode, TEdge>(Graph<TConfig, TNode, TEdge> graph, GraphPath path, Material edgeMaterial, Matrix4x4 matrix)
        where TConfig : GraphConfig
        where TNode : GraphNode, IGraphNodeForUnity
        where TEdge : GraphEdge
    {
        edgeMaterial.SetPass(0);
        GL.Begin(GL.LINES);

        IEnumerator<GraphEdge> edges = path.Edges.GetEnumerator();
        while (edges.MoveNext() == true)
        {
            DrawEdge(graph, edges.Current, matrix);
        }

        GL.End();
    }

    static private void DrawEdge<TConfig, TNode, TEdge>(Graph<TConfig, TNode, TEdge> graph, GraphEdge edge, Matrix4x4 matrix)
        where TConfig : GraphConfig
        where TNode : GraphNode, IGraphNodeForUnity
        where TEdge : GraphEdge
    {
        GL.Vertex( matrix.MultiplyPoint( graph.GetNode(edge.SourceNodeId).GetPosition() ) );
        GL.Vertex( matrix.MultiplyPoint( graph.GetNode(edge.TargetNodeId).GetPosition() ) );
    }
}
