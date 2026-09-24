using AerationSterilize.API.Abstractions;
using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AerationSterilize.API.Controllers.V1;
[ApiVersion(1)]
public class HealthController : ApiController
{
    public HealthController(ISender sender) : base(sender)
    {
    }

    [HttpGet]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Health()
    {
        return Ok();
    }
}
