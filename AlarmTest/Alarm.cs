using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlarmTest
{
    public enum AlarmStatus
    {
        Waiting,
        Completed
    }

    public class Alarm
    {
        public DateTime DateAndTime { get; set; }
        public AlarmStatus Status { get; set; } = 0;
    }
}
