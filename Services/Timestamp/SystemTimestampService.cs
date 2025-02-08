namespace ASP_SPR311.Services.Timestamp
{
    public class SystemTimestampService : ITimestampService
    {
        public long Timestamp => DateTime.Now.Ticks;
    }
}
