/*
 * 4.	Write a program which recursively creates 10 threads.
 * Each thread should be with the same body and receive a state with integer number, decrement it,
 * print and pass as a state into the newly created thread.
 * Use Thread class for this task and Join for waiting threads.
 * 
 * Implement all of the following options:
 * - a) Use Thread class for this task and Join for waiting threads.
 * - b) ThreadPool class for this task and Semaphore for waiting threads.
 */

using System;
using System.Threading;

namespace MultiThreading.Task4.Threads.Join
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("4.	Write a program which recursively creates 10 threads.");
            Console.WriteLine("Each thread should be with the same body and receive a state with integer number, decrement it, print and pass as a state into the newly created thread.");
            Console.WriteLine("Implement all of the following options:");
            Console.WriteLine();
            Console.WriteLine("- a) Use Thread class for this task and Join for waiting threads.");
            Console.WriteLine("- b) ThreadPool class for this task and Semaphore for waiting threads.");

            Console.WriteLine();
            Console.WriteLine("Decrease state using Thread class and Thread.Join()");
            RunDecrease(new ThreadJoinConcurrentNumberDecreaser(), 10);
            Console.WriteLine();
            Console.WriteLine("Decrease state using ThreadPool class and Semaphore class");
            RunDecrease(new ThreadPoolSemaphorNumberDecreaser(), 10);
        }

        static void RunDecrease(IConcurrentNumberDecreaser decreaser, int times) {
            decreaser.RunConcurrentDecrease(times);
        }
    }
}
