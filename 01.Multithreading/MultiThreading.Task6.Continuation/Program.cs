/*
*  Create a Task and attach continuations to it according to the following criteria:
   a.    Continuation task should be executed regardless of the result of the parent task.
   b.    Continuation task should be executed when the parent task finished without success.
   c.    Continuation task should be executed when the parent task would be finished with fail and parent task thread should be reused for continuation
   d.    Continuation task should be executed outside of the thread pool when the parent task would be cancelled
   Demonstrate the work of the each case with console utility.
*/
using System;
using System.Threading.Tasks;
using System.Threading;

namespace MultiThreading.Task6.Continuation
{
    class Program
    {
        static Random random = new Random();
        object locker = new object();
        static void Main(string[] args)
        {
            Console.WriteLine("Create a Task and attach continuations to it according to the following criteria:");
            Console.WriteLine("a.    Continuation task should be executed regardless of the result of the parent task.");
            Console.WriteLine("b.    Continuation task should be executed when the parent task finished without success.");
            Console.WriteLine("c.    Continuation task should be executed when the parent task would be finished with fail and parent task thread should be reused for continuation.");
            Console.WriteLine("d.    Continuation task should be executed outside of the thread pool when the parent task would be cancelled.");
            Console.WriteLine("Demonstrate the work of the each case with console utility.");
            Console.WriteLine();

            // feel free to add your code
            random = new Random();
            RegardlessOfResultExample();

            Thread.Sleep(500);
            Console.WriteLine();
            OnlyWhenTaskFailedExample();

            Thread.Sleep(500);
            Console.WriteLine();
            OnlyWhenTaskFailedInSameThreadExample();

            Thread.Sleep(500);
            Console.WriteLine();
            WhenAntecedentCancelledNotInThreadPool();
            Console.ReadLine();
        }

        static void RegardlessOfResultExample()
        {
            Task antecedent = Task.Factory.StartNew(() => AntecedentTaskMethod());
            Task continuation = antecedent.ContinueWith((t) => {
                Console.WriteLine("This message from continuation task printed regardless of antecedent result");
            }, TaskContinuationOptions.None);
        }

        static void OnlyWhenTaskFailedExample()
        {
            Task antecedent = Task.Factory.StartNew(() => {
                Console.WriteLine($"Message from antecedent task. Thread: {Thread.CurrentThread.ManagedThreadId}");
                throw new Exception(); });
            Task continuation = antecedent.ContinueWith((t) =>
            {
                Console.WriteLine("This message from continuation printed only when antecedent task failed");
            }, TaskContinuationOptions.OnlyOnFaulted);
        }

        static void OnlyWhenTaskFailedInSameThreadExample()
        {
            Task antecedent = Task.Factory.StartNew(() => {
                Console.WriteLine($"Antecedent task started in thread: {Thread.CurrentThread.ManagedThreadId}");
                throw new Exception();
            });
            Task continuation = antecedent.ContinueWith((t) =>
            {
                Console.WriteLine($"Task continuation started in thread {Thread.CurrentThread.ManagedThreadId}");
                Console.WriteLine("This message from continuation printed only when antecedent task failed and run in the same thread");
            }, TaskContinuationOptions.ExecuteSynchronously);
        }

        static void WhenAntecedentCancelledNotInThreadPool()
        {
            CancellationTokenSource tokenSource = new CancellationTokenSource();
            CancellationToken ct = tokenSource.Token;
            var antecedent = Task.Factory.StartNew(() =>
            {
                Console.WriteLine($"Antecedent task started in thread: {Thread.CurrentThread.ManagedThreadId}");
                ct.ThrowIfCancellationRequested();
                while (true)
                {
                    if (ct.IsCancellationRequested)
                        ct.ThrowIfCancellationRequested();
                }
            }, tokenSource.Token);
            Task continuation = antecedent.ContinueWith((t, a) =>
            {
                Console.WriteLine($"Message from continuation task that runs after antecedent task cencelled outside of thread pool. ThreadId: {Thread.CurrentThread.ManagedThreadId}");
            }, null, CancellationToken.None, TaskContinuationOptions.OnlyOnCanceled | TaskContinuationOptions.LongRunning, TaskScheduler.Default);
            tokenSource.Cancel();
        }

        static void AntecedentTaskMethod()
        {
            Console.WriteLine($"Antecedent task started in thread: {Thread.CurrentThread.ManagedThreadId}");
            if (random.Next() % 2 == 0)
            {
                Console.WriteLine("Task run with exception");
                throw new Exception();
            }
            Console.WriteLine("Hello from antescedent");
        }
    }
}
