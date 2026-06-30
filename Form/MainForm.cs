using System;
using System.Drawing;
using System.Windows.Forms;

namespace NonCombustibilityTestSimulator.Forms
{
    public partial class MainForm : Form
    {
        private readonly (string Username, string DisplayName, string Role) _userInfo;

        public MainForm((string Username, string DisplayName, string Role) userInfo)
        {
            _userInfo = userInfo;
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.Text = $"建筑材料不燃性试验仿真系统 - 主界面（{_userInfo.DisplayName}）";
            this.Size = new Size(500, 320);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.White;

            var lblWelcome = new Label
            {
                Text = $"👋 欢迎 {_userInfo.DisplayName}（{_userInfo.Role}）",
                Font = new Font("微软雅黑", 14, FontStyle.Bold),
                ForeColor = Color.FromArgb(33, 150, 243),
                Location = new Point(30, 20),
                Size = new Size(400, 40)
            };

            var lblHint = new Label
            {
                Text = "请选择要执行的操作：",
                Font = new Font("微软雅黑", 11),
                ForeColor = Color.Gray,
                Location = new Point(30, 65),
                Size = new Size(400, 30)
            };

            var btnRecord = new Button
            {
                Text = "📝 2.8 试验现象记录",
                Location = new Point(50, 110),
                Size = new Size(380, 50),
                BackColor = Color.FromArgb(33, 150, 243),
                ForeColor = Color.White,
                Font = new Font("微软雅黑", 12, FontStyle.Bold),
                FlatStyle = FlatStyle.Flat
            };
            btnRecord.FlatAppearance.BorderSize = 0;
            btnRecord.Click += (s, e) => new FrmTestRecord().ShowDialog();

            var btnExport = new Button
            {
                Text = "📊 2.9 数据导出",
                Location = new Point(50, 180),
                Size = new Size(380, 50),
                BackColor = Color.FromArgb(76, 175, 80),
                ForeColor = Color.White,
                Font = new Font("微软雅黑", 12, FontStyle.Bold),
                FlatStyle = FlatStyle.Flat
            };
            btnExport.FlatAppearance.BorderSize = 0;
            btnExport.Click += (s, e) => new FrmExport().ShowDialog();

            this.Controls.Add(lblWelcome);
            this.Controls.Add(lblHint);
            this.Controls.Add(btnRecord);
            this.Controls.Add(btnExport);
        }
    }
}