// Создание классов

public class City
{
    public string Name { get; set; }
    public GeoLocation Location { get; set; }
}

public class GeoLocation
{
    public double Latitude { get; set; }
    public double Longitude { get; set; }
}

public class Program
{
    public static void Main()
    {
        var city = new City();
        city.Name = "Ekaterinburg";
        city.Location = new GeoLocation();
        city.Location.Latitude = 56.50;
        city.Location.Longitude = 60.35;
        Console.WriteLine("I love {0} located at ({1}, {2})",
            city.Name,
            city.Location.Longitude.ToString(CultureInfo.InvariantCulture),
            city.Location.Latitude.ToString(CultureInfo.InvariantCulture));
    }
}

// Сокращенный синтаксис

public static MenuItem[] GenerateMenu()
        {
            return new MenuItem[]
            {
                new MenuItem {Caption = "File", HotKey = "F", Items = new MenuItem[] { new MenuItem {Caption = "New", HotKey = "N", Items = null }, new MenuItem {Caption = "Save", HotKey = "S", Items = null } } },
                new MenuItem {Caption = "Edit", HotKey = "E", Items = new MenuItem[] { new MenuItem {Caption = "Copy", HotKey = "C", Items = null }, new MenuItem {Caption = "Paste", HotKey = "V", Items = null } } }
            };
        }

// Поля классов

1. SomeClass.s = 42
2. 42 43

// Методы классов

1. Верно
2. Неверно
3. 1 1 2 1 3 2
4. Верно
5. Верно

// Карты памяти

1. B
2. A
3. B
4. C
5. C
6. C

// Создание методов расширения

public static class MyExtensions
{
    public static int ToInt(this string str)
    {
        return int.Parse(str); 
    }
}

// Список директорий

public static List<DirectoryInfo> GetAlbums(List<FileInfo> files)
{
	List<DirectoryInfo> ext = new List<DirectoryInfo>();
	foreach(var file in files)
	if(file.Extension == ".mp3" || file.Extension == ".wav" && !ext.Any(x => x.FullName == file.DirectoryName))
	ext.Add(file.Directory);
	return ext;
}


// Рефакторинг статического класса

public class SuperBeautyImageFilter
{
    public string ImageName;
    public double GaussianParameter;
    public void Run()
    {
        Console.WriteLine("Processing {0} with parameter {1}", 
            this.ImageName, 
            this.GaussianParameter.ToString(CultureInfo.InvariantCulture));
    }
}

// Практика «Вектор»

using System;

namespace Geometry
{
    public class Vector
    {
        public double X;
        public double Y;
    }

    public static class Geometry
    {
        public static double GetLength(Vector v)
        {
            return Math.Sqrt(v.X * v.X + v.Y * v.Y);
        }

        public static Vector Add(Vector v1, Vector v2)
        {
            return new Vector { X = v1.X + v2.X, Y = v1.Y + v2.Y };
        }
    }
}

// Практика «Отрезок»

using System;

namespace Geometry
{
    public class Vector
    {
        public double X;
        public double Y;
    }

    public class Segment
    {
        public Vector Begin;
        public Vector End;
    }

    public static class Geometry
    {
        public static double GetLength(Vector v)
        {
            return Math.Sqrt(v.X * v.X + v.Y * v.Y);
        }

        public static Vector Add(Vector v1, Vector v2)
        {
            return new Vector { X = v1.X + v2.X, Y = v1.Y + v2.Y };
        }

        public static double GetLength(Segment s)
        {
            Vector diff = new Vector { X = s.End.X - s.Begin.X, Y = s.End.Y - s.Begin.Y };
            return GetLength(diff);
        }

        public static bool IsVectorInSegment(Vector v, Segment s)
        {
            double segmentLength = GetLength(s);
            double distToBegin = GetLength(new Vector { X = v.X - s.Begin.X, Y = v.Y - s.Begin.Y });
            double distToEnd = GetLength(new Vector { X = v.X - s.End.X, Y = v.Y - s.End.Y });

            return Math.Abs(distToBegin + distToEnd - segmentLength) < 1e-6;
        }
    }
}

// Практика «Нестатические методы»

using System;

namespace Geometry
{
    public class Vector
    {
        public double X;
        public double Y;

        public double GetLength()
        {
            return Geometry.GetLength(this);
        }

        public Vector Add(Vector v)
        {
            return Geometry.Add(this, v);
        }

        public bool Belongs(Segment s)
        {
            return Geometry.IsVectorInSegment(this, s);
        }
    }

    public class Segment
    {
        public Vector Begin;
        public Vector End;

        public double GetLength()
        {
            return Geometry.GetLength(this);
        }

        public bool Contains(Vector v)
        {
            return Geometry.IsVectorInSegment(v, this);
        }
    }

    public static class Geometry
    {
        public static double GetLength(Vector v)
        {
            return Math.Sqrt(v.X * v.X + v.Y * v.Y);
        }

        public static Vector Add(Vector v1, Vector v2)
        {
            return new Vector { X = v1.X + v2.X, Y = v1.Y + v2.Y };
        }

        public static double GetLength(Segment s)
        {
            Vector diff = new Vector { X = s.End.X - s.Begin.X, Y = s.End.Y - s.Begin.Y };
            return GetLength(diff);
        }

        public static bool IsVectorInSegment(Vector v, Segment s)
        {
            double segmentLength = GetLength(s);
            double distToBegin = GetLength(new Vector { X = v.X - s.Begin.X, Y = v.Y - s.Begin.Y });
            double distToEnd = GetLength(new Vector { X = v.X - s.End.X, Y = v.Y - s.End.Y });

            return Math.Abs(distToBegin + distToEnd - segmentLength) < 1e-6;
        }
    }
}

