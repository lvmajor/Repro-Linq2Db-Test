using System;
using System.Collections.Generic;

namespace WebApplication1.Models
{
    public class Equipment
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string SerialNumber { get; set; }
        public DateTime? LastTested { get; set; }
        public string Manufacturer { get; set; }

        /// <summary>
        /// Represents all the test results sets related to this equipment
        /// </summary>
        public List<TestResults> TestResults { get; set; } = new List<TestResults>();
    }
}
