using Adz.BLL.Lib.Report;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Adz.Scheduler.Report
{
    public partial class DailyReportForm : Form
    {
        Timer MyTimer = new Timer();
        IReportService ReportService = new ReportService();
        int TimerInterval = 60 * 1000; //1 min
        DateTime DoAtTime = new DateTime();

        string Time_FilePath = @"Time.txt";

        private void MyTimer_Tick(object sender, EventArgs e)
        {
            try
            {
                if (DateTime.Now.Hour == DoAtTime.Hour && DateTime.Now.Minute == DoAtTime.Minute)
                {
                    var SendToEmail = ReportService.GetDailyReportTargetEmail();

                    ReportService.SendEmailDailyReport(SendToEmail);
                    Log("Success: Sent to " + SendToEmail);
                }
            }
            catch (Exception ex)
            {
                Log("Error: " + ex.Message);

                StopTimer();
            }
        }

        public void Log(string logMessage)
        {
            using (StreamWriter w = File.AppendText("log.txt"))
            {
                w.Write("\r\n");
                w.Write("{0} {1}", DateTime.Now.ToLongTimeString(),
                    DateTime.Now.ToLongDateString());
                w.WriteLine(" - {0}", logMessage);
            }
        }

        public DailyReportForm()
        {
            MyTimer.Interval = TimerInterval;
            MyTimer.Tick += new EventHandler(MyTimer_Tick);

            InitializeComponent();

            //get initial time from file (if only file exists)
            if (File.Exists(Time_FilePath))
            {
                string Time = System.IO.File.ReadAllText(Time_FilePath);
                txtTime.Text = Time;

                StartTimer();
            }
            else
            {
                btnStart.Enabled = true;
                btnStop.Enabled = false;
            }
        }

        private void StartTimer()
        {
            txtTime.ReadOnly = true;
            btnStart.Enabled = false;
            btnStop.Enabled = true;

            MyTimer.Start();

            try
            {
                DoAtTime = DateTime.ParseExact(txtTime.Text, "HH:mm", CultureInfo.InvariantCulture);

                System.IO.File.WriteAllText(Time_FilePath, txtTime.Text);
            }
            catch
            {
                StopTimer();
                MessageBox.Show("Time format is wrong.", "Error");
            }
        }

        private void StopTimer()
        {
            txtTime.ReadOnly = false;
            btnStart.Enabled = true;
            btnStop.Enabled = false;

            MyTimer.Stop();
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            StartTimer();
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            StopTimer();
        }
    }
}
