using DevHoro.WebAPI.Domain.HoroService;
using DevHoro.WebAPI.Domain.ExistLanguageChecker;
using DevHoro.WebAPI.Infrastructure.CurrentDateTimeProvider;
using DevHoro.WebAPI.Infrastructure.DevHoroService;
using Moq;
using DevHoro.WebAPI.Domain.Common;
using AutoFixture;
using AutoFixture.AutoMoq;
using Shouldly;

namespace DevHoro.UnitTests;

public class StupidDevHoroServiceTests
{
    private DateOnly Today { get; init; }
    private string Language { get; init; }

    //private readonly IExistLanguageChecker _stubExistLanguageChecker;
    //private readonly ICurrentDateProvider _stubCurrentDateProvider;

    private readonly IFixture _fixture = new Fixture().Customize(new AutoMoqCustomization());

    public StupidDevHoroServiceTests()
    {
        Today = DateOnly.FromDateTime(_fixture.Create<DateTime>());

        Language = _fixture.Create<string>();

        var stubExistLanguageChecker = _fixture.Freeze<Mock<IExistLanguageChecker>>();
        
        stubExistLanguageChecker
            .Setup(o => o.Check(It.IsAny<string>()))
            .ReturnsAsync(true);

        var mockCurrentDateProvider = _fixture.Freeze<Mock<ICurrentDateProvider>>();

        mockCurrentDateProvider
            .Setup(o => o.Today)
            .Returns(Today);

        //_stubExistLanguageChecker =
        //    Mock.Of<IExistLanguageChecker>(o => o.Check(It.IsAny<string>()).Result == true);

        //_stubCurrentDateProvider =
        //    Mock.Of<ICurrentDateProvider>(o => o.Today == Today);
    }

    [Fact]
    public async Task Get_horo()
    {
        var sut = _fixture.Create<StupidDevHoroService>();

        var horo = await sut.GetAsync(new GetHoroQuery(Language, Today));

        horo.Date.ShouldBe(Today);
        horo.Language.ShouldBe(Language);
        horo.Text.ShouldNotBeNullOrEmpty();
    }

    [Fact]
    public async Task Get_horo_for_tomorow()
    {
        var tomorow = Today.AddDays(1);

        var sut = _fixture.Create<StupidDevHoroService>();

        var horo = await sut.GetAsync(new GetHoroQuery(Language, tomorow));

        horo.Date.ShouldBe(tomorow);
        horo.Language.ShouldBe(Language);
        horo.Text.ShouldNotBeNullOrEmpty();
    }

    [Fact]
    public async Task Get_horo_for_too_late_date()
    {
        const int maximumPossibleNumberOfDays = 7;

        var tooLateDate = Today.AddDays(maximumPossibleNumberOfDays + 1);

        var sut = _fixture.Create<StupidDevHoroService>();

        await Assert.ThrowsAsync<DomainException>(
            async () => await sut.GetAsync(new GetHoroQuery(Language, tooLateDate))
        );
    }

    [Fact]
    public async Task Get_horo_for_yesterday()
    {
        var yesterday = Today.AddDays(-1);

        var sut = _fixture.Create<StupidDevHoroService>();

        await Should.ThrowAsync<DomainException>(
            async () => await sut.GetAsync(new GetHoroQuery(Language, yesterday))
        );
    }

    [Fact]
    public async Task Get_horo_for_unknown_language()
    {
        var stubExistLanguageChecker = Mock.Of<IExistLanguageChecker>(
            o => o.Check(It.IsAny<string>()).Result == false
        );

        var stubCurrentDateProvider = _fixture.Create<ICurrentDateProvider>();

        var sut = new StupidDevHoroService(stubExistLanguageChecker, stubCurrentDateProvider);

        await Should.ThrowAsync<DomainException>(
            async () => await sut.GetAsync(new GetHoroQuery(Language, Today))
        );
    }
}