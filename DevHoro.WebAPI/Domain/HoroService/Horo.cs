namespace DevHoro.WebAPI.Domain.HoroService;

public sealed record Horo(
    string Language, 
    DateOnly Date, 
    string Text
);