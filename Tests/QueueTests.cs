using System.Threading;
using System.Threading.Tasks;
using Kasper;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests
{
    [TestClass]
    public class QueueTests
    {
        [TestMethod]
        public async Task Simple()
        {
            var threadSafeQueue = new ThreadSafeQueue<int>();
            threadSafeQueue.Push(5);
            var result = await threadSafeQueue.PopAsync();
            Assert.AreEqual(5, result);
        }

        [TestMethod]
        public void PopIsWaiting()
        {
            var threadSafeQueue = new ThreadSafeQueue<int>();
            var task = threadSafeQueue.PopAsync();
            Thread.Sleep(10);
            Assert.AreEqual(TaskStatus.WaitingForActivation, task.Status);
        }

        [TestMethod]
        public async Task PopIsWaitingSuccesfully()
        {
            var threadSafeQueue = new ThreadSafeQueue<int>();
            var task = threadSafeQueue.PopAsync();
            threadSafeQueue.Push(5);
            await task.ContinueWith(t => Assert.AreEqual(5, t.Result));
        }

        [TestMethod]
        public async Task MultiThreaded()
        {
            var threadSafeQueue = new ThreadSafeQueue<int>();

            var t1 = new Thread(() =>
            {
                for (int i = 0; i < 10000; i++)
                {
                    threadSafeQueue.Push(i);
                }
            });
            t1.Start();

            var t2 = new Thread(() =>
            {
                for (int i = 0; i < 10000; i++)
                {
                    threadSafeQueue.Push(i);
                }
            });
            t2.Start();

            var t3 = new Thread(async () =>
            {
                for (int i = 0; i < 10000-1; i++)
                {
                    await threadSafeQueue.PopAsync();
                }
            });
            t3.Start();

            var t4 = new Thread(async () =>
            {
                for (int i = 0; i < 10000; i++)
                {
                    await threadSafeQueue.PopAsync();
                }
            });
            t4.Start();

            t1.Join();
            t2.Join();
            t3.Join();
            t4.Join();

            var result = await threadSafeQueue.PopAsync();
            Assert.AreEqual(9999, result);

            var task = threadSafeQueue.PopAsync();
            Thread.Sleep(10);
            Assert.AreEqual(TaskStatus.WaitingForActivation, task.Status);
            threadSafeQueue.Push(5);
            await task.ContinueWith(t => Assert.AreEqual(5, t.Result));
        }
    }
}
