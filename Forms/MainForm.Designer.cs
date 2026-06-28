namespace NonCombustibilityTest.Forms
{
    partial class MainForm
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Button btnNewTest;
        private System.Windows.Forms.Button btnStartHeating;
        private System.Windows.Forms.Button btnStartRecording;
        private System.Windows.Forms.Button btnStopRecording;
        private System.Windows.Forms.Button btnStopHeating;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label lblTF1;
        private System.Windows.Forms.Label lblTF2;
        private System.Windows.Forms.Label lblTS;
        private System.Windows.Forms.Label lblTC;
        private System.Windows.Forms.Label lblTCal;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label lblCurrentState;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label lblElapsedSeconds;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label lblDrift;
        private System.Windows.Forms.RichTextBox richTextBoxLog;
        private System.Windows.Forms.Panel panelPlot;
        private System.Windows.Forms.ComboBox comboBoxDuration;
        private System.Windows.Forms.Label label6;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.btnNewTest = new System.Windows.Forms.Button();
            this.btnStartHeating = new System.Windows.Forms.Button();
            this.btnStartRecording = new System.Windows.Forms.Button();
            this.btnStopRecording = new System.Windows.Forms.Button();
            this.btnStopHeating = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.lblTF1 = new System.Windows.Forms.Label();
            this.lblTF2 = new System.Windows.Forms.Label();
            this.lblTS = new System.Windows.Forms.Label();
            this.lblTC = new System.Windows.Forms.Label();
            this.lblTCal = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.lblCurrentState = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.lblElapsedSeconds = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.lblDrift = new System.Windows.Forms.Label();
            this.richTextBoxLog = new System.Windows.Forms.RichTextBox();
            this.panelPlot = new System.Windows.Forms.Panel();
            this.comboBoxDuration = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnNewTest
            // 
            this.btnNewTest.Location = new System.Drawing.Point(20, 20);
            this.btnNewTest.Name = "btnNewTest";
            this.btnNewTest.Size = new System.Drawing.Size(180, 36);
            this.btnNewTest.TabIndex = 0;
            this.btnNewTest.Text = "新建试验";
            this.btnNewTest.UseVisualStyleBackColor = true;
            this.btnNewTest.Click += new System.EventHandler(this.btnNewTest_Click);
            // 
            // btnStartHeating
            // 
            this.btnStartHeating.Location = new System.Drawing.Point(20, 68);
            this.btnStartHeating.Name = "btnStartHeating";
            this.btnStartHeating.Size = new System.Drawing.Size(180, 36);
            this.btnStartHeating.TabIndex = 1;
            this.btnStartHeating.Text = "开始升温";
            this.btnStartHeating.UseVisualStyleBackColor = true;
            this.btnStartHeating.Click += new System.EventHandler(this.btnStartHeating_Click);
            // 
            // btnStartRecording
            // 
            this.btnStartRecording.Location = new System.Drawing.Point(20, 116);
            this.btnStartRecording.Name = "btnStartRecording";
            this.btnStartRecording.Size = new System.Drawing.Size(180, 36);
            this.btnStartRecording.TabIndex = 2;
            this.btnStartRecording.Text = "开始记录";
            this.btnStartRecording.UseVisualStyleBackColor = true;
            this.btnStartRecording.Click += new System.EventHandler(this.btnStartRecording_Click);
            // 
            // btnStopRecording
            // 
            this.btnStopRecording.Location = new System.Drawing.Point(20, 164);
            this.btnStopRecording.Name = "btnStopRecording";
            this.btnStopRecording.Size = new System.Drawing.Size(180, 36);
            this.btnStopRecording.TabIndex = 3;
            this.btnStopRecording.Text = "停止记录";
            this.btnStopRecording.UseVisualStyleBackColor = true;
            this.btnStopRecording.Click += new System.EventHandler(this.btnStopRecording_Click);
            // 
            // btnStopHeating
            // 
            this.btnStopHeating.Location = new System.Drawing.Point(20, 212);
            this.btnStopHeating.Name = "btnStopHeating";
            this.btnStopHeating.Size = new System.Drawing.Size(180, 36);
            this.btnStopHeating.TabIndex = 4;
            this.btnStopHeating.Text = "停止升温";
            this.btnStopHeating.UseVisualStyleBackColor = true;
            this.btnStopHeating.Click += new System.EventHandler(this.btnStopHeating_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(20, 264);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(98, 17);
            this.label6.TabIndex = 5;
            this.label6.Text = "试验时长模式";
            // 
            // comboBoxDuration
            // 
            this.comboBoxDuration.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxDuration.FormattingEnabled = true;
            this.comboBoxDuration.Items.AddRange(new object[] {
            "标准 60 分钟",
            "自定义 30 分钟"});
            this.comboBoxDuration.Location = new System.Drawing.Point(20, 288);
            this.comboBoxDuration.Name = "comboBoxDuration";
            this.comboBoxDuration.Size = new System.Drawing.Size(180, 25);
            this.comboBoxDuration.TabIndex = 6;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label1.Location = new System.Drawing.Point(220, 18);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(52, 15);
            this.label1.TabIndex = 7;
            this.label1.Text = "炉温1：";
            // 
            // lblTF1
            // 
            this.lblTF1.AutoSize = true;
            this.lblTF1.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblTF1.Location = new System.Drawing.Point(280, 15);
            this.lblTF1.Name = "lblTF1";
            this.lblTF1.Size = new System.Drawing.Size(82, 20);
            this.lblTF1.TabIndex = 8;
            this.lblTF1.Text = "25.0 °C";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label2.Location = new System.Drawing.Point(220, 45);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(52, 15);
            this.label2.TabIndex = 9;
            this.label2.Text = "炉温2：";
            // 
            // lblTF2
            // 
            this.lblTF2.AutoSize = true;
            this.lblTF2.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblTF2.Location = new System.Drawing.Point(280, 42);
            this.lblTF2.Name = "lblTF2";
            this.lblTF2.Size = new System.Drawing.Size(82, 20);
            this.lblTF2.TabIndex = 10;
            this.lblTF2.Text = "24.9 °C";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label3.Location = new System.Drawing.Point(220, 75);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(76, 15);
            this.label3.TabIndex = 11;
            this.label3.Text = "表面温度：";
            // 
            // lblTS
            // 
            this.lblTS.AutoSize = true;
            this.lblTS.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblTS.Location = new System.Drawing.Point(300, 72);
            this.lblTS.Name = "lblTS";
            this.lblTS.Size = new System.Drawing.Size(82, 20);
            this.lblTS.TabIndex = 12;
            this.lblTS.Text = "24.5 °C";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label4.Location = new System.Drawing.Point(220, 105);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(76, 15);
            this.label4.TabIndex = 13;
            this.label4.Text = "中心温度：";
            // 
            // lblTC
            // 
            this.lblTC.AutoSize = true;
            this.lblTC.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblTC.Location = new System.Drawing.Point(300, 102);
            this.lblTC.Name = "lblTC";
            this.lblTC.Size = new System.Drawing.Size(82, 20);
            this.lblTC.TabIndex = 14;
            this.lblTC.Text = "24.3 °C";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label5.Location = new System.Drawing.Point(220, 135);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(76, 15);
            this.label5.TabIndex = 15;
            this.label5.Text = "校准温度：";
            // 
            // lblTCal
            // 
            this.lblTCal.AutoSize = true;
            this.lblTCal.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblTCal.Location = new System.Drawing.Point(300, 132);
            this.lblTCal.Name = "lblTCal";
            this.lblTCal.Size = new System.Drawing.Size(82, 20);
            this.lblTCal.TabIndex = 16;
            this.lblTCal.Text = "25.1 °C";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label7.Location = new System.Drawing.Point(220, 170);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(68, 15);
            this.label7.TabIndex = 17;
            this.label7.Text = "当前状态：";
            // 
            // lblCurrentState
            // 
            this.lblCurrentState.AutoSize = true;
            this.lblCurrentState.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblCurrentState.ForeColor = System.Drawing.Color.DodgerBlue;
            this.lblCurrentState.Location = new System.Drawing.Point(300, 168);
            this.lblCurrentState.Name = "lblCurrentState";
            this.lblCurrentState.Size = new System.Drawing.Size(38, 20);
            this.lblCurrentState.TabIndex = 18;
            this.lblCurrentState.Text = "Idle";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label9.Location = new System.Drawing.Point(220, 200);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(76, 15);
            this.label9.TabIndex = 19;
            this.label9.Text = "已记录秒数：";
            // 
            // lblElapsedSeconds
            // 
            this.lblElapsedSeconds.AutoSize = true;
            this.lblElapsedSeconds.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblElapsedSeconds.Location = new System.Drawing.Point(300, 198);
            this.lblElapsedSeconds.Name = "lblElapsedSeconds";
            this.lblElapsedSeconds.Size = new System.Drawing.Size(17, 20);
            this.lblElapsedSeconds.TabIndex = 20;
            this.lblElapsedSeconds.Text = "0";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label11.Location = new System.Drawing.Point(220, 230);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(68, 15);
            this.label11.TabIndex = 21;
            this.label11.Text = "温度漂移：";
            // 
            // lblDrift
            // 
            this.lblDrift.AutoSize = true;
            this.lblDrift.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblDrift.Location = new System.Drawing.Point(300, 228);
            this.lblDrift.Name = "lblDrift";
            this.lblDrift.Size = new System.Drawing.Size(99, 20);
            this.lblDrift.TabIndex = 22;
            this.lblDrift.Text = "0.00 °C/10min";
            // 
            // panelPlot
            // 
            this.panelPlot.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelPlot.Location = new System.Drawing.Point(220, 265);
            this.panelPlot.Name = "panelPlot";
            this.panelPlot.Size = new System.Drawing.Size(760, 230);
            this.panelPlot.TabIndex = 23;
            // 
            // richTextBoxLog
            // 
            this.richTextBoxLog.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.richTextBoxLog.BackColor = System.Drawing.Color.Black;
            this.richTextBoxLog.ForeColor = System.Drawing.Color.White;
            this.richTextBoxLog.Location = new System.Drawing.Point(20, 505);
            this.richTextBoxLog.Name = "richTextBoxLog";
            this.richTextBoxLog.ReadOnly = true;
            this.richTextBoxLog.Size = new System.Drawing.Size(960, 110);
            this.richTextBoxLog.TabIndex = 24;
            this.richTextBoxLog.Text = string.Empty;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1000, 620);
            this.Controls.Add(this.richTextBoxLog);
            this.Controls.Add(this.panelPlot);
            this.Controls.Add(this.lblDrift);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.lblElapsedSeconds);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.lblCurrentState);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.lblTCal);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.lblTC);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.lblTS);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.lblTF2);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.lblTF1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.comboBoxDuration);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.btnStopHeating);
            this.Controls.Add(this.btnStopRecording);
            this.Controls.Add(this.btnStartRecording);
            this.Controls.Add(this.btnStartHeating);
            this.Controls.Add(this.btnNewTest);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.Text = "NonCombustibility Test 仿真系统";
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}
