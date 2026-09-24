using AerationSterilize.API.Abstractions;
using AerationSterilize.Application.Features.V1.Setting.Commands.CreateSetting;
using AerationSterilize.Application.Features.V1.Setting.Commands.DeleteSetting;
using AerationSterilize.Application.Features.V1.Setting.Commands.UpdateSetting;
using AerationSterilize.Application.Features.V1.Setting.Common.Dtos;
using AerationSterilize.Application.Features.V1.Setting.Queries.GetSettingById;
using AerationSterilize.Application.Features.V1.Setting.Queries.GetSettings;
using Asp.Versioning;
using Contracts.Responses;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AerationSterilize.API.Controllers.V1;

[ApiVersion(1)]
public class SettingController : ApiController
{
    public SettingController(ISender sender) : base(sender)
    {
    }

    [HttpGet]
    [Route("get-setting")]
    [ProducesResponseType(statusCode: StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetSetting()
    {
        var result = await Sender.Send(new GetSettingQuery());

        return Ok(result);
    }

    [HttpGet("{id:int}", Name = "GetSettingById")]
    [ProducesResponseType(type: typeof(Result<SettingDto>), statusCode: StatusCodes.Status200OK)]
    [ProducesResponseType(statusCode: StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetSettingById(int id)
    {
        var result = await Sender.Send(new GetSettingByIdQuery(id));
        return Ok(result);
    }

    [HttpPost]
    [ProducesResponseType(type: typeof(Result<int>), statusCode: StatusCodes.Status200OK)]
    [ProducesResponseType(statusCode: StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateSetting([FromBody] CreateSettingCommand command)
    {
        var result = await Sender.Send(command);
        return Ok(result);
    }

    [HttpPut("{id:int}", Name = "update-setting")]
    [ProducesResponseType(type: typeof(Result<SettingDto>), statusCode: StatusCodes.Status200OK)]
    [ProducesResponseType(statusCode: StatusCodes.Status400BadRequest)]
    [ProducesResponseType(statusCode: StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateSetting(int id, [FromBody] UpdateSettingCommand command)
    {
        command.SetId(id);
        var result = await Sender.Send(command);
        return Ok(result);
    }

    [HttpDelete("{id:int}", Name = "DeleteSetting")]
    [ProducesResponseType(type: typeof(Result), statusCode: StatusCodes.Status200OK)]
    [ProducesResponseType(statusCode: StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteSetting(int id)
    {
        var result = await Sender.Send(new DeleteSettingCommand(id));
        return Ok(result);
    }
}
