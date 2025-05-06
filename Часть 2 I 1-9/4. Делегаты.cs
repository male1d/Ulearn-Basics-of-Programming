// Синтаксис делегатов

public delegate void TellUser(string message);

public class Program
{
    public static void Main()
    {
        Run(Console.WriteLine);
    }

    static void Run(TellUser tellUser)
    {
        tellUser("hi!");
        tellUser("how r u?");
    }
}

// Синтаксис делегатов 2

public delegate bool TryGet<T1, T2>(T1 arg1, Action<T1> arg2, out T2 arg3);

public class Program
{
    public static void Main()
    {
        Run(AskUser, Console.WriteLine);
    }

    static void Run(TryGet<string, int> askUser, Action<string> tellUser)
    {
        int age;
        if (askUser("What is your age?", tellUser, out age))
            tellUser("Age: " + age);
    }

    static bool AskUser(string questionText, Action<string> tellUser, out int age)
    {
        tellUser(questionText);
        var answer = Console.ReadLine();
        return int.TryParse(answer, out age);
    }
}

// Синтаксис лямбд

private static readonly Func<int> zero = () => 0;
    private static readonly Func<int, string> toString = x => x.ToString();
    private static readonly Func<double, double, double> add = (x, y) => x + y;
    private static readonly Action<string> print = Console.WriteLine;

// Синтаксис лямбд 2

static Func<T1, T3> Combine<T1, T2, T3>(Func<T1, T2> f, Func<T2, T3> g)
{
    return x => g(f(x));
}

// Делегаты

1.Func<int, string, double>
2.Func<int, List<int>>[]
3. 5 5 5 5 5
4. символ 'Z'
5. 0 1 2 3 4
6. 0 1 2 3 4

// Практика «Виртуальная машина Brainfuck»

using System;
using System.Collections.Generic;

namespace func.brainfuck
{
    public class VirtualMachine : IVirtualMachine
    {
        public string Instructions { get; }
        public int InstructionPointer { get; set; }
        public byte[] Memory { get; }
        public int MemoryPointer { get; set; }

        private readonly Dictionary<char, Action<IVirtualMachine>> _commands;

        public VirtualMachine(string program, int memorySize)
        {
            Instructions = program;
            Memory = new byte[memorySize];
            MemoryPointer = 0;
            InstructionPointer = 0;
            _commands = new Dictionary<char, Action<IVirtualMachine>>();
        }

        public void RegisterCommand(char symbol, Action<IVirtualMachine> execute)
        {
            _commands[symbol] = execute;
        }

        public void Run()
        {
            while (InstructionPointer < Instructions.Length)
            {
                char currentInstruction = Instructions[InstructionPointer];
                if (_commands.TryGetValue(currentInstruction, out var command))
                {
                    command(this);
                }
                InstructionPointer++;
            }
        }
    }
}

// Практика «Простые команды Brainfuck»

using System;
using System.Collections.Generic;
using System.Linq;

namespace func.brainfuck
{
    public class BrainfuckBasicCommands
    {
        public static void RegisterTo(IVirtualMachine vm, Func<int> read, Action<char> write)
        {
            RegisterOutputCommand(vm, write);
            RegisterArithmeticCommands(vm);
            RegisterInputCommand(vm, read);
            RegisterPointerCommands(vm);
            RegisterCharacterCommands(vm);
        }

        private static void RegisterOutputCommand(IVirtualMachine vm, Action<char> write)
        {
            vm.RegisterCommand('.', b =>
            {
                write((char)b.Memory[b.MemoryPointer]);
            });
        }

        private static void RegisterArithmeticCommands(IVirtualMachine vm)
        {
            vm.RegisterCommand('+', b =>
            {
                b.Memory[b.MemoryPointer]++;
            });

            vm.RegisterCommand('-', b =>
            {
                b.Memory[b.MemoryPointer]--;
            });
        }

        private static void RegisterInputCommand(IVirtualMachine vm, Func<int> read)
        {
            vm.RegisterCommand(',', b =>
            {
                int input = read();
                if (input != -1)
                {
                    b.Memory[b.MemoryPointer] = (byte)input;
                }
            });
        }

        private static void RegisterPointerCommands(IVirtualMachine vm)
        {
            vm.RegisterCommand('>', b =>
            {
                b.MemoryPointer = (b.MemoryPointer + 1) % b.Memory.Length;
            });

            vm.RegisterCommand('<', b =>
            {
                b.MemoryPointer = (b.MemoryPointer - 1 + b.Memory.Length) % b.Memory.Length;
            });
        }

        private static void RegisterCharacterCommands(IVirtualMachine vm)
        {
            RegisterCharacterCommands(vm, 'A', 26); // Для заглавных букв
            RegisterCharacterCommands(vm, 'a', 26); // Для строчных букв
            RegisterDigitCommands(vm);               // Для цифр
        }

        private static void RegisterCharacterCommands(IVirtualMachine vm, char start, int count)
        {
            foreach (char c in Enumerable.Range(start, count).Select(x => (char)x))
            {
                vm.RegisterCommand(c, b =>
                {
                    b.Memory[b.MemoryPointer] = (byte)c;
                });
            }
        }

        private static void RegisterDigitCommands(IVirtualMachine vm)
        {
            RegisterCharacterCommands(vm, '0', 10); // Для цифр
        }
    }
}

// Практика «Циклы Brainfuck»

using System.Collections.Generic;

namespace func.brainfuck
{
    public class BrainfuckLoopCommands
    {
        public static void RegisterTo(IVirtualMachine vm)
        {
            Dictionary<int, int> loopsByStart = new Dictionary<int, int>();
            Stack<int> currentLoopsByStart = new Stack<int>();

            vm.RegisterCommand('[', b => HandleOpenLoop(b, loopsByStart, currentLoopsByStart));
            vm.RegisterCommand(']', b => HandleCloseLoop(b, currentLoopsByStart));
        }

        private static void HandleOpenLoop(
            IVirtualMachine vm,
            Dictionary<int, int> loopsByStart,
            Stack<int> currentLoopsByStart)
        {
            if (loopsByStart.Count == 0)
            {
                Stack<int> startOfLoops = new Stack<int>();
                startOfLoops.Push(vm.InstructionPointer);
                for (int tempIndex = vm.InstructionPointer + 1; tempIndex != vm.Instructions.Length; tempIndex++)
                {
                    if (vm.Instructions[tempIndex] == '[')
                        startOfLoops.Push(tempIndex);
                    else if (vm.Instructions[tempIndex] == ']')
                        loopsByStart[startOfLoops.Pop()] = tempIndex;
                }
            }
            if (vm.Memory[vm.MemoryPointer] == 0)
                vm.InstructionPointer = loopsByStart[vm.InstructionPointer];
            else
                currentLoopsByStart.Push(vm.InstructionPointer);
        }

        private static void HandleCloseLoop(IVirtualMachine vm, Stack<int> currentLoopsByStart)
        {
            if (vm.Memory[vm.MemoryPointer] != 0)
                vm.InstructionPointer = currentLoopsByStart.Peek();
            else
                currentLoopsByStart.Pop();
        }
    }
}