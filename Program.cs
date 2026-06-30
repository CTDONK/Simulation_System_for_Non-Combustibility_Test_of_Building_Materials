using NonCombustibilityTestSimulator.Data;
using NonCombustibilityTestSimulator.Forms;
using System;
using System.Windows.Forms;

namespace NonCombustibilityTestSimulator;

internal static class Program
{
    [STAThread]
    static void Main()
    {
        try
        {
            // 初始化数据库
            var init = new DatabaseInitializer("Data\\ISO11820.db");
            init.Initialize();

            ApplicationConfiguration.Initialize();
            Application.Run(new LoginForm());
        }
        catch (Exception ex)
        {
            MessageBox.Show($"程序启动失败：\n{ex.Message}\n\n{ex.StackTrace}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}