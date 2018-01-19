using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace newProject.Models
{
    public class DataControl
    {

        public DataControl()
        {
                
        }

        public DataControl(string date, char op, int minutes)
        {
            Date = date;
            Op = op;
            Minutes = minutes;
        }

        public string Date { get; set; }
        public char Op { get; set; }
        public int Minutes { get; set; }
    }
}
