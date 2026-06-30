using System;
using System.Drawing;
using System.Windows.Forms;
using NonCombustibilityTestSimulator.Data;
using NonCombustibilityTestSimulator.Forms; // 引用以使用 TestListItem

namespace NonCombustibilityTestSimulator.Forms
{
    public partial class FrmTestRecord : Form
    {
        private ComboBox cboTestId;
        private CheckBox chkFlame;
        private NumericUpDown nudFlameTime;
        private NumericUpDown nudFlameDuration;
        private NumericUpDown nudPostWeight;
        private TextBox txtMemo;
        private Button btnSave;
        private Label lblResult;
        private DatabaseHelper _db;

        public FrmTestRecord()
        {
            InitializeComponent();
            _db = new DatabaseHelper("Data\\ISO11820.db");
            LoadTestList();
        }

        private void InitializeComponent()
        {
            this.Text = "2.8 试验现象记录";
            this.Size = new Size(540, 500);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.White;

            int y = 20;

            var lblTest = new Label
            {
                Text = "选择试验：",
                Location = new Point(20, y),
                Size = new Size(100, 30),
                Font = new Font("微软雅黑", 10)
            };
            cboTestId = new ComboBox
            {
                Location = new Point(130, y),
                Size = new Size(280, 30),
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = new Font("微软雅黑", 10)
            };
            y += 45;

            chkFlame = new CheckBox
            {
                Text = "☑ 出现持续火焰",
                Location = new Point(20, y),
                Size = new Size(180, 30),
                Font = new Font("微软雅黑", 10)
            };
            chkFlame.CheckedChanged += (s, e) =>
            {
                nudFlameTime.Enabled = chkFlame.Checked;
                nudFlameDuration.Enabled = chkFlame.Checked;
            };
            y += 40;

            var lblFlameTime = new Label
            {
                Text = "火焰发生时刻（秒）：",
                Location = new Point(20, y),
                Size = new Size(160, 30),
                Font = new Font("微软雅黑", 10)
            };
            nudFlameTime = new NumericUpDown
            {
                Location = new Point(190, y),
                Size = new Size(100, 30),
                Minimum = 0,
                Maximum = 3600,
                Enabled = false,
                Font = new Font("微软雅黑", 10)
            };
            y += 40;

            var lblFlameDuration = new Label
            {
                Text = "火焰持续时间（秒）：",
                Location = new Point(20, y),
                Size = new Size(160, 30),
                Font = new Font("微软雅黑", 10)
            };
            nudFlameDuration = new NumericUpDown
            {
                Location = new Point(190, y),
                Size = new Size(100, 30),
                Minimum = 0,
                Maximum = 3600,
                Enabled = false,
                Font = new Font("微软雅黑", 10)
            };
            y += 40;

            var lblPostWeight = new Label
            {
                Text = "试验后质量（g）：*",
                Location = new Point(20, y),
                Size = new Size(140, 30),
                Font = new Font("微软雅黑", 10),
                ForeColor = Color.Red
            };
            nudPostWeight = new NumericUpDown
            {
                Location = new Point(170, y),
                Size = new Size(120, 30),
                DecimalPlaces = 2,
                Minimum = 0,
                Maximum = 10000,
                Font = new Font("微软雅黑", 10)
            };
            y += 45;

            var lblMemo = new Label
            {
                Text = "备注：",
                Location = new Point(20, y),
                Size = new Size(80, 30),
                Font = new Font("微软雅黑", 10)
            };
            txtMemo = new TextBox
            {
                Location = new Point(110, y),
                Size = new Size(370, 60),
                Multiline = true,
                Font = new Font("微软雅黑", 10)
            };
            y += 75;

            btnSave = new Button
            {
                Text = "💾 保存记录",
                Location = new Point(160, y),
                Size = new Size(160, 45),
                BackColor = Color.FromArgb(33, 150, 243),
                ForeColor = Color.White,
                Font = new Font("微软雅黑", 11, FontStyle.Bold),
                FlatStyle = FlatStyle.Flat
            };
            btnSave.FlatAppearance.BorderSize = 0;
            btnSave.Click += BtnSave_Click;
            y += 55;

            lblResult = new Label
            {
                Text = "",
                Location = new Point(20, y),
                Size = new Size(480, 50),
                ForeColor = Color.FromArgb(33, 150, 243),
                Font = new Font("微软雅黑", 10)
            };

            this.Controls.Add(lblTest);
            this.Controls.Add(cboTestId);
            this.Controls.Add(chkFlame);
            this.Controls.Add(lblFlameTime);
            this.Controls.Add(nudFlameTime);
            this.Controls.Add(lblFlameDuration);
            this.Controls.Add(nudFlameDuration);
            this.Controls.Add(lblPostWeight);
            this.Controls.Add(nudPostWeight);
            this.Controls.Add(lblMemo);
            this.Controls.Add(txtMemo);
            this.Controls.Add(btnSave);
            this.Controls.Add(lblResult);
        }

        private void LoadTestList()
        {
            var list = _db.GetTestList();
            cboTestId.DataSource = list;
            cboTestId.DisplayMember = "DisplayName";
            cboTestId.ValueMember = "TestId";
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (cboTestId.SelectedItem == null)
            {
                lblResult.Text = "⚠️ 请先选择一条试验记录";
                return;
            }

            if (nudPostWeight.Value <= 0)
            {
                lblResult.Text = "⚠️ 试验后质量为必填项，请输入有效值";
                return;
            }

            var selected = (TestListItem)cboTestId.SelectedItem;
            bool hasFlame = chkFlame.Checked;
            int flameTime = (int)nudFlameTime.Value;
            int flameDuration = (int)nudFlameDuration.Value;
            double postWeight = (double)nudPostWeight.Value;
            string memo = txtMemo.Text.Trim();

            bool success = _db.SaveTestRecord(selected.TestId, hasFlame, flameTime, flameDuration, postWeight, memo);

            if (success)
            {
                lblResult.Text = "✅ 试验记录保存成功！";
                LoadTestList();
            }
            else
            {
                lblResult.Text = "❌ 保存失败，请检查数据";
            }
        }
    }

   
}