// HeapifyUp

public static void HeapifyUp(List<int> heap)
{
    var itemIndex = heap.Count - 1; // Этот элемент только что вставили в кучу.

    // Поднимем его до нужного уровня в дереве
    while (itemIndex > 1) // Не забываем, что элемент с индексом 0 - фиктивный
    {
        var parentIndex = itemIndex / 2; // Индекс родителя

        // Если приоритет текущего элемента меньше приоритета родителя, меняем их местами
        if (heap[itemIndex] < heap[parentIndex])
        {
            // Меняем местами
            var t = heap[itemIndex];
            heap[itemIndex] = heap[parentIndex];
            heap[parentIndex] = t;

            // Переходим к родителю
            itemIndex = parentIndex;
        }
        else
        {
            // Свойство кучи восстановлено, выходим из цикла
            break;
        }
    }
}

// GetMinValue

public static int GetMinValue(TreeNode root)
{
    return root.Left == null ? root.Value : GetMinValue(root.Left);
}

// Реализация бинарного дерева

public static TreeNode Search(TreeNode root, int element)
{
    if (root == null) return null; // Если узел пустой, возвращаем null
    if (element == root.Value) return root; // Если нашли нужный элемент, возвращаем текущий узел

    // Рекурсивно ищем в левой или правой части дерева
    return element < root.Value ? Search(root.Left, element) : Search(root.Right, element);
}

// Структуры данных

1.Это может быть ни тем и ни другим
2.При добавлении N чисел по порядку начиная с меньших к большим, высота дерева окажется порядка N , Вставка элемента в бинарное дерево поиска имеет сложность Θ(h) где h - высота дерева
3.Это куча
4.Верно

// Практика «Add и Contains»

using System;
using System.Collections;
using System.Collections.Generic;

namespace BinaryTrees
{
    public class BinaryTree<T> : IEnumerable<T> where T : IComparable
    {
        private class Node
        {
            public T Value;
            public Node Left;
            public Node Right;

            public Node(T value)
            {
                Value = value;
            }
        }

        private Node root;

        public void Add(T key)
        {
            if (root == null)
            {
                root = new Node(key);
                return;
            }

            InsertRecursively(root, key);
        }

        private void InsertRecursively(Node current, T key)
        {
            if (key.CompareTo(current.Value) < 0)
            {
                if (current.Left == null)
                {
                    current.Left = new Node(key);
                }
                else
                {
                    InsertRecursively(current.Left, key);
                }
            }
            else
            {
                if (current.Right == null)
                {
                    current.Right = new Node(key);
                }
                else
                {
                    InsertRecursively(current.Right, key);
                }
            }
        }

        public bool Contains(T key)
        {
            Node current = root;
            while (current != null)
            {
                int comparison = key.CompareTo(current.Value);
                if (comparison == 0)
                {
                    return true; // found
                }
                if (comparison < 0)
                {
                    current = current.Left;
                }
                else
                {
                    current = current.Right;
                }
            }
            return false; // not found
        }

        public IEnumerator<T> GetEnumerator()
        {
            return InOrderTraversal(root).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        private IEnumerable<T> InOrderTraversal(Node node)
        {
            if (node != null)
            {
                foreach (var left in InOrderTraversal(node.Left))
                {
                    yield return left;
                }
                yield return node.Value;
                foreach (var right in InOrderTraversal(node.Right))
                {
                    yield return right;
                }
            }
        }
    }
}

// Практика «Enumerable и Индексатор»

using System;
using System.Collections;
using System.Collections.Generic;

namespace BinaryTrees
{
    public class BinaryTree<T> : IEnumerable<T> where T : IComparable
    {
        private class Node
        {
            public T Value;
            public Node Left;
            public Node Right;
            public int Size = 1;

            public Node(T value) => Value = value;
        }

        private Node root;

        public void Add(T key)
        {
            root = Insert(root, key);
        }

        private Node Insert(Node node, T key)
        {
            if (node == null)
                return new Node(key);

            if (key.CompareTo(node.Value) < 0)
                node.Left = Insert(node.Left, key);
            else
                node.Right = Insert(node.Right, key);

            node.Size = 1 + GetSize(node.Left) + GetSize(node.Right);
            return node;
        }

        public bool Contains(T key) => Search(root, key) != null;

        private Node Search(Node node, T key)
        {
            if (node == null)
                return null;

            int cmp = key.CompareTo(node.Value);
            if (cmp == 0)
                return node;

            return cmp < 0 ? Search(node.Left, key) : Search(node.Right, key);
        }

        public T this[int index]
        {
            get
            {
                if (index < 0 || index >= GetSize(root))
                    throw new IndexOutOfRangeException();

                return GetNodeAt(root, index).Value;
            }
        }

        private Node GetNodeAt(Node node, int index)
        {
            int leftSize = GetSize(node.Left);

            if (index < leftSize)
                return GetNodeAt(node.Left, index);

            if (index > leftSize)
                return GetNodeAt(node.Right, index - leftSize - 1);

            return node;
        }

        private int GetSize(Node node) => node?.Size ?? 0;

        public IEnumerator<T> GetEnumerator() => InOrder(root).GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        private IEnumerable<T> InOrder(Node node)
        {
            if (node == null)
                yield break;

            foreach (var val in InOrder(node.Left))
                yield return val;

            yield return node.Value;

            foreach (var val in InOrder(node.Right))
                yield return val;
        }
    }
}

// Практика «Disk Tree»

using System;
using System.Collections.Generic;
using System.Linq;

namespace DiskTree
{
    public static class DiskTreeTask
    {
        public static List<string> Solve(List<string> input)
        {
            var root = new DirectoryNode("");
            foreach (var path in input)
            {
                var parts = path.Split('\\');
                var currentNode = root;
                foreach (var part in parts)
                {
                    if (!currentNode.Children.ContainsKey(part))
                    {
                        currentNode.Children[part] = new DirectoryNode(part);
                    }
                    currentNode = currentNode.Children[part];
                }
            }

            var result = new List<string>();
            BuildTreeOutput(root, -1, result);
            return result;
        }

        private static void BuildTreeOutput(DirectoryNode node, int level, List<string> result)
        {
            if (level >= 0)
            {
                result.Add(new string(' ', level) + node.Name);
            }

            foreach (var child in node.Children.OrderBy(x => x.Key, StringComparer.Ordinal))
            {
                BuildTreeOutput(child.Value, level + 1, result);
            }
        }
    }

    public class DirectoryNode
    {
        public string Name { get; }
        public Dictionary<string, DirectoryNode> Children { get; }

        public DirectoryNode(string name)
        {
            Name = name;
            Children = new Dictionary<string, DirectoryNode>(StringComparer.Ordinal);
        }
    }
}