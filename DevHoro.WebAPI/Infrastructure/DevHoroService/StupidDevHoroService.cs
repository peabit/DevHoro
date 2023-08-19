using DevHoro.WebAPI.Domain.Common;
using DevHoro.WebAPI.Domain.HoroService;
using DevHoro.WebAPI.Domain.ExistLanguageChecker;
using DevHoro.WebAPI.Infrastructure.CurrentDateTimeProvider;

namespace DevHoro.WebAPI.Infrastructure.DevHoroService;

public class StupidDevHoroService : IHoroService
{
    private readonly IReadOnlyList<string> _texts = new List<string>()
    {
        "Эти жалкие лентяи все время стараются свою работу на кого-нибудь еще свалить",
        "Надеюсь, ты хорошо подготовился. Не хочу, чтобы твоя смерть была на моей совести",
        "Единственное, что может нас всех накормить - это огромный червь там, на западе",
        "Давай, пойдем прикончим этого могильного червя!"
    };

    private readonly Random _random = new();
    private readonly List<Horo> _cache = new();

    private readonly IExistLanguageChecker _existLanguageChecker;
    private readonly ICurrentDateProvider _currentDateProvider;

    public StupidDevHoroService(IExistLanguageChecker existLanguageChecker, ICurrentDateProvider currentDateProvider)
    {
        _existLanguageChecker = existLanguageChecker ?? throw new ArgumentNullException(nameof(existLanguageChecker));
        _currentDateProvider = currentDateProvider ?? throw new ArgumentNullException(nameof(currentDateProvider));
    }

    public async Task<Horo> GetAsync(GetHoroQuery query)
    {
        EnsureQueryIsValid(query);

        await EnsureLanguageExists(query.Language);

        var horo = _cache.FirstOrDefault(
            h => h.Language == query.Language && h.Date == query.Date
        );

        if (horo is null)
        {
            var randomText = GetRandomText();

            horo = new Horo(query.Language, query.Date, randomText);

            _cache.Add(horo);
        }

        return await Task.FromResult(horo);
    }

    private string GetRandomText()
    {
        var randomIndex = _random.Next(0, _texts.Count);

        return _texts[randomIndex];
    }

    private void EnsureQueryIsValid(GetHoroQuery query)
    {
        if (String.IsNullOrEmpty(query.Language))
        {
            throw new DomainException("Language is empty");
        }

        var allowedFrom = _currentDateProvider.Today;
        var allowedTo = _currentDateProvider.Today.AddDays(7);

        if (query.Date < allowedFrom || query.Date > allowedTo)
        {
            throw new DomainException(
                $"The date must be in range from {allowedFrom} to {allowedTo}"
            );
        }
    }

    private async Task EnsureLanguageExists(string language)
    {
        if (!await _existLanguageChecker.Check(language))
        {
            throw new DomainException("Language not found");
        }
    }
}