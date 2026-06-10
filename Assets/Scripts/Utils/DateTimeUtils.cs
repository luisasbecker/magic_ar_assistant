using System;

namespace MagicARAssistant.Utils
{
    public static class DateTimeUtils
    {
        public static string NowIsoUtc()
        {
            return DateTime.UtcNow.ToString("o");
        }
    }
}

