// Структура или класс?

1.X может быть структурой
2. Всё , кроме X не может быть ни структурой, ни классом
3. X может быть классом и X может быть структурой
4. X может быть структурой

// Карты памяти

1. D
2. A
3. D
4. E
5. B
6. B
7. C

// Применение ref

public static void WriteAllNumbersFromText(string text)
{
    int pos = 0;
    string finalText = "";
    while (pos < text.Length)
    {
        SkipSpaces(text, ref pos);
        finalText += ReadNumber(text, ref pos) + " ";
    }
    Console.WriteLine(finalText);
}

// Последствия boxing

1.False
2.True

// Практика «Benchmark»

using System;
using System.Diagnostics;
using System.Text;
using NUnit.Framework;

namespace StructBenchmarking
{
    public class Benchmark : IBenchmark
    {
        // метод для измерения времени выпонления задачи
        public double MeasureDurationInMs(ITask task, int repetitionCount)
        {
            var timer = new Stopwatch(); // создание секундомера
            GC.Collect(); // сборщик мусора
            GC.WaitForPendingFinalizers();
            task.Run();
            timer.Start(); // запуск секундомера
            for (var i = 0; i < repetitionCount; i++)
                task.Run();
            timer.Stop(); // остановка секундомера
            return timer.Elapsed.TotalMilliseconds / repetitionCount;
        }
    }
    // класс для тестов
    [TestFixture]
    public class RealBenchmarkUsageSample
    {
        private class BuilderTest : ITask
        {
            public void Run()
            {
                var builder = new StringBuilder();
                for (var i = 0; i < 10000; i++) // 1000 символов 'а'
                    builder.Append("a");
                var temp = builder.ToString(); // получаем строку
                builder.Clear();
            }
        }

        private class StringTest : ITask
        {
            public void Run()
            {
                var temp = new string('a', 10000);
            }
        }

        [Test]
        public void StringConstructorFasterThanStringBuilder()
        {
            var builderTest = new BuilderTest();
            var stringTest = new StringTest();
            var benchmark = new Benchmark();

            var builderTestResult = benchmark.MeasureDurationInMs(builderTest, 10);
            var stringTestResult = benchmark.MeasureDurationInMs(stringTest, 10);

            Assert.Less(stringTestResult, builderTestResult);
        }
    }
}

// Практика «Эксперименты»

using System.Collections.Generic;

namespace StructBenchmarking
{
    public interface IExperimentsFactory
    {
        IClass CreateClassFactory();
        IStruct CreateStructFactory();
    }

    public class ArrayCreationExperimentsFactory : IExperimentsFactory
    {
        public IClass CreateClassFactory() => new ArrayCreationForClass();
        public IStruct CreateStructFactory() => new ArrayCreationForStruct();
    }

    public class MethodCallExperimentsFactory : IExperimentsFactory
    {
        public IClass CreateClassFactory() => new MethodCallForClass();
        public IStruct CreateStructFactory() => new MethodCallForStruct();
    }

    public interface IClass
    {
        List<ExperimentResult> DoExperimentsForClass(IBenchmark benchmark, int repetitionsCount);
    }

    public class ArrayCreationForClass : IClass
    {
        public List<ExperimentResult> DoExperimentsForClass(IBenchmark benchmark, int repetitionsCount)
        {
            var results = new List<ExperimentResult>();
            for (var i = 16; i <= 512; i *= 2)
                results.Add(
                    new ExperimentResult(i,
                    benchmark.MeasureDurationInMs(new ClassArrayCreationTask(i), repetitionsCount)));

            return results;
        }
    }

    public class MethodCallForClass : IClass
    {
        public List<ExperimentResult> DoExperimentsForClass(IBenchmark benchmark, int repetitionsCount)
        {
            var results = new List<ExperimentResult>();
            for (var i = 16; i <= 512; i *= 2)
                results.Add(new ExperimentResult(i,
                    benchmark.MeasureDurationInMs(new MethodCallWithClassArgumentTask(i), repetitionsCount)));

            return results;
        }
    }

    public interface IStruct
    {
        List<ExperimentResult> DoExperimentsForStruct(IBenchmark benchmark, int repetitionsCount);
    }

    public class ArrayCreationForStruct : IStruct
    {
        public List<ExperimentResult> DoExperimentsForStruct(IBenchmark benchmark, int repetitionsCount)
        {
            var results = new List<ExperimentResult>();
            for (var i = 16; i <= 512; i *= 2)
                results.Add(new ExperimentResult(i,
                    benchmark.MeasureDurationInMs(new StructArrayCreationTask(i), repetitionsCount)));

            return results;
        }
    }

    public class MethodCallForStruct : IStruct
    {
        public List<ExperimentResult> DoExperimentsForStruct(IBenchmark benchmark, int repetitionsCount)
        {
            var results = new List<ExperimentResult>();
            for (var i = 16; i <= 512; i *= 2)
                results.Add(new ExperimentResult(i,
                    benchmark.MeasureDurationInMs(new MethodCallWithStructArgumentTask(i), repetitionsCount)));

            return results;
        }
    }

    public class Experiments
    {
        private static readonly IExperimentsFactory ArrayCreationFactory = new ArrayCreationExperimentsFactory();
        private static readonly IExperimentsFactory MethodCallFactory = new MethodCallExperimentsFactory();

        public static ChartData BuildChartDataForArrayCreation(IBenchmark benchmark, int repetitionsCount)
        {
            var classFactory = ArrayCreationFactory.CreateClassFactory();
            var structFactory = ArrayCreationFactory.CreateStructFactory();
            var classPoints = classFactory.DoExperimentsForClass(benchmark, repetitionsCount);
            var structPoints = structFactory.DoExperimentsForStruct(benchmark, repetitionsCount);

            return new ChartData
            {
                Title = "Создание массива",
                ClassPoints = classPoints,
                StructPoints = structPoints
            };
        }

        public static ChartData BuildChartDataForMethodCall(IBenchmark benchmark, int repetitionsCount)
        {
            var classFactory = MethodCallFactory.CreateClassFactory();
            var structFactory = MethodCallFactory.CreateStructFactory();
            var classPoints = classFactory.DoExperimentsForClass(benchmark, repetitionsCount);
            var structPoints = structFactory.DoExperimentsForStruct(benchmark, repetitionsCount);

            return new ChartData
            {
                Title = "Вызов метода с аргументом",
                ClassPoints = classPoints,
                StructPoints = structPoints
            };
        }
    }
}