//Ошибки компиляции

1.Как можно дополнить код, чтобы он начал компилироваться? Выберите все возможные варианты.

Дописать "using System;" в начало
Написать "System.Math.Min" вместо "Math.Min"

2. Что это может значить? Отметьте все корректные варианты. 

Вася попытался вызвать функцию Min с одним аргументом
Ошибка в файле Controller.cs
Есть ошибка в девятой строке

// Очепятки

public static void Main()
{
    Console.WriteLine("Hello, World!");
    var number = 5.5;
    number += 7;
    Console.WriteLine(number);
}

// Минимум функции

private static string GetMinX(int a, int b, int c)
{
    return (a == 0 && b == 0) ? "0" : a == 0 ? "Impossible" : ((-1.0 * b) / (2.0 * a)).ToString();
}

// Горячие клавиши отладки

Запуск программы под отладчиком / продолжение после точки останова - F5

Шаг без захода внутрь метода (Step Over) - F10

Шаг с заходом внутрь метода (Step Into) - F11

Выполнение инструкций вплоть до выхода из текущего метода. (Step Out) - Shift+F11

// Дизайн кода

1. Что неправильного в этом коде? Как его можно улучшить? Выберите все верные варианты. 

Всё кроме - theNumberToSquareAndSum названа с маленькой буквы, что противоречит принятым в C# правилам именования

2. Что стилистически неверно в коде выше? Выберите все верные варианты.

Название переменной не соответствует содержанию
Название переменной не соответствует правилу camelCase

3. Исправлять стилистические ошибки нужно только если больше нечего делать

Неверно

4. Выберите все имена подходящие для переменной, хранящей количество яблок?

amountOfApples
appleCount

5. Выберите все подходящие названия для метода, который печатает переданную ему строку на экран?

Write(string s)
PrintString(string s)

// Рефакторинг и улучшение кода

1. Что следует изменить в коде выше? Выберите все верные варианты. 

Использовать константу Math.PI, вместо 3.14
Выделить нахождение площади круга в отдельный метод

2. Как следует отрефакторить код выше? Выберите все верные варианты.
Использовать enum вместо целых чисел

3. Метод в 100 строк — это норма!

Неверно

4. Почему необходимо писать качественный код? Выберите все верные варианты.

Всё

// Сделай то, не знаю что

public static string Decode(string a)
{
    var b = int.Parse(a.Replace(".", "")) % 1024;
    return b.ToString();
}

// Практика «Angry Birds»

using System;

namespace AngryBirds;

public static class AngryBirdsTask
{
    public static double FindSightAngle(double velocity, double distance)
    {
        const double g = 9.8;
        double speed = velocity * velocity;
        return 0.5 * Math.Asin((distance * g) / speed);
    }
}


// Практика «Бильярд»

namespace Billiards
{
    public static class BilliardsTask
    {
        public static double BounceWall(double directionRadians, double wallInclinationRadians)
        {
            const double circle = 360.0;

            double angletilt = wallInclinationRadians + 90;
            double angledirection = directionRadians + 180;


            double angledifference = angletilt - angledirection;
            double newdirection = angledirection + 2 * angledifference + circle * 5;

            newdirection %= circle;
            if (newdirection > 180)
            {
                newdirection -= circle;
            }

            return newdirection;
        }
    }
}


// Практика «Проценты»

public static double Calculate(string userInput)
{
    string[] inputParts = userInput.Split(' ');

    double cashSum = double.Parse(inputParts[0]);
    double monthlyRate = double.Parse(inputParts[1]) / 1200;
    int months = int.Parse(inputParts[2]);

    return cashSum * Math.Pow(1 + monthlyRate, months);
}
