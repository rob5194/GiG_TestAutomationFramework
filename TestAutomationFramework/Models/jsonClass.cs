using System;
using System.Collections.Generic;
using System.Text;

namespace TestAutomationFramework.Models
{
    /// <summary>
    /// A class used to read data from a JSON file for configuration settings of Kafka
    /// </summary>
    public class jsonClass
    {
        public string group_id { get; set; }
        public string bootstrap_servers { get; set; }
        public string auto_commit_interval_ms { get; set; }
        public string auto_offset_reset { get; set; }
        public string topic { get; set; }

        public jsonClass() {  }
    }
}
