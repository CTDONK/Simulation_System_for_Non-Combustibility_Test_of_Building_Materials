using System;

namespace NonCombustibilityTestSimulator.Models
{
    public class TestRecordModel
    {
        public string ProductId { get; set; } = "";
        public string TestId { get; set; } = "";
        public DateTime TestDate { get; set; }
        public string Operator { get; set; } = "";
        public double PreWeight { get; set; }
        public double PostWeight { get; set; }
        public double LostWeight => PreWeight - PostWeight;
        public double LostWeightPercent { get; set; }
        public int TotalTestTime { get; set; }
        public double DeltaTf { get; set; }
        public string Flag { get; set; } = "";
        public string? Memo { get; set; }
        public int FlameTime { get; set; }
        public int FlameDuration { get; set; }
    }
}