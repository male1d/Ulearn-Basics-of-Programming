// Четный массив

public static int[] GetFirstEvenNumbers(int count)
{
    int[] array = new int[count];

    for (int k = 1; k <= count; k++)
        array[k - 1] = k * 2;

    return array;
}

// Индекс максимума

public static int MaxIndex(double[] array)
{
    var max = double.MinValue;
    int index = -1;

    for (int i = 0; i < array.Length; i++)
    {
        if (max < array[i])
        {
            max = array[i];
            index = i;
        }
    }
    return index;
}

// Подсчет

public static int GetElementCount(int[] items, int itemToFind)
{
    int amount = 0;
    for (int i = 0; i < items.Length; i++)
    {
        if (items[i] == itemToFind)
            amount++;

    }
    return amount;
}

// Поиск массива в массиве

static bool ContainsAtIndex(int[] array, int[] subArray, int index)
{
    for (int j = 0; j < subArray.Length; j++)
    {
        if (array[index + j] != subArray[j])
        {
            return false;
        }
    }
    return true;
}

// Карты Таро

private static string GetSuit(Suits suit)
{
    return suit == Suits.Wands ? "жезлов" :
    suit == Suits.Coins ? "монет" :
    suit == Suits.Cups ? "кубков" :
    "мечей";
}

// Карты памяти

1. B
2. C
3. B
4. B
5. B
6. F

// Null или не Null?

public static bool CheckFirstElement(int[] array)
{
    return array != null && array.Length != 0 && array[0] == 0;
}

// Возвести массив в степень

public static int[] GetPoweredArray(int[] arr, int power)
{
    int[] result = new int[arr.Length];
    for (int i = 0; i < arr.Length; i++)
    {
        result[i] = (int)(Math.Pow(arr[i], power));
    }
    return result;
}

// Массивы

1.Что не так в приведенном выше коде? Отметьте все подходящие варианты. 

Всё кроме "Ничего, код великолепен!"

2. В массив типа int[] можно положить элемент типа string 

Неверно

3. Массивы отлично использовать для...

поиска значения по его номеру
хранения набора однотипных данных

4. Отметьте все верные факты про массивы a и b

Всё кроме "Все ячейки массива `a` содержат значение null" и "Массив `b` двумерный"

// Крестики-нолики

private static GameResult GetGameResult(Mark[,] field)
{
    if (MarkLogics(field, Mark.Circle) && MarkLogics(field, Mark.Cross)) return GameResult.Draw;
    else if (MarkLogics(field, Mark.Cross)) return GameResult.CrossWin;
    else if (MarkLogics(field, Mark.Circle)) return GameResult.CircleWin;
    else return GameResult.Draw;
}
private static bool MarkLogics(Mark[,] field, Mark mark)
{
    return field[0, 0] == mark && field[0, 1] == mark && field[0, 2] == mark ||
           field[1, 0] == mark && field[1, 1] == mark && field[1, 2] == mark ||
           field[2, 0] == mark && field[2, 1] == mark && field[2, 2] == mark ||
           field[0, 0] == mark && field[1, 0] == mark && field[2, 0] == mark ||
           field[0, 1] == mark && field[1, 1] == mark && field[2, 1] == mark ||
           field[0, 2] == mark && field[1, 2] == mark && field[2, 2] == mark ||
           field[0, 0] == mark && field[1, 1] == mark && field[2, 2] == mark ||
           field[2, 0] == mark && field[1, 1] == mark && field[0, 2] == mark;
}

// Практика «Гистограмма»

using System.Linq;

namespace Names;

internal static class HistogramTask
{
    public static HistogramData GetBirthsPerDayHistogram(NameData[] names, string name)
    {
        // создание массива для хранения количества рождений по дням месяца
        var birthCounts = new int[31];

        // подсчитывание количества рождений для каждого дня месяца
        foreach (var data in names.Where(d => d.Name == name && d.BirthDate.Day != 1))
        {
            birthCounts[data.BirthDate.Day - 1]++;
        }

        var labels = Enumerable.Range(1, 31).Select(i => i.ToString()).ToArray();

        // создание объекта HistogramData с результатами
        return new HistogramData(
            $"Рождаемость людей с именем '{name}'",
            labels,
            birthCounts.Select(count => (double)count).ToArray());
    }
}


// Практика «Тепловая карта»

using System;
using System.Linq;

namespace Names
{
    internal static class HeatmapTask
    {
        public static HeatmapData GetBirthsPerDateHeatmap(NameData[] names)
        {
            if (names == null) { throw new ArgumentNullException(nameof(names)); }

            string[] days = Enumerable.Range(2, 30).Select(x => x.ToString()).ToArray();
            string[] months = Enumerable.Range(1, 12).Select(x => x.ToString()).ToArray();

            double[,] heat = new double[30, 12];
            foreach (var name in names)
            {
                int dayIndex = name.BirthDate.Day - 2;
                int monthIndex = name.BirthDate.Month - 1;
                if (dayIndex >= 0 && dayIndex < 30 && monthIndex >= 0 && monthIndex < 12)
                {
                    heat[dayIndex, monthIndex]++;
                }
            }

            return new HeatmapData("Пример карты интенсивностей", heat, days, months);
        }
    }
}