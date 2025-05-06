// Терминология

1.Как в языке C# называют именованную последовательность инструкций 

Метод

2. Какие утверждения верны? 

Сборка — это как правило результат компиляции проекта
Solution (решение) может содержать несколько проектов
В проекте может быть более одного кодового файла
Разные проекты могут объявлять классы в одном и том же пространстве имён

3. Что перечисляется в секции References (ссылки) проекта в Visual Studio (или других IDE)

Сборки, классы которых доступны для использования в кодовых файлах проекта

4. Каково предназначение инструкции using в начале кодового файла?

Избавляет программиста от необходимости указывать пространство имён перед именами классов данного пространства имён, сокращая код

5. Где найти exe-файл — результат компиляции моего проекта 

Скорее всего в подпапке bin/Debug папки вашего проекта

// Первый шаг

public static void Main()
{
    Console.WriteLine("The first step!");
}

// Неверный тип данных

public static void Main()
{
    double num1 = +5.5e-2;
    float num2 = 7.8f;
    int num3 = 0;
    long num4 = 2000000000000L;
    Console.WriteLine(num1);
    Console.WriteLine(num2);
    Console.WriteLine(num3);
    Console.WriteLine(num4);
}

// Ошибки преобразования типов

public static void Main()
{
    double pi = Math.PI;
    int tenThousand = (int)10000L;
    float tenThousandPi = (float)(pi * tenThousand);
    int roundedTenThousandPi = (int)Math.Round(tenThousandPi);
    int integerPartOfTenThousandPi = (int)tenThousandPi;
    Console.WriteLine(integerPartOfTenThousandPi);
    Console.WriteLine(roundedTenThousandPi);
}

// Биткоины в массы!

public static void Main()
{
    double amount = 1.11; //количество биткоинов от одного человека
    int peopleCount = 60; // количество человек
    int totalMoney = (int)Math.Round(amount * peopleCount); // ← исправьте ошибку в этой строке
    Console.WriteLine(totalMoney);
}

// Преобразование строки в число

public static void Main()
{
    string doubleNumber = "894376.243643";
    double number = double.Parse(doubleNumber); // Вася уверен, что ошибка где-то тут
    Console.WriteLine(number + 1);
}

// Использование var

static public void Main()
{
    var a = 5.0; // ← исправьте эту строку
    a += 0.5;
    Console.WriteLine(a);
}

// Добрый работодатель

private static string GetGreetingMessage(string name, double salary)
{
    string namee = (string)name;
    int salaryy = (int)Math.Ceiling(salary);
    string s1 = "Hello, ";
    string s2 = namee;
    string s3 = ", your salary is ";
    string s4 = salaryy.ToString();
    string total = s1 + s2 + s3 + s4;
    return total;
    // возвращает "Hello, <name>, your salary is <salary>"
}

// Главный вопрос Вселенной

static int GetSquare(int a)
{
    return (int)Math.Pow(a, 2);
}
static void Print(int number)
{
    Console.WriteLine(number);
}


// Разыскиваются методы!

static string GetLastHalf(string text)
{
    int str = text.Length / 2;
    return (text.Substring(str)).Replace(" ", string.Empty);
}

// Области видимости

1.Корректно ли использование поля класса ДО его определения как в примере выше? 

Да, корректно

2. Что будет выведено на консоль при вызове метода G()?

class

3.Что будет выведено при вызове метода H()?

H

4. Что будет выведено при вызове метода Mixed()?

Ничего. Будет ошибка компиляции

5. В каких методах компилятор сгенерирует ошибки компиляции? 

M1
M2