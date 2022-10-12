namespace OnlinStore;

public class TimeInUTC : IClock
{
    public DateTime GetUTCTime()
    {
        return TimeZoneInfo.ConvertTimeToUtc(DateTime.Now, TimeZoneInfo.Local);
    }
}