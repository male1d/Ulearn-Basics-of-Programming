// Генерация последовательности

public static IEnumerable<int> GenerateCycle(int maxValue)
{
    while (true)
    {
        for (int i = 0; i < maxValue; i++)
        {
            yield return i;
        }
    }
}

// ZipSum

private static IEnumerable<int> ZipSum(IEnumerable<int> first, IEnumerable<int> second)
{
    var e1 = first.GetEnumerator();
    var e2 = second.GetEnumerator();

    while (e1.MoveNext() && e2.MoveNext())
    {
        yield return e1.Current + e2.Current;
    }
}

// Особые случаи yield return

yield break в конце метода не нужен , Нельзя в одном методе использовать и обычный return и yield return

// Практика «Экспоненциальное сглаживание»

using System.Collections.Generic;
using System.Linq;

namespace yield
{
    public static class ExpSmoothingTask
    {
        public static IEnumerable<DataPoint> SmoothExponentialy(this IEnumerable<DataPoint> data, double alpha)
        {
            double? lastSmoothedY = null;

            foreach (var dataPoint in data)
            {
                double smoothedY;
                if (lastSmoothedY == null)
                {
                    smoothedY = dataPoint.OriginalY;
                }
                else
                {
                    smoothedY = alpha * dataPoint.OriginalY + (1 - alpha) * lastSmoothedY.Value;
                }

                lastSmoothedY = smoothedY;
                yield return dataPoint.WithExpSmoothedY(smoothedY);
            }
        }
    }
}

// Практика «Скользящее среднее»

using System.Collections.Generic;

namespace yield
{
    public static class MovingAverageTask
    {
        public static IEnumerable<DataPoint> MovingAverage(this IEnumerable<DataPoint> data, int windowWidth)
        {
            if (windowWidth <= 0)
                throw new System.ArgumentOutOfRangeException(nameof(windowWidth), "Ширина окна>0");

            Queue<double> window = new Queue<double>();
            double sum = 0;

            foreach (var dataPoint in data)
            {
                sum += dataPoint.OriginalY;
                window.Enqueue(dataPoint.OriginalY);

                if (window.Count > windowWidth)
                {
                    sum -= window.Dequeue();
                }

                double average = sum / window.Count;
                yield return dataPoint.WithAvgSmoothedY(average);
            }
        }
    }
}

// Практика «Скользящий максимум»

using System;
using System.Collections.Generic;
using System.Linq;

namespace yield
{
    public static class MovingMaxTask
    {
        public static IEnumerable<DataPoint> MovingMax(this IEnumerable<DataPoint> data, int windowWidth)
        {
            ValidateWindowWidth(windowWidth);

            var maxPotentials = new LinkedList<double>();
            var windowNumbers = new Queue<double>();
            int currentWindowSize = 0;

            foreach (DataPoint dataPoint in data)
            {
                currentWindowSize++;

                ProcessDataPoint(dataPoint, windowWidth, currentWindowSize, maxPotentials, windowNumbers);

                double maxY = maxPotentials.First.Value;
                var newDataPoint = dataPoint.WithMaxY(maxY);

                yield return newDataPoint;
            }
        }

        private static void ValidateWindowWidth(int windowWidth)
        {
            if (windowWidth <= 0)
            {
                throw new ArgumentOutOfRangeException();
            }
        }

        private static void ProcessDataPoint(
            DataPoint dataPoint,
            int windowWidth,
            int currentWindowSize,
            LinkedList<double> maxPotentials,
            Queue<double> windowNumbers)
        {
            AdjustWindow(windowWidth, currentWindowSize, maxPotentials, windowNumbers);

            windowNumbers.Enqueue(dataPoint.OriginalY);
            UpdateMaxPotentials(dataPoint.OriginalY, maxPotentials);
        }

        private static void AdjustWindow(
            int windowWidth,
            int currentWindowSize,
            LinkedList<double> maxPotentials,
            Queue<double> windowNumbers)
        {
            if (currentWindowSize > windowWidth)
            {
                RemoveOldestElement(maxPotentials, windowNumbers);
            }
        }

        private static void RemoveOldestElement(LinkedList<double> maxPotentials, Queue<double> windowNumbers)
        {
            if (maxPotentials.Count > 0 && maxPotentials.First.Value == windowNumbers.Peek())
            {
                maxPotentials.RemoveFirst();
            }
            windowNumbers.Dequeue();
        }

        private static void UpdateMaxPotentials(double currentValue, LinkedList<double> maxPotentials)
        {
            while (maxPotentials.Count > 0 && maxPotentials.Last.Value <= currentValue)
            {
                maxPotentials.RemoveLast();
            }

            maxPotentials.AddLast(currentValue);
        }
    }
}