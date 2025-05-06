// Практика «Манипулятор»

using System;
using Point = Avalonia.Point;
using NUnit.Framework;
 
namespace Manipulation
{
    public static class AnglesToCoordinatesTask
    {
        public static Point[] GetJointPositions(double shoulder, double elbow, double wrist)
        {
            float upperArmComponentX = Manipulator.UpperArm * (float)Math.Cos(shoulder);
            float upperArmComponentY = Manipulator.UpperArm * (float)Math.Sin(shoulder);
            Point elbowPos = new Point(upperArmComponentX, upperArmComponentY);
 
            upperArmComponentX += Manipulator.Forearm * (float)Math.Cos(elbow + shoulder - Math.PI);
            upperArmComponentY += Manipulator.Forearm * (float)Math.Sin(elbow + shoulder - Math.PI);
            Point wristPos = new Point(upperArmComponentX, upperArmComponentY);
 
            upperArmComponentX += Manipulator.Palm * (float)Math.Cos(wrist + elbow + shoulder - 2 * Math.PI);
            upperArmComponentY += Manipulator.Palm * (float)Math.Sin(wrist + elbow + shoulder - 2 * Math.PI);
 
            Point endPositionPalm = new Point(upperArmComponentX, upperArmComponentY);
 
            return new Point[]
            {
                elbowPos,
                wristPos,
                endPositionPalm
            };
        }
    }
 
    [TestFixture]
    public class AnglesToCoordinatesTask_Tests
    {
        [TestCase(Math.PI / 2, Math.PI / 2, Math.PI, Manipulator.Forearm + Manipulator.Palm, Manipulator.UpperArm)]
        [TestCase(Math.PI, Math.PI, Math.PI, -Manipulator.UpperArm - Manipulator.Forearm - Manipulator.Palm, 0)]
        [TestCase(Math.PI / 2, -Math.PI / 2, Math.PI, -Manipulator.Forearm - Manipulator.Palm, Manipulator.UpperArm)]
        [TestCase(Math.PI, Math.PI, Math.PI, -Manipulator.UpperArm - Manipulator.Forearm - Manipulator.Palm, 0)]
        public void TestGetJointPositions(double shoulder, double elbow, double wrist, double palmEndX, double palmEndY)
        {
            var joints = AnglesToCoordinatesTask.GetJointPositions(shoulder, elbow, wrist);
            Assert.AreEqual(palmEndX, joints[2].X, 1e-5);
            Assert.AreEqual(palmEndY, joints[2].Y, 1e-5);
        }
    }
}

// Практика «Поиск угла»

using System;
using NUnit.Framework;
 
 
namespace Manipulation
{
    public class TriangleTask
    {
        public static double GetABAngle(double side1, double side2, double side3)
        {
            if (side1 <= 0 || side2 <= 0 || side3 < 0)
                return double.NaN;
            
            return Math.Acos((side1 * side1 + side2 * side2 - side3 * side3) / (2 * side1 * side2));
        }
    }
 
 
    [TestFixture]
    public class TriangleTask_Tests
    {
        [TestCase(3, 4, 5, Math.PI / 2)]
        [TestCase(1, 1, 1, Math.PI / 3)]
        [TestCase(4, 0, 5, double.NaN)] 
        [TestCase(3, -4, 5, double.NaN)]
 
        public void TestGetABAngle(double a, double b, double c, double expectedAngle)
        {
            double currentAngle = TriangleTask.GetABAngle(a, b, c);
            Assert.AreEqual(expectedAngle, currentAngle, 1e-10);
        }
    }
}

// Практика «Решение манипулятора»

using System;
using NUnit.Framework;
using Avalonia;
using Avalonia.Platform;
 
namespace Manipulation
{
    public static class ManipulatorTask
    {
        public static Point GetPoint(double angle) => new Point(Math.Cos(angle), Math.Sin(angle));
 
        public static double[] MoveManipulatorTo(double targetX, double targetY, double targetAngle)
        {
            var wristPoint = new Point(targetX, targetY) - Manipulator.Palm * GetPoint(-targetAngle);
            var wristDistToOrigin = wristPoint.GetDistToOrigin();
            var elbow = TriangleTask.GetABAngle(Manipulator.UpperArm, Manipulator.Forearm, wristDistToOrigin);
            var atan2Result = Math.Atan2(wristPoint.Y, wristPoint.X);
            var shoulder = TriangleTask.GetABAngle(Manipulator.UpperArm, wristDistToOrigin, Manipulator.Forearm) + atan2Result;
            var wrist = -targetAngle - shoulder - elbow;
 
            return new[] { shoulder, elbow, wrist };
        }
 
        public static double GetDistToOrigin(this Point p) => Math.Sqrt(p.X * p.X + p.Y * p.Y);
    }
 
    [TestFixture]
    public class ManipulatorTask_Tests
    {
        [Test]
        public void TestMoveManipulatorTo()
        {
            const int numberOfTests = 1000;
            const double maxRange = 10.0;
            const double tolerance = 1e-10; 
            Random rand = new Random();
 
            for (int i = 0; i < numberOfTests; i++)
            {
                double randomX = rand.NextDouble() * maxRange;
                double randomY = rand.NextDouble() * maxRange;
                double randomAngle = rand.NextDouble() * Math.PI * 2;
 
                double[] result = ManipulatorTask.MoveManipulatorTo(randomX, randomY, randomAngle);
                double[] expected = CalculateExpectedAngles(randomX, randomY, randomAngle);
 
                Assert.AreEqual(expected[0], result[0], tolerance);
                Assert.AreEqual(expected[1], result[1], tolerance);
                Assert.AreEqual(expected[2], result[2], tolerance);
            }
        }
 
        private double[] CalculateExpectedAngles(double x, double y, double angle)
        {
            var wristPoint = new Point(x, y) - Manipulator.Palm * ManipulatorTask.GetPoint(-angle);
            var wristDistToOrigin = wristPoint.GetDistToOrigin();
            var elbow = TriangleTask.GetABAngle(Manipulator.UpperArm, Manipulator.Forearm, wristDistToOrigin);
            var atan2Result = Math.Atan2(wristPoint.Y, wristPoint.X);
            var shoulder = TriangleTask.GetABAngle(Manipulator.UpperArm, wristDistToOrigin, Manipulator.Forearm) + atan2Result;
            var wrist = -angle - shoulder - elbow;
 
            return new[] { shoulder, elbow, wrist };
        }
    }
}