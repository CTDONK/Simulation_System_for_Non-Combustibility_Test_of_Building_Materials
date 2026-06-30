using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NonCombustibilityTestSimulator.Forms;
using NonCombustibilityTestSimulator.Models;
using OfficeOpenXml;
using OfficeOpenXml.Drawing.Chart;
using OfficeOpenXml.Style;
using MigraDoc.DocumentObjectModel;
using MigraDoc.Rendering;
using PdfSharp.Pdf;

namespace NonCombustibilityTestSimulator.Data
{
    public class DatabaseHelper
    {
        private readonly string _connectionString;

        public DatabaseHelper(string dbPath)
        {
            // 移除 EPPlus 许可证设置，只保留数据库连接
            // 许可证已在 Program.cs 中全局设置
            _connectionString = $"Data Source={dbPath}";
        }

        // ===== 登录验证 =====
        public bool Login(string username, string password, out string userId, out string userType)
        {
            userId = "";
            userType = "";
            using var conn = new SqliteConnection(_connectionString);
            conn.Open();
            var cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT userid, usertype FROM operators WHERE username = $name AND pwd = $pwd";
            cmd.Parameters.AddWithValue("$name", username);
            cmd.Parameters.AddWithValue("$pwd", password);
            using var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                userId = reader.GetString(0);
                userType = reader.GetString(1);
                return true;
            }
            return false;
        }

