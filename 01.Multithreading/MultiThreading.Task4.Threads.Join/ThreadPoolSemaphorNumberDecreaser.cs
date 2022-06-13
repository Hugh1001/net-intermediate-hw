using System;
using System.Threading;

namespace MultiThreading.Task4.Threads.Join
{
    public class ThreadPoolSemaphorNumberDecreaser : IConcurrentNumberDecreaser
    {
        readonly Semaphore pool;
        int state;
        public ThreadPoolSemaphorNumberDecreaser()
        {
            pool = new Semaphore(1, 3);
        }

        public void RunConcurrentDecrease(int number)
        {
            state = number;
            for (int i = 0; i < 10; i++)
            {
                pool.WaitOne();
                ThreadPool.QueueUserWorkItem(new WaitCallback(DecreasWithSemaphor), state);
            }
            Thread.Sleep(1000);
        }

        public void DecreasWithSemaphor(object obj)
        {
            Console.WriteLine($"Thread started. ThreadId: {Thread.CurrentThread.ManagedThreadId}");
            Console.WriteLine($"State is equal to {state}");
            int num = (int) obj;
            num--;
            state = num;
            Console.WriteLine($"State decreased. Now state is equal to {state}");
            pool.Release();
        }
    }
}
