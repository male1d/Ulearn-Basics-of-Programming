// Графы

1. 2
2. 1
3.В нем есть цикл , Он связен , В нем есть путь из вершины 0 в вершину 4

// Сложность обхода лабиринта

1. O(N2)
2. O(N2)
3. O(N3)

// Сложность обходов графа

1. O(V+E)
2. O(V)
3. O(V+E)
4. O(V)

// Поиск цикла в неориентированном графе

public static bool HasCycle(List<Node> graph)
{
    var visited = new HashSet<Node>();  // Серые вершины
    var finished = new HashSet<Node>(); // Черные вершины
    var stack = new Stack<Node>();
    visited.Add(graph.First());
    stack.Push(graph.First());
    while (stack.Count != 0)
    {
        var node = stack.Pop();
        foreach (var nextNode in node.IncidentNodes)
        {
            if (finished.Contains(nextNode)) continue;
            if (visited.Contains(nextNode)) return true;
            visited.Add(nextNode);
            stack.Push(nextNode);
        }
        finished.Add(node); // красим в черный, когда рассмотрели все пути из node
    }
    return false;
}

// Практика «Поиск в ширину»

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Dungeon
{
    public class BfsTask
    {
        public static IEnumerable<Point> GetNewPoints(Map map, Point start)
        {
            var neighbors = new List<Point>();

            if (start.X - 1 >= 0 && map.Dungeon[start.X - 1, start.Y] is MapCell.Empty)
            {
                neighbors.Add(new Point(start.X - 1, start.Y));
            }
            if (start.Y - 1 >= 0 && map.Dungeon[start.X, start.Y - 1] is MapCell.Empty)
            {
                neighbors.Add(new Point(start.X, start.Y - 1));
            }
            if (start.X + 1 <= map.Dungeon.GetUpperBound(0) && map.Dungeon[start.X + 1, start.Y] is MapCell.Empty)
            {
                neighbors.Add(new Point(start.X + 1, start.Y));
            }
            if (start.Y + 1 <= map.Dungeon.GetUpperBound(1) && map.Dungeon[start.X, start.Y + 1] is MapCell.Empty)
            {
                neighbors.Add(new Point(start.X, start.Y + 1));
            }
            return neighbors;
        }

        public static HashSet<Point> GetHashSetPoint(Point[] points)
        {
            return new HashSet<Point>(points);
        }

        public static IEnumerable<SinglyLinkedList<Point>> FindPaths(Map map, Point start, Point[] chestPositions)
        {
            var queue = new Queue<SinglyLinkedList<Point>>();
            var chestsSet = GetHashSetPoint(chestPositions);
            var visited = new HashSet<Point> { start };

            queue.Enqueue(new SinglyLinkedList<Point>(start));

            while (queue.Count > 0)
            {
                var currentNode = queue.Dequeue();
                foreach (var foundPath in ProcessNeighbors(map, currentNode, visited, queue, chestsSet))
                {
                    yield return foundPath;
                }
            }
        }

        private static IEnumerable<SinglyLinkedList<Point>> ProcessNeighbors(
            Map map,
            SinglyLinkedList<Point> current,
            HashSet<Point> visited,
            Queue<SinglyLinkedList<Point>> queue,
            HashSet<Point> chestsSet)
        {
            foreach (var neighbor in GetNewPoints(map, current.Value))
            {
                if (visited.Contains(neighbor))
                    continue;

                var newPath = new SinglyLinkedList<Point>(neighbor, current);
                visited.Add(neighbor);
                queue.Enqueue(newPath);

                if (chestsSet.Contains(neighbor))
                {
                    yield return newPath;
                }
            }
        }

        public static IEnumerable<SinglyLinkedList<Point>> FindPaths(Map map, Point start, Chest[] chests)
        {
            var chestPositions = chests.Select(c => c.Location).ToArray();
            return FindPaths(map, start, chestPositions);
        }

        public static IEnumerable<SinglyLinkedList<Point>> FindPaths(Map map, Point start, EmptyChest[] chests)
        {
            var chestPositions = chests.Select(c => c.Location).ToArray();
            return FindPaths(map, start, chestPositions);
        }
    }
}

// Практика «Вынести клад!»

using System;
using System.Collections.Generic;
using System.Linq;

