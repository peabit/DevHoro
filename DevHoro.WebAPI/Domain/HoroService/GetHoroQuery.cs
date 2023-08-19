namespace DevHoro.WebAPI.Domain.HoroService;

public sealed record GetHoroQuery(
    string Language, 
    DateOnly Date
);