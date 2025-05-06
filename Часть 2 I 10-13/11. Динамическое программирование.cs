// Динамическое программирование

using System.Numerics;
using System.Windows.Forms.VisualStyles;

1.Перебрать все подмножества мероприятий и выбрать наилучшую комбинацию.
2.основы программирования и математика

// Планирование встреч

public static int GetOptimalScheduleGain(params Event[] events)
{
    // добавление fakeBorderEvent позволяет не обрабатывать некоторые граничные случаи
    var fakeBorderEvent = new Event { StartTime = int.MinValue, FinishTime = int.MinValue, Price = 0 };
    events = events.Concat(new[] { fakeBorderEvent }).OrderBy(e => e.FinishTime).ToArray();

    // OPT(k) = Max(OPT(k-1), w(k) + OPT(p(k))
    var opt = new int[events.Length];
    opt[0] = 0; // нулевым всегда будет fakeBorderEvent

    for (var k = 1; k < events.Length; k++)
    {
        // Находим последний неперекрывающийся событие
        int lastNonConflictingIndex = 0;
        for (int j = k - 1; j >= 0; j--)
        {
            if (events[j].FinishTime <= events[k].StartTime)
            {
                lastNonConflictingIndex = j;
                break;
            }
        }

        // Вычисляем максимальную выгоду
        opt[k] = Math.Max(opt[k - 1], events[k].Price + opt[lastNonConflictingIndex]);
    }

    return opt.Last();
}

// Расстояние Левенштейна

1.2
2.7

// Расстояние Левенштейна

public static int LevenshteinDistance(string first, string second)
{
    var opt = new int[first.Length + 1, second.Length + 1];

    // Инициализация первой строки (вставка всех символов второй строки)
    for (var i = 0; i <= first.Length; ++i) opt[i, 0] = i;

    // Инициализация первого столбца (удаление всех символов первой строки)
    for (var j = 0; j <= second.Length; ++j) opt[0, j] = j;

    for (var i = 1; i <= first.Length; ++i)
        for (var j = 1; j <= second.Length; ++j)
        {
            if (first[i - 1] == second[j - 1])
                opt[i, j] = opt[i - 1, j - 1]; // Символы совпадают
            else
                opt[i, j] = Math.Min(Math.Min(
                    opt[i - 1, j] + 1,    // Удаление
                    opt[i, j - 1] + 1),   // Вставка
                    opt[i - 1, j - 1] + 1 // Замена
                );
        }

    return opt[first.Length, second.Length];
}

// Реализация Форда-Беллмана

public static int GetMinPathCost(List<Edge> edges, int startNode, int finalNode)
{
    var maxNodeIndex =
        edges.SelectMany(e => new[] { e.From, e.To })
        .Concat(new[] { startNode, finalNode })
        .Max();

    var opt = Enumerable.Repeat(int.MaxValue, maxNodeIndex + 1).ToArray();
    opt[startNode] = 0;

    for (var pathSize = 1; pathSize <= maxNodeIndex; pathSize++)
        foreach (var edge in edges)
            if (opt[edge.From] != int.MaxValue)
                opt[edge.To] = Math.Min(opt[edge.To], opt[edge.From] + edge.Cost);

    return opt[finalNode];
}

// Практика «Антиплагиат»

using System;
using System.Collections.Generic;

using DocumentTokens = System.Collections.Generic.List<string>;

namespace Antiplagiarism
{
    public class LevenshteinCalculator
    {
        public List<ComparisonResult> CompareDocumentsPairwise(List<DocumentTokens> documents)
        {
            var results = new List<ComparisonResult>();
            int count = documents.Count;

            for (int i = 0; i < count; i++)
            {
                for (int j = i + 1; j < count; j++)
                {
                    results.Add(CompareDocuments(documents[i], documents[j]));
                }
            }

            return results;
        }

        private static ComparisonResult CompareDocuments(DocumentTokens first, DocumentTokens second)
        {
            var distanceMatrix = InitializeDistanceMatrix(first.Count, second.Count);
            FillDistanceMatrix(distanceMatrix, first, second);
            return new ComparisonResult(first, second, distanceMatrix[first.Count, second.Count]);
        }

