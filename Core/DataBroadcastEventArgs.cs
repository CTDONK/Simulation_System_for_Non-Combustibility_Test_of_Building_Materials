using System;
using NonCombustibilityTest.Models;

namespace NonCombustibilityTest.Core
{
    public class DataBroadcastEventArgs : EventArgs
    {
        public SensorSnapshot Snapshot { get; set; } = new SensorSnapshot();
        public MasterMessage[] Messages { get; set; } = Array.Empty<MasterMessage>();
        public TestState State { get; set; }
        public double TemperatureDrift { get; set; }
    }
}
