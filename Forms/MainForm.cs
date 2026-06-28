using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using NonCombustibilityTest.Core;
using NonCombustibilityTest.Models;
using OxyPlot;
using OxyPlot.Series;
using OxyPlot.WindowsForms;
using OxyPlot.Axes;

namespace NonCombustibilityTest.Forms
{
    public partial class MainForm : Form
    {
        private readonly TestController _controller;
        private readonly PlotView _plotView;
        private readonly LineSeries _seriesTF1;
        private readonly LineSeries _seriesTF2;
        private readonly LineSeries _seriesTS;
        private readonly LineSeries _seriesTC;
        private readonly PlotModel _plotModel;

        public MainForm()
        {
            InitializeComponent();
            _controller = new TestController();
            _controller.DataBroadcast += OnDataBroadcast;
            comboBoxDuration.SelectedIndex = 0;

            _plotModel = new PlotModel { Title = "温度曲线" };
            _plotModel.Axes.Add(new DateTimeAxis { Position = AxisPosition.Bottom, StringFormat = "HH:mm:ss", Title = "时间" });
            _plotModel.Axes.Add(new LinearAxis { Position = AxisPosition.Left, Minimum = 0, Maximum = 800, Title = "温度 (°C)" });

            _seriesTF1 = new LineSeries { Title = "TF1", Color = OxyColors.Red, StrokeThickness = 2 };
            _seriesTF2 = new LineSeries { Title = "TF2", Color = OxyColors.Orange, StrokeThickness = 2 };
            _seriesTS = new LineSeries { Title = "TS", Color = OxyColors.Green, StrokeThickness = 2 };
            _seriesTC = new LineSeries { Title = "TC", Color = OxyColors.Blue, StrokeThickness = 2 };

            _plotModel.Series.Add(_seriesTF1);
            _plotModel.Series.Add(_seriesTF2);
            _plotModel.Series.Add(_seriesTS);
            _plotModel.Series.Add(_seriesTC);

            _plotView = new PlotView { Model = _plotModel, Dock = DockStyle.Fill };
            panelPlot.Controls.Add(_plotView);

            UpdateButtonStates();
        }

        private void btnNewTest_Click(object sender, EventArgs e)
        {
            var duration = comboBoxDuration.SelectedIndex == 1 ? 1800 : 3600;
            _controller.CreateNewTest(duration);
            UpdateButtonStates();
        }

        private void btnStartHeating_Click(object sender, EventArgs e)
        {
            _controller.StartHeating();
            UpdateButtonStates();
        }

        private void btnStartRecording_Click(object sender, EventArgs e)
        {
            _controller.StartRecording();
            UpdateButtonStates();
        }

        private void btnStopRecording_Click(object sender, EventArgs e)
        {
            _controller.StopRecording();
            UpdateButtonStates();
        }

        private void btnStopHeating_Click(object sender, EventArgs e)
        {
            _controller.StopHeating();
            UpdateButtonStates();
        }

        private void OnDataBroadcast(object? sender, DataBroadcastEventArgs e)
        {
            if (InvokeRequired)
            {
                Invoke(new Action<object?, DataBroadcastEventArgs>(OnDataBroadcast), sender, e);
                return;
            }

            lblCurrentState.Text = e.State.ToString();
            lblTF1.Text = $"{e.Snapshot.TF1:F1} °C";
            lblTF2.Text = $"{e.Snapshot.TF2:F1} °C";
            lblTS.Text = $"{e.Snapshot.TS:F1} °C";
            lblTC.Text = $"{e.Snapshot.TC:F1} °C";
            lblTCal.Text = $"{e.Snapshot.TCal:F1} °C";
            lblElapsedSeconds.Text = e.Snapshot.RecordedSeconds.ToString();
            lblDrift.Text = $"{e.TemperatureDrift:F2} °C/10min";

            if (e.Messages.Any())
            {
                foreach (var message in e.Messages)
                {
                    AppendLog(message);
                }
            }

            AppendChart(e.Snapshot);
            UpdateButtonStates();
        }

        private void AppendLog(MasterMessage message)
        {
            richTextBoxLog.SelectionStart = richTextBoxLog.TextLength;
            richTextBoxLog.SelectionLength = 0;
            richTextBoxLog.SelectionColor = message.Message.Contains("终止") || message.Message.Contains("停止") ? Color.Orange : Color.White;
            richTextBoxLog.AppendText($"{message.Time}  {message.Message}\n");
            richTextBoxLog.ScrollToCaret();
        }

        private void AppendChart(SensorSnapshot snapshot)
        {
            var x = DateTimeAxis.ToDouble(DateTime.Now);
            _seriesTF1.Points.Add(new DataPoint(x, snapshot.TF1));
            _seriesTF2.Points.Add(new DataPoint(x, snapshot.TF2));
            _seriesTS.Points.Add(new DataPoint(x, snapshot.TS));
            _seriesTC.Points.Add(new DataPoint(x, snapshot.TC));

            if (_seriesTF1.Points.Count > 150)
            {
                _seriesTF1.Points.RemoveAt(0);
                _seriesTF2.Points.RemoveAt(0);
                _seriesTS.Points.RemoveAt(0);
                _seriesTC.Points.RemoveAt(0);
            }

            _plotModel.InvalidatePlot(true);
        }

        private void UpdateButtonStates()
        {
            btnNewTest.Enabled = _controller.CanNewTest;
            btnStartHeating.Enabled = _controller.CanStartHeating;
            btnStartRecording.Enabled = _controller.CanStartRecording;
            btnStopRecording.Enabled = _controller.CanStopRecording;
            btnStopHeating.Enabled = !_controller.CanStartHeating && !_controller.CanStartRecording && _controller.State != TestState.Complete;
        }
    }
}
