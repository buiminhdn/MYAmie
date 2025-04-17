namespace Utility;
public static class DateTimeUtils
{
    // <summary>
    /// Convert DateTime to UnixTime
    /// </summary>
    public static long TimeInEpoch(DateTime? dt = null)
    {
        if (!dt.HasValue)
        {
            dt = DateTime.UtcNow;
        }
        // Ensure the DateTime is in UTC
        DateTime utcDateTime = DateTime.SpecifyKind(dt.Value, DateTimeKind.Utc);
        DateTimeOffset dtOff = utcDateTime;
        return dtOff.ToUnixTimeMilliseconds();
    }

    /// <summary>
    /// Convert UnixTime to DateTime
    /// </summary>
    public static DateTime EpochToTime(long epoch)
    {
        // Convert Unix Epoch time to UTC and then to local time
        var utcTime = DateTimeOffset.FromUnixTimeMilliseconds(epoch).UtcDateTime;
        var vnTimeZone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
        return TimeZoneInfo.ConvertTimeFromUtc(utcTime, vnTimeZone);
    }

    public static string EpochToDateString(long epoch)
    {
        var utcTime = DateTimeOffset.FromUnixTimeMilliseconds(epoch).UtcDateTime;
        var vnTimeZone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
        var vnTime = TimeZoneInfo.ConvertTimeFromUtc(utcTime, vnTimeZone);
        return vnTime.ToString("dd-MM-yyyy");
    }

    public static string EpochToTimeString(long epoch)
    {
        var utcTime = DateTimeOffset.FromUnixTimeMilliseconds(epoch).UtcDateTime;
        var vnTimeZone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
        var vnTime = TimeZoneInfo.ConvertTimeFromUtc(utcTime, vnTimeZone);
        return vnTime.ToString("HH:mm");
    }
}
