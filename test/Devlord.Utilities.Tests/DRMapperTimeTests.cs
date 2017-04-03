#pragma warning disable S101 // Types should be named in camel case

using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using FastMember;
using FizzWare.NBuilder;
using Xunit;
using Xunit.Abstractions;

namespace Devlord.Utilities.Tests
{
    /// <summary>
    /// Provides benchmarking for custom ORM tools
    /// </summary>
    public class DRMapperTimeTests
    {
        private readonly ITestOutputHelper _output;
        
        public DRMapperTimeTests(ITestOutputHelper output)
        {
            _output = output;
        }

        /// <summary>
        /// Takes 21 ms for 2000 records
        /// </summary>
        [Fact]
        public void TestDataReaderWithReflection()
        {
            // Put the test data into a datareader to parse it back out into test data using reflection.
            Stopwatch stopwatch;
            List<TestData> results;
            var inMemoryData = Builder<TestData>.CreateListOfSize(1000).Build().ToList();
            inMemoryData.First().Id.ShouldEqual(1);
            inMemoryData[899].Id.ShouldEqual(900);
            using (var dataReader = ObjectReader.Create(inMemoryData))
            {
                stopwatch = new Stopwatch();
                stopwatch.Start();

                results = DRMapper.ParseList<TestData>(dataReader);
            }
            stopwatch.Stop();
            _output.WriteLine($"Elapsed: {stopwatch.Elapsed}");
            var nineHundredth = results[899];
            nineHundredth.Id.ShouldEqual(900);
            nineHundredth.FirstName.ShouldEqual(inMemoryData[899].FirstName);
        }

        /// <summary>
        /// Takes 22 ms for 2000 records
        /// </summary>
        [Fact]
        public void TestDataReaderWithFastMember()
        {
            // Put the test data into a datareader to parse it back out into test data using FastMember.
            Stopwatch stopwatch;
            List<TestData> results;
            var inMemoryData = Builder<TestData>.CreateListOfSize(1000).Build().OrderBy(c => c.Id).ToList();
            inMemoryData.First().Id.ShouldEqual(1);
            using (var dataReader = ObjectReader.Create(inMemoryData))
            {
                stopwatch = new Stopwatch();
                stopwatch.Start();
                results = ParseDataReaderWithFastMember<TestData>(dataReader);
            }
            stopwatch.Stop();
            _output.WriteLine($"Elapsed: {stopwatch.Elapsed}");
            var nineHundredThird = results[902];
            nineHundredThird.Id.ShouldEqual(903);
            nineHundredThird.LastName.ShouldEqual(inMemoryData[902].LastName);
        }

        [Fact]
        public void TestDRMapperSingleRow()
        {
            Stopwatch stopwatch;
            TestData result;
            var inMemoryData = Builder<TestData>.CreateListOfSize(100).Build().OrderBy(c => c.Id);
            inMemoryData.First().Id.ShouldEqual(1);
            var filter = inMemoryData.Where(x => x.Id == 45);
            using (var dataReader = ObjectReader.Create(filter))
            {
                stopwatch = new Stopwatch();
                stopwatch.Start();
                result = DRMapper.ParseRecord<TestData>(dataReader);
            }

            stopwatch.Stop();
            _output.WriteLine($"Elapsed: {stopwatch.Elapsed}");
            result.Id.ShouldEqual(45);
            result.FirstName.ShouldEqual("FirstName45");
            result.LastName.ShouldEqual("LastName45");
        }
        

        private static List<T> ParseDataReaderWithFastMember<T>(IDataReader dr) where T : new()
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
