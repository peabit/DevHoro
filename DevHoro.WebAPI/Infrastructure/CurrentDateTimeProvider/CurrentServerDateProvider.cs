namespace DevHoro.WebAPI.Infrastructure.CurrentDateTimeProvider;

public sealed class CurrentServerDateProvider : ICurrentDateProvider
{
    public DateOnly Today => DateOnly.FromDateTime(DateTime.Now);
}