namespace DevHoro.WebAPI.Domain.HoroService;

public interface IHoroService
{
    Task<Horo> GetAsync(GetHoroQuery query);
}