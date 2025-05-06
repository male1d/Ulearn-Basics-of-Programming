// Использование private

1. строка 5 , 7 , 11

// Не откладывать ошибки

public class Student
{
    private string name;
    public string Name
    {
        get { return name; }
        set
        {
            if (value == null)
            {
                throw new ArgumentException("Имя не может быть нулевым");
            }
            name = value;
        }
    }
}

// Свойство вместо поля

public class Book
{
    public string Title { get; set; } 
}

// Вектор

public class Vector
{
    public double X;
    public double Y;

    public Vector(double x, double y)
    {
        X = x;
        Y = y;
    }

    public double Length { get { return Math.Sqrt(X * X + Y * Y); } }

    public override string ToString()
    {
        return string.Format("({0}, {1}) with length: {2}", X, Y, Length);
    }
}

// Дробь

public class Ratio
{
    public readonly int Numerator;
    public readonly int Denominator;
    public readonly double Value;

    public Ratio(int num, int den)
    {
        if (den <= 0)
        {
            throw new ArgumentException("Denominator must be greater than zero.");
        }

        Numerator = num;
        Denominator = den;
        Value = (double)num / den; // Initialize Value here
    }

    public override string ToString()
    {
        return $"{Numerator}/{Denominator} = {Value.ToString(CultureInfo.InvariantCulture)}";
    }
}

// Практика «Readonly Vector»

namespace ReadOnlyVector
{
    public class ReadOnlyVector
    {
        public readonly double X;
        public readonly double Y;

        public ReadOnlyVector(double x, double y)
        {
            X = x;
            Y = y;
        }

        public ReadOnlyVector Add(ReadOnlyVector other)
        {
            return new ReadOnlyVector(X + other.X, Y + other.Y);
        }

        public ReadOnlyVector WithX(double x)
        {
            return new ReadOnlyVector(x, Y);
        }

        public ReadOnlyVector WithY(double y)
        {
            return new ReadOnlyVector(X, y);
        }
    }
}

// Практика «Счет из отеля»

using System;
using System.ComponentModel;

namespace HotelAccounting
{
    public class AccountingModel : ModelBase
    {
        private double _price;
        private int _nightsCount;
        private double _discount;
        private double _total;

        public double Price
        {
            get => _price;
            set
            {
                if (value < 0) throw new ArgumentException("Цена должна быть неотрицательной");
                _price = value;
                CalculateTotal();
                Notify(nameof(Price));
            }
        }

        public int NightsCount
        {
            get => _nightsCount;
            set
            {
                if (value <= 0) throw new ArgumentException("Количество ночей должно быть положительным");
                _nightsCount = value;
                CalculateTotal();
                Notify(nameof(NightsCount));
            }
        }

        public double Discount
        {
            get => _discount;
            set
            {
                _discount = value;
                CalculateTotal();
                Notify(nameof(Discount));
            }
        }

        public double Total
        {
            get => _total;
            set
            {
                if (value < 0) throw new ArgumentException("Сумма должна быть неотрицательной");
                if (_price <=0 || _nightsCount <= 0) throw new ArgumentException("цена и ночи >= 0");
                _total = value;
                CalculateDiscount();
                Notify(nameof(Total));
            }
        }

        private void CalculateTotal()
        {
            _total = _price * _nightsCount * (1 - _discount / 100);
            if (_total < 0) throw new ArgumentException("Рассчитанная сумма отрицательна");
            Notify(nameof(Total));
        }

        private void CalculateDiscount()
        {
            if (_price <= 0 || _nightsCount <=0 || _total <=0) throw new ArgumentException("Цена, ночи и сум >=0");
            _discount = 100 * (1 - _total / (_price * _nightsCount));
            Notify(nameof(Discount));
        }
    }
}

// Практика «Карманный гугл»

using System.Collections.Generic;
using System.Linq;

namespace PocketGoogle
{
    public class Indexer : IIndexer
    {
        private readonly char[] charsForSplit = { ' ', '.', ',', '!', '?', ':', '-', '\r', '\n' };
        private readonly Dictionary<int, Dictionary<string, List<int>>> wordsAndPosByIndex =
    new Dictionary<int, Dictionary<string, List<int>>>();

        private readonly Dictionary<string, HashSet<int>> indexesByWord = new Dictionary<string, HashSet<int>>();

        public void Add(int id, string documentText)
        {
            string[] words = SplitDocumentText(documentText);
            InitializeIndex(id);
            PopulateIndexes(id, words);
        }

        private string[] SplitDocumentText(string documentText)
        {
            return documentText.Split(charsForSplit);
        }

        private void InitializeIndex(int id)
        {
            wordsAndPosByIndex.Add(id, new Dictionary<string, List<int>>());
        }

        private void PopulateIndexes(int id, string[] words)
        {
            int currentPos = 0;
            foreach (string word in words)
            {
                UpdateIndexes(id, word, currentPos);
                currentPos += word.Length + 1;
            }
        }

        private void UpdateIndexes(int id, string word, int currentPos)
        {
            UpdateWordIndex(id, word);
            UpdatePositionIndex(id, word, currentPos);
        }

        private void UpdateWordIndex(int id, string word)
        {
            if (!indexesByWord.ContainsKey(word))
                indexesByWord[word] = new HashSet<int>();

            indexesByWord[word].Add(id);
        }

        private void UpdatePositionIndex(int id, string word, int currentPos)
        {
            if (!wordsAndPosByIndex[id].ContainsKey(word))
                wordsAndPosByIndex[id].Add(word, new List<int>());

            wordsAndPosByIndex[id][word].Add(currentPos);
        }

        public List<int> GetIds(string word)
        {
            return indexesByWord.ContainsKey(word) ? indexesByWord[word].ToList() : new List<int>();
        }

        public List<int> GetPositions(int id, string word)
        {
            List<int> positions = new List<int>();

            if (wordsAndPosByIndex.ContainsKey(id) && wordsAndPosByIndex[id].ContainsKey(word))
                positions = wordsAndPosByIndex[id][word];

            return positions;
        }

        public void Remove(int id)
        {
            string[] words = wordsAndPosByIndex[id].Keys.ToArray();

            foreach (var word in words)
                indexesByWord[word].Remove(id);

            wordsAndPosByIndex.Remove(id);
        }
    }
}
