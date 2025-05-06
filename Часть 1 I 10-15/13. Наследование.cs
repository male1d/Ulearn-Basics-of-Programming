// Наследование

1. FieldA
2. FieldA FieldB
3. FieldA FieldC
4. FieldA FieldC FieldD

// Касты

1. Upcast
2. Пройдет без ошибок
3. Downcast
4. Вызовет ошибку на этапе выполнения
5. ClassB
6. Ни то, ни другое
7. C

// Всем печать!

public static void Print(params object[] args)
{
    for (int i = 0; i < args.Length; i++)
    {
        if (i > 0) Console.Write(", ");
        Console.Write(args[i] ?? ""); //Обработка null
    }
    Console.WriteLine();
}

// Склейка массивов

static private bool TypeInArray(params Array[] array)
        {
            bool flag = true;
            Type firstType =null; 
            foreach(var arr in array)
            {
              if(firstType==null)   firstType = arr.GetType().GetElementType();
                Type nextType = arr.GetType().GetElementType();
                if(firstType!= nextType) return false;
            }
            return flag;
        }
        static private Array Combine(params Array[] array)
        {
            if (array.Length == 0) return null;
            if (!TypeInArray(array)) return null;
           var resultList = new List<object>();
           foreach(var i in array)
            {
                foreach (var item in i)
                {
                    resultList.Add(item);
                }
            }
            return resultList.ToArray();
        }

// Снова среднее трех

private static object MiddleOfThree(params object[] objects)
{
Array.Sort(objects);
return objects[1];
}

// Поиск минимума

static T Min<T>(T[] list) where T : IComparable
 {
    T min = list[0];

        foreach (T item in list)
        {
            if (item.CompareTo(min) < 0)
            {
                min = item;
            }
        }

        return min;
 }

// Сравнение книг

public class Book : IComparable<Book>, IComparable
{
    public string Title;
    public int Theme;

    public int CompareTo(Book other)
    {
        if (ReferenceEquals(other, null)) return 1;

        int themeComparison = Theme.CompareTo(other.Theme);
        if (themeComparison != 0) return themeComparison;
        return string.Compare(Title, other.Title, StringComparison.OrdinalIgnoreCase);
    }

    public int CompareTo(object obj)
    {
        if (obj == null) return 1;
        if (!(obj is Book other)) throw new ArgumentException("Object is not a Book");
        return CompareTo(other);
    }
}

// Против часовой стрелки

public class ClockwiseComparer : IComparer
{
public int DeterminePart(double cosObj, double sinObj)
{
if(cosObj >= 0 && sinObj >= 0)
{
return 1;
} else if(cosObj <= 0 && sinObj >= 0)
{
return 2;
} else if(cosObj <= 0 && sinObj <= 0)
{
return 3;
}
else if(cosObj >= 0 && sinObj >= -1)
{
return 4;
}
return 5;
}
public double GetDistance(object obj)
{
var result = (Point)obj;
return Math.Sqrt(result.X * result.X + result.Y * result.Y);
}
public int Compare(object x, object y)
{
if(x is Point && y is Point)
{
var cosX = ((Point)x).X / GetDistance(x);
var sinX = ((Point)x).Y / GetDistance(x);
var cosY = ((Point)y).X / GetDistance(y);
var sinY = ((Point)y).Y / GetDistance(y);
var partX = DeterminePart(cosX, sinX);
var partY = DeterminePart(cosY, sinY);
if (partX == partY)
{
switch(partY)
{
case 1:
return sinX.CompareTo(sinY);
case 2:
return sinX.CompareTo(sinY) * (-1);
case 3:
return sinX.CompareTo(sinY);
case 4:
return sinX.CompareTo(sinY);
}
} else
{
return partX.CompareTo(partY);
}
}
throw new Exception();
}
}

// Метод ToString

public class Triangle
{
    public Point A;
    public Point B;
    public Point C;

    public override string ToString()
    {
        return $"({A.X} {A.Y}) ({B.X} {B.Y}) ({C.X} {C.Y})";
    }
}

// Практика «Земля и Диггер»

using System;
using Avalonia.Input;
using Digger.Architecture;

namespace Digger;

public class Terrain : ICreature
{
    public string GetImageFileName() => "Terrain.png";

    public bool DeadInConflict(ICreature conflictObj) => true;

    public int GetDrawingPriority() => 0;

    public CreatureCommand Act(int x, int y) => new CreatureCommand { DeltaX = 0, DeltaY = 0 };
}

