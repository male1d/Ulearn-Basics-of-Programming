// Процессы и потоки

using System.Net.Sockets;

1.Верно
2.Неверно
3.Верно
4.Верно
5.Неверно
6.Неверно

// Потоки и блокировки

1.Чтобы обеспечить корректное использование разделяемого между потоками ресурса
2.Верно
3.Верно
4.Неверно
5.Верно
6.... все остальные потоки не окажутся вне секций, заключенных в `lock(obj)`, с тем же `obj` , ... все остальные потоки не окажутся вне этой секции
7.Сверяться с документацией , По умолчанию считать, что все операции не являются потокобезопасными

// Практика «Поток для AI»

using System;
using System.Collections.Generic;

namespace rocket_bot
{
    public class Channel<T> where T : class
    {
        // Список для хранения элементов
        private readonly List<T> items = new List<T>();
        // Объект для синхронизации доступа к списку
        private readonly object syncLock = new object();

        // Индексатор для доступа к элементам по индексу
        public T this[int index]
        {
            get
            {
                lock (syncLock) // Блокировка для обеспечения потокобезопасности
                {
                    // Проверяем, что индекс в пределах допустимого диапазона
                    return IsValidIndex(index) ? items[index] : null;
                }
            }
            set
            {
                lock (syncLock) // Блокировка для обеспечения потокобезопасности
                {
                    if (IsValidIndex(index))
                    {
                        // Обновляем элемент по индексу и удаляем все последующие элементы
                        items[index] = value;
                        items.RemoveRange(index + 1, items.Count - (index + 1));
                    }
                    else if (index == items.Count)
                    {
                        // Добавляем новый элемент, если индекс равен размеру списка
                        items.Add(value);
                    }
                }
            }
        }

        // Проверяет, является ли индекс допустимым
        private bool IsValidIndex(int index) => index >= 0 && index < items.Count;

        // Возвращает последний элемент в списке, если он существует
        public T LastItem()
        {
            lock (syncLock) // Блокировка для обеспечения потокобезопасности
            {
                return items.Count > 0 ? items[^1] : null; // Используем оператор ^ для получения последнего элемента
            }
        }

        // Добавляет элемент в список, если последний элемент не изменился
        public void AppendIfLastItemIsUnchanged(T item, T knownLastItem)
        {
            lock (syncLock) // Блокировка для обеспечения потокобезопасности
            {
                if (LastItem() == knownLastItem)
                {
                    items.Add(item); // Добавляем элемент, если последний элемент соответствует известному
                }
            }
        }

        // Возвращает количество элементов в списке
        public int Count
        {
            get
            {
                lock (syncLock) // Блокировка для обеспечения потокобезопасности
                {
                    return items.Count;
                }
            }
        }
    }
}

// Практика «Параллельный AI»

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace rocket_bot;

public partial class Bot
{
    public Rocket GetNextMove(Rocket rocket)
    {
        // Создаем список задач для параллельного выполнения
        var tasks = CreateTasks(rocket);
        // Ждем завершения всех задач и получаем результаты
        var results = Task.WhenAll(tasks).GetAwaiter().GetResult();

        // Выбираем лучший результат
        var bestMove = results.OrderByDescending(r => r.Score).First();

        return rocket.Move(bestMove.Turn, level);
    }

    public List<Task<(Turn Turn, double Score)>> CreateTasks(Rocket rocket)
    {
        var tasks = new List<Task<(Turn Turn, double Score)>>();

        // Разделяем количество итераций между потоками
        int iterationsPerThread = iterationsCount / threadsCount;

        for (int i = 0; i < threadsCount; i++)
        {
            // Создаем уникальный экземпляр Random для каждого потока
            var random = new Random(Guid.NewGuid().GetHashCode());
            tasks.Add(Task.Run(() => SearchBestMove(rocket, random, iterationsPerThread)));
        }

        return tasks;
    }
}