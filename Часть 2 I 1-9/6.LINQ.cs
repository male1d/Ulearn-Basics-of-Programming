// Чтение массива чисел

using System.Reflection.Metadata;
using System.Text.RegularExpressions;

public static int[] ParseNumbers(IEnumerable<string> lines)
{
    return lines
        .Where(line => !string.IsNullOrWhiteSpace(line)) // Отбрасываем пустые строки
        .Select(line => int.Parse(line.Trim())) // Преобразуем строку в число
        .ToArray(); // Конвертируем в массив
}

// Чтение списка точек

public static List<Point> ParsePoints(IEnumerable<string> lines)
{
    return lines
        .Select(z => z.Split(' '))
        .Select(z => new Point(int.Parse(z[0]), int.Parse(z[1])))
        .ToList();
}

// Объединение коллекций

public static string[] GetAllStudents(Classroom[] classes)
{
    return classes.SelectMany(c => c.Students).ToArray();
}

// Декартово произведение

public static IEnumerable<Point> GetNeighbours(Point p)
{
    int[] d = { -1, 0, 1 };
    return d.SelectMany(dx => d.Select(dy => new Point(p.X + dx, p.Y + dy)))
            .Where(neighbour => !neighbour.Equals(p));
}

// Составление словаря

public static string[] GetSortedWords(params string[] textLines)
{
    return textLines
        .SelectMany(line => line.Split(new char[] { ' ', ',', '.', ';', '!', '?', ':', '-', '_', '(', ')', '[', ']', '{', '}', '\'', '\"' }, StringSplitOptions.RemoveEmptyEntries))
        .Select(word => word.ToLower())
        .Distinct()
        .OrderBy(word => word)
        .ToArray();
}

// Сортировка кортежей

public static List<string> GetSortedWords(string text)
{
    return Regex.Split(text, @"\W+")
                .Where(x => x.Length > 0)
                .Select(t => Tuple.Create(t.Length, t.ToLowerInvariant()))
                .OrderBy(x => x)
                .Select(x => x.Item2)
                .Distinct()
                .ToList();
}

// Поиск самого длинного слова

public static string GetLongest(IEnumerable<string> words)
{
    return words.Where(w => w.Length == words.Max(w => w.Length)).Min();
}

// Создание частотного словаря

public static (string, int)[] GetMostFrequentWords(string text, int count)
{
    return Regex.Split(text, @"\W+")
        .Where(word => word != "")
        .Select(word => word.ToLower())
        .GroupBy(word => word)
        .Select(group => (group.Key, group.Count()))
        .OrderByDescending(pair => pair.Item2)
        .ThenBy(pair => pair.Item1)
        .Take(count)
        .ToArray();
}

// Создание обратного индекса

public static ILookup<string, int> BuildInvertedIndex(Document[] documents)
{
    return documents
.SelectMany(document => Regex.Split(document.Text.ToLower(), @"\W+")
.Where(word => word != "")
.Distinct()
.Select(stor => (stor, document.Id)))
.ToLookup(a => a.stor, a => a.Id);
}

// Практика «Median & Bigrams»

using System;
using System.Collections.Generic;
using System.Linq;

namespace linq_slideviews;

public static class ExtensionsTask
{
    public static double Median(this IEnumerable<double> items)
    {
        var list = items.ToList();
        if (list.Count == 0)
            throw new InvalidOperationException("Последовательность не содержит элементов");

        list.Sort();
        var middle = list.Count / 2;

        if (list.Count % 2 == 0)
            return (list[middle - 1] + list[middle]) / 2.0;
        else
            return list[middle];
    }

    public static IEnumerable<(T First, T Second)> Bigrams<T>(this IEnumerable<T> items)
    {
        using (var iterator = items.GetEnumerator())
        {
            if (!iterator.MoveNext())
                yield break;

            var previous = iterator.Current;
            while (iterator.MoveNext())
            {
                yield return (previous, iterator.Current);
                previous = iterator.Current;
            }
        }
    }
}