public class Player : ICreature
{
    public string GetImageFileName() => "Digger.png";

    public bool DeadInConflict(ICreature conflictObj) => false;

    public int GetDrawingPriority() => 1;

    public CreatureCommand Act(int x, int y)
    {
        var key = Game.KeyPressed;
        var command = new CreatureCommand { DeltaX = 0, DeltaY = 0 };

        switch (key)
        {
            case Key.Down when y < Game.MapHeight - 1: command.DeltaY = 1; break;
            case Key.Up when y > 0: command.DeltaY = -1; break;
            case Key.Left when x > 0: command.DeltaX = -1; break;
            case Key.Right when x < Game.MapWidth - 1: command.DeltaX = 1; break;
        }

        return command;
    }
}

// Практика «Мешки и Золото»

using System;
using System.Diagnostics.Metrics;
using System.Threading;
using Avalonia.Input;
using Digger.Architecture;
 
namespace Digger;
 
public class Terrain : ICreature
{
    public string GetImageFileName()
    {
        return "Terrain.png";
    }
 
    public bool DeadInConflict(ICreature conflictObj)
    {
        return true;
    }
 
    public int GetDrawingPriority()
    {
        return 0;
    }
 
    public CreatureCommand Act(int x, int y)
    {
        return new CreatureCommand { DeltaX = 0, DeltaY = 0 };
    }
}
 
public class Gold : ICreature
{
    public string GetImageFileName()
    {
        return "Gold.png";
    }
 
    public int GetDrawingPriority()
    {
        return 4;
    }
 
    public CreatureCommand Act(int x, int y)
    {
        return new CreatureCommand { DeltaX = 0, DeltaY = 0 } ;
    }
 
    public bool DeadInConflict(ICreature conflictObj)
    {
        return true;
    }
}
 
public class Sack : ICreature
{
    private int counter = 0;
    public string GetImageFileName()
    {
        return "Sack.png";
    }
 
    public int GetDrawingPriority()
    {
        return 3;
    }
 
    public CreatureCommand Act(int x, int y)
    {
        if (y < Game.MapHeight - 1)
        {
            var map = Game.Map[x, y + 1];
            if (map == null ||
                (counter > 0 && (map.ToString() == "Digger.Player" ||
                map.ToString() == "Digger.Monster")))
            {
                counter++;
                return new CreatureCommand { DeltaX = 0, DeltaY = 1 };
            }
        }
 
        if (counter > 1)
        {
            counter = 0;
            return new CreatureCommand { DeltaX = 0, DeltaY = 0, TransformTo = new Gold() };
        }
        counter = 0;
        return new CreatureCommand { DeltaX = 0, DeltaY = 0};
    }
 
    public bool DeadInConflict(ICreature conflictedObject)
    {
        return false;
    }
}
 
public class Monster : ICreature
{
    public string GetImageFileName()
    {
        return "Monster.png";
    }
 
    public int GetDrawingPriority()
    {
        return 2;
    }
 
    public bool DeadInConflict(ICreature conflictedObject)
    {
        return conflictedObject is Sack || conflictedObject is Monster;
    }
 
    public CreatureCommand Act(int x, int y)
    {
        (var playerX, var playerY) = Player.GetCurrentPosition();
        if (playerX == -1)
            return new CreatureCommand { DeltaX = 0, DeltaY = 0 };;
 
        (var moveX, var moveY) = Player.GetMoveToPlayer(x, y, playerX, playerY);
 
        if (x + moveX != playerX || y + moveY != playerY - 1)
            return CanMonsterMoveTo(x, y, moveX, moveY)
                ? new CreatureCommand { DeltaX = 0, DeltaY = 0 }
                : new CreatureCommand { DeltaX = moveX, DeltaY = moveY };
        Game.Map[x + moveX, y + moveY] = null;
        return new CreatureCommand { DeltaX = 0, DeltaY = 0};
    }
 
    private static bool CanMonsterMoveTo(int x, int y, int moveX, int moveY)
    {
        var newX = x + moveX;
        var newY = y + moveY;
 
        var isOutsideMap = newX < 0 || newY < 0 || newX >= Game.MapWidth || newY >= Game.MapHeight;
        if (isOutsideMap)
            return true;
 
        var newPosition = Game.Map[newX, newY];
 
        return newPosition is Sack || newPosition is Monster || newPosition is Terrain;
    }
}
 
public class Player : ICreature
{
    public string GetImageFileName()
    {
        return "Digger.png";
    }
 
