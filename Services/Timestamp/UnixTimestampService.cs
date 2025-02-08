namespace ASP_SPR311.Services.Timestamp
{
    public class UnixTimestampService : ITimestampService
    {
        public long Timestamp => ((DateTimeOffset)DateTime.UtcNow).ToUnixTimeSeconds();
    }
}
