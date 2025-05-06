// Комбинаторные задачи

using static System.Windows.Forms.AxHost;

1.Верно
2.Верно
3.Неверно

// Реализация планировщика

public static IEnumerable<(int Start, int End)> PlanSchedule(IEnumerable<(int Start, int End)> meetings)
{
    var leftEdge = int.MinValue;
    foreach (var meeting in meetings.OrderBy(m => m.End))
    {
        if (meeting.Start >= leftEdge)
        {
            leftEdge = meeting.End;
            yield return meeting;
        }
    }
}

// Реализация алгоритма Краскала

public static IEnumerable<Edge> FindMinimumSpanningTree(IEnumerable<Edge> edges)
{
    var tree = new List<Edge>();
    // Сортируем ребра по весу
    foreach (var edge in edges.OrderBy(x => x.Weight))
    {
        // Добавляем текущее ребро в остов, если оно не создает цикл
        if (!HasCycle(tree.Concat(new List<Edge> { edge })))
        {
            tree.Add(edge);
        }
    }
    return tree;
}

// Размен монет

1.Неверно
2.К задаче поиска кратчайшего пути

// Практика «Путь в лабиринте»

using System.Collections.Generic;
using System.Linq;
using Greedy.Architecture;

namespace Greedy
{
    public class DijkstraData
    {
        public Point Previous;
        public int Price;
    }

    public class DijkstraPathFinder
    {
        private void FindIncidentsPoint(
            Point toOpen,
            State state,
            HashSet<Point> visitedCells,
            HashSet<Point> notVisitedCells,
            IDictionary<Point, DijkstraData> track)
        {
            var directions = new[] { (0, 1), (1, 0), (0, -1), (-1, 0) };

            foreach (var (dx, dy) in directions)
            {
                var probablyPoint = new Point(toOpen.X + dx, toOpen.Y + dy);
                if (state.InsideMap(probablyPoint)
                    && !state.IsWallAt(probablyPoint)
                    && probablyPoint != toOpen
                    && !visitedCells.Contains(probablyPoint))
                {
                    notVisitedCells.Add(probablyPoint);
                    var currentPrice = track[toOpen].Price + state.CellCost[probablyPoint.X, probablyPoint.Y];
                    if (!track.ContainsKey(probablyPoint) || track[probablyPoint].Price > currentPrice)
                        track[probablyPoint] = new DijkstraData { Previous = toOpen, Price = currentPrice };
                }
            }

            notVisitedCells.Remove(toOpen);
            visitedCells.Add(toOpen);
        }

        private IEnumerable<PathWithCost> FindPath(
            HashSet<Point> chests,
            Point toOpen,
            Dictionary<Point, DijkstraData> track)
        {
            if (!chests.Contains(toOpen)) yield break;
            chests.Remove(toOpen);
            var pathToChest = new List<Point>();
            for (var target = toOpen; target != new Point(-1, -1); target = track[target].Previous)
                pathToChest.Add(target);
            pathToChest.Reverse();
            yield return new PathWithCost(track[toOpen].Price, pathToChest.ToArray());
        }

        private (Point, double) GetToOpenWithBestPrice(
            Dictionary<Point, DijkstraData> track,
            IEnumerable<Point> notVisitedCells)
        {
            var bestPrice = double.PositiveInfinity;
            var toOpen = new Point(-1, -1);

            foreach (var point in notVisitedCells)
            {
                if (track.TryGetValue(point, out var data) && data.Price < bestPrice)
                {
                    bestPrice = data.Price;
                    toOpen = point;
                }
            }

            return (toOpen, bestPrice);
        }

        public IEnumerable<PathWithCost> GetPathsByDijkstra(State state, Point start, IEnumerable<Point> targets)
        {
            var chests = new HashSet<Point>(targets);
            var notVisitedCells = new HashSet<Point> { start };
            var visitedCells = new HashSet<Point>();
            var track = new Dictionary<Point, DijkstraData>
            {
                [start] = new DijkstraData
                {
                    Previous = new Point(-1, -1),
                    Price = 0
                }
            };

            while (chests.Count > 0)
            {
                var toOpen = GetToOpenWithBestPrice(track, notVisitedCells);

                if (toOpen.Item1 == new Point(-1, -1)) yield break;

                FindIncidentsPoint(toOpen.Item1, state, visitedCells, notVisitedCells, track);

                foreach (var item in FindPath(chests, toOpen.Item1, track))
                    yield return item;
            }
        }
    }
}

