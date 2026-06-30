using System;
using System.Drawing;
using System.Windows.Forms;

namespace NonCombustibilityTestSimulator.Forms
{
    public partial class LoginForm : Form
    {
        private TextBox txtPassword;
        private Button btnLogin;
        private Label lblStatus;

        public LoginForm()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.Text = "建筑材料不燃性试验仿真系统 - 登录";
            this.Size = new Size(380, 220);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.BackColor = Color.White;

            var lblTitle = new Label
            {
                Text = "🏭 不燃性试验仿真系统",
                Font = new Font("微软雅黑", 16, FontStyle.Bold),
                ForeColor = Color.FromArgb(33, 150, 243),
                TextAlign = ContentAlignment.MiddleCenter,
                Location = new Point(20, 20),
                Size = new Size(320, 40)
            };

            var lblPwd = new Label
            {
                Text = "密码：",
                Font = new Font("微软雅黑", 11),
                Location = new Point(50, 80),
                Size = new Size(60, 30)
            };

            txtPassword = new TextBox
            {
                Font = new Font("微软雅黑", 11),
                Location = new Point(120, 80),
                Size = new Size(150, 30),
                PasswordChar = '*',
                UseSystemPasswordChar = true
            };
            txtPassword.KeyDown += (s, e) => { if (e.KeyCode == Keys.Enter) BtnLogin_Click(s, e); };

            btnLogin = new Button
            {
                Text = "登 录",
                Font = new Font("微软雅黑", 11, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = Color.FromArgb(33, 150, 243),
                FlatStyle = FlatStyle.Flat,
                Location = new Point(120, 125),
                Size = new Size(100, 40)
            };
            btnLogin.FlatAppearance.BorderSize = 0;
            btnLogin.Click += BtnLogin_Click;

            lblStatus = new Label
            {
                Text = "",
                Font = new Font("微软雅黑", 9),
                ForeColor = Color.Red,
                TextAlign = ContentAlignment.MiddleCenter,
                Location = new Point(20, 170),
                Size = new Size(320, 25)
            };

            this.Controls.Add(lblTitle);
            this.Controls.Add(lblPwd);
            this.Controls.Add(txtPassword);
            this.Controls.Add(btnLogin);
            this.Controls.Add(lblStatus);
        }

        private void BtnLogin_Click(object sender, EventArgs e)
        {
            if (txtPassword.Text == "123456")
            {
                var main = new MainForm(("admin", "管理员", "管理员"));
                main.Show();
                this.Hide();
            }
            else
            {
                lblStatus.Text = "❌ 密码错误，请重新输入";
                txtPassword.Clear();
                txtPassword.Focus();
            }
        }
    }
}