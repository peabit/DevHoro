using DevHoro.WebAPI.Domain.Common;
using DevHoro.WebAPI.Domain.HoroService;
using DevHoro.WebAPI.Domain.ExistLanguageChecker;
using DevHoro.WebAPI.Infrastructure.DevHoroService;
using DevHoro.WebAPI.Infrastructure.ExistLanguageChecker;
using DevHoro.WebAPI.Infrastructure.CurrentDateTimeProvider;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<ICurrentDateProvider, CurrentServerDateProvider>();
builder.Services.AddSingleton<IExistLanguageChecker, StupidExistLanguageChecker>();
builder.Services.AddSingleton<IHoroService, StupidDevHoroService>();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

//app.MapGet("horo{language}&{date:datetime}", async (string language, DateTime date, IHoroService horoService) =>
//{
//	try
//	{
//        var query = new GetHoroQuery(language, DateOnly.FromDateTime(date));
//        return Results.Ok(await horoService.GetAsync(query));
//    }
//	catch (DomainException ex)
//	{
//		return Results.Problem(detail: ex.Message, statusCode: StatusCodes.Status400BadRequest);
//    }
//});

app.MapControllers();

app.Run();