namespace Dungeon
{
    public static class DungeonTask
    {
        public static MoveDirection[] FindShortestPath(Map map)
        {
            var pathsFromStart = BfsTask.FindPaths(map, map.InitialPosition, map.Chests);
            var pathsFromExit = BfsTask.FindPaths(map, map.Exit, map.Chests).ToDictionary(path => path.Value);
            var chestsDict = map.Chests.ToDictionary(chest => chest.Location);
            var validPaths = new List<(SinglyLinkedList<Point> ToChest, SinglyLinkedList<Point> FromChest, int ChestValue)>();

            foreach (var pathToChest in pathsFromStart)
            {
                if (pathsFromExit.TryGetValue(pathToChest.Value, out var pathFromChest))
                {
                    var totalLength = pathToChest.Length + pathFromChest.Length - 1;
                    var chestValue = chestsDict[pathToChest.Value].Value;
                    validPaths.Add((pathToChest, pathFromChest, chestValue));
                }
            }

            if (!validPaths.Any())
            {
                var exitPath = BfsTask.FindPaths(map, map.InitialPosition, new[] { new Chest(map.Exit, 0) }).FirstOrDefault();
                return exitPath?.ToDirections() ?? Array.Empty<MoveDirection>();
            }

            var bestPath = validPaths.OrderBy(x => x.ToChest.Length + x.FromChest.Length).ThenByDescending(x => x.ChestValue).First();
            return CombinePaths(bestPath.ToChest, bestPath.FromChest).ToDirections();
        }

        private static SinglyLinkedList<Point> CombinePaths(SinglyLinkedList<Point> pathToChest, SinglyLinkedList<Point> pathFromChest)
        {
            pathFromChest = pathFromChest.Previous;
            while (pathFromChest != null)
            {
                pathToChest = new SinglyLinkedList<Point>(pathFromChest.Value, pathToChest);
                pathFromChest = pathFromChest.Previous;
            }
            return pathToChest;
        }
    }

    public static class PathExtensions
    {
        public static MoveDirection[] ToDirections(this SinglyLinkedList<Point> path)
        {
            if (path?.Previous == null) return Array.Empty<MoveDirection>();

            var directions = new List<MoveDirection>();
            for (var current = path; current.Previous != null; current = current.Previous)
            {
                directions.Add(GetDirection(current.Previous.Value, current.Value));
            }

            directions.Reverse();
            return directions.ToArray();
        }

        private static MoveDirection GetDirection(Point from, Point to)
        {
            var diff = to - from;
            return diff.X switch
            {
                1 => MoveDirection.Right,
                -1 => MoveDirection.Left,
                _ => diff.Y == 1 ? MoveDirection.Down : MoveDirection.Up
            };
        }
    }
}

// Практика «Поделить территорию!»

using System.Collections.Generic;
using System.Linq;

namespace Rivals
{
    public static class RivalsTask
    {
        public static IEnumerable<OwnedLocation> AssignOwners(Map map)
        {
            var visited = new Dictionary<Point, (int Owner, int Distance)>();
            var queue = new Queue<(Point Point, int Owner, int Distance)>();

            InitializeQueue(map, visited, queue);

            while (queue.Count > 0)
            {
                var current = queue.Dequeue();
                yield return new OwnedLocation(current.Owner, current.Point, current.Distance);
                if (map.Chests.Contains(current.Point))
                    continue;

                ProcessNeighbors(current, map, visited, queue);
            }
        }

        private static void InitializeQueue(
            Map map,
            Dictionary<Point, (int Owner, int Distance)> visited,
            Queue<(Point Point, int Owner, int Distance)> queue)
        {
            for (var i = 0; i < map.Players.Length; i++)
            {
                var playerPos = map.Players[i];
                if (!visited.ContainsKey(playerPos))
                {
                    visited[playerPos] = (i, 0);
                    queue.Enqueue((playerPos, i, 0));
                }
            }
        }

        private static void ProcessNeighbors(
            (Point Point, int Owner, int Distance) current,
            Map map,
            Dictionary<Point, (int Owner, int Distance)> visited,
            Queue<(Point Point, int Owner, int Distance)> queue)
        {
            foreach (var neighbor in GetNeighbors(current.Point, map))
            {
                if (!visited.TryGetValue(neighbor, out var existing) ||
                    current.Distance + 1 < existing.Distance ||
                    (current.Distance + 1 == existing.Distance && current.Owner < existing.Owner))
                {
                    visited[neighbor] = (current.Owner, current.Distance + 1);
                    queue.Enqueue((neighbor, current.Owner, current.Distance + 1));
                }
            }
        }

        private static IEnumerable<Point> GetNeighbors(Point point, Map map)
        {
            int[] dx = { -1, 0, 1, 0 };
            int[] dy = { 0, -1, 0, 1 };

            for (int i = 0; i < 4; i++)
            {
                var newX = point.X + dx[i];
                var newY = point.Y + dy[i];

                if (newX >= 0 && newX < map.Maze.GetLength(0) &&
                    newY >= 0 && newY < map.Maze.GetLength(1) &&
                    map.Maze[newX, newY] != MapCell.Wall)
                {
                    yield return new Point(newX, newY);
                }
            }
        }
    }
}