using Newtonsoft.Json;

namespace ActivityWatchAnalyzer
{
    public class AwWatcherWindowChjay
    {
        public string id { get; set; }
        public string type { get; set; }
        public string client { get; set; }
        public string hostname { get; set; }
        public DateTime created { get; set; }
        public Data data { get; set; }
        public Metadata metadata { get; set; }
        public List<Event> events { get; set; }
        public object last_updated { get; set; }
    }

    public class Buckets
    {
        [JsonProperty("aw-watcher-window_chjay")]
        public AwWatcherWindowChjay awwatcherwindow_chjay { get; set; }
    }

    public class Data
    {
        public string app { get; set; }
        public string title { get; set; }
    }

    public class Event
    {
        public int id { get; set; }
        public DateTime timestamp { get; set; }
        public double duration { get; set; }
        public Data data { get; set; }
    }

    public class Metadata
    {
        public DateTime start { get; set; }
        public DateTime end { get; set; }
    }

    public class AwData
    {
        public Buckets buckets { get; set; }
    }

    public class ProcessActivity()
    {
        public string Process { get; set; }
        public TimeSpan Duration { get; set; }
        public List<AppActivity> Activities { get; set; }
    }

    public class AppActivity
    {
        public string WindowTitle { get; set; }
        public TimeSpan Duration { get; set; }
    }
}