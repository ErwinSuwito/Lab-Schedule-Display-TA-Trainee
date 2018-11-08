using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Composition;
using Windows.Media;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml;

namespace Lab_Schedule_Display.DataLayers
{
    public class Schedules : INotifyPropertyChanged
    {
        public string ModuleCode { get; set; }
        public string LecturerID { get; set; }
        public string LecturerName { get; set; }
        public string IntakeCode { get; set; }
        public string ModuleName { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public SolidColorBrush myBrush {get; set;}

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyEventChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