        // ===== 获取试验列表 =====
        public List<TestListItem> GetTestList()
        {
            var result = new List<TestListItem>();
            using var conn = new SqliteConnection(_connectionString);
            conn.Open();
            var cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT testid, productid, testdate FROM testmaster ORDER BY testdate DESC";
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                result.Add(new TestListItem
                {
                    TestId = reader.GetString(0),
                    DisplayName = $"{reader.GetString(1)} - {reader.GetString(2)}"
                });
            }
            return result;
        }

        // ===== 保存试验记录 =====
        public bool SaveTestRecord(string testId, bool hasFlame, int flameTime, int flameDuration, double postWeight, string memo)
        {
            try
            {
                using var conn = new SqliteConnection(_connectionString);
                conn.Open();
                var cmd = conn.CreateCommand();
                cmd.CommandText = @"
                    UPDATE testmaster SET 
                        postweight = $post,
                        lostweight = preweight - $post,
                        lostweight_per = ((preweight - $post) / preweight) * 100,
                        flametime = $flameTime,
                        flameduration = $flameDuration,
                        memo = $memo,
                        flag = '10000000'
                    WHERE testid = $testId";
                cmd.Parameters.AddWithValue("$post", postWeight);
                cmd.Parameters.AddWithValue("$flameTime", hasFlame ? flameTime : 0);
                cmd.Parameters.AddWithValue("$flameDuration", hasFlame ? flameDuration : 0);
                cmd.Parameters.AddWithValue("$memo", memo);
                cmd.Parameters.AddWithValue("$testId", testId);
                return cmd.ExecuteNonQuery() > 0;
            }
            catch
            {
                return false;
            }
        }

        // ===== 获取单条试验记录 =====
        public TestRecordModel GetTestRecord(string testId)
        {
            using var conn = new SqliteConnection(_connectionString);
            conn.Open();
            var cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT * FROM testmaster WHERE testid = $testId";
            cmd.Parameters.AddWithValue("$testId", testId);
            using var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                return new TestRecordModel
                {
                    ProductId = reader.GetString(reader.GetOrdinal("productid")),
                    TestId = reader.GetString(reader.GetOrdinal("testid")),
                    TestDate = DateTime.Parse(reader.GetString(reader.GetOrdinal("testdate"))),
                    Operator = reader.GetString(reader.GetOrdinal("operator")),
                    PreWeight = reader.GetDouble(reader.GetOrdinal("preweight")),
                    PostWeight = reader.IsDBNull(reader.GetOrdinal("postweight")) ? 0 : reader.GetDouble(reader.GetOrdinal("postweight")),
                    TotalTestTime = reader.GetInt32(reader.GetOrdinal("totaltesttime")),
                    DeltaTf = reader.GetDouble(reader.GetOrdinal("deltatf")),
                    LostWeightPercent = reader.IsDBNull(reader.GetOrdinal("lostweight_per")) ? 0 : reader.GetDouble(reader.GetOrdinal("lostweight_per")),
                    Flag = reader.GetString(reader.GetOrdinal("flag")),
                    Memo = reader.IsDBNull(reader.GetOrdinal("memo")) ? "" : reader.GetString(reader.GetOrdinal("memo")),
                    FlameTime = reader.GetInt32(reader.GetOrdinal("flametime")),
                    FlameDuration = reader.GetInt32(reader.GetOrdinal("flameduration"))
                };
            }
            return new TestRecordModel();
        }

        // ===== 获取温度数据（模拟） =====
        private List<SensorDataModel> GetTestData(string testId)
        {
            var list = new List<SensorDataModel>();
            Random rand = new Random(testId.GetHashCode());
            
            for (int i = 0; i < 60; i++)
            {
                double progress = i / 59.0;
                double temp1 = 25 + progress * 725 + rand.NextDouble() * 2 - 1;
                double temp2 = 25 + progress * 723.5 + rand.NextDouble() * 2 - 1;
                double tempSurface = 25 + progress * 595 + rand.NextDouble() * 1.5 - 0.75;
                double tempCenter = 25 + progress * 455 + rand.NextDouble() * 1.5 - 0.75;
                double tempCal = temp1 + rand.NextDouble() * 2 - 1;

                list.Add(new SensorDataModel
                {
                    Time = i,
                    Temp1 = Math.Round(temp1, 2),
                    Temp2 = Math.Round(temp2, 2),
                    TempSurface = Math.Round(tempSurface, 2),
                    TempCenter = Math.Round(tempCenter, 2),
                    TempCalibration = Math.Round(tempCal, 2)
                });
            }
            return list;
        }

        // ============================================================
        // 2.9 导出功能
        // ============================================================

        // -------- CSV 导出 --------
        public void ExportToCsv(string testId, string filePath)
        {
            var record = GetTestRecord(testId);
            var data = GetTestData(testId);

            using var writer = new StreamWriter(filePath, false, System.Text.Encoding.UTF8);
            
            writer.WriteLine($"# 试验ID: {record.TestId}");
            writer.WriteLine($"# 样品编号: {record.ProductId}");
            writer.WriteLine($"# 操作员: {record.Operator}");
            writer.WriteLine($"# 试验日期: {record.TestDate:yyyy-MM-dd HH:mm:ss}");
            writer.WriteLine($"# 试验前质量: {record.PreWeight:F2} g");
            writer.WriteLine($"# 试验后质量: {record.PostWeight:F2} g");
            writer.WriteLine($"# 失重率: {record.LostWeightPercent:F2} %");
            writer.WriteLine($"# 总时长: {record.TotalTestTime} 秒");
            writer.WriteLine($"# 温升: {record.DeltaTf:F2} °C");
            writer.WriteLine($"# 火焰时长: {record.FlameDuration} 秒");
            writer.WriteLine("#");
            
            writer.WriteLine("Time(秒),Temp1(°C),Temp2(°C),TempSurface(°C),TempCenter(°C),TempCalibration(°C)");
            
            foreach (var row in data)
            {
                writer.WriteLine($"{row.Time},{row.Temp1:F2},{row.Temp2:F2},{row.TempSurface:F2},{row.TempCenter:F2},{row.TempCalibration:F2}");
            }
        }

        // -------- Excel 导出 --------
        public void ExportToExcel(string testId, string filePath)
        {
            var record = GetTestRecord(testId);
            var data = GetTestData(testId);

            using var package = new ExcelPackage(new FileInfo(filePath));
            
            // Sheet1: 试验信息
            var sheet1 = package.Workbook.Worksheets.Add("试验信息");
            sheet1.Cells["A1"].Value = "建筑材料不燃性试验报告";
            sheet1.Cells["A1"].Style.Font.Size = 18;
            sheet1.Cells["A1"].Style.Font.Bold = true;
            
            int row = 3;
            var info = new[,] {
                { "试验ID", record.TestId },
                { "样品编号", record.ProductId },
                { "操作员", record.Operator },
                { "试验日期", record.TestDate.ToString("yyyy-MM-dd HH:mm:ss") },
                { "试验前质量 (g)", record.PreWeight.ToString("F2") },
                { "试验后质量 (g)", record.PostWeight.ToString("F2") },
                { "失重率 (%)", record.LostWeightPercent.ToString("F2") },
                { "总时长 (秒)", record.TotalTestTime.ToString() },
                { "温升 (°C)", record.DeltaTf.ToString("F2") },
                { "火焰持续时间 (秒)", record.FlameDuration.ToString() },
                { "备注", record.Memo ?? "" }
            };
            
            for (int i = 0; i < info.GetLength(0); i++)
            {
                sheet1.Cells[$"A{row + i}"].Value = info[i, 0];
                sheet1.Cells[$"A{row + i}"].Style.Font.Bold = true;
                sheet1.Cells[$"B{row + i}"].Value = info[i, 1];
            }
            sheet1.Column(1).Width = 20;
            sheet1.Column(2).Width = 30;

            // Sheet2: 温度数据
            var sheet2 = package.Workbook.Worksheets.Add("温度数据");
            
            sheet2.Cells["A1"].Value = "Time(s)";
            sheet2.Cells["B1"].Value = "Temp1(°C)";
            sheet2.Cells["C1"].Value = "Temp2(°C)";
            sheet2.Cells["D1"].Value = "TempSurface(°C)";
            sheet2.Cells["E1"].Value = "TempCenter(°C)";
            sheet2.Cells["F1"].Value = "TempCal(°C)";
            using (var range = sheet2.Cells["A1:F1"])
            {
                range.Style.Font.Bold = true;
                range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                range.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);
            }

            for (int i = 0; i < data.Count; i++)
            {
                sheet2.Cells[i + 2, 1].Value = data[i].Time;
                sheet2.Cells[i + 2, 2].Value = data[i].Temp1;
                sheet2.Cells[i + 2, 3].Value = data[i].Temp2;
                sheet2.Cells[i + 2, 4].Value = data[i].TempSurface;
                sheet2.Cells[i + 2, 5].Value = data[i].TempCenter;
                sheet2.Cells[i + 2, 6].Value = data[i].TempCalibration;
            }
            sheet2.Cells.AutoFitColumns();

            // Sheet3: 温度曲线图
            var sheet3 = package.Workbook.Worksheets.Add("温度曲线");
            
            sheet3.Cells["A1:F1"].Value = sheet2.Cells["A1:F1"].Value;
            for (int i = 0; i < data.Count; i++)
            {
                sheet3.Cells[i + 2, 1].Value = data[i].Time;
                sheet3.Cells[i + 2, 2].Value = data[i].Temp1;
                sheet3.Cells[i + 2, 3].Value = data[i].Temp2;
                sheet3.Cells[i + 2, 4].Value = data[i].TempSurface;
                sheet3.Cells[i + 2, 5].Value = data[i].TempCenter;
                sheet3.Cells[i + 2, 6].Value = data[i].TempCalibration;
            }

            var chart = sheet3.Drawings.AddChart("温度曲线", eChartType.Line);
            chart.SetPosition(2, 0, 1, 0);
            chart.SetSize(700, 400);
            chart.Title.Text = "温度变化曲线";
            chart.Title.Font.Size = 14;
            chart.Title.Font.Bold = true;
            chart.XAxis.Title.Text = "时间 (秒)";
            chart.YAxis.Title.Text = "温度 (°C)";

            var names = new[] { "炉温1", "炉温2", "表面温", "中心温", "校准温" };
            for (int col = 2; col <= 6; col++)
            {
                var series = chart.Series.Add(
                    sheet3.Cells[2, col, data.Count + 1, col],
                    sheet3.Cells[2, 1, data.Count + 1, 1]
                );
                series.Header = names[col - 2];
            }

            package.Save();
        }

        // -------- PDF 导出 --------
        public void ExportToPdf(string testId, string filePath)
        {
            var record = GetTestRecord(testId);
            var data = GetTestData(testId);

            var document = new Document();
            document.Info.Title = $"试验报告_{record.TestId}";

            var style = document.Styles["Normal"];
            style.Font.Name = "宋体";
            style.Font.Size = 10;

            var headingStyle = document.Styles.AddStyle("Heading1", "Normal");
            headingStyle.Font.Size = 16;
            headingStyle.Font.Bold = true;
            headingStyle.Font.Color = MigraDoc.DocumentObjectModel.Colors.Blue;

            var headingStyle2 = document.Styles.AddStyle("Heading2", "Normal");
            headingStyle2.Font.Size = 12;
            headingStyle2.Font.Bold = true;

            var section = document.AddSection();
            var para = section.AddParagraph("建筑材料不燃性试验报告");
            para.Style = "Heading1";
            para.Format.Alignment = ParagraphAlignment.Center;
            para.AddLineBreak();

            para = section.AddParagraph("一、试验信息");
            para.Style = "Heading2";

            var table = section.AddTable();
            table.AddColumn(Unit.FromCentimeter(4));
            table.AddColumn(Unit.FromCentimeter(8));

            var info = new[,] {
                { "试验ID", record.TestId },
                { "样品编号", record.ProductId },
                { "操作员", record.Operator },
                { "试验日期", record.TestDate.ToString("yyyy-MM-dd HH:mm:ss") },
                { "试验前质量", $"{record.PreWeight:F2} g" },
                { "试验后质量", $"{record.PostWeight:F2} g" },
                { "失重率", $"{record.LostWeightPercent:F2} %" },
                { "总时长", $"{record.TotalTestTime} 秒" },
                { "温升", $"{record.DeltaTf:F2} °C" },
                { "火焰持续时间", $"{record.FlameDuration} 秒" },
                { "备注", record.Memo ?? "" }
            };

            for (int i = 0; i < info.GetLength(0); i++)
            {
                var row = table.AddRow();
                row.Cells[0].AddParagraph(info[i, 0]);
                row.Cells[0].Format.Font.Bold = true;
                row.Cells[1].AddParagraph(info[i, 1]);
            }

            para = section.AddParagraph();
            para.AddLineBreak();

            para = section.AddParagraph("二、温度数据摘要");
            para.Style = "Heading2";

            if (data.Count > 0)
            {
                var maxTemp1 = data.Max(d => d.Temp1);
                var maxTemp2 = data.Max(d => d.Temp2);
                var maxSurface = data.Max(d => d.TempSurface);
                var maxCenter = data.Max(d => d.TempCenter);

                table = section.AddTable();
                table.AddColumn(Unit.FromCentimeter(4));
                table.AddColumn(Unit.FromCentimeter(4));
                table.AddColumn(Unit.FromCentimeter(4));

                var header = table.AddRow();
                header.Cells[0].AddParagraph("通道");
                header.Cells[1].AddParagraph("最大值 (°C)");
                header.Cells[2].AddParagraph("最终值 (°C)");
                header.Cells[0].Format.Font.Bold = true;
                header.Cells[1].Format.Font.Bold = true;
                header.Cells[2].Format.Font.Bold = true;
                header.Cells[0].Format.Shading.Color = MigraDoc.DocumentObjectModel.Colors.LightGray;
                header.Cells[1].Format.Shading.Color = MigraDoc.DocumentObjectModel.Colors.LightGray;
                header.Cells[2].Format.Shading.Color = MigraDoc.DocumentObjectModel.Colors.LightGray;

                var last = data.Last();
                var summary = new[,] {
                    { "炉温1", maxTemp1.ToString("F2"), last.Temp1.ToString("F2") },
                    { "炉温2", maxTemp2.ToString("F2"), last.Temp2.ToString("F2") },
                    { "表面温", maxSurface.ToString("F2"), last.TempSurface.ToString("F2") },
                    { "中心温", maxCenter.ToString("F2"), last.TempCenter.ToString("F2") }
                };
                for (int i = 0; i < summary.GetLength(0); i++)
                {
                    var row = table.AddRow();
                    row.Cells[0].AddParagraph(summary[i, 0]);
                    row.Cells[1].AddParagraph(summary[i, 1]);
                    row.Cells[2].AddParagraph(summary[i, 2]);
                }
            }

            var renderer = new PdfDocumentRenderer();
            renderer.Document = document;
            renderer.RenderDocument();
            renderer.PdfDocument.Save(filePath);
        }
    }

    // ===== SensorDataModel =====
    public class SensorDataModel
    {
        public int Time { get; set; }
        public double Temp1 { get; set; }
        public double Temp2 { get; set; }
        public double TempSurface { get; set; }
        public double TempCenter { get; set; }
        public double TempCalibration { get; set; }
    }
}