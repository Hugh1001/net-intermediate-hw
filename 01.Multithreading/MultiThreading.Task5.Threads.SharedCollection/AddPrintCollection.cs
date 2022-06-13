using System;
using System.Collections.Generic;
using System.Threading;

namespace MultiThreading.Task5.Threads.SharedCollection
{
    public class AddPrintCollection
    {
        readonly object locker;
        readonly List<int> numbers;
        const int waitTime = 100;
        Thread AddNumbersThread, PrintNumbersThread;
        bool newStatePrinted;

        public AddPrintCollection()
        {
            locker = new object();
            numbers = new List<int>();
            newStatePrinted = true;
            PrintNumbersThread = new Thread(() => NewNumbersPrintMethod());
            PrintNumbersThread.IsBackground = true;
            PrintNumbersThread.Start();
        }

        public void AddNumbersInNewThread(int n)
        {
            AddNumbersThread = new Thread(() => AddNumbers(n));
            AddNumbersThread.Start();
        }

        private void NewNumbersPrintMethod()
        {
            while (true)
            {
                if (!newStatePrinted)
                {
                    lock (locker)
                    {
                        Console.Write("[");
                        for (int i = 0; i < numbers.Count; i++)
                        {
                            Console.Write(numbers[i]);
                            if (i < numbers.Count - 1)
                                Console.Write(",");
                        }
                        Console.WriteLine("]");
                        newStatePrinted = true;
                    }
                }
            }
        }

        private void AddNumbers(int n)
        {
            for (int i = 1; i <= n; i++)
            {
                while (!newStatePrinted)
                    Thread.Sleep(waitTime);
                lock (locker)
                {
                    numbers.Add(i);
                    newStatePrinted = false;
                };
            }
        }
    }
}
