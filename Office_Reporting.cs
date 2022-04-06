using System;

namespace WebApplication3
{
    public class Office_Reporting
    {
        public class EntryLog
        {
            public int ID { get; set; }
            public DateTime Time { get; set; }

            public EntryLog(int id, DateTime time)
            {
                ID = id;
               Time = time;
            }

        }

    }
}
