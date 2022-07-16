using System;
using System.Collections.Generic;
using Random = UnityEngine.Random;

namespace Terrain
{
    public static class Pathfinder
    {

        public class Node
        {
            public Node(int x, int y, int weight)
            {
                posX = x;
                posY = y;
                this.weight = weight;
            }
            
            public int posX;
            public int posY;
            public int weight;
            public Node child;
            public bool completed;
            
            public int GetDepth()
            {
                return child?.GetDepth() ?? weight;
            }
            
            public bool isCompleted()
            {
                return child?.completed ?? completed;
            }
        }
        
        public static List<Node>  GetPath(int fromX, int fromY, int toX, int toY, bool [,] array)
        {
            bool[,] ground = new bool[array.GetLength(0), array.GetLength(1)];
            for (int i = 0; i < array.GetLength(0); i++)
            {
                for (int j = 0; j < array.GetLength(1); j++)
                {
                    ground[i, j] = array[i, j];
                }
            }

            List<Node> firstNode = new List<Node>(){ new Node(fromX, fromY,0)};
            List<List<Node>> paths = new List<List<Node>>();
            for (int i = 0; i < 1000; i++)
            {
                paths.Add(FollowPath(firstNode, toX, toY, ref ground));
            }
            
            return GetBestPath(paths);
        }

        private static List<Node> FollowPath(List<Node> from, int toX, int toY, ref bool [,] array)
        {
            int i = 0;
            while (!from[0].isCompleted())
            {
                try
                {
                    i++;
                    if (Random.Range(0, 10) > 5)
                    {
                        if (toX > from[^1].posX && array[from[^1].posX + 1, from[^1].posY])
                        {
                            Node nextNode = new Node(from[^1].posX + 1, from[^1].posY, from[^1].weight + 1);
                            from.Add(nextNode);
                        }
                        else if (toY > from[^1].posY && array[from[^1].posX, from[^1].posY + 1])
                        {
                            Node nextNode = new Node(from[^1].posX, from[^1].posY + 1, from[^1].weight + 1);
                            from.Add(nextNode);

                        }
                        else if (toX < from[^1].posX && array[from[^1].posX - 1, from[^1].posY])
                        {
                            Node nextNode = new Node(from[^1].posX - 1, from[^1].posY, from[^1].weight + 1);
                            from.Add(nextNode);

                        }
                        else if (toY < from[^1].posY && array[from[^1].posX, from[^1].posY - 1])
                        {
                            Node nextNode = new Node(from[^1].posX, from[^1].posY - 1, from[^1].weight + 1);
                            from.Add(nextNode);
                        }
                    }

                    else
                    {
                        if (toY < from[^1].posY && array[from[^1].posX, from[^1].posY - 1])
                        {
                            Node nextNode = new Node(from[^1].posX, from[^1].posY - 1, from[^1].weight + 1);
                            from.Add(nextNode);
                        }
                        else if (toX < from[^1].posX && array[from[^1].posX - 1, from[^1].posY])
                        {
                            Node nextNode = new Node(from[^1].posX - 1, from[^1].posY, from[^1].weight + 1);
                            from.Add(nextNode);
                        }

                        else if (toY > from[^1].posY && array[from[^1].posX, from[^1].posY + 1])
                        {
                            Node nextNode = new Node(from[^1].posX, from[^1].posY + 1, from[^1].weight + 1);
                            from.Add(nextNode);
                        }
                        else if (toX > from[^1].posX && array[from[^1].posX + 1, from[^1].posY])
                        {
                            Node nextNode = new Node(from[^1].posX + 1, from[^1].posY, from[^1].weight + 1);
                            from.Add(nextNode);
                        }

                    }

                    if (toX == from[^1].posX && toY == from[^1].posY)
                    {
                        from[^1].completed = true;
                        return from;
                    }

                    if (from.Count > 200)
                    {
                        return from;
                    }

                    if (i > 10)
                    {
                        int rand = Random.Range(0, 40);
                        if (rand < 10 && array[from[^1].posX + 1, from[^1].posY])
                        {
                            i = 0;
                            Node nextNode = new Node(from[^1].posX + 1, from[^1].posY, from[^1].weight + 1);
                            from.Add(nextNode);
                        }
                        else if (rand < 20 && array[from[^1].posX, from[^1].posY + 1])
                        {
                            i = 0;
                            Node nextNode = new Node(from[^1].posX, from[^1].posY + 1, from[^1].weight + 1);
                            from.Add(nextNode);

                        }
                        else if (rand < 30 && array[from[^1].posX - 1, from[^1].posY])
                        {
                            i = 0;
                            Node nextNode = new Node(from[^1].posX - 1, from[^1].posY, from[^1].weight + 1);
                            from.Add(nextNode);

                        }
                        else if (array[from[^1].posX, from[^1].posY - 1])
                        {
                            i = 0;
                            Node nextNode = new Node(from[^1].posX, from[^1].posY - 1, from[^1].weight + 1);
                            from.Add(nextNode);
                        }
                        else if (i > 20)
                        {
                            array[from[^1].posX, from[^1].posY] = false;
                            break;
                        }
                    }
                }
                catch
                {
                    Exception e;
                }
            }
            
            return from;
        }

        private static List<Node>  GetBestPath(List<List<Node>> paths)
        {
            List<Node>  bestNode = null;
            
            foreach (var path in paths)
            {
                if (bestNode is null || path[^1].weight < bestNode[^1].weight ||
                    bestNode[^1].isCompleted() && !bestNode[^1].isCompleted())
                    {
                        bestNode = path;
                    }
                
            }

            bool redo = false;

            do
            {redo = false;
                for (var i = bestNode.Count - 1; i >= 1; i--)
                {
                    var node = bestNode[i];

                    for (var j = i - 1; j >= 0; j--)
                    {
                        var node2 = bestNode[j];
                        if (node.posX == node2.posX && node.posY == node2.posY)
                        {
                            var toRemove = i - j;
                            while (toRemove > 0)
                            {
                                toRemove--;
                                bestNode.RemoveAt(j);
                                redo = true;
                            }
                            j = i = 0;
                        }
                    }
                }
            } while (redo);
          
            
            return bestNode;
        }
        
    }
}