// Практика «Чтение файла»

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace linq_slideviews
{
    public class ParsingTask
    {
        public static IDictionary<int, SlideRecord> ParseSlideRecords(IEnumerable<string> lines)
        {
            return lines.Select(line =>
            {
                var lineParams = line.Split(new[] { ';' }, 3, StringSplitOptions.RemoveEmptyEntries);
                if (lineParams.Length < 3 ||
                    !int.TryParse(lineParams[0], out int slideID))
                    return null;
                switch (lineParams[1])
                {
                    case "theory":
                        return new SlideRecord(slideID, SlideType.Theory, lineParams[2]);
                    case "quiz":
                        return new SlideRecord(slideID, SlideType.Quiz, lineParams[2]);
                    case "exercise":
                        return new SlideRecord(slideID, SlideType.Exercise, lineParams[2]);
                    default:
                        return null;
                }
            })
                .Where(slideRecord => slideRecord != null).ToDictionary(slideRecord => slideRecord.SlideId);
        }

        public static IEnumerable<VisitRecord> ParseVisitRecords(
            IEnumerable<string> lines, IDictionary<int, SlideRecord> slides)
        {
            string format = "yyyy-MM-dd;HH:mm:ss";
            return lines.Skip(1).Select(line =>
            {
                var lineParams = line.Split(new[] { ';' }, 3, StringSplitOptions.RemoveEmptyEntries);
                if (lineParams.Length < 3 ||
                    !int.TryParse(lineParams[0], out int userID) ||
                    !int.TryParse(lineParams[1], out int slideID) ||
                    !slides.TryGetValue(slideID, out SlideRecord slideRecord) ||
                    !DateTime.TryParseExact(
                        lineParams[2],
                        format,
                        CultureInfo.InvariantCulture,
                        DateTimeStyles.None,
                        out DateTime dateTime)
                    )
                    throw new FormatException($"Wrong line [{line}]");

                return new VisitRecord(userID, slideID, dateTime, slideRecord.SlideType);
            });
        }
    }
}

// Практика «Статистика»

using System;
using System.Collections.Generic;
using System.Linq;

namespace linq_slideviews
{
    public static class StatisticsTask
    {
        public static double GetMedianTimePerSlide(List<VisitRecord> visits, SlideType slideType)
        {
            var userVisits = GroupVisitsByUser(visits);
            var timeSpent = new List<double>();

            foreach (var userGroup in userVisits)
            {
                var sortedVisits = SortVisitsByDate(userGroup);
                var visitPairs = CreateVisitPairs(sortedVisits);

                timeSpent.AddRange(CalculateTimeSpent(visitPairs, slideType));
            }

            if (timeSpent.Count == 0)
                return 0;

            return CalculateMedian(timeSpent);
        }

        private static IEnumerable<IGrouping<int, VisitRecord>> GroupVisitsByUser(List<VisitRecord> visits)
        {
            return visits.GroupBy(v => v.UserId);
        }

        private static List<VisitRecord> SortVisitsByDate(IEnumerable<VisitRecord> visits)
        {
            return visits.OrderBy(v => v.DateTime).ToList();
        }

        private static IEnumerable<(VisitRecord, VisitRecord)> CreateVisitPairs(List<VisitRecord> visits)
        {
            return visits.Bigrams();
        }

        private static IEnumerable<double> CalculateTimeSpent(
            IEnumerable<(VisitRecord, VisitRecord)> visitPairs,
            SlideType slideType)
        {
            var timeSpent = new List<double>();

            foreach (var (first, second) in visitPairs)
            {
                if (first.SlideType == slideType)
                {
                    var timeDiff = (second.DateTime - first.DateTime).TotalMinutes;

                    if (timeDiff >= 1 && timeDiff <= 120)
                    {
                        timeSpent.Add(timeDiff);
                    }
                }
            }

            return timeSpent;
        }