        private static double[,] InitializeDistanceMatrix(int len1, int len2)
        {
            var matrix = new double[len1 + 1, len2 + 1];

            for (var i = 0; i <= len1; i++)
                matrix[i, 0] = i; // удаление

            for (var j = 0; j <= len2; j++)
                matrix[0, j] = j; // вставка

            return matrix;
        }

        private static void FillDistanceMatrix(double[,] matrix, DocumentTokens first, DocumentTokens second)
        {
            for (var i = 1; i <= first.Count; i++)
            {
                for (var j = 1; j <= second.Count; j++)
                {
                    double cost = TokenDistanceCalculator.GetTokenDistance(first[i - 1], second[j - 1]);
                    matrix[i, j] = Math.Min(Math.Min(matrix[i - 1, j] + 1, // удаление
                                                        matrix[i, j - 1] + 1), // вставка
                                            matrix[i - 1, j - 1] + cost); // замена
                }
            }
        }
    }
}

// Практика «Diff Tool»

using System;
using System.Collections.Generic;

namespace Antiplagiarism
{
    public static class LongestCommonSubsequenceCalculator
    {
        public static List<string> Calculate(List<string> first, List<string> second)
        {
            var opt = CreateOptimizationTable(first, second);
            return RestoreAnswer(opt, first, second);
        }

        private static int[,] CreateOptimizationTable(IReadOnlyList<string> first, IReadOnlyList<string> second)
        {
            var opt = new int[first.Count + 1, second.Count + 1];

            for (var i = first.Count - 1; i >= 0; i--)
            {
                for (var j = second.Count - 1; j >= 0; j--)
                {
                    opt[i, j] = CalculateCellValue(first, second, i, j, opt);
                }
            }
            return opt;
        }

        private static int CalculateCellValue(
            IReadOnlyList<string> first,
            IReadOnlyList<string> second,
            int i,
            int j,
            int[,] opt)
        {
            if (first[i] == second[j])
            {
                return 1 + opt[i + 1, j + 1];
            }
            return Math.Max(opt[i + 1, j], opt[i, j + 1]);
        }

        private static List<string> RestoreAnswer(int[,] opt, IReadOnlyList<string> first, IReadOnlyList<string> second)
        {
            var result = new List<string>();
            int x = 0, y = 0;

            while (x < first.Count && y < second.Count)
            {
                if (first[x] == second[y])
                {
                    result.Add(first[x]);
                    x++;
                    y++;
                }
                else
                {
                    MoveToNext(opt, ref x, ref y);
                }
            }

            return result;
        }

        private static void MoveToNext(int[,] opt, ref int x, ref int y)
        {
            if (opt[x, y] == opt[x + 1, y])
            {
                x++;
            }
            else
            {
                y++;
            }
        }
    }
}

// Практика «Счастливые билеты»

using System.Numerics;

namespace Tickets
{
    public static class TicketsTask
    {
        public static BigInteger Solve(int totalLen, int totalSum)
        {
            if (totalSum % 2 != 0) return 0;

            var halfSum = totalSum / 2;
            var bigIntegers = new BigInteger[totalLen + 1, halfSum + 1];

            InitializeBaseCases(totalLen, halfSum, bigIntegers);

            for (var i = 1; i <= totalLen; i++)
            {
                for (var j = 1; j <= halfSum; j++)
                {
                    bigIntegers[i, j] = CalculateWays(i, j, bigIntegers);
                }
            }

            return bigIntegers[totalLen, halfSum] * bigIntegers[totalLen, halfSum];
        }

        private static void InitializeBaseCases(int totalLen, int halfSum, BigInteger[,] bigIntegers)
        {
            for (var i = 1; i <= totalLen; i++)
            {
                bigIntegers[i, 0] = 1;
            }
            for (var i = 0; i <= halfSum; i++)
            {
                bigIntegers[0, i] = 0;
            }
        }

        private static BigInteger CalculateWays(int i, int j, BigInteger[,] bigIntegers)
        {
            if (j > i * 9) return 0;

            var ways = bigIntegers[i - 1, j] + bigIntegers[i, j - 1];
            if (j > 9)
            {
                ways -= bigIntegers[i - 1, j - 10];
            }

            return ways;
        }
    }
}