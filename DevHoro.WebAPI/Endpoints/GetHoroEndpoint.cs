using DevHoro.WebAPI.Domain.Common;
using DevHoro.WebAPI.Domain.HoroService;
using Microsoft.AspNetCore.Mvc;

namespace DevHoro.WebAPI.Endpoints;

[ApiController]
public class GetHoroEndpoint : ControllerBase
{
    private readonly IHoroService _horoService;

    public GetHoroEndpoint(IHoroService horoService) 
        => _horoService = horoService ?? throw new ArgumentNullException(nameof(horoService));

    [HttpGet(template: "horo{language}&{date:datetime}", Name = "GetHoro")]
    [Produces("application/json", Type = typeof(Horo))]
    public async Task<IActionResult> Handle(string language, DateTime date)
    {
        try
        {
            var query = new GetHoroQuery(language, DateOnly.FromDateTime(date));
            return Ok(await _horoService.GetAsync(query));
        }
        catch (DomainException ex)
        {
            return Problem(detail: ex.Message, statusCode: StatusCodes.Status400BadRequest);
        }
    }
}