        private static double CalculateMedian(List<double> values)
        {
            var sortedValues = values.OrderBy(v => v).ToList();
            int count = sortedValues.Count;
            if (count == 0)
                return 0;

            if (count % 2 == 0)
            {
                return (sortedValues[count / 2 - 1] + sortedValues[count / 2]) / 2;
            }
            else
            {
                return sortedValues[count / 2];
            }
        }
    }
}

// Практика «Метод Гаусса»

using System;
using System.Linq;

namespace GaussAlgorithm
{
    public class Solver
    {
        public double[] Solve(double[][] matrix, double[] freeMembers) =>
            new LinearEquationSystem(matrix, freeMembers).Solve();
    }

    public class LinearEquationSystem
    {
        private const double Epsilon = 1e-6;
        private readonly double[][] matrix;
        private readonly double[] freeMembers;
        public int Height => matrix.Length;
        public int Width => Height == 0 ? 0 : matrix[0].Length;
        private int prepCols, depVarsCount;
        private readonly bool[][] depVars;

        public LinearEquationSystem(double[][] matrix, double[] freeMembers)
        {
            this.matrix = matrix;
            this.freeMembers = freeMembers;
            depVars = Enumerable.Range(0, Height)
                        .Select(_ => new bool[Width])
                        .ToArray();
        }

        private double Normalize(double val) => Math.Abs(val) < Epsilon ? 0 : val;

        public void AddLineMultiple(int target, int source, double multiplier)
        {
            for (int i = 0; i < Width; i++)
                matrix[target][i] = Normalize(matrix[target][i] + multiplier * matrix[source][i]);
            freeMembers[target] = Normalize(freeMembers[target] + multiplier * freeMembers[source]);
        }

        public void MultiplyLine(int idx, double multiplier)
        {
            matrix[idx] = matrix[idx].Select(x => Normalize(x * multiplier)).ToArray();
            freeMembers[idx] = Normalize(freeMembers[idx] * multiplier);
        }

        public void SwitchLines(int i, int j)
        {
            if (i < 0 || i >= Height || j < 0 || j >= Height) return;
            (matrix[i], matrix[j]) = (matrix[j], matrix[i]);
            (freeMembers[i], freeMembers[j]) = (freeMembers[j], freeMembers[i]);
            (depVars[i], depVars[j]) = (depVars[j], depVars[i]);
        }

        private bool RowIsZero(int row) => matrix[row].All(x => x == 0);

        private void PrepareColumn(int rowIdx, int colIdx)
        {
            if (prepCols == Width) return;
            if (rowIdx >= Height) { prepCols++; return; }
            if (matrix[rowIdx][colIdx] == 0) { PrepareColumn(rowIdx + 1, colIdx); return; }
            MultiplyLine(rowIdx, 1 / matrix[rowIdx][colIdx]);
            for (int row = 0; row < Height; row++)
                if (row != rowIdx)
                    AddLineMultiple(row, rowIdx, -matrix[row][colIdx]);
            depVars[rowIdx][colIdx] = true;
            if (rowIdx != prepCols) SwitchLines(rowIdx, prepCols);
            prepCols++; depVarsCount++;
        }

        public double[] Solve()
        {
            while (prepCols < Width)
                PrepareColumn(depVarsCount, prepCols);

            for (int row = 0; row < Height; row++)
                if (RowIsZero(row) && freeMembers[row] != 0)
                    throw new NoSolutionException("No solution!");

            var sol = new double[Width];
            var defined = new bool[Width];

            for (int row = Height - 1; row >= 0; row--)
            {
                if (RowIsZero(row)) continue;
                bool found = false;
                for (int col = Width - 1; col >= 0 && !found; col--)
                {
                    if (depVars[row][col])
                    {
                        double sum = 0;
                        for (int j = col + 1; j < Width; j++)
                            sum += matrix[row][j] * sol[j];
                        sol[col] = (freeMembers[row] - sum) / matrix[row][col];
                        found = true;
                    }
                    else if (!defined[col])
                        sol[col] = 0;
                    defined[col] = true;
                }
            }
            return sol;
        }
    }
}