// Практика «Жадина в лабиринте»

using System.Collections.Generic;
using System.Linq;
using Greedy.Architecture;

namespace Greedy
{
    public class GreedyPathFinder : IPathFinder
    {
        public List<Point> FindPathToCompleteGoal(State state)
        {
            if (state.Chests.Count < state.Goal) return new List<Point>();

            var resultPath = new List<Point>();

            while (state.Scores < state.Goal)
            {
                var dijkstra = new DijkstraPathFinder();
                var shortestPathChest = dijkstra.GetPathsByDijkstra(state, state.Position, state.Chests)
                                                   .FirstOrDefault();

                if (shortestPathChest == null) return new List<Point>();

                state.Energy -= shortestPathChest.Cost;
                if (state.Energy < 0) return new List<Point>();

                var pathToChest = shortestPathChest.Path.Skip(1);
                resultPath.AddRange(pathToChest);
                state.Position = shortestPathChest.Path.Last();
                state.Chests.Remove(state.Position);
                state.Scores++;
            }

            return resultPath;
        }
    }
}

// Практика «Оптимальный маршрут»

using System.Collections.Generic;
using System.Linq;
using Greedy.Architecture;

namespace Greedy;

public class NotGreedyPathFinder : IPathFinder
{
    private List<Point> optimalPath = new();
    private int maxChestsCollected;

    public List<Point> FindPathToCompleteGoal(State state)
    {
        var currentPath = new List<Point>();
        var chestsList = new List<Point>(state.Chests) { state.Position };
        var allPaths = GeneratePaths(chestsList, new DijkstraPathFinder(), state);

        foreach (var pathEntry in allPaths?[state.Position]!)
        {
            var costList = allPaths[pathEntry.Key].Select(pair => pair.Key).ToList();
            var pathPoints = new List<Point>
            {
                state.Position,
                pathEntry.Key
            };
            ExplorePaths(state.Energy - pathEntry.Value.Cost, pathEntry.Key, costList, 1, pathPoints, allPaths);
        }

        for (var i = 0; i < optimalPath.Count - 1; i++)
            currentPath = currentPath.Concat(allPaths[optimalPath[i]][optimalPath[i + 1]].Path.Skip(1)).ToList();
        return currentPath;
    }

    private void ExplorePaths(int energy, Point currentPosition, IEnumerable<Point> remainingChests,
        int collectedChests, List<Point> visitedPoints, Dictionary<Point, Dictionary<Point, PathWithCost>> paths)
    {
        var chestsSet = new HashSet<Point>(remainingChests);
        chestsSet.Remove(currentPosition);
        foreach (var point in chestsSet)
        {
            if (paths[currentPosition][point].Cost > energy)
                continue;

            var updatedPath = new List<Point>(visitedPoints) { point };
            ExplorePaths(
                energy - paths[currentPosition][point].Cost,
                point,
                chestsSet,
                collectedChests + 1,
                updatedPath,
                paths);
        }

        if (collectedChests <= maxChestsCollected)
            return;
        maxChestsCollected = collectedChests;
        optimalPath = visitedPoints;
    }

    private static Dictionary<Point, Dictionary<Point, PathWithCost>>?
        GeneratePaths(List<Point> points, DijkstraPathFinder pathFinder, State state)
    {
        var pathsMap = new Dictionary<Point, Dictionary<Point, PathWithCost>>();
        foreach (var point in points)
        {
            if (!pathsMap.ContainsKey(point))
                pathsMap.Add(point, new Dictionary<Point, PathWithCost>());

            foreach (var path in pathFinder.GetPathsByDijkstra(state, point, state.Chests))
            {
                if (path.Start.Equals(path.End))
                    continue;
                pathsMap[path.Start][path.End] = path;
            }
        }

        return pathsMap;
    }
}