    public static (int, int) GetMoveToPlayer(int x, int y, int playerX, int playerY)
    {
        var deltaX = 0;
        var deltaY = 0;
 
        if (x != playerX)
        {
            deltaX = (x < playerX) ? 1 : -1;
        }
        else if (y != playerY)
        {
            deltaY = (y < playerY) ? 1 : -1;
        }
 
        return (deltaX, deltaY);
    }
 
    public bool DeadInConflict(ICreature conflictObj)
    {
        if (conflictObj is Gold)
        {
            Game.Scores += 10;
        }
        return conflictObj is Monster || conflictObj is Sack;
    }
 
    public int GetDrawingPriority()
    {
        return 1;
    }
 
    public static (int playerX, int playerY) GetCurrentPosition()
    {
        for (var i = 0; i < Game.MapWidth; i++)
        {
            for (var j = 0; j < Game.MapHeight; j++)
            {
                if (Game.Map[i, j] is Player)
                    return (i, j);
            }
        }
 
        return (-1, -1);
    }
 
    public CreatureCommand Act(int x, int y)
			{
						var key = Game.KeyPressed;
						var command = new CreatureCommand { DeltaX = 0, DeltaY = 0 };
 
						command = HandleMovement(x, y, key, command);

						if (IsObstacle(x + command.DeltaX, y + command.DeltaY))
    					{
							return new CreatureCommand { DeltaX = 0, DeltaY = 0 };
    					}

						return command;
			}

	private CreatureCommand HandleMovement(int x, int y, Key key, CreatureCommand command)
	{
    	switch (key)
    	{
        	case Key.Down: command = MoveDown(x, y, command); break;
        	case Key.Up:   command = MoveUp(x, y, command); break;
        	case Key.Left: command = MoveLeft(x, y, command); break;
        	case Key.Right:command = MoveRight(x, y, command); break;
    	}
    	return command;
	}


	private CreatureCommand MoveDown(int x, int y, CreatureCommand command)
	{
    	if (y < Game.MapHeight - 1)
    	{
        	command.DeltaY = 1;
    	}
    	return command;
	}

	private CreatureCommand MoveUp(int x, int y, CreatureCommand command)
	{
    	if (y > 0)
    	{
        	command.DeltaY = -1;
    	}
    	return command;
	}

	private CreatureCommand MoveLeft(int x, int y, CreatureCommand command)
	{
    	if (x > 0)
    	{
        	command.DeltaX = -1;
    	}
    	return command;
	}

	private CreatureCommand MoveRight(int x, int y, CreatureCommand command)
	{
    	if (x < Game.MapWidth - 1)
    	{
        	command.DeltaX = 1;
    	}
    	return command;
	}

	private bool IsObstacle(int x, int y)
	{
    	return Game.Map[x, y] is Sack;
	}
}

// Практика «Монстры»

using System;
using System.Diagnostics.Metrics;
using System.Threading;
using Avalonia.Input;
using Digger.Architecture;
 
namespace Digger;
 
public class Terrain : ICreature
{
    public string GetImageFileName()
    {
        return "Terrain.png";
    }
 
    public bool DeadInConflict(ICreature conflictObj)
    {
        return true;
    }
 
    public int GetDrawingPriority()
    {
        return 0;
    }
 
    public CreatureCommand Act(int x, int y)
    {
        return new CreatureCommand { DeltaX = 0, DeltaY = 0 };
    }
}
 
public class Gold : ICreature
{
    public string GetImageFileName()
    {
        return "Gold.png";
    }
 
    public int GetDrawingPriority()
    {
        return 4;
    }
 
    public CreatureCommand Act(int x, int y)
    {
        return new CreatureCommand { DeltaX = 0, DeltaY = 0 } ;
    }
 
    public bool DeadInConflict(ICreature conflictObj)
    {
        return true;
    }
}
 
public class Sack : ICreature
{
    private int counter = 0;
    public string GetImageFileName()
    {
        return "Sack.png";
    }
 
    public int GetDrawingPriority()
    {
        return 3;
    }
 
    public CreatureCommand Act(int x, int y)
    {
        if (y < Game.MapHeight - 1)
        {
            var map = Game.Map[x, y + 1];
            if (map == null ||
                (counter > 0 && (map.ToString() == "Digger.Player" ||
                map.ToString() == "Digger.Monster")))
            {
                counter++;
                return new CreatureCommand() { DeltaX = 0, DeltaY = 1 };
            }
        }
 
        if (counter > 1)
        {
            counter = 0;
            return new CreatureCommand() { DeltaX = 0, DeltaY = 0, TransformTo = new Gold() };
        }
        counter = 0;
        return new CreatureCommand() { DeltaX = 0, DeltaY = 0};
    }
 
