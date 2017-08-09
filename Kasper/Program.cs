using System;
using System.Collections.Generic;
using System.Linq;

namespace Kasper
{
    class Program
    {
        private static ThreadSafeQueue<int> _queue = new ThreadSafeQueue<int>();
        static void Main(string[] args)
        {
            Console.WriteLine("Task 1. Enter 'pop', 'exit' or any integer number (without words) to push element");
            var command = Console.ReadLine();
            while (command != "exit")
            {
                if (command == "pop")
                {
                    var task = _queue.PopAsync();
                    task.ContinueWith(t => Console.WriteLine($"Popped Item: {t.Result}"));
                }
                else if (int.TryParse(command, out int item))
                {
                    _queue.Push(item);
                    Console.WriteLine($"Item pushed: {item}");
                }
                else
                {
                    Console.WriteLine("Wrong command, use 'pop', 'exit' or just any number to push element");
                }

                command = Console.ReadLine();
            }

            Console.WriteLine("Task 2. Enter integers separated with spaces (first integer is X) or 'exit'");
            command = Console.ReadLine();
            while (command != "exit")
            {
                try
                {
                    string[] input = command?.Split(' '); //no input protection
                    int[] integers = new int[input.Length - 1];
                    for (int i = 1; i < input.Length; i++)
                    {
                        integers[i - 1] = int.Parse(input[i]);
                    }
                    int x = int.Parse(input[0]);
                    var result = PairSum.GetAllPairsWithSum(integers, x);

                    foreach (var item in result)
                    {
                        Console.WriteLine($"{item.Key} {item.Value}");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Wrong input: {ex.Message}");
                }

                Console.WriteLine("Try again. Enter integers separated with spaces (first integer is X) or 'exit'");
                command = Console.ReadLine();
            }
        }
    }
}
