using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using NonCombustibilityTestSimulator.Data;

namespace NonCombustibilityTestSimulator.Forms
{
    public partial class FrmExport : Form
    {
        private ComboBox cboTestId;
        private Button btnExportCsv;
        private Button btnExportExcel;
        private Button btnExportPdf;
        private Label lblResult;
        private DatabaseHelper _db;

        public FrmExport()
        {
            InitializeComponent();
            _db = new DatabaseHelper("Data\\ISO11820.db");
            LoadTestList();
        }

        private void InitializeComponent()
        {
            this.Text = "2.9 数据导出";
            this.Size = new Size(540, 380);
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
            y += 55;

            btnExportCsv = new Button
            {
                Text = "📄 导出 CSV",
                Location = new Point(40, y),
                Size = new Size(130, 50),
                BackColor = Color.FromArgb(96, 125, 139),
                ForeColor = Color.White,
                Font = new Font("微软雅黑", 11, FontStyle.Bold),
                FlatStyle = FlatStyle.Flat
            };
            btnExportCsv.FlatAppearance.BorderSize = 0;
            btnExportCsv.Click += (s, e) => ExportFile("csv");

            btnExportExcel = new Button
            {
                Text = "📊 导出 Excel",
                Location = new Point(190, y),
                Size = new Size(130, 50),
                BackColor = Color.FromArgb(76, 175, 80),
                ForeColor = Color.White,
                Font = new Font("微软雅黑", 11, FontStyle.Bold),
                FlatStyle = FlatStyle.Flat
            };
            btnExportExcel.FlatAppearance.BorderSize = 0;
            btnExportExcel.Click += (s, e) => ExportFile("excel");

            btnExportPdf = new Button
            {
                Text = "📕 导出 PDF",
                Location = new Point(340, y),
                Size = new Size(130, 50),
                BackColor = Color.FromArgb(233, 30, 99),
                ForeColor = Color.White,
                Font = new Font("微软雅黑", 11, FontStyle.Bold),
                FlatStyle = FlatStyle.Flat
            };
            btnExportPdf.FlatAppearance.BorderSize = 0;
            btnExportPdf.Click += (s, e) => ExportFile("pdf");
            y += 70;

            lblResult = new Label
            {
                Text = "",
                Location = new Point(20, y),
                Size = new Size(480, 80),
                ForeColor = Color.FromArgb(33, 150, 243),
                Font = new Font("微软雅黑", 10)
            };

            this.Controls.Add(lblTest);
            this.Controls.Add(cboTestId);
            this.Controls.Add(btnExportCsv);
            this.Controls.Add(btnExportExcel);
            this.Controls.Add(btnExportPdf);
            this.Controls.Add(lblResult);
        }

        private void LoadTestList()
        {
            var list = _db.GetTestList();
            cboTestId.DataSource = list;
            cboTestId.DisplayMember = "DisplayName";
            cboTestId.ValueMember = "TestId";
        }

        private void ExportFile(string format)
        {
            if (cboTestId.SelectedItem == null)
            {
                lblResult.Text = "⚠️ 请先选择一条试验记录";
                return;
            }

            var selected = (TestListItem)cboTestId.SelectedItem;
            string testId = selected.TestId;

            string exportDir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "Exports");
            Directory.CreateDirectory(exportDir);

            string fileName = $"Test_{testId}_{DateTime.Now:yyyyMMddHHmmss}";
            string fullPath = "";

            try
            {
                switch (format.ToLower())
                {
                    case "csv":
                        fullPath = Path.Combine(exportDir, $"{fileName}.csv");
                        _db.ExportToCsv(testId, fullPath);
                        break;
                    case "excel":
                        fullPath = Path.Combine(exportDir, $"{fileName}.xlsx");
                        _db.ExportToExcel(testId, fullPath);
                        break;
                    case "pdf":
                        fullPath = Path.Combine(exportDir, $"{fileName}.pdf");
                        _db.ExportToPdf(testId, fullPath);
                        break;
                }
                lblResult.Text = $"✅ 文件已导出到：\n{fullPath}";
            }
            catch (Exception ex)
            {
                lblResult.Text = $"❌ 导出失败：{ex.Message}";
            }
        }
    }

    public class TestListItem
    {
        public string TestId { get; set; } = "";
        public string DisplayName { get; set; } = "";
    }
}