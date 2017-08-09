using System;
using Kasper;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests
{
    [TestClass]
    public class PairsTests
    {
        [TestMethod]
        public void Simple()
        {
            var input = new[] {1, 2, 3, 5, 6};
            var x = 7;
            var result = PairSum.GetAllPairsWithSum(input, x);
            Assert.AreEqual(2, result.Count);
            Assert.AreEqual(1, result[6]);
            Assert.AreEqual(2, result[5]);
        }

        [TestMethod]
        public void MaxValue()
        {
            var input = new[] { Int32.MaxValue, Int32.MaxValue, Int32.MaxValue, Int32.MaxValue };
            var x = 1;
            var result = PairSum.GetAllPairsWithSum(input, x);
            Assert.AreEqual(0, result.Count);
        }

        [TestMethod]
        public void MinValue()
        {
            var input = new[] { Int32.MinValue, Int32.MinValue, Int32.MinValue, Int32.MinValue };
            var x = Int32.MinValue;
            var result = PairSum.GetAllPairsWithSum(input, x);
            Assert.AreEqual(0, result.Count);
        }

        [TestMethod]
        public void Duplicates()
        {
            var input = new[] { 2, 2, 2, 2, 3, 1 };
            var x = 4;
            var result = PairSum.GetAllPairsWithSum(input, x);
            Assert.AreEqual(2, result.Count);
            Assert.AreEqual(3, result[1]);
            Assert.AreEqual(2, result[2]);
        }

        [TestMethod]
        public void MaxMin()
        {
            var input = new[] { Int32.MinValue, Int32.MaxValue };
            var x = -1;
            var result = PairSum.GetAllPairsWithSum(input, x);
            Assert.AreEqual(1, result.Count);
        }
    }
}
