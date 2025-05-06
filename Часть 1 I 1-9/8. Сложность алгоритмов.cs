// Базовые понятия

1.Верно
2. Существует некоторый вход размера n n, на котором алгоритм F F выполнит в точности f(n)f(n) "элементарных" операций.
3. Да, так как любая информация (как переданная на вход, так и вычисленная алгоритмом) может быть описана последовательностью байт, а байты можно считать буквами алфавита.
4. Зависит от выбранного алфавита и способа кодирования входа

// Сложность и скорость

1. Не исключено, что G может выполняться быстрее F на некоторых входах.
2. Всё

// O-символика

1. Верно
2. Верно
3. Верно
4. Неверно
5. Неверно
6. Верно
7. Неверно
8. Верно
9. Верно

// Оценка сложности алгоритма

1. Всё кроме Θ(n)
2. Всё кроме O(n) , o(n 2) , Θ(n)
3. Θ(2 n)

// O-символика 2

1. Всё кроме f(n)=o(2n+5)f(n)=o(2n+5) , f(n)=Θ(n+10/n) , f(n)=Θ(n)
2. Всё кроме G может быть таким, что для любого nn, он делает n2n 2  операций на некотором входе размера nn.
3. Всё кроме 0.0001f(n)=o(g(n))
4. Существует такой вход, на котором FF выполнит меньше операций, чем G , Начиная с некоторого размера входа FF в худшем случае выполняет меньше операций, чем GG в худшем случае

// Практика «Оттенки серого»

namespace Recognizer
{
    public static class GrayscaleTask
    {
        const double Red = 0.299;
        const double Green = 0.587;
        const double Blue = 0.114;

        public static double[,] ToGrayscale(Pixel[,] original)
        {
            var width = original.GetLength(0);
            var height = original.GetLength(1);
            var grayscale = new double[width, height];

            for (var x = 0; x < width; x++)
            {
                for (var y = 0; y < height; y++)
                {
                    var pixel = original[x, y];
                    grayscale[x, y] = (Red * pixel.R + Green * pixel.G + Blue * pixel.B) / 255;
                }
            }

            return grayscale;
        }
    }
}

// Практика «Медианный фильтр»

using System.Collections.Generic;

namespace Recognizer
{
    internal static class MedianFilterTask
    {
        public static double[,] MedianFilter(double[,] original)
        {
            var width = original.GetLength(0);
            var height = original.GetLength(1);
            var filteredPixels = new double[width, height];

            for (var x = 0; x < width; x++)
            {
                for (var y = 0; y < height; y++)
                {
                    filteredPixels[x, y] = GetMedianValue(x, y, original, width, height);
                }
            }
            return filteredPixels;
        }
        private static double GetMedianValue(int x, int y, double[,] original, int width, int height)
        {
            var edgeValues = new List<double>();
            for (var offsetX = -1; offsetX < 2; offsetX++)
            {
                for (var offsetY = -1; offsetY < 2; offsetY++)
                {
                    if (PixelIsInsideBoundaries(x + offsetX, y + offsetY, width, height))
                    {
                        edgeValues.Add(original[x + offsetX, y + offsetY]);
                    }
                }
            }

            edgeValues.Sort();

            return GetMedianValue(edgeValues);
        }

        private static bool PixelIsInsideBoundaries(int x, int y, int width, int height)
        {
            return (x > -1 && y > -1) && (x < width && y < height);
        }

        private static double GetMedianValue(List<double> edge)
        {
            if (edge.Count % 2 == 0)
            {
                return (edge[edge.Count / 2 - 1] + edge[edge.Count / 2]) / 2;
            }
            else
            {
                return edge[edge.Count / 2];
            }
        }
    }
}

// Практика «Пороговый фильтр»

using System.Collections.Generic;

namespace Recognizer
{
    public static class ThresholdFilterTask
    {
        public static double[,] ThresholdFilter(double[,] original, double threshold)
        {
            var width = original.GetLength(0);
            var height = original.GetLength(1);
            var filteredPixels = new double[width, height];

            var pixels = new List<double>(width * height);
            for (var x = 0; x < width; x++)
            {
                for (var y = 0; y < height; y++)
                {
                    pixels.Add(original[x, y]);
                }
            }
            pixels.Sort();

            var thresholdValue = GetThresholdValue(pixels, width * height, (int)(width * height * threshold));

            for (var x = 0; x < width; x++)
            {
                for (var y = 0; y < height; y++)
                {
                    filteredPixels[x, y] = (original[x, y] >= thresholdValue) ? 1.0 : 0.0;
                }
            }

            return filteredPixels;
        }

        private static double GetThresholdValue(List<double> pixels, int numberOfPixels, int pixelsToChange)
        {
            if (pixelsToChange == numberOfPixels)
            {
                return -1.0;
            }
            else if (pixelsToChange == 0)
            {
                return double.MaxValue;
            }
            else
            {
                return pixels[numberOfPixels - pixelsToChange];
            }
        }
    }
}

// Практика «Фильтр Собеля»

using System;

namespace Recognizer
{
    internal static class SobelFilterTask
    {
        public static double[,] SobelFilter(double[,] original, double[,] sx)
        {
            var width = original.GetLength(0);
            var height = original.GetLength(1);
            var filteredPixels = new double[width, height];

            var offsetX = sx.GetLength(0) / 2;
            var offsetY = sx.GetLength(1) / 2;

            var sy = new double[sx.GetLength(1), sx.GetLength(0)];
            for (var x = 0; x < sx.GetLength(0); x++)
            {
                for (var y = 0; y < sx.GetLength(1); y++)
                {
                    sy[y, x] = sx[x, y];
                }
            }

            for (var x = offsetX; x < width - offsetX; x++)
            {
                for (var y = offsetY; y < height - offsetY; y++)
                {
                    var gx = GetConvolution(original, sx, x, y, offsetX);
                    var gy = GetConvolution(original, sy, x, y, offsetY);

                    filteredPixels[x, y] = Math.Sqrt(gx * gx + gy * gy);
                }
            }

            return filteredPixels;
        }
        private static double GetConvolution(double[,] original, double[,] s, int x, int y, int offset)
        {
            var width = s.GetLength(0);
            var height = s.GetLength(1);
            var result = 0.0;

            for (var sx = 0; sx < width; sx++)
            {
                for (var sy = 0; sy < height; sy++)
                {
                    result += s[sx, sy] * original[x + sx - offset, y + sy - offset];
                }
            }

            return result;
        }
    }
}

