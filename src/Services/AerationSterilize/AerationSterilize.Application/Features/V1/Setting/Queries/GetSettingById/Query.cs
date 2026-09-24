using AerationSterilize.Application.Features.V1.Setting.Common.Dtos;
using Contracts.Common.Messages;

namespace AerationSterilize.Application.Features.V1.Setting.Queries.GetSettingById;

public record GetSettingByIdQuery(int Id) : IQuery<SettingDto>;
