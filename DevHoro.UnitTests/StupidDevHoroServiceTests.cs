using DevHoro.WebAPI.Domain.HoroService;
using DevHoro.WebAPI.Domain.ExistLanguageChecker;
using DevHoro.WebAPI.Infrastructure.CurrentDateTimeProvider;
using DevHoro.WebAPI.Infrastructure.DevHoroService;
using Moq;
using DevHoro.WebAPI.Domain.Common;

namespace DevHoro.UnitTests;

public class StupidDevHoroServiceTests
{
    private DateOnly Today => new DateOnly(2001, 1, 1);
    private string Language => "Some language";

    private readonly IExistLanguageChecker _stubExistLanguageChecker;
    private readonly ICurrentDateProvider _mockCurrentDateProvider;

    public StupidDevHoroServiceTests()
    {
        _stubExistLanguageChecker =
            Mock.Of<IExistLanguageChecker>(o => o.Check(It.IsAny<string>()).Result == true);

        _mockCurrentDateProvider =
            Mock.Of<ICurrentDateProvider>(o => o.Today == Today);
    }

    [Fact]
    public async Task Get_horo()
    {
        var service = new StupidDevHoroService(_stubExistLanguageChecker, _mockCurrentDateProvider);
        
        var horo = await service.GetAsync(new GetHoroQuery(Language, Today));

        Assert.Equal(Today, horo.Date);
        Assert.Equal(Language, horo.Language);
        Assert.NotEmpty(horo.Text);
    }

    [Fact]
    public async Task Get_horo_for_tomorow()
    {
        var tomorow = Today.AddDays(1);

        var service = new StupidDevHoroService(_stubExistLanguageChecker, _mockCurrentDateProvider);

        var horo = await service.GetAsync(new GetHoroQuery(Language, tomorow));

        Assert.Equal(tomorow, horo.Date);
        Assert.Equal(Language, horo.Language);
        Assert.NotEmpty(horo.Text);
    }

    [Fact]
    public async Task Get_horo_for_too_late_date()
    {
        const int maximumPossibleNumberOfDays = 7;
        
        var tooLateDate = Today.AddDays(maximumPossibleNumberOfDays + 1);

        var service = new StupidDevHoroService(_stubExistLanguageChecker, _mockCurrentDateProvider);

        await Assert.ThrowsAsync<DomainException>(
            async () => await service.GetAsync(new GetHoroQuery(Language, tooLateDate))
        );
    }
}