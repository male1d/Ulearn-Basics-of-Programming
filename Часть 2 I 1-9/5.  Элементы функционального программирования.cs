// Частичное применение функций

1. 5
2. 111
3. 53

// Реализация метода FirstOrDefault

private static T FirstOrDefault<T>(IEnumerable<T> source, Func<T, bool> filter)
{
    foreach (var item in source)
    {
        if (filter(item))
        {
            return item;
        }
    }
    return default(T);
}

// Реализация метода Take

private static IEnumerable<T> Take<T>(IEnumerable<T> source, int count)
{
    return source.Take(count);
}

// Последовательность вызовов

1. s f s f
2. s s f f
3. s f
4. s s f
5. Не вызовутся совсем
6. Сначала вызовется n % 2 == 0, потом n * 31
7. n % 2 == 0 вызовется 4 раза. n * 31 — не вызовется.
8. n % 2 == 0 вызовется 4 раза. n * 31 — 2 раза.

// Практика «Лямбды и делегаты»

namespace func_rocket;

using System;

public class ForcesTask
{
    public static RocketForce GetThrustForce(double forceValue) => r =>
    {
        var directionVector = new Vector(Math.Cos(r.Direction), Math.Sin(r.Direction));
        return directionVector * forceValue;
    };
    public static RocketForce ConvertGravityToForce(Gravity gravity, Vector spaceSize) => r =>
    {
        return gravity(spaceSize, r.Location);
    };
    public static RocketForce Sum(params RocketForce[] forces) => r =>
    {
        Vector sum = Vector.Zero;
        foreach (var force in forces)
        {
            sum += force(r);
        }
        return sum;
    };
}

// Практика «Уровни»

using System;
using System.Collections.Generic;

namespace func_rocket;

public class LevelsTask
{
    static readonly Physics standardPhysics = new();
    static readonly Vector initialRocketPosition = new Vector(200, 500);
    static readonly double initialDirection = -0.5 * Math.PI;

    private static Level CreateLevel(string name, Vector target, Gravity gravity)
    {
        Vector adjustedTarget = AdjustTargetIfNecessary(target);

        return new Level(name,
            new Rocket(initialRocketPosition, Vector.Zero, initialDirection),
            adjustedTarget,
            gravity, standardPhysics);
    }

    private static Vector AdjustTargetIfNecessary(Vector target)
    {
        var distance = (target - initialRocketPosition).Length;
        var angleToTarget = Math.Atan2(target.Y - initialRocketPosition.Y, target.X - initialRocketPosition.X);
        var angleDifference = Math.Abs(angleToTarget - initialDirection);

        if (distance < 450 || distance > 550 || angleDifference < Math.PI / 4)
        {
            return AdjustTargetPosition(target);
        }

        return target;
    }

    private static Vector AdjustTargetPosition(Vector target)
    {
        target = AdjustDistance(target);
        target = AdjustAngle(target);
        return target;
    }

    private static Vector AdjustDistance(Vector target)
    {
        double currentDistance = (target - initialRocketPosition).Length;

        if (currentDistance < 450)
        {
            return IncreaseDistance(target, 450);
        }

        if (currentDistance > 550)
        {
            return DecreaseDistance(target, 550);
        }

        return target;
    }

    private static Vector IncreaseDistance(Vector target, double newDistance)
    {
        var direction = (target - initialRocketPosition).Normalize();
        return initialRocketPosition + direction * newDistance;
    }

    private static Vector DecreaseDistance(Vector target, double newDistance)
    {
        var direction = (target - initialRocketPosition).Normalize();
        return initialRocketPosition + direction * newDistance;
    }

    private static Vector AdjustAngle(Vector target)
    {
        double angleToTarget = Math.Atan2(target.Y - initialRocketPosition.Y, target.X - initialRocketPosition.X);
        double angleDifference = Math.Abs(angleToTarget - initialDirection);

        if (angleDifference < Math.PI / 4)
        {
            return AdjustAngleToMinimumDifference(target, angleToTarget, angleDifference);
        }

        return target;
    }

    private static Vector AdjustAngleToMinimumDifference(Vector target, double angleToTarget, double angleDifference)
    {
        double adjustmentAngle = Math.PI / 4 - angleDifference;
        if (angleToTarget < initialDirection)
            angleToTarget -= adjustmentAngle;
        else
            angleToTarget += adjustmentAngle;

        return CalculateNewTargetPosition(target, angleToTarget);
    }

