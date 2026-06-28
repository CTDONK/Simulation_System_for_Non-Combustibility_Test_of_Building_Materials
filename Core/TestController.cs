using System;
using System.Collections.Generic;
using System.Linq;
using System.Timers;
using Timer = System.Timers.Timer;
using NonCombustibilityTest.Models;

namespace NonCombustibilityTest.Core
{
    public class TestController
    {
        private const double TargetTemperature = 750.0;
        private const double StableLower = 745.0;
        private const double StableUpper = 755.0;
        private const double StableThreshold = 3.0;
        private const double HeatingRatePerSecond = 40.0;
        private const double TempFluctuation = 0.5;
        private const double CoolingRate = 0.5;
        private const int SimulationIntervalMs = 800;

        private readonly Timer _timer;
        private readonly Random _random = new();
        private readonly List<(double time, double value)> _temperatureHistory = new();
        private readonly List<SensorSnapshot> _recordedRows = new();
        private double _tf1;
        private double _tf2;
        private double _ts;
        private double _tc;
        private double _tcal;
        private int _stableTicks;
        private int _recordedSeconds;
        private int _recordDurationSeconds;
        private bool _isCooling;

        public event EventHandler<DataBroadcastEventArgs>? DataBroadcast;

        public TestState State { get; private set; } = TestState.Idle;

        public int RecordedSeconds => _recordedSeconds;
        public double TemperatureDrift { get; private set; }

        public bool CanStartHeating => State == TestState.Idle;
        public bool CanStartRecording => State == TestState.Ready;
        public bool CanStopRecording => State == TestState.Recording;
        public bool CanNewTest => State == TestState.Idle || State == TestState.Complete;

        public TestController()
        {
            ResetTemperatures();
            _timer = new Timer(SimulationIntervalMs);
            _timer.Elapsed += OnTick;
            _timer.AutoReset = true;
            _timer.Start();
            BroadcastState("系统初始化，试验准备就绪");
        }

        public void CreateNewTest(int durationSeconds = 3600)
        {
            _recordDurationSeconds = durationSeconds;
            _recordedSeconds = 0;
            _recordedRows.Clear();
            ResetTemperatures();
            State = TestState.Idle;
            _stableTicks = 0;
            _isCooling = false;
            BroadcastState("新建试验完成，等待开始升温");
        }

        public void StartHeating()
        {
            if (State != TestState.Idle)
            {
                return;
            }

            State = TestState.Preparing;
            _stableTicks = 0;
            _isCooling = false;
            BroadcastState("开始升温，系统升温中");
        }

        public void StartRecording()
        {
            if (State != TestState.Ready)
            {
                return;
            }

            State = TestState.Recording;
            _recordedSeconds = 0;
            _recordedRows.Clear();
            BroadcastState("开始记录，计时开始");
        }

        public void StopRecording()
        {
            if (State != TestState.Recording)
            {
                return;
            }

            State = TestState.Complete;
            BroadcastState("用户手动停止记录");
        }

        public void StopHeating()
        {
            if (State != TestState.Preparing && State != TestState.Ready)
            {
                return;
            }

            State = TestState.Idle;
            _isCooling = true;
            BroadcastState("停止升温，开始降温");
        }

        private void OnTick(object? sender, ElapsedEventArgs e)
        {
            lock (this)
            {
                UpdateSimulation();
                BroadcastState();
            }
        }

        private void ResetTemperatures()
        {
            _tf1 = 25.0;
            _tf2 = 24.0;
            _ts = _tf1 * 0.3;
            _tc = _tf1 * 0.25;
            _tcal = _tf1;
            _temperatureHistory.Clear();
            TemperatureDrift = 0.0;
        }

        private void UpdateSimulation()
        {
            switch (State)
            {
                case TestState.Preparing:
                    SimulatePreparing();
                    break;
                case TestState.Ready:
                    SimulateReady();
                    break;
                case TestState.Recording:
                    SimulateRecording();
                    break;
                case TestState.Idle when _isCooling:
                    SimulateCooling();
                    break;
                default:
                    SimulateIdle();
                    break;
            }

            RecordTemperatureHistory();
            CalculateTemperatureDrift();
        }

        private void SimulatePreparing()
        {
            var delta = HeatingRatePerSecond * (SimulationIntervalMs / 1000.0) + RandomNoise();
            _tf1 += delta;
            _tf2 += HeatingRatePerSecond * (SimulationIntervalMs / 1000.0) + RandomNoise();
            _ts = _tf1 * 0.3 + RandomNoise();
            _tc = _tf1 * 0.25 + RandomNoise();
            _tcal = _tf1 + RandomNoise() * 2.0;

            if (_tf1 >= StableLower)
            {
                _stableTicks++;
            }
            else
            {
                _stableTicks = 0;
            }

            if (_stableTicks > StableThreshold && _tf1 <= StableUpper)
            {
                State = TestState.Ready;
                BroadcastState("温度已稳定，可以开始记录");
            }
        }

