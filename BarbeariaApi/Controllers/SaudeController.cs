using Microsoft.AspNetCore.Mvc;

namespace BarbeariaApi.Controllers;

[ApiController]
[Route("api/saude")]
public class SaudeController : ControllerBase
{
    [HttpGet]
    public IActionResult Consultar()
    {
        return Ok(new
        {
            status = "Saudavel",
            dataHora = DateTime.UtcNow,
            aplicacao = "Barbearia API"
        });
    }
}
