// Стеки и очереди

1. 0
2. 2
3. Dequeue работает за O(размер_очереди)
4. Очередь пуста и Очередь содержит один элемент

// Максимум в массиве

static T Max<T>(T[] source) where T : IComparable<T>
{
    if (source.Length == 0)
        return default(T);

    T max = source[0];
    for (int i = 1; i < source.Length; i++)
    {
        if (source[i].CompareTo(max) > 0)
        {
            max = source[i];
        }
    }
    return max;
}

// Практика «Limited Size Stack»

using System;
using System.Collections.Generic;

public class LimitedSizeStack<T>
{
    private readonly int _maxSize;
    private readonly LinkedList<T> _list;

    public LimitedSizeStack(int maxSize)
    {
        if (maxSize < 0)
            throw new ArgumentOutOfRangeException(nameof(maxSize), "Макс размер>0");

        _maxSize = maxSize;
        _list = new LinkedList<T>();
    }

    public void Push(T item)
    {
        if (_maxSize == 0)
            return;

        if (_list.Count == _maxSize)
        {
            _list.RemoveLast();
        }


        _list.AddFirst(item);
    }

    public T Pop()
    {
        if (_list.Count == 0)
            throw new InvalidOperationException("Стек пуст");

        var firstNode = _list.First;
        _list.RemoveFirst();
        return firstNode.Value;
    }

    public int Count => _list.Count;

    public T Last()
    {
        if (_list.Count == 0)
            throw new InvalidOperationException("Стек пуст");

        return _list.First.Value;
    }
}

// Практика «Отмена»

using System;
using System.Collections.Generic;

namespace LimitedSizeStack;

public class ListModel<TItem>
{
    public List<TItem> Items { get; }
    public int UndoLimit;

    private readonly LimitedSizeStack<ICommand> history;

    public ListModel(int undoLimit) : this(new List<TItem>(), undoLimit)
    {
    }

    public ListModel(List<TItem> items, int undoLimit)
    {
        Items = items;
        UndoLimit = undoLimit;
        history = new LimitedSizeStack<ICommand>(undoLimit);
    }

    public void AddItem(TItem item)
    {
        history.Push(new AddCommand(this, item));
        Items.Add(item);
    }

    public void RemoveItem(int index)
    {
        TItem item = Items[index];
        history.Push(new RemoveCommand(this, item, index));
        Items.RemoveAt(index);
    }

    public bool CanUndo()
    {
        return history.Count > 0;
    }

    public void Undo()
    {
        if (!CanUndo())
            throw new InvalidOperationException("Нет действий для отмены");

        ICommand command = history.Pop();
        command.Undo();
    }

    private interface ICommand
    {
        void Undo();
    }

    private class AddCommand : ICommand
    {
        private readonly ListModel<TItem> model;
        private readonly TItem item;

        public AddCommand(ListModel<TItem> model, TItem item)
        {
            this.model = model;
            this.item = item;
        }

        public void Undo()
        {
            model.Items.Remove(item);
        }
    }

    private class RemoveCommand : ICommand
    {
        private readonly ListModel<TItem> model;
        private readonly TItem item;
        private readonly int index;

        public RemoveCommand(ListModel<TItem> model, TItem item, int index)
        {
            this.model = model;
            this.item = item;
            this.index = index;
        }

        public void Undo()
        {
            model.Items.Insert(index, item);
        }
    }
}

// Практика «CVS»

using System.Collections.Generic;

namespace Clones
{
    public class CloneVersionSystem : ICloneVersionSystem
    {
        private readonly List<Clone> cloneList;

        public CloneVersionSystem()
        {
            cloneList = new List<Clone> { new Clone() };
        }

        public string Execute(string query)
        {
            var queryArr = query.Split(' ');
            var cloneNum = int.Parse(queryArr[1]) - 1;
            return ExecuteCommand(queryArr, cloneNum);
        }

        private string ExecuteCommand(string[] queryArr, int cloneNum)
        {
            switch (queryArr[0])
            {
                case "check":
                    return cloneList[cloneNum].Check();

                case "learn":
                    return LearnCommand(queryArr, cloneNum);

                case "rollback":
                    cloneList[cloneNum].RollBack();
                    return null;

                case "relearn":
                    cloneList[cloneNum].Relearn();
                    return null;

                case "clone":
                    cloneList.Add(new Clone(cloneList[cloneNum]));
                    return null;
            }
            return null;
        }

        private string LearnCommand(string[] queryArr, int cloneNum)
        {
            var programNum = int.Parse(queryArr[2]);
            cloneList[cloneNum].Learn(programNum);
            return null;
        }
    }

    public class Clone
    {
        private readonly Stack learnedProgramms;
        private readonly Stack rollBackHistory;

        public Clone()
        {
            learnedProgramms = new Stack();
            rollBackHistory = new Stack();
        }

        public Clone(Clone anotherClone)
        {
            learnedProgramms = new Stack(anotherClone.learnedProgramms);
            rollBackHistory = new Stack(anotherClone.rollBackHistory);
        }

        public void Learn(int commandNumber)
        {
            rollBackHistory.Clear();
            learnedProgramms.Push(commandNumber);
        }

        public void RollBack()
        {
            rollBackHistory.Push(learnedProgramms.Pop());
        }

        public void Relearn()
        {
            learnedProgramms.Push(rollBackHistory.Pop());
        }

        public string Check()
        {
            return learnedProgramms.IsEmpty() ? "basic" : learnedProgramms.Peek().ToString();
        }
    }

    public class Stack
    {
        private StackItem last;
        public Stack() { }

        public Stack(Stack stack)
        {
            last = stack.last;
        }

        public void Push(int value)
        {
            last = new StackItem(value, last);
        }

        public int Peek()
        {
            return last.Value;
        }

        public int Pop()
        {
            var value = last.Value;
            last = last.Previous;
            return value;
        }

        public bool IsEmpty()
        {
            return last == null;
        }

        public void Clear()
        {
            last = null;
        }
    }

    public class StackItem
    {
        public readonly int Value;
        public readonly StackItem Previous;

        public StackItem(int value, StackItem previous)
        {
            Value = value;
            Previous = previous;
        }
    }
}