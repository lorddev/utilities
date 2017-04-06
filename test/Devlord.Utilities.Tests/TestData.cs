using System;
// ReSharper disable InconsistentNaming

namespace Devlord.Utilities.Tests
{
    public class TestData
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime LastSeen { get; set; }
    }

    public class TestDataUpperCase
    {
        public int ID { get; set; }
        public string FIRSTNAME { get; set; }
        public string LASTNAME { get; set; }
        public DateTime LASTSEEN { get; set; }
    }
}
