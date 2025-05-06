// Високосный год

public static bool IsLeapYear(int year)
{
    return year % 4 == 0 && (year % 100 != 0 || year % 400 == 0);
}

// Ход ферзя

public static bool IsCorrectMove(string from, string to)
{
    var dx = Math.Abs(to[0] - from[0]);
    var dy = Math.Abs(to[1] - from[1]);
    if ((dx > 0) && (dy > 0) && (dx == dy))
        return true;
    else if (((dx == 0) && (dy > 0)) || ((dy == 0) && (dx > 0)))
        return true;
    else
        return false;
}

// Среднее трех

public static int MiddleOf(int a, int b, int c)
{
    if (a < b)
    {
        if (b < c)
        {
            return b;
        }
        else if (a > c)
        {
            return a;
        }
        else return c;
    }
    else if (a > b)
    {
        if (a < c)
        {
            return a;
        }
        else if (b > c)
        {
            return b;
        }
        else return c;
    }
    else return a;
}

// Логические выражения и условия

1. Что может стоять на месте [Нечто]? Отметьте все верные утверждения. 

Сравнение чисел или строк
Любое выражение типа bool
Вызов метода, возвращающего bool
Переменная, поле или свойство класса типа bool

2. Что не так в этом коде? 

Двойное отрицание в условии

3. Не будем анализировать юмор в этом анекдоте, просто отметьте все верные утверждения про этот код

Всё

// Управление роботом

public static bool ShouldFire2(bool enemyInFront, string enemyName, int robotHealth)
{
    return (enemyInFront && (enemyName == "boss" && (robotHealth >= 50))) || (enemyInFront && (enemyName != "boss"));
}

// Практика «Рубль -лей -ля»

namespace Pluralize;

public static class PluralizeTask
{
    public static string PluralizeRubles(int count)
    {
        if (count % 10 == 1 && count % 100 != 11) return "рубль";

        if (count % 10 >= 2 && count % 10 <= 4 && (count % 100 < 12 || count % 100 > 14)) return "рубля";
        return "рублей";
    }
}

// Практика «Два прямоугольника»

using System;

namespace Rectangles
{
    public static class RectanglesTask
    {
        public static bool AreIntersected(Rectangle r1, Rectangle r2)
        {
            return (r1.Left <= r2.Left + r2.Width && r1.Left + r1.Width >= r2.Left) &&
                   (r1.Top <= r2.Top + r2.Height && r1.Top + r1.Height >= r2.Top);
        }


        public static int IntersectionSquare(Rectangle r1, Rectangle r2)
        {
            if (!AreIntersected(r1, r2))
            {
                return 0;
            }


            int intersectionLeft = Math.Max(r1.Left, r2.Left);
            int intersectionTop = Math.Max(r1.Top, r2.Top);


            int intersectionWidth = Math.Min(r1.Left + r1.Width, r2.Left + r2.Width) - intersectionLeft;
            int intersectionHeight = Math.Min(r1.Top + r1.Height, r2.Top + r2.Height) - intersectionTop;

            return intersectionWidth * intersectionHeight;
        }


        public static int IndexOfInnerRectangle(Rectangle r1, Rectangle r2)
        {
            if (r1.Left >= r2.Left && r1.Left + r1.Width <= r2.Left + r2.Width &&
                r1.Top >= r2.Top && r1.Top + r1.Height <= r2.Top + r2.Height)
            {
                return 0;
            }


            if (r2.Left >= r1.Left && r2.Left + r2.Width <= r1.Left + r1.Width &&
                r2.Top >= r1.Top && r2.Top + r2.Height <= r1.Top + r1.Height)
            {
                return 1;
            }

            return -1;
        }
    }
}

// Практика «Расстояние до отрезка»

using System;

namespace DistanceTask
{
    public static class DistanceTask
    {
        public static double GetDistanceToSegment(double ax, double ay, double bx, double by, double x, double y)
        {
            // проверка совпадают ли точки A и B
            if (ax == bx && ay == by)
            {
                // если точки совпадают , то вычисляем расстояние до точки A (или B т.к они совпадают)
                return Math.Sqrt((x - ax) * (x - ax) + (y - ay) * (y - ay));
            }
            // проверка находится ли точка вне отрезка AB
            else if ((x - ax) * (bx - ax) + (y - ay) * (by - ay) < 0 ||
                     (x - bx) * (ax - bx) + (y - by) * (ay - by) < 0)
            {
                // если точка P вне отрезка вычисляем расстояние от точки до точек A и B
                // затем выбираем минимальное расстояние
                double distanceToA = Math.Sqrt((x - ax) * (x - ax) + (y - ay) * (y - ay));
                double distanceToB = Math.Sqrt((x - bx) * (x - bx) + (y - by) * (y - by));
                return Math.Min(distanceToA, distanceToB);
            }
            else
            {
                // если точка на отрезке AB вычисляем расстояние от точки до отрезка AB
                return Math.Abs((bx - ax) * (y - ay) - (by - ay) * (x - ax)) /
                       Math.Sqrt((bx - ax) * (bx - ax) + (by - ay) * (by - ay));
            }
        }
    }
}

