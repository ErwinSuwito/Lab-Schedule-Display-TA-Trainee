using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab_Schedule_Display.DbTables
{
    class Labs
    {
        public string LabName { get; set; }
        public string Location { get; set; }
        public int Level { get; set; }
        public TimeSpan CloseTime {get;set;}
    }

    class Lecturer
    {
        public string LecturerID { get; set; }
        public string LecName { get; set; }
    }

    class Modules
    {
        public string ModuleCode { get; set; }
        public string ModuleName { get; set; }
        public string LecturerID { get; set; }
    }

    class Schedule
    {
        public int ScheduleID { get; set; }
        public string LabName { get; set; }
        public string ModuleCode { get; set; }
        public string LecturerID { get; set; }
        public string IntakeCode { get; set; }
        public DateTime UseDate { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
    }

}
