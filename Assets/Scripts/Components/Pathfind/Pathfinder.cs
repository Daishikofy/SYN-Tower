using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace TOWER
{
    public class Pathfinder : MonoBehaviour
    {
        public List<Tilemap> obstacles;
        private DynamicGrid _grid;

        private void Awake()
        {
            _grid = new DynamicGrid(obstacles);
        }

        public List<Vector2> ShortestPath(Vector2 initialPosition, Vector2 targetPosition, Vector2 offset)
        {
            List<Node> openNodes = new List<Node>();
            List<Node> closedNodes = new List<Node>();
            List<Vector2> path = new List<Vector2>();

            Node startNode = new Node(new Vector2Int((int) initialPosition.x, (int) initialPosition.y));

            Vector2Int target = new Vector2Int((int) targetPosition.x, (int) targetPosition.y);

            openNodes.Add(startNode);

            int overtimeCheck = 0;
            while (openNodes.Count != 0)
            {
                if (overtimeCheck++ > 1000)
                {
                    Debug.LogWarning("[PATHFINDER] Took over 1000 iterations, canceled for overtime.");
                    return path;
                }

                Node currentNode = openNodes[0];
                openNodes.RemoveAt(0);

                if (currentNode.position == target)
                {
                    path.Add(currentNode.position);
                    Vector2Int startPosition = startNode.position;
                    while (currentNode.position != startPosition)
                    {
                        currentNode = currentNode.parent;
                        path.Add(currentNode.position + offset);
                    }

                    path.Reverse();
                    return path;
                }
                
                
                List<Node> neighbours = GetNeighbours(currentNode, target, _grid);

                foreach (Node neighbour in neighbours)
                {
                    if (!closedNodes.Contains(neighbour))
                    {
                        bool inOpenNode = false;
                        foreach (Node openNode in openNodes)
                        {
                            if (openNode == neighbour)
                            {
                                if (openNode.H >= neighbour.H)
                                {
                                    openNode.G = neighbour.G;
                                    openNode.parent = neighbour.parent;
                                }
                                inOpenNode = true;
                            }
                        }

                        if (!inOpenNode)
                        {
                            int index = InsertAtIndex(openNodes, neighbour);
                            if (index < openNodes.Count)
                            {
                                openNodes.Insert(index, neighbour);
                            }
                            else
                            {
                                openNodes.Add(neighbour);
                            }
                        }
                    }
                    closedNodes.Add(currentNode);
                }
            }
            return path;
        }

        private int CompareHeuristic(Node a, Node b)
        {
            if (a.H < b.H)
            {
                return 1;
            }

            if (a.H == b.H)
            {
                return 0;
            }

            return -1;
        }

        private List<Node> GetNeighbours(Node node, Vector2Int targetPosition, DynamicGrid grid)
        {
            Vector2Int[] verticalPositions = new Vector2Int[2];
            Vector2Int[] horizontalPositions = new Vector2Int[2];
            Vector2Int[] adjacentPositions = new Vector2Int[8];
            int[] adjacentValues = new int[8];
            int adjacentIndex = 0;
            Vector2Int definedPosition = node.position + Vector2Int.down;
            int definedCellValue = grid.GetCellValue(definedPosition);
            if (definedCellValue < Int32.MaxValue)
            {
                adjacentPositions[adjacentIndex] = definedPosition;
                adjacentValues[adjacentIndex] = definedCellValue;
                adjacentIndex++;
                
                verticalPositions[0] = Vector2Int.down;
            }
            
            definedPosition = node.position + Vector2Int.up;
            definedCellValue = grid.GetCellValue(definedPosition);
            if (definedCellValue < Int32.MaxValue)
            {
                adjacentPositions[adjacentIndex] = definedPosition;
                adjacentValues[adjacentIndex] = definedCellValue;
                adjacentIndex++;
                
                verticalPositions[1] = Vector2Int.up;
            }
            
            definedPosition = node.position + Vector2Int.left;
            definedCellValue = grid.GetCellValue(definedPosition);
            if (definedCellValue < Int32.MaxValue)
            {
                adjacentPositions[adjacentIndex] = definedPosition;
                adjacentValues[adjacentIndex] = definedCellValue;
                adjacentIndex++;
                
                horizontalPositions[0] = Vector2Int.left;
            }           
            
            definedPosition = node.position + Vector2Int.right;
            definedCellValue = grid.GetCellValue(definedPosition);
            if (definedCellValue < Int32.MaxValue)
            {
                adjacentPositions[adjacentIndex] = definedPosition;
                adjacentValues[adjacentIndex] = definedCellValue;
                adjacentIndex++;
                
                horizontalPositions[1] = Vector2Int.right;
            }
            
            foreach (Vector2Int verticalPosition in verticalPositions)
            {
                foreach (Vector2Int horizontalPosition in horizontalPositions)
                {
                    definedPosition = node.position + horizontalPosition + verticalPosition;
                    definedCellValue = grid.GetCellValue(definedPosition);
                    if (definedCellValue < Int32.MaxValue)
                    {
                        adjacentPositions[adjacentIndex] = definedPosition;
                        adjacentIndex++;
                    }
                }
            }

            List<Node> neighbours = new List<Node>(8);

            for (int i = 0; i < adjacentIndex; i++)
            {
                Vector2Int position = adjacentPositions[i];
            
                Vector2Int distance = new Vector2Int(Mathf.Abs(position.x - targetPosition.x), Mathf.Abs(position.y - targetPosition.y));

                int lowest = Mathf.Min(distance.x, distance.y);
                int highest = Mathf.Max(distance.x, distance.y);

                int horizontalMovesRequired = highest - lowest;

                int costToTarget = lowest * 14 + horizontalMovesRequired * 10 ;
                neighbours.Add(new Node(node, position, costToTarget, adjacentValues[i]));
            }

            return neighbours;
        }

        private int InsertAtIndex(List<Node> list, Node node)
        {
            int startIndex = 0;
            int endIndex = list.Count - 1;

            while (startIndex <= endIndex)
            {
                int midIndex = startIndex + (endIndex - startIndex) / 2;
                int comparision = CompareHeuristic(node, list[midIndex]);
                if (comparision > 0)
                {
                    endIndex = midIndex - 1;
                }
                else if (comparision < 0)
                {
                    startIndex = midIndex + 1;
                }
                else
                {
                    return midIndex;
                }
            }

            return startIndex;
        }
        
        private class Node
        {
            public readonly Vector2Int position;
            
            public int F, G;
            public int H => F + G;
            public Node parent;

            public Node(Vector2Int position)
            {
                this.position = position;
            }

            public Node(Node parent, Vector2Int position, int costToTarget, int value)
            {
                this.parent = parent;
                this.position = position;
                F = costToTarget;
                G = parent.G + 1 + value;
            }

            public static bool operator ==(Node a, Node b)
            {
                
                return a?.position == b?.position;
            }

            public override bool Equals(object obj)
            {
                return this == (Node) obj;
            }

            public override int GetHashCode()
            {
                return HashCode.Combine(position.x, position.y);
            }

            public static bool operator !=(Node a, Node b)
            {
                return !(a?.position == b?.position);
            }
        }
    }
}