    public bool DeadInConflict(ICreature conflictedObject)
    {
        return false;
    }
}
 
public class Monster : ICreature
{
    public string GetImageFileName()
    {
        return "Monster.png";
    }
 
    public int GetDrawingPriority()
    {
        return 2;
    }
 
    public bool DeadInConflict(ICreature conflictedObject)
    {
        return conflictedObject is Sack || conflictedObject is Monster;
    }
 
    public CreatureCommand Act(int x, int y)
    {
        (var playerX, var playerY) = Player.GetCurrentPosition();
        if (playerX == -1)
            return new CreatureCommand();
 
        (var moveX, var moveY) = Player.GetMoveToPlayer(x, y, playerX, playerY);
 
        if (x + moveX != playerX || y + moveY != playerY - 1)
            return CanMonsterMoveTo(x, y, moveX, moveY)
                ? new CreatureCommand { DeltaX = 0, DeltaY = 0 }
                : new CreatureCommand { DeltaX = moveX, DeltaY = moveY };
        Game.Map[x + moveX, y + moveY] = null;
        return new CreatureCommand { DeltaX = 0, DeltaY = 0};
    }
 
    private static bool CanMonsterMoveTo(int x, int y, int moveX, int moveY)
    {
        var newX = x + moveX;
        var newY = y + moveY;
 
        var isOutsideMap = newX < 0 || newY < 0 || newX >= Game.MapWidth || newY >= Game.MapHeight;
        if (isOutsideMap)
            return true;
 
        var newPosition = Game.Map[newX, newY];
 
        return newPosition is Sack || newPosition is Monster || newPosition is Terrain;
    }
}
 
public class Player : ICreature
{
    public string GetImageFileName()
    {
        return "Digger.png";
    }
 
    public static (int, int) GetMoveToPlayer(int x, int y, int playerX, int playerY)
    {
        var deltaX = 0;
        var deltaY = 0;
 
        if (x != playerX)
        {
            deltaX = (x < playerX) ? 1 : -1;
        }
        else if (y != playerY)
        {
            deltaY = (y < playerY) ? 1 : -1;
        }
 
        return (deltaX, deltaY);
    }
 
    public bool DeadInConflict(ICreature conflictObj)
    {
        if (conflictObj is Gold)
        {
            Game.Scores += 10;
        }
        return conflictObj is Monster || conflictObj is Sack;
    }
 
    public int GetDrawingPriority()
    {
        return 1;
    }
 
    public static (int playerX, int playerY) GetCurrentPosition()
    {
        for (var i = 0; i < Game.MapWidth; i++)
        {
            for (var j = 0; j < Game.MapHeight; j++)
            {
                if (Game.Map[i, j] is Player)
                    return (i, j);
            }
        }
 
        return (-1, -1);
    }
 
    public CreatureCommand Act(int x, int y)
	{
    	var key = Game.KeyPressed;
    	var command = GetMovementCommand(key, x, y);
	
    	if (Game.Map[x + command.DeltaX, y + command.DeltaY] is Sack)
    	{
        	return new CreatureCommand { DeltaX = 0, DeltaY = 0 };
    	}

    	return command;
	}

	private CreatureCommand GetMovementCommand(Key key, int x, int y)
	{
    	var command = new CreatureCommand { DeltaX = 0, DeltaY = 0 };

    	switch (key)
    	{
        	case Key.Down:
            	command = HandleMovement(key, x, y, 1, 0);
            	break;

        	case Key.Up:
            	command = HandleMovement(key, x, y, -1, 0);
            	break;

        	case Key.Left:
            	command = HandleMovement(key, x, y, 0, -1);
            	break;

        	case Key.Right:
            	command = HandleMovement(key, x, y, 0, 1);
            	break;
    	}

    	return command;
	}

	private CreatureCommand HandleMovement(Key key, int x, int y, int deltaY, int deltaX)
	{
    	var command = new CreatureCommand { DeltaX = deltaX, DeltaY = deltaY };

    	if ((key == Key.Down && y < Game.MapHeight - 1) ||
        	(key == Key.Up && y > 0) ||
        	(key == Key.Left && x > 0) ||
        	(key == Key.Right && x < Game.MapWidth - 1))
    	{
       	return command;
    	}
    	else
    	{
        	return new CreatureCommand { DeltaX = 0, DeltaY = 0 };
    	}
	}
}