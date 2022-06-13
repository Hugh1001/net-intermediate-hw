/*
 * 2.	Write a program, which creates a chain of four Tasks.
 * First Task – creates an array of 10 random integer.
 * Second Task – multiplies this array with another random integer.
 * Third Task – sorts this array by ascending.
 * Fourth Task – calculates the average value. All this tasks should print the values to console.
 */
using System;
using System.Threading.Tasks;

namespace MultiThreading.Task2.Chaining
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(".Net Mentoring Program. MultiThreading V1 ");
            Console.WriteLine("2.	Write a program, which creates a chain of four Tasks.");
            Console.WriteLine("First Task – creates an array of 10 random integer.");
            Console.WriteLine("Second Task – multiplies this array with another random integer.");
            Console.WriteLine("Third Task – sorts this array by ascending.");
            Console.WriteLine("Fourth Task – calculates the average value. All this tasks should print the values to console");
            Console.WriteLine();

            // feel free to add your code
            Random random = new Random();
            var maxVal = 1000;
            Task<int[]> createRandomNumbersTask = Task.Factory.StartNew(() => {
                Console.WriteLine("-------------------------------------------------------");
                Console.WriteLine("First task started");
                var nums = new int[10]; 
                for (int i = 0; i < 10; i++) {
                    nums[i] = random.Next(maxVal);
                    Console.WriteLine($"Number with index {i} is equal to {nums[i]}");
                }
                return nums;
            });
            Task<int[]> multiplyByRandomNumbersTask = createRandomNumbersTask.ContinueWith(antecendent =>
            {
                Console.WriteLine("-------------------------------------------------------");
                Console.WriteLine("Second task started");
                var nums = antecendent.Result;
                for (int i = 0; i < nums.Length; i++)
                {
                    var randInt = random.Next(maxVal);
                    nums[i] = nums[i] * random.Next(randInt);
                    Console.WriteLine($"Number with index {i} is multiplied by {randInt}, result is {nums[i]}");
                }
                return nums;
            });
            Task<int[]> sortNumbersTask = multiplyByRandomNumbersTask.ContinueWith(antecedent =>
            {
                Console.WriteLine("-------------------------------------------------------");
                Console.WriteLine("Third task started");
                var nums = antecedent.Result;
                Array.Sort(nums);
                Console.WriteLine($"Array of integers is sorted");
                for (var i = 0; i < nums.Length; i++)
                {
                    Console.WriteLine($"{nums[i]}");
                }
                return nums;
            });
            Task<double> returnNumbersAverageTask = sortNumbersTask.ContinueWith(antecedent =>
            {
                Console.WriteLine("-------------------------------------------------------");
                Console.WriteLine("Fourth task started");
                var nums = antecedent.Result;
                var sum = 0;
                foreach (var num in nums)
                {
                    sum += num;
                }
                double result = (double)sum / nums.Length;
                Console.WriteLine($"The average nums is equal to {result}");
                return result;
            });

            Task.WaitAll(createRandomNumbersTask, multiplyByRandomNumbersTask, sortNumbersTask, returnNumbersAverageTask);
            Console.ReadLine();
        }
    }
}