    private static Vector CalculateNewTargetPosition(Vector target, double angleToTarget)
    {
        double distance = (target - initialRocketPosition).Length;
        return new Vector(initialRocketPosition.X + Math.Cos(angleToTarget) * distance,
                            initialRocketPosition.Y + Math.Sin(angleToTarget) * distance);
    }

    public static IEnumerable<Level> CreateLevels()
    {
        yield return CreateZeroLevel();
        yield return CreateHeavyLevel();
        yield return CreateUpLevel();
        yield return CreateWhiteHoleLevel();
        yield return CreateBlackHoleLevel();
        yield return CreateBlackAndWhiteLevel();
    }

    private static Level CreateZeroLevel()
    {
        return new Level("Zero",
            new Rocket(initialRocketPosition, Vector.Zero, initialDirection),
            new Vector(600, 200),
            (size, v) => Vector.Zero, standardPhysics);
    }

    private static Level CreateHeavyLevel()
    {
        return CreateLevel("Heavy", new Vector(600, 200), (size, v) => new Vector(0, 0.9));
    }

    private static Level CreateUpLevel()
    {
        return CreateLevel("Up", new Vector(700, 500), (size, v) => new Vector(0, -300 / (size.Y - v.Y + 300.0)));
    }

    private static Level CreateWhiteHoleLevel()
    {
        Vector targetWhiteHole = new Vector(600, 200);
        Gravity whiteHoleGravity = (size, v) =>
        {
            var d = (targetWhiteHole - v).Length;
            return (targetWhiteHole - v).Normalize() * (-140 * d) / (d * d + 1);
        };
        return CreateLevel("WhiteHole", targetWhiteHole, whiteHoleGravity);
    }

    private static Level CreateBlackHoleLevel()
    {
        Vector targetBlackHole = new Vector(600, 200);
        var anomaly = (targetBlackHole + initialRocketPosition) / 2;
        Gravity blackHoleGravity = (size, v) =>
        {
            var d = (anomaly - v).Length;
            return (anomaly - v).Normalize() * (300 * d) / (d * d + 1);
        };
        return CreateLevel("BlackHole", targetBlackHole, blackHoleGravity);
    }

    private static Level CreateBlackAndWhiteLevel()
    {
        Vector targetBlackHole = new Vector(600, 200);
        Gravity whiteHoleGravity = (size, v) =>
        {
            var d = (new Vector(600, 200) - v).Length;
            return (new Vector(600, 200) - v).Normalize() * (-140 * d) / (d * d + 1);
        };

        var anomaly = (targetBlackHole + initialRocketPosition) / 2;
        Gravity blackHoleGravity = (size, v) =>
        {
            var d = (anomaly - v).Length;
            return (anomaly - v).Normalize() * (300 * d) / (d * d + 1);
        };
        Gravity blackAndWhiteGravity = (size, v) =>
        {
            return (whiteHoleGravity(size, v) + blackHoleGravity(size, v)) / 2;
        };
        return CreateLevel("BlackAndWhite", targetBlackHole, blackAndWhiteGravity);
    }
}

// Практика «Управление»

using System;

namespace func_rocket
{
    public class ControlTask
    {
        public static Turn ControlRocket(Rocket rocket, Vector target)
        {
            double targetAngle = Math.Atan2((target - rocket.Location).Y, (target - rocket.Location).X);
            double rocketDirectionAngle = Math.Atan2(Math.Sin(rocket.Direction), Math.Cos(rocket.Direction));
            double velocityAngle = rocket.Velocity.Angle;
            double difBetweenTargetAndVelocity = DifferenceBetweenAngles(targetAngle, velocityAngle);
            double idealRocketDirectionAngle;
            if (Math.Abs(difBetweenTargetAndVelocity) < Math.PI / 4)
                idealRocketDirectionAngle = targetAngle + difBetweenTargetAndVelocity;
            else
                idealRocketDirectionAngle = targetAngle + Math.PI / 8;
            double sinAngle = Math.Sin(idealRocketDirectionAngle);
            double cosAngle = Math.Cos(idealRocketDirectionAngle);
            idealRocketDirectionAngle = Math.Atan2(sinAngle, cosAngle);
            double difBetweenIdealAndRealRocketDir = DifferenceBetweenAngles(idealRocketDirectionAngle, rocketDirectionAngle);
            return difBetweenIdealAndRealRocketDir > 0
                ? Turn.Right
                : difBetweenIdealAndRealRocketDir < 0
                    ? Turn.Left
                    : Turn.None;
        }

        static double DifferenceBetweenAngles(double first, double second) =>
            (((first - second) + Math.PI) % (2 * Math.PI)) - Math.PI;
    }
}

