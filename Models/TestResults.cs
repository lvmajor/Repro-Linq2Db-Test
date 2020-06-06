using Innofactor.EfCoreJsonValueConverter;
using System;

namespace WebApplication1.Models
{
    public class TestResults
    {
        public int Id { get; set; }
        public int EquipmentId { get; set; }
        public Equipment Equipment { get; set; }

        [JsonField]
        public SingleTestResultsTemplate Test1Results { get; set; }
        [JsonField]
        public SingleTestResultsTemplate Test2Results { get; set; }
        [JsonField]
        public SingleTestResultsTemplate Test3Results { get; set; }

        public DateTime? TestDate { get; set; }

        /// <summary>
        /// Even though different tests are performed on the equipment, all of them share some properties together, which are defined by the SingleTestResultsTemplate class
        /// </summary>
        public class SingleTestResultsTemplate
        {
            public double? Result { get; set; }
            public double? RateOfChange { get; set; }
            public double? Increment { get; set; }
            /// <summary>
            /// This represents the last set of results used to compute the rates of change and increments/decrements for this specific result
            /// </summary>
            public int? LastComparedWith { get; set; }
        }
    }
}
