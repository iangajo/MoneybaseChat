namespace MoneybaseChat.Domain.Helpers
{
    public static class Common
    {
        public static bool IsOfficeHours(DateTime now)
        {
            if (now.DayOfWeek is DayOfWeek.Saturday or DayOfWeek.Sunday)
                return false;

            var start = new TimeSpan(8, 0, 0);
            var end = new TimeSpan(17, 0, 0);

            return now.TimeOfDay >= start && now.TimeOfDay < end;
        }
    }
}
