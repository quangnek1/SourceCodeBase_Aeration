using AerationSterilize.Application.Features.V1.Setting.Common.Dtos;
using Contracts.Common.Messages;

namespace AerationSterilize.Application.Features.V1.Setting.Queries.GetSettings;

public record GetSettingQuery() : IQuery<SettingDto>;
