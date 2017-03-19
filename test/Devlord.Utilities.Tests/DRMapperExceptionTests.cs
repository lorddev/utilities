#pragma warning disable S101 // Types should be named in camel case

using System;
using System.Collections.Generic;
using System.Linq;
using FastMember;
using FizzWare.NBuilder;
using NUnit.Framework;
using Should;

namespace Devlord.Utilities.Tests
{
    [TestFixture]
    public class DRMapperExceptionTests
    {
        public class WrongTestDto
        {
            public int Id { get; set; }
            public string FooName { get; set; }
            public string LastName { get; set; }
            public DateTime LastBarred { get; set; }
        }
        
        [Test]
        public void TestDataReaderWithBadFieldNames()
        {
            // fake data columns map to WrongTestData class
            var fakeData = Builder<TestData>.CreateListOfSize(100).Build().ToList();

            // Put the test data into a datareader to parse it back out into test data using reflection.
            var dataReader = ObjectReader.Create(fakeData);

            // What do we expect to do if the DR doesn't have a record for this field?
            Assert.Throws<Exception>(() =>
            {
                // Attempt to map data to "TestData" class.
                var results = DRMapper.ParseList<WrongTestDto>(dataReader);
            });
        }


        [Test]
        public void TestDRMapperSingleRowNull()
        {
            // fake data columns map to WrongTestData class
            var fakeData = new List<TestData>();    

            // Put the test data into a datareader to parse it back out into test data using reflection.
            var dataReader = ObjectReader.Create(fakeData);
            var result = DRMapper.ParseRecord<TestData>(dataReader);
            result.ShouldBeNull();
        }

        [Test]
        public void TestDRMapperSingleRowOutOfRange()
        {
            // fake data columns map to WrongTestData class
            var fakeData = Builder<WrongTestDto>.CreateListOfSize(5).Build().ToList();

            // Put the test data into a datareader to parse it back out into test data using reflection.
            var dataReader = ObjectReader.Create(fakeData);

            Assert.Throws<IndexOutOfRangeException>(() =>
            {
                var result = DRMapper.ParseRecord<TestData>(dataReader, 7);
            });
        }
    }
}