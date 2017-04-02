#pragma warning disable S101 // Types should be named in camel cas
using System;
using System.Collections.Generic;
using System.Linq;
using Devlord.Utilities.Tests;
using FastMember;
using FizzWare.NBuilder;
using Xunit;

namespace Devlord.Utilities.Tests
{
    public class DRMapperExceptionTests
    {
        public class WrongTestDto
        {
            public int Id { get; set; }
            public string FooName { get; set; }
            public string LastName { get; set; }
            public DateTime LastBarred { get; set; }
        }
        
        [Fact]
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


        [Fact]
        public void TestDRMapperSingleRowNull()
        {
            // fake data columns map to WrongTestData class
            var fakeData = new List<TestData>();    

            // Put the test data into a datareader to parse it back out into test data using reflection.
            var dataReader = ObjectReader.Create(fakeData);
            var result = DRMapper.ParseRecord<TestData>(dataReader);
            result.ShouldBeNull();
        }

        [Fact]
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