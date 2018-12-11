using System;

namespace NazdaqSearch.Models
{

    public class NDData 
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string TimeandDate { get; set; }
        public string Symbol { get; set; }
        public string Data { get; set; }

        public override string ToString()
        {
            return Title + "\n" + Symbol + "\n" + TimeandDate + "\n" + Data + "\n\n";
        }

    }
}