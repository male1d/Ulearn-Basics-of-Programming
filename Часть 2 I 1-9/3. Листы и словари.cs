// Сложность операций

Всё кроме Сложность операции Add всегда Θ(1)

// Тонкости операторов

1. Неверно
2. Неверно
3. Верно
4. Верно
5. Верно
6. public static bool operator ==(A a, int b) и public static A operator ==(A a, A b)
7.Не стоит перегружать оператор, если это сделает код более загадочным. и Не стоит перегружать оператор, если это может подтолкнуть читателя к неверной интерпретации кода.

// Dictionary, Equals и GetHashCode

1. Словарь позволяет эффективно проверить, содержит ли он ключ и Для каждого ключа словарь хранит только одно значение
2. return 42 , return Surname.GetHashCode() , return Surname.GetHashCode() * 31 + Name.GetHashCode() , return (Surname.GetHashCode() * 31 + Name.GetHashCode()) * 31 + Patronymic.GetHashCode()

// Практика «Readonly bytes»

using System;
using System.Collections;
using System.Collections.Generic;

namespace hashes
{
    public class ReadonlyBytes : IReadOnlyList<byte>
    {
        private readonly byte[] _data;
        private readonly int _hash;

        public ReadonlyBytes(params byte[] data)
        {
            if (data == null)
                throw new ArgumentNullException(nameof(data));

            _data = new byte[data.Length];
            Array.Copy(data, _data, data.Length);
            _hash = ComputeHashCode();
        }

        public byte this[int index]
        {
            get
            {
                try
                {
                    return _data[index];
                }
                catch (Exception ex)
                {
                    throw new IndexOutOfRangeException(ex.Message, ex);
                }
            }
        }

        public int Count => _data.Length;
        public int Length => _data.Length;

        private bool AreEqual(ReadonlyBytes other)
        {
            if (other == null || Length != other.Length)
                return false;

            for (int i = 0; i < Length; i++)
            {
                if (this[i] != other[i])
                    return false;
            }
            return true;
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            return AreEqual((ReadonlyBytes)obj);
        }

        private int ComputeHashCode()
        {
            int hash = -985847861;
            foreach (byte b in _data)
            {
                hash = unchecked(hash * -1521134295 + b.GetHashCode());
            }
            return hash;
        }

        public override int GetHashCode() => _hash;

        public override string ToString()
        {
            var result = new System.Text.StringBuilder("[");
            if (_data.Length > 0)
            {
                foreach (byte b in _data)
                {
                    result.Append(b).Append(", ");
                }
                result.Length -= 2;
            }
            result.Append("]");
            return result.ToString();
        }

        public IEnumerator<byte> GetEnumerator()
        {
            return ((IEnumerable<byte>)_data).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}

// Практика «Ghosts»

using System;
using System.Runtime.CompilerServices;
using System.Text;
 
namespace hashes
{
    public class GhostsTask :
        IFactory<Document>, IFactory<Vector>, IFactory<Segment>, IFactory<Cat>, IFactory<Robot>,
        IMagic
    {
        private byte[] documentArray = { 1, 1, 2 };
        Document document;
        Segment segment;
        Cat cat = new Cat("Abra", "Bara", DateTime.MaxValue);
        Vector vector = new Vector(0, 0);
        Robot robot = new Robot("CJP-42");

        public GhostsTask()
        {
            segment = new Segment(vector, vector);
            document = new Document("Bebra", Encoding.Unicode, documentArray);
        }

        public void DoMagic()
        {
            documentArray[0] = 10;
            vector.Add(new Vector(2, 28));
            cat.Rename("Alen");
            Robot.BatteryCapacity++;
        }

        Document IFactory<Document>.Create()
        {
            return document;
        }

        Vector IFactory<Vector>.Create()
        {
            return vector;
        }

        Segment IFactory<Segment>.Create()
        {
            return segment;
        }

        Cat IFactory<Cat>.Create()
        {
            return cat;
        }

        Robot IFactory<Robot>.Create()
        {
            return robot;
        }
    }
}