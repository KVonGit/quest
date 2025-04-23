using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TextAdventures.Quest.Functions
{
    public static class DateTimeFunctions
    {
        public static int CurrentDateUTC()
        {
            return (int)(DateTimeOffset.UtcNow.ToUnixTimeSeconds());
        }

        public static string CurrentDate()
        {
            return DateTime.Now.ToShortDateString();
        }

        public static string CurrentTime()
        {
            return DateTime.Now.ToShortTimeString();
        }

        public static string CurrentDateUTCString()
        {
            return DateTime.UtcNow.ToShortDateString();
        }

        public static string CurrentTimeUTC()
        {
            return DateTime.UtcNow.ToShortTimeString();
        }
    }
}
