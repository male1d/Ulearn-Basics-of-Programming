// Исправить рекурсию

public static void WriteReversed(char[] items, int startIndex = 0)
{
	if (items.Length != 0)
    {
		if (startIndex != items.Length - 1)
        {
			WriteReversed(items, startIndex + 1);
        }
        Console.Write(items[startIndex]);
    }
}

// Понимание рекурсии

1. Вычисляет минимальный разряд десятичного числа xx, в котором стоит 0
2. 2
3. Вычисляет НОД x и y по алгоритму Евклида
4. CADB
5. DACB
6. C
7. DFBECA

// Перебор паролей

static void MakeSubsets(char[] subset, int position = 0)
		{
			if (position == subset.Length)
			{
				Console.WriteLine(new string(subset));
				return;
			}
			subset[position] = 'a';
			MakeSubsets(subset, position + 1);
			subset[position] = 'b';
			MakeSubsets(subset, position + 1);
			subset[position] = 'c';
			MakeSubsets(subset, position + 1);
		}

// Брошенный код :(

static void MakePermutations(int[] permutation, int position, List<int[]> result)
{
    if (position == permutation.Length)
    {
		WritePermutation(permutation);
        return;
    }
    else
    {
        for (int i = 0; i < permutation.Length; i++)
        {
            var index = Array.IndexOf(permutation, i, 0, position);
            if (index == -1)
            {
				permutation[position] = i;
				MakePermutations(permutation, position+1, result);
            }
        }
    }
}

// Практика «Перебор паролей 2»

namespace Passwords
{
    public class CaseAlternatorTask
    {
        public static List<string> AlternateCharCases(string lowercaseWord)
        {
            var result = new List<string>();
            AlternateCharCases(lowercaseWord.ToCharArray(), 0, result);
            result = result.Distinct().ToList();
            return result;
        }
 
        static void AlternateCharCases(char[] word, int startIndex, List<string> result)
        {
            if (startIndex == word.Length)
            {
                result.Add(new string(word));
                return;
            }
 
            if (char.IsLetter(word[startIndex]))
            {
                word[startIndex] = char.ToLower(word[startIndex]);
                AlternateCharCases(word, startIndex + 1, result);
                word[startIndex] = char.ToUpper(word[startIndex]);
                AlternateCharCases(word, startIndex + 1, result);
                word[startIndex] = char.ToLower(word[startIndex]);
            }
 
            else
            {
                AlternateCharCases(word, startIndex + 1, result);
            }
        }
    }
}

// Практика «Хождение по чекпоинтам»

using System;
using System.Drawing;
 
namespace RoutePlanning
{
    public static class PathFinderTask
    {
        public static int[] FindBestCheckpointsOrder(Point[] checkpoints)
        {
            int[] bestOrder = new int[checkpoints.Length];
            int[] order = new int[checkpoints.Length];
            for (int i = 0; i < checkpoints.Length; i++)
                order[i] = i;
 
            double minLength = double.MaxValue;
            FindBestOrder(checkpoints, order, bestOrder, 1, 0, ref minLength);
 
            return bestOrder;
        }
 
        private static void FindBestOrder(Point[] checkpoints, int[] order, int[] bestOrder,
        int index, double currentLength, ref double minLength)
        {
            if (index == order.Length)
            {
                if (currentLength < minLength)
                {
                    minLength = currentLength;
                    Array.Copy(order, bestOrder, order.Length);
                }
            }
            else
            {
                for (int i = index; i < order.Length; i++)
                {
                    Swap(ref order[index], ref order[i]);
                    double newLength = currentLength + GetDistanceBetween(checkpoints[order[index - 1]],
checkpoints[order[index]]);
                    if (newLength < minLength)
                        FindBestOrder(checkpoints, order, bestOrder, index + 1, newLength, ref minLength);
                    Swap(ref order[index], ref order[i]);
                }
            }
        }
 
        private static void Swap(ref int a, ref int b)
        {
            int temp = a;
            a = b;
            b = temp;
        }
 
        private static double GetDistanceBetween(Point a, Point b)
        {
            var dx = a.X - b.X;
            var dy = a.Y - b.Y;
            return Math.Sqrt(dx * dx + dy * dy);
        }
    }
}