using Adz.BLL.Lib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adz.Tasks
{
    class Program
    {
        static void Main(string[] args)
        {
            IUserService userservice = new UserService();

            string currentDir = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location) + @"\";

            //process today usage data
            var Task_TodayUsage_Start_DateTime = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss");
            try
            {
                var Result = userservice.UserPointUpdate().Result;

                // Write the point update logging to a file.
                System.IO.StreamWriter file = new System.IO.StreamWriter(currentDir + "PointUpdate.log", true);
                file.WriteLine(Task_TodayUsage_Start_DateTime + " - " + DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss") + " - " + "Successfully done." + " " + String.Join(",", Result));
                file.Close();
            }
            catch (Exception e)
            {
                // Write the exception logging to a file.
                System.IO.StreamWriter file = new System.IO.StreamWriter(currentDir + "PointUpdate.log", true);
                file.WriteLine(Task_TodayUsage_Start_DateTime + " - " + DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss") + " - " + "Error: " + e);
                file.Close();
            }

            //process past usage data
            var Task_PastUsage_Start_DateTime = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss");
            try
            {
                var Result = userservice.UserPointUpdatePastUsage().Result;

                // Write the point update logging to a file.
                System.IO.StreamWriter file = new System.IO.StreamWriter(currentDir + "PointUpdate_PastUsage.log", true);
                file.WriteLine(Task_PastUsage_Start_DateTime + " - " + DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss") + " - " + "Successfully done." + " " + string.Join(", ", Result.Select(x => string.Format("{0}:{1}", x.Key, x.Value.ToString("dd/MM/yyyy"))).ToArray()));
                file.Close();
            }
            catch (Exception e)
            {
                // Write the exception logging to a file.
                System.IO.StreamWriter file = new System.IO.StreamWriter(currentDir + "PointUpdate_PastUsage.log", true);
                file.WriteLine(Task_PastUsage_Start_DateTime + " - " + DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss") + " - " + "Error: " + e);
                file.Close();
            }
        }
    }
}