        private void SimulateReady()
        {
            _tf1 = TargetTemperature + RandomNoise();
            _tf2 = TargetTemperature + RandomNoise();
            _ts = _tf1 * 0.95 + RandomNoise();
            _tc = _tf1 * 0.85 + RandomNoise();
            _tcal = _tf1 + RandomNoise() * 2.0;

            if (_tf1 < StableLower || _tf1 > StableUpper)
            {
                State = TestState.Preparing;
                BroadcastState("温度跌出稳定区，回到升温等待");
            }
        }

        private void SimulateRecording()
        {
            _recordedSeconds = Math.Min(_recordDurationSeconds, _recordedSeconds + 1);

            var targetSurface = Math.Min(_tf1 * 0.95, 800.0);
            _ts += (targetSurface - _ts) * 0.02 + RandomNoise();
            var targetCenter = Math.Min(_tf1 * 0.85, 750.0);
            _tc += (targetCenter - _tc) * 0.01 + RandomNoise();
            _tf1 = TargetTemperature + RandomNoise();
            _tf2 = TargetTemperature + RandomNoise();
            _tcal = _tf1 + RandomNoise() * 2.0;

            if (_recordedSeconds >= _recordDurationSeconds)
            {
                State = TestState.Complete;
                BroadcastState("记录时间到达，试验自动结束");
            }
        }

        private void SimulateCooling()
        {
            _tf1 = Math.Max(25.0, _tf1 - CoolingRate + RandomNoise() * 0.1);
            _tf2 = Math.Max(24.0, _tf2 - CoolingRate + RandomNoise() * 0.1);
            _ts = Math.Max(25.0 * 0.3, _ts - CoolingRate * 0.8 + RandomNoise());
            _tc = Math.Max(25.0 * 0.25, _tc - CoolingRate * 0.7 + RandomNoise());
            _tcal = _tf1 + RandomNoise() * 2.0;

            if (_tf1 <= 40.0)
            {
                _isCooling = false;
                State = TestState.Idle;
                BroadcastState("炉温已降低到安全区，系统回到空闲");
            }
        }

        private void SimulateIdle()
        {
            _tf1 = Math.Max(25.0, _tf1 + RandomNoise() * 0.1);
            _tf2 = Math.Max(24.0, _tf2 + RandomNoise() * 0.1);
            _ts = Math.Max(25.0 * 0.3, _ts + RandomNoise() * 0.1);
            _tc = Math.Max(25.0 * 0.25, _tc + RandomNoise() * 0.1);
            _tcal = _tf1 + RandomNoise() * 2.0;
        }

        private double RandomNoise()
        {
            return (_random.NextDouble() * 2.0 - 1.0) * TempFluctuation;
        }

        private void RecordTemperatureHistory()
        {
            var timeSeconds = _temperatureHistory.Count * SimulationIntervalMs / 1000.0;
            _temperatureHistory.Add((timeSeconds, _tf1));
            if (_temperatureHistory.Count > 750)
            {
                _temperatureHistory.RemoveAt(0);
            }
        }

        private void CalculateTemperatureDrift()
        {
            if (_temperatureHistory.Count < 2)
            {
                TemperatureDrift = 0.0;
                return;
            }

            var n = _temperatureHistory.Count;
            var sumX = 0.0;
            var sumY = 0.0;
            var sumX2 = 0.0;
            var sumXY = 0.0;

            for (var index = 0; index < n; index++)
            {
                var x = _temperatureHistory[index].time;
                var y = _temperatureHistory[index].value;
                sumX += x;
                sumY += y;
                sumX2 += x * x;
                sumXY += x * y;
            }

            var denominator = n * sumX2 - sumX * sumX;
            if (Math.Abs(denominator) < 1e-6)
            {
                TemperatureDrift = 0.0;
                return;
            }

            var slope = (n * sumXY - sumX * sumY) / denominator;
            TemperatureDrift = slope * 600.0;
        }

        private void BroadcastState(string? message = null)
        {
            var snapshot = new SensorSnapshot
            {
                TF1 = _tf1,
                TF2 = _tf2,
                TS = _ts,
                TC = _tc,
                TCal = _tcal,
                RecordedSeconds = _recordedSeconds
            };

            var messages = string.IsNullOrWhiteSpace(message)
                ? Array.Empty<MasterMessage>()
                : new[] { new MasterMessage { Time = DateTime.Now.ToString("HH:mm:ss"), Message = message } };

            DataBroadcast?.Invoke(this, new DataBroadcastEventArgs
            {
                Snapshot = snapshot,
                Messages = messages,
                State = State,
                TemperatureDrift = TemperatureDrift
            });
        }
    }
}
