// Анализ алгоритма

1. Всё кроме Оптимизация алгоритма , Тестирование алгоритма , Отладка алгоритма
2. Θ(n), где n — сумма длин всех строк в массиве array

// Рекурсивный бинарный поиск

public static int BinSearchLeftBorder(long[] array, long value, int left, int right)
{
    if (right - left == 1) return left;
    var m = (left + right) / 2;
    if (array[m] < value)
        return BinSearchLeftBorder(array, value,  m, right);
    return BinSearchLeftBorder(array, value, left, m);
}

// Сортировка диапазона

public static void BubbleSortRange(int[] array, int left, int right)
{
    for (int i = left; i < right; i++)
        for (int j = left; j < right; j++)
            if (array[j] > array[j + 1])
            {
                var t = array[j + 1];
                array[j + 1] = array[j];
                array[j] = t;
            }
}

// Сложность поиска и сортировки

1. Θ(n)
2. Θ(logn)
3. Θ(n2)
4. Θ(nlogn)
5. Θ(n2)
6. «Быстрая сортировка» в среднем работает быстрее и «Быстрая сортировка» использует меньше дополнительной памяти

// Практика «Левая граница»

using System;
using System.Collections.Generic;

namespace Autocomplete
{
    public class LeftBorderTask
    {
        public static int GetLeftBorderIndex(IReadOnlyList<string> phrases, string prefix, int left, int right)
        {
            if (right - left <= 1)
            {
                return left;
            }

            var middle = (right - left) / 2 + left;

            if (string.Compare(prefix, phrases[middle], StringComparison.OrdinalIgnoreCase) < 0
                || phrases[middle].StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
            {
                return GetLeftBorderIndex(phrases, prefix, left, middle);
            }

            return GetLeftBorderIndex(phrases, prefix, middle, right);
        }
    }
}


// Практика «Правая граница»

using System;
using System.Collections.Generic;

namespace Autocomplete
{
    public class RightBorderTask
    {
        public static int GetRightBorderIndex(IReadOnlyList<string> phrases, string prefix, int left, int right)
        {
            if (phrases.Count == 0)
                return right;
            left += 1;
            right -= 1;
            while (left < right)
            {
                var middle = left + (right - left) / 2;
                var condition = string.Compare(prefix, phrases[middle],
                    StringComparison.InvariantCultureIgnoreCase) >= 0
                    || phrases[middle].StartsWith(prefix,
                    StringComparison.InvariantCultureIgnoreCase);
                if (condition)
                    left = middle + 1;
                else
                    right = middle - 1;
            }
            var finalCondition = string.Compare(prefix, phrases[right],
                StringComparison.InvariantCultureIgnoreCase) >= 0
                || phrases[left].StartsWith(prefix,
                StringComparison.InvariantCultureIgnoreCase);
            return finalCondition ? right + 1 : right;
        }
    }
}

// Практика «Автодополнение»

using System.Collections.Generic;
using System;
using Autocomplete;
using NUnit.Framework;

namespace Autocomplete
{
    internal class AutocompleteTask
    {
        public static string FindFirstByPrefix(IReadOnlyList<string> phrases, string prefix)
        {
            var left = LeftBorderTask.GetLeftBorderIndex(phrases, prefix, -1, phrases.Count);

            if (left < phrases.Count && phrases[left].StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
            {
                return phrases[left];
            }

            return null;
		}

        public static string[] GetTopByPrefix(IReadOnlyList<string> phrases, string prefix, int count)
        {
            var left = LeftBorderTask.GetLeftBorderIndex(phrases, prefix, -1, phrases.Count);

            var countByPrefix = RightBorderTask.GetRightBorderIndex(phrases, prefix, -1, phrases.Count) - left - 1;

            if (countByPrefix <= 0)
            {
                return new string[0];
            }
            count = Math.Min(count, countByPrefix);

            var words = new string[count];

            for (int i = 0; i < count; i++)
            {
                words[i] = phrases[left + i + 1];
            }

            return words;
        }

        public static int GetCountByPrefix(IReadOnlyList<string> phrases, string prefix)
        {
            var left = LeftBorderTask.GetLeftBorderIndex(phrases, prefix, -1, phrases.Count);
            var right = RightBorderTask.GetRightBorderIndex(phrases, prefix, -1, phrases.Count);

            return right - left - 1;
        }
    }
}

[TestFixture]
public class AutocompleteTests
{
    private static readonly IReadOnlyList<string> phrases = new List<string>
    {
        "apple", "banana", "cherry", "date", "elderberry", "fig", "grape"
    };

    [Test]
    public void FindFirstByPrefix_ReturnsCorrectPhrase_WhenPrefixExists()
    {
        var prefix = "ba";
        var expected = "banana";
        var actual = AutocompleteTask.FindFirstByPrefix(phrases, prefix);
        Assert.AreEqual(expected, actual);
    }

    [Test]
    public void FindFirstByPrefix_ReturnsNull_WhenPrefixDoesNotExist()
    {
        var prefix = "kiwi";
        var actual = AutocompleteTask.FindFirstByPrefix(phrases, prefix);
        Assert.IsNull(actual);
    }

    [Test]
    public void GetTopByPrefix_ReturnsCorrectPhrases_WhenCountIsLessThanTotal()
    {
        var prefix = "b";
        var count = 1;
        var expected = new string[] { "banana" };
        var actual = AutocompleteTask.GetTopByPrefix(phrases, prefix, count);
        CollectionAssert.AreEqual(expected, actual);
    }

    [Test]
    public void GetTopByPrefix_ReturnsAllPhrases_WhenCountIsMoreThanTotal()
    {
        var prefix = "b";
        var count = 10;
        var expected = new string[] { "banana" };
        var actual = AutocompleteTask.GetTopByPrefix(phrases, prefix, count);
        CollectionAssert.AreEqual(expected, actual);
    }

    [Test]
    public void GetCountByPrefix_ReturnsCorrectCount_WhenPrefixExists()
    {
        var prefix = "b";
        var expected = 1;
        var actual = AutocompleteTask.GetCountByPrefix(phrases, prefix);
        Assert.AreEqual(expected, actual);
    }
}