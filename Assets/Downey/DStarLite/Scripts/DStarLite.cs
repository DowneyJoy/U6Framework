using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using UnityEngine;
using UnityUtils;

namespace  Pathfinding
{
    public class Node<T>
    {
        public T Data { get; set; }
        // calculate the "real" cost from one node to another
        public Func<Node<T>,Node<T>,float> Cost { get; set; }
        // Estimate the cost from one node to another node
        public Func<Node<T>,Node<T>,float> Heuristic { get; }
        
        public float G { get; set; }//Real cost from start to this node
        public float RHS { get; set; }//Estimate the cost from start to this node
        public bool GEqualRHS => G.Approx(RHS);

        public List<Node<T>> Neighbours { get; set; } = new();

        public Node(T data, Func<Node<T>, Node<T>, float> cost, Func<Node<T>, Node<T>, float> heuristic)
        {
            Data = data;
            Cost = cost;
            Heuristic = heuristic;

            G = float.MaxValue;
            RHS = float.MaxValue;
        }
    }

    public readonly struct Key
    {
        private readonly float k1;
        private readonly float k2;

        public Key(float k1, float k2)
        {
            this.k1 = k1;
            this.k2 = k2;
        }

        public static bool operator <(Key a, Key b) => a.k1 < b.k1 || a.k1.Approx(b.k1) && a.k2 < b.k2;
        public static bool operator >(Key a, Key b) => a.k1 > b.k1 || a.k1.Approx(b.k1) && a.k2 > b.k2;

        public static bool operator ==(Key a, Key b) => a.k1.Approx(b.k1) && a.k2.Approx(b.k2);
        public static bool operator !=(Key a, Key b) => !(a == b);

        public override bool Equals(object obj) => obj is Key key && this == key;
        public override int GetHashCode() => HashCode.Combine(k1, k2);
        public override string ToString() => $"({k1}, {k2})";
    }

    public class DStarLite<T>
    {
        readonly Node<T> startNode;
        readonly Node<T> goalNode;
        private readonly List<Node<T>> allNodes;
        private float km;//Key modifier

        class KeyNodeComparer : IComparer<(Key, Node<T>)>
        {
            public int Compare((Key, Node<T>) x, (Key, Node<T>) y)
            {
                return x.Item1 < y.Item1 ? -1 : x.Item1 > y.Item1 ? 1 : 0;
            }
        }
        // TODO Move to a custom priority queue implementation
        // SortedSet will add or remove elements in O(log n) time and fetch the minimum element in O(1) time
        readonly SortedSet<(Key,Node<T>)> openSet = new (new KeyNodeComparer());
        // Dictionary will add or remove elements in O(1) and fetch the element in O(1) time
        private readonly Dictionary<Node<T>, Key> lookups = new();

        public DStarLite(Node<T> start, Node<T> goal, List<Node<T>> allNodes)
        {
            this.startNode = start;
            this.goalNode = goal;
            this.allNodes = allNodes;
        }

        private const int k_maxCycles = 1000;

        public void RecalculateNode(Node<T> node)
        {
            km += startNode.Heuristic(startNode, node);

            var allConnectredNode = Successors(node).Concat(Predecessors(node)).ToList();

            foreach (var s in allConnectredNode)
            {
                if (s != startNode)
                {
                    s.RHS = Mathf.Min(s.RHS,s.Cost(s, node) + node.G);
                }
                UpdateVertex(s);
            }
            UpdateVertex(node);
            ComputeShortestPath();
        }

        public void ComputeShortestPath()
        {
            int maxSteps = k_maxCycles;
            while (openSet.Count > 0&& (openSet.Min.Item1 < CalculateKey(startNode) || startNode.RHS > startNode.G))
            {
                if (maxSteps-- <= 0)
                {
                    Debug.LogWarning("ComputeShortestPath error: max steps exceeded");
                    break;
                }
                var smallest = openSet.Min;
                openSet.Remove(smallest);
                lookups.Remove(smallest.Item2);
                var node = smallest.Item2;

                if (smallest.Item1 < CalculateKey(node))
                {
                    var newKey = CalculateKey(node);
                    openSet.Add((newKey, node));
                    lookups[node]  = newKey;
                }
                else if (node.G > node.RHS)
                {
                    node.G = node.RHS;
                    foreach (var s in Predecessors(node))
                    {
                        if (s != goalNode)
                        {
                            s.RHS = Mathf.Min(s.RHS, s.Cost(s, node) + node.G);
                        }
                        UpdateVertex(s);
                    }
                }
                else
                {
                    var gOld = node.G;
                    node.G = float.MaxValue;
                    foreach (var s in Predecessors(node).Concat(new[]{node}))
                    {
                        if (s.RHS.Approx(s.Cost(s, node) + gOld))
                        {
                            if (s != goalNode)
                            {
                                s.RHS = float.MaxValue;
                            }

                            foreach (var sPrime in Successors(s))
                            {
                                s.RHS = Mathf.Min(s.RHS, s.Cost(s, sPrime) + sPrime.G);
                            }
                        }
                        UpdateVertex(s);
                    }
                }
            }
            startNode.G = startNode.RHS;
            Debug.Log("Shortest path computed in " + (k_maxCycles - maxSteps) + " steps");
        }

        IEnumerable<Node<T>> Predecessors(Node<T> node)
        {
            //May need to be more complex depending on Type T
            //f. eks. return allNodes.Where(n => n.Neighbors.Contains(node));
            return node.Neighbours;
        }

        IEnumerable<Node<T>> Successors(Node<T> node)
        {
            // May need to be more complex depending on Type T
            return node.Neighbours;
        }

        void UpdateVertex(Node<T> node)
        {
            var key = CalculateKey(node);
            if (!node.GEqualRHS && !lookups.ContainsKey(node))
            {
                openSet.Add((key, node));
                lookups[node] = key;
            }
            else if (node.GEqualRHS && lookups.ContainsKey(node))
            {
                openSet.Remove((key, node));
                lookups.Remove(node);
            }
            else if (lookups.ContainsKey(node))
            {
                openSet.Remove((lookups[node], node));
                openSet.Add((key, node));
                lookups[node] = key;
            }
        }
        Key CalculateKey(Node<T> node)
        {
            return new Key(
                Mathf.Min(node.G, node.RHS) + node.Heuristic(node, startNode) + km,
                Mathf.Min(node.G, node.RHS));
        }

        public void Initialize()
        {
            openSet.Clear();
            lookups.Clear();
            km = 0;
        }
    }
}