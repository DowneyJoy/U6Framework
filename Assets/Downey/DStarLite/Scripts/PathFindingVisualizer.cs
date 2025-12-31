using System;
using System.Collections.Generic;
using System.Linq;
using Pathfinding;
using UnityEditor.ShaderGraph.Legacy;
using UnityEngine;

public class PathFindingVisualizer : MonoBehaviour
{
    public DStarLite<Vector3> dStarLite;
    public WaypointManager waypointManager;
    public GameObject goal;
    public GameObject start;
    public Node<Vector3> goalNode;
    public Node<Vector3> startNode;
    public List<Node<Vector3>> allNodes;
    private readonly Func<Node<Vector3>, Node<Vector3>, float> cost = (node, neighbor) =>
        Vector3.Distance(node.Data, neighbor.Data);
    readonly Func<Node<Vector3>, Node<Vector3>, float> heuristic = (node, neighbor) =>
        Vector3.Distance(neighbor.Data, node.Data);

    void CalculateGraph(GameObject p, bool b)
    {
        goalNode = new Node<Vector3>(goal.transform.position, cost, heuristic);
        startNode = new Node<Vector3>(start.transform.position, cost, heuristic);
        allNodes.Clear();

        foreach (GameObject waypoint in waypointManager.waypoints)
        {
            Node<Vector3> node = new Node<Vector3>(waypoint.transform.position, cost, heuristic);
            waypoint.GetComponent<NodeVisualizer>().Setup(node);
            allNodes.Add(node);
        }
        
        start.GetComponent<NodeVisualizer>().Setup(startNode);
        goal.GetComponent<NodeVisualizer>().Setup(goalNode);
        allNodes.Add(startNode);
        allNodes.Add(goalNode);
        
        // Set up 3 neighbors for each none from allNodes
        foreach (var node in allNodes)
        {
            node.Neighbours = allNodes.OrderBy(n => Vector3.Distance(n.Data, node.Data))
                .Where(n => n != node)
                .Distinct()
                .Take(3)
                .ToList();
        }
        
        startNode.Neighbours = startNode.Neighbours.Where(n => n != goalNode && n != startNode).ToList();
        goalNode.Neighbours = goalNode.Neighbours.Where(n => n != goalNode && n != startNode).ToList();

        dStarLite = new DStarLite<Vector3>(startNode, goalNode, allNodes);
        dStarLite.Initialize();
        dStarLite.ComputeShortestPath();
        UpdatePathVisual();
    }

    public void UpdatePathVisual()
    {
        
    }
}
