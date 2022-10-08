namespace OnlinStore;

public class TimeInUTC : ITimeInUTC
{
    public string GetUTCTime()
    {
        return TimeZoneInfo.ConvertTimeToUtc(DateTime.Now, TimeZoneInfo.Local).ToString("h:mm:ss tt");
    }
}