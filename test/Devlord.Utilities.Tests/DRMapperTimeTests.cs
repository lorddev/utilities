#pragma warning disable S101 // Types should be named in camel case

using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using FastMember;
using FizzWare.NBuilder;
using NUnit.Framework;
using Should;

namespace Devlord.Utilities.Tests
{
    /// <summary>
    /// Provides benchmarking for custom ORM tools
    /// </summary>
    [TestFixture]
    public class DRMapperTimeTests : IDisposable
    {

        private static readonly List<TestData> _inMemoryData;

        private IDataReader _dataReader;

        static DRMapperTimeTests()
        {
            _inMemoryData = Builder<TestData>.CreateListOfSize(2000).Build().ToList();
        }

        [SetUp]
        public void SetUpDataReader()
        {
            _dataReader = ObjectReader.Create(_inMemoryData);
        }

        /// <summary>
        /// Takes 21 ms for 2000 records
        /// </summary>
        [Test]
        public void TestDataReaderWithReflection()
        {
            // Put the test data into a datareader to parse it back out into test data using reflection.
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            var results = DRMapper.ParseList<TestData>(_dataReader);
            stopwatch.Stop();
            Console.Write($"Elapsed: {stopwatch.Elapsed}");
            var nineHundredth = results[899];
            nineHundredth.Id.ShouldEqual(900);
            nineHundredth.FirstName.ShouldEqual(_inMemoryData[899].FirstName);
        }

        /// <summary>
        /// Takes 22 ms for 2000 records
        /// </summary>
        [Test]
        public void TestDataReaderWithFastMember()
        {
            // Put the test data into a datareader to parse it back out into test data using FastMember.
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            var results = ParseDataReaderWithFastMemeber<TestData>(_dataReader);
            stopwatch.Stop();
            Console.Write($"Elapsed: {stopwatch.Elapsed}");
            var nineHundredThird = results[902];
            nineHundredThird.Id.ShouldEqual(903);
            nineHundredThird.LastName.ShouldEqual(_inMemoryData[902].LastName);
        }

        [Test]
        public void TestDRMapperSingleRow()
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            var result = DRMapper.ParseRecord<TestData>(_dataReader, 44);
            stopwatch.Stop();
            Console.Write($"Elapsed: {stopwatch.Elapsed}");
            result.Id.ShouldEqual(45);
            result.FirstName.ShouldEqual("FirstName45");
            result.LastName.ShouldEqual("LastName45");
        }

        public void Dispose()
        {
            _dataReader?.Dispose();
        }

        private static List<T> ParseDataReaderWithFastMemeber<T>(IDataReader dr) where T : new()
        {
            var accessor = TypeAccessor.Create(typeof(T));
            var list = new List<T>();

            while (dr.Read())
            {
                var currentInstance = new T();
                for (var i = 0; i < dr.FieldCount; i++)
                {
                    var currentField = dr[i];
                    if (currentField != null)
                    {
                        accessor[currentInstance, dr.GetName(i)] = dr.GetValue(i);
                    }
                }
                list.Add(currentInstance);
            }

            return list;
        }
    }
}
