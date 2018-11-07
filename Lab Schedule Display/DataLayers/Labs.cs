using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab_Schedule_Display.DataLayers
{
    public class Labs : INotifyPropertyChanged
    {
        public string LabName { get; set; }
        public string LabLocation { get; set; }
        public string Level { get; set; }
        public TimeSpan currentTime { get; set; }

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
