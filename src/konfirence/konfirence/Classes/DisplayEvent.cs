using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace konfirence.Classes
{
    public class DisplayEvent
    {
        public int ID { get; set; }
        public string EventTopic { get; set; }
        public string ImagePath { get; set; } // Свойство для хранения пути к изображению
        public DateTime Date { get; internal set; }
        public object EventDirection { get; internal set; }
    }

}
