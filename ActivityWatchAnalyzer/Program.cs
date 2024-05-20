using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Specialized;

namespace ActivityWatchAnalyzer
{
    internal class Program
    {
        public static List<ProcessActivity> allActivity = new List<ProcessActivity>();
        static void Main(string[] args)
        {
            Console.WriteLine("File Location:");
            string fileLocation = "C:\\Users\\chjay\\Downloads\\aw-bucket-export_aw-watcher-window_chjay (3).json"; //testing
            //fileLocation = Console.ReadLine();
            string awJson = File.ReadAllText(fileLocation);
            List<Event> awData = JsonConvert.DeserializeObject<AwData>(awJson).buckets.awwatcherwindow_chjay.events;
            var groupedDataByProcess = awData
                    .GroupBy(m => new { m.data.app})
                    .Select(group => group.First())
                    .ToList();            
            var durations = awData.Select(x => x.duration);           
            foreach(var gData in groupedDataByProcess)
            {
                double totalDurationByApp = awData.Where(x => x.data.app == gData.data.app).Sum(x => x.duration);
                allActivity.Add(new ProcessActivity { Process = gData.data.app, Duration = TimeSpan.FromSeconds(totalDurationByApp), Activities = new List<AppActivity>() });
            }
            foreach(var Activity in allActivity)
            {
                var groupedDataByTitle = awData
                    .Where(x => x.data.app == Activity.Process)
                    .GroupBy(m => new { m.data.title })
                    .Select(group => group.First())
                    .ToList();
                foreach(var Window in groupedDataByTitle)
                {
                    TimeSpan duration = TimeSpan.FromSeconds(awData.Where(x => x.data.app == Window.data.app && x.data.title == Window.data.title).Sum(x => x.duration));
                    Activity.Activities.Add(new AppActivity { WindowTitle = Window.data.title, Duration = duration });
                }
            }
            allActivity = allActivity.OrderByDescending(x => x.Duration).ToList();
            for(int i=0; i< allActivity.Count; ++i)
            {
                allActivity[i].Activities = allActivity[i].Activities.OrderByDescending(x => x.Duration).ToList();
            }
            PrintAllProcess();
            var state = "Process";
            while (true)
            {              
                var input = Console.ReadLine();
                if(input == "q")
                {
                    Environment.Exit(0);
                }
                else if(input == "gb")
                {
                    state = "Process";
                    Console.Clear();
                    PrintAllProcess();
                }
                else if(input == "clr")
                {
                    Console.Clear();
                }
                else
                {
                    Console.Clear();
                    PrintProcess(input + ".exe");
                }
            }
        }

        private static void PrintProcess(string value)
        {
            var Process = allActivity.Where(x => x.Process == value).First();
            foreach(var window in Process.Activities)
            {
                if (window.Duration.TotalMinutes < 60)
                {
                    Console.WriteLine($"{window.WindowTitle}     ---     {TruncuateTo2(window.Duration.TotalMinutes)} M");
                }
                else
                {
                    Console.WriteLine($"{window.WindowTitle}     ---     {TruncuateTo2(window.Duration.TotalHours)} H");
                }
            }
            Console.WriteLine("$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$");
        }

        private static void PrintAllProcess()
        {
            foreach (var item in allActivity)
            {
                if (item.Duration.TotalMinutes < 60)
                {
                    Console.WriteLine($"{item.Process}     ---     {TruncuateTo2(item.Duration.TotalMinutes)} M");
                }
                else
                {
                    Console.WriteLine($"{item.Process}     ---     {TruncuateTo2(item.Duration.TotalHours)} H");
                }
            }
            Console.WriteLine("$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$");
        }

        private static double TruncuateTo2(double value)
        {
            return Math.Truncate(100 * value) / 100;
        }         
    }
}
