// В поисках степени двойки

private static int GetMinPowerOfTwoLargerThan(int number)
{
    int result = 1;
    while (result <= number)
    {
        result *= 2;
    }
    return result;
}

// Убрать пробелы

public static string RemoveStartSpaces(string text)
{
    int i = 0;
    while (i < text.Length && char.IsWhiteSpace(text[i]))
    {
        i++;
    }
    return text.Substring(i);
}

// Рамочка

private static void WriteTextWithBorder(string text)
{
    string border = "+";
    string line = "| " + text + " |";
    for (int i = 0; i < text.Length + 2; i++)
        border += ('-');
    border += '+';
    Console.WriteLine(border);
    Console.WriteLine(line);
    Console.WriteLine(border);
}

// Шахматная доска

private static void WriteBoard(int size)
{
    string str1 = "#.#.#.#.#.#.#.#.#.#.#.#.";
    string str2 = ".#.#.#.#.#.#.#.#.#.#.#.#";
    for (int i = 0; i < size; i++)
    {
        if (i % 2 == 0) { Console.WriteLine(str1.Substring(0, size)); }
        else { Console.WriteLine(str2.Substring(0, size)); }
    }
    Console.WriteLine();
}

// Практика «Пустой лабиринт»

namespace Mazes
{
    public static class EmptyMazeTask
    {
        public static void MoveOut(Robot robot, int distance, Direction dir)
        {
            for (int a = 0; a < distance - 3; ++a)
                robot.MoveTo(dir);
        }

        public static void MoveOut(Robot robot, int width, int height)
        {
            MoveOut(robot, width, Direction.Right);
            MoveOut(robot, height, Direction.Down);
        }
    }
}

// Практика «Лабиринт змейка»

namespace Mazes
{
    public static class SnakeMazeTask
    {
        public static void MoveOut(Robot robot, int distance, Direction dir)
        {
            for (int i = 0; i < distance - 3; ++i)
                robot.MoveTo(dir);
        }

        public static void MakeCurve(Robot robot, int width, int height)
        {
            MoveOut(robot, width, Direction.Right);
            robot.MoveTo(Direction.Down);
            robot.MoveTo(Direction.Down);
            MoveOut(robot, width, Direction.Left);
        }

        public static void MoveOut(Robot robot, int width, int height)
        {
            int len = height > 5 ? height / 4 : 1;
            for (int i = 0; i < len; i++)
            {
                MakeCurve(robot, width, height);
                if (i < len - 1)
                {
                    robot.MoveTo(Direction.Down);
                    robot.MoveTo(Direction.Down);
                }
            }
        }
    }
}

// Практика «Лабиринт диагональ»

namespace Mazes
{
    public static class DiagonalMazeTask
    {
        public static void MoveOut(Robot robot, int width, int height)
        {
            var mainDirection = GetMainDirection(width, height);
            var steps = GetSteps(width, height);

            MoveAlongDiagonal(robot, mainDirection, steps, width, height);
        }

        private static Direction GetMainDirection(int width, int height)
        {
            return width > height ? Direction.Right : Direction.Down;
        }

        private static int GetSteps(int width, int height)
        {
            return width > height ? width / (height - 1) : (height - 3) / (width - 2);
        }

        private static void MoveAlongDiagonal(Robot robot, Direction mainDirection, int steps, int width, int height)
        {
            for (int k = 0; k < (width > height ? height : width) - 2; ++k)
            {
                MoveInMainDirection(robot, mainDirection, steps);

                if (k != (width > height ? height : width) - 3)
                {
                    MoveInOppositeDirection(robot, mainDirection);
                }
            }
        }

        private static void MoveInMainDirection(Robot robot, Direction mainDirection, int steps)
        {
            for (int i = 0; i < steps; i++)
            {
                robot.MoveTo(mainDirection);
            }
        }

        private static void MoveInOppositeDirection(Robot robot, Direction mainDirection)
        {
            robot.MoveTo(mainDirection == Direction.Right ? Direction.Down : Direction.Right);
        }
    }
}

// Практика «Dragon curve»

using System;

namespace Fractals
{
    internal static class DragonFractalTask
    {
        public static void DrawDragonFractal(Pixels pixels, int iterationsCount, int seed)
        {
            double x = 1.0;
            double y = 0.0;
            double angle1 = Math.PI * 45 / 180;
            double angle2 = Math.PI * 135 / 180;
            Random random = new Random(seed);
            for (int i = 0; i < iterationsCount; i++)
            {
                bool rotate45 = random.Next(2) == 0;
                (x, y) = ApplyRotation(x, y, angle1, angle2, rotate45);
                pixels.SetPixel(x, y);
            }
        }

        private static (double, double) ApplyRotation(double x, double y, double angle1, double angle2, bool rotate45)
        {
            if (rotate45)
            {
                return ((x * Math.Cos(angle1) - y * Math.Sin(angle1)) / Math.Sqrt(2),
                        (x * Math.Sin(angle1) + y * Math.Cos(angle1)) / Math.Sqrt(2));
            }
            else
            {
                (double rotatedX, double rotatedY) = ((x * Math.Cos(angle2) - y * Math.Sin(angle2)) / Math.Sqrt(2),
                                                    (x * Math.Sin(angle2) + y * Math.Cos(angle2)) / Math.Sqrt(2));
                return (rotatedX + (rotate45 ? 0 : 1), rotatedY);
            }
        }
    }
}