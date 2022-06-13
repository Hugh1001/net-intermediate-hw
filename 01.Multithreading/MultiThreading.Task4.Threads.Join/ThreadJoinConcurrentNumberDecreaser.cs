using System;
using System.Threading;

namespace MultiThreading.Task4.Threads.Join
{
    public class ThreadJoinConcurrentNumberDecreaser : IConcurrentNumberDecreaser
    {
        int state;
        public void RunConcurrentDecrease(int number)
        {
            state = number;
            Thread previous = null;
            Thread current;
            for(int i = 0; i < number; i++)
            {
                if(previous != null)
                    previous.Join();
                current = new Thread(() =>
                {
                    Console.WriteLine($"Thread started. ThreadId: {Thread.CurrentThread.ManagedThreadId}");
                    Console.WriteLine($"State is equal to {state}");
                    state--;
                    Console.WriteLine($"State decreased. Now state is equal {state}");
                });
                current.Start();
                previous = current;
            }
        }
    }
}
