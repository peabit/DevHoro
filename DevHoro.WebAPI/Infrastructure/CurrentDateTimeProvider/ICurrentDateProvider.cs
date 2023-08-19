namespace DevHoro.WebAPI.Infrastructure.CurrentDateTimeProvider;

public interface ICurrentDateProvider
{
    DateOnly Today